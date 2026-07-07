using Microsoft.EntityFrameworkCore;
using AutoMapper;
using School.Domain.Auth;
using School.Infrastructure;
using School.Models.Configuration;
using School.Services.Mapping;
using School_API;
using School_API.Middleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
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
   .AddCorsPolicy(builder.Configuration);

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
});
builder.Services.AddDistributedMemoryCache();

builder.Services.AddAutoMapper(cfg => { cfg.AddProfile(new AutoMapperProfile()); });
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


var app = builder.Build();

app.UseCustomExceptionHandler();

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
 

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SchoolDbContext>();
    var encryptionService = scope.ServiceProvider.GetRequiredService<School.Utilities.Security.IEncryptionService>();

    dbContext.Database.Migrate();

    DbInitializer.Seed(dbContext, encryptionService);
}

app.Run();


