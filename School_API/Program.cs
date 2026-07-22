using System.Threading.RateLimiting;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using School.Domain.Auth;
using School.Infrastructure;
using School.Models.Configuration;
using School.Services.Mapping;
using School_API;
using School_API.Filters;
using School_API.Middleware;
using School_API.Common.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// ─── Rate Limiting ────────────────────────────────────────────────────────────
builder.Services.AddRateLimiter(options =>
{
    // Strict policy for auth endpoints (login / forgot-password)
    options.AddFixedWindowLimiter("auth", o =>
    {
        o.PermitLimit = 10;
        o.Window = TimeSpan.FromMinutes(1);
        o.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        o.QueueLimit = 0;
    });

    // General API throttle — per IP, sliding window
    options.AddSlidingWindowLimiter("api", o =>
    {
        o.PermitLimit = 200;
        o.Window = TimeSpan.FromMinutes(1);
        o.SegmentsPerWindow = 4;
        o.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        o.QueueLimit = 20;
    });

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.ContentType = "application/json";
        await context.HttpContext.Response.WriteAsJsonAsync(
            new { success = false, message = "Too many requests. Please slow down and try again later." },
            token);
    };
});

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<Microsoft.AspNetCore.ResponseCompression.BrotliCompressionProvider>();
    options.Providers.Add<Microsoft.AspNetCore.ResponseCompression.GzipCompressionProvider>();
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
    options.Filters.Add<School_API.Filters.ApiResponseFilter>();
    options.Filters.Add<AuditActionFilter>(); // Global audit logging
    options.MaxModelBindingCollectionSize = 1000; // Limit collection size
});
builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.WriteIndented = false;
});
builder.Services
   .AddDatabase(builder.Configuration)
   .AddRepositories()
    .AddServices(builder.Configuration)
   .AddSessionWithOptions()
   .AddAuthentication(builder.Configuration)
   .AddCorsPolicy(builder.Configuration)
   .AddHttpContextAccessor()
   .AddScoped<AuditActionFilter>();

builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });

    option.AddSecurityDefinition("Basic", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        Description = "Basic Authentication"
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        },
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Basic"
                }
            },
            Array.Empty<string>()
        }
    });

    option.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "School_Erp API",
        Version = "v1",
        Description = "School API Version 1.0 - Enhanced Security",
        Contact = new OpenApiContact
        {
            Name = "School API Support",
            Email = "support@b2bapi.com"
        }
    });

    option.CustomSchemaIds(type => type.FullName);
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSignalR();

builder.Services.AddAutoMapper(cfg => { cfg.AddProfile(new AutoMapperProfile()); });
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
var appSettings = builder.Configuration.GetSection("AppSettings").Get<AppSettings>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 12; // Enhanced from 8
    options.Password.RequiredUniqueChars = 2;

    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

    options.Lockout = new LockoutOptions()
    {
        AllowedForNewUsers = true,
        DefaultLockoutTimeSpan = TimeSpan.FromMinutes(Convert.ToInt32(appSettings?.AccountBlockedPeriodinMinutes ?? "30")),
        MaxFailedAccessAttempts = Convert.ToInt32(appSettings?.InvalidAllowedLoginAttempts ?? "5")
    };

    options.SignIn.RequireConfirmedEmail = true;
    options.SignIn.RequireConfirmedPhoneNumber = false;
})
.AddEntityFrameworkStores<SchoolDbContext>()
.AddDefaultTokenProviders();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});


builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10485760; // 10MB
    options.ValueLengthLimit = 10485760;
    options.ValueCountLimit = 1000;
});


var redisConnection = builder.Configuration.GetConnectionString("RedisConnection");
if (!string.IsNullOrEmpty(redisConnection))
{
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = redisConnection;
    });
}
else
{
    builder.Services.AddDistributedMemoryCache();
}

builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("Database")
    .AddCheck<RedisHealthCheck>("Redis");

var app = builder.Build();

app.UseCustomExceptionHandler();

app.UseResponseCompression();

app.Use(async (context, next) =>
{
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "SAMEORIGIN";
    context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
    context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
    await next();
});

if (app.Environment.IsProduction() && appSettings?.RequireHttps == true)
{
    app.UseHsts();
    app.UseHttpsRedirection();
}
else if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();

var configuredImagePath = builder.Configuration.GetSection("AppSettings:ImageStoragePath").Value;
if (!string.IsNullOrWhiteSpace(configuredImagePath))
{
    var storageFullPath = Path.GetFullPath(configuredImagePath.Trim().Replace('/', Path.DirectorySeparatorChar));
    if (!Directory.Exists(storageFullPath))
    {
        Directory.CreateDirectory(storageFullPath);
    }
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(storageFullPath),
        RequestPath = "/uploads"
    });
}

app.UseRateLimiter();

app.UseRouting();

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.UseCookiePolicy(new CookiePolicyOptions
{
    HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always,
    MinimumSameSitePolicy = SameSiteMode.Strict
});

app.UseSession();
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        o.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
        o.EnablePersistAuthorization();
        o.DefaultModelsExpandDepth(-1); // Hide schemas by default
    });
}


app.MapHub<School_API.Hubs.CommunicationHub>("/hubs/communication");
app.MapControllers();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var response = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(entry => new
            {
                name = entry.Key,
                status = entry.Value.Status.ToString(),
                description = entry.Value.Description,
                duration = entry.Value.Duration.ToString()
            }),
            totalDuration = report.TotalDuration.ToString()
        };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
});

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SchoolDbContext>();
    var encryptionService = scope.ServiceProvider.GetRequiredService<School.Utilities.Security.IEncryptionService>();

    dbContext.Database.Migrate();

    DbInitializer.Seed(dbContext, encryptionService);
}

app.Run();


