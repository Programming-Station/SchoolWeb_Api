using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using School.Infrastructure.JWTAuthenticationManager.Interfaces;
using School.Infrastructure.JWTAuthenticationManager;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.Repositories;
using School.Infrastructure.UnitOfWork.Interfaces;
using School.Services.Interfaces;
using School.Services;
using School.Infrastructure.UnitOfWork;
using School_API;
using School.Infrastructure;
using School.Models.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using School_API.Common.Interface;
using School_API.Common;
using School_API.Middleware;

namespace School_API
{
    public static class ServiceCollection
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<SchoolDbContext>(options =>
                options.UseSqlServer(configuration?.GetConnectionString("SchoolConnection") ?? string.Empty));

            services.AddScoped<Func<SchoolDbContext>>((provider) => () => provider.GetService<SchoolDbContext>()!);
            services.AddScoped<DbFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork1>();
            services.Configure<AppSettings>(configuration?.GetSection("AppSettings")!);

            return services;
        }

        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettings = configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
            var key = Encoding.UTF8.GetBytes(appSettings?.SecretKey ?? string.Empty);

            if (key.Length < 32)
            {
                throw new ArgumentException("SecretKey must be at least 32 characters long");
            }

            services.AddAuthentication(o =>
            {
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = appSettings?.Issuer,
                    ValidAudience = appSettings?.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero // Remove clock skew for more accurate token validation
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Append("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        return Task.CompletedTask;
                    }
                };
            })
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null);

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
            .AddScoped(typeof(IRepository<>), typeof(Repository<>))
            .AddScoped(typeof(IReadOnlyRepository<>), typeof(ReadOnlyRepository<>))
            .AddTransient<IAccountRepository, AccountRepository>()
            .AddTransient<IMenuRepository, MenuRepository>()
            .AddTransient<IModuleRepository, ModuleRepository>()
            .AddTransient<ICategoryModuleRepository, CategoryModuleRepository>()
            .AddTransient<ICourseRepository, CourseRepository>()
            .AddTransient<IClassRepository, ClassRepository>()
            .AddTransient<IStudentRepository, StudentRepository>()
         
            .AddTransient<ICityRepository, CityRepository>()
            .AddTransient<IStateRepository, StateRepository>()
            .AddTransient<IAffiliatedRepository, AffiliatedRepository>()
            .AddScoped<IStudentRegistrationRepository, StudentRegistrationRepository>()
            .AddTransient<IAcademicYearRepository, AcademicYearRepository>()
       
            .AddTransient<IEventRepository, EventRepository>()
            .AddTransient<IDashboardRepository, DashboardRepository>()
            .AddTransient<IFacultyRepository, FacultyRepository>()
            .AddTransient<IDepartmentRepository, DepartmentRepository>()
            .AddTransient<IFeeTypeRepository, FeeTypeRepository>()
            .AddTransient<ISchoolRepository, SchoolRepository>()
            .AddTransient<ISchoolProfileSettingRepository, SchoolProfileSettingRepository>()
            .AddTransient<IAffiliationBoardRepository, AffiliationBoardRepository>()
            .AddTransient<ISchoolTypeRepository, SchoolTypeRepository>()
            .AddTransient<ISchoolMediumRepository, SchoolMediumRepository>()
            .AddTransient<ISchoolSubscriptionRepository, SchoolSubscriptionRepository>()
            .AddTransient<ISchoolOwnerRepository, SchoolOwnerRepository>()
            ;
        }
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
            .AddScoped<ICurrentUserService, CurrentUserService>()
            .AddScoped<ITenantService, TenantService>()
            .AddHttpContextAccessor()
            .AddScoped<IJWTAuthenticationManager, JWTAuthenticationManager>()
            .AddScoped<IAccountService, AccountService>()
            .AddScoped<IMenuService, MenuService>()
            .AddScoped<IModuleService, ModuleService>()
            .AddScoped<ICategoryModuleService, CategoryModuleService>()
            .AddScoped<ICourseService, CourseService>()
            .AddScoped<IClassService, ClassService>()
            .AddScoped<IStudentService, StudentService>()
           
            .AddScoped<IImageService, ImageService>()
            .AddScoped<ICityService, CityService>()
            .AddScoped<IStateService, StateService>()
            .AddScoped<IAffiliatedService, AffiliatedService>()
            .AddScoped<IMasterService, MasterService>()
            .AddScoped<IStudentRegistrationService, StudentRegistrationService>()
            .AddScoped<IAcademicYearService, AcademicYearService>()
           
            .AddScoped<IEventService, EventService>()
            .AddScoped<IDashboardService, DashboardService>()
            .AddScoped<IFacultyService, FacultyService>()
            .AddScoped<IDepartmentService, DepartmentService>()
            .AddScoped<IPdfCertificateService, PdfCertificateService>()
            .AddScoped<IFeeTypeService, FeeTypeService>()
            .AddScoped<School.Services.School.ISchoolServices.ISchoolService, School.Services.School.SchoolService>()
            .AddScoped<School.Services.School.ISchoolServices.ISchoolProfileSettingService, School.Services.School.SchoolProfileSettingService>()
            .AddScoped<School.Services.School.ISchoolServices.IAffiliationBoardService, School.Services.School.AffiliationBoardService>()
            .AddScoped<School.Services.School.ISchoolServices.ISchoolTypeService, School.Services.School.SchoolTypeService>()
            .AddScoped<School.Services.School.ISchoolServices.ISchoolMediumService, School.Services.School.SchoolMediumService>()
            .AddScoped<School.Services.School.ISchoolServices.ISchoolSubscriptionService, School.Services.School.SchoolSubscriptionService>()
            .AddScoped<School.Services.School.ISchoolServices.ISchoolOwnerService, School.Services.School.SchoolOwnerService>()
            ;
        }
        public static IServiceCollection AddSessionWithOptions(this IServiceCollection services)
        {
            return services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(2);
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Strict;
            });
        }
        public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();

            if (appSettings?.EnableCors == true && appSettings.AllowedOrigins?.Any() == true)
            {
                services.AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy", builder =>
                    {
                        builder.WithOrigins(appSettings.AllowedOrigins.ToArray())
                               .AllowAnyHeader()
                               .AllowAnyMethod()
                               .AllowCredentials()
                               .WithExposedHeaders("X-Total-Count", "X-Page-Number", "X-Page-Size");
                    });
                });
            }
            else
            {
                services.AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy", builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
                });
            }

            return services;
        }
    }
}

