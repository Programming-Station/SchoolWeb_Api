using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using School.Infrastructure.JWTAuthenticationManager.Interfaces;
using School.Infrastructure.JWTAuthenticationManager;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.Repositories;
using School.Infrastructure.UnitOfWork.Interfaces;
using School.Infrastructure.Interfaces;
using School.Services.Interfaces;
using School.Services;
using School_API.Filters;
using School.Infrastructure.UnitOfWork;
using School_API;
using School.Infrastructure;
using School.Models.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using School_API.Common.Interface;
using School_API.Common;
using School_API.Middleware;
using School.Infrastructure.Repositories.AccessControl;
using School.Services.AccessControl;
using School.Services.AccessControl.Interfaces;
using School.Infrastructure.Repositories.Hr.Attendance;
using School.Infrastructure.Repositories.Hr.LeaveManagement;
using School.Infrastructure.Repositories.Hr.Timesheet;
using School.Infrastructure.Repositories.School;
using School.Services.Hr.Attendance;
using School.Services.Hr.LeaveManagement;
using School.Services.Hr.Timesheet;
using School.Services.Interfaces.Hr.LeaveManagement;
using School.Services.Interfaces.Hr.Attendance;
using School.Services.Interfaces.Hr.Timesheet;
using School.Services.Hr;
using School.Services.Library;
using School.Services.Interfaces.Payroll;
using School.Services.Interfaces.Academic;
using School.Infrastructure.Repositories.Email;
using School.Services.Email;
using School.Services.Interfaces.Email;
using School.Services.Location;
using School.Services.Hostel;
using School.Services.Finance;
using School.Services.Inventory;
using School.Services.Communication;
using School.Services.Analytics;
using School.Services.Administration;
using School.Services.AI;


namespace School_API
{
    public static class ServiceCollection
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SchoolDbContext>(options =>
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
            .AddTransient<ISubMenuRepository, SubMenuRepository>()
            .AddTransient<IMenuPermessionRepository, MenuPermessionRepository>()
            .AddTransient<IModuleRepository, ModuleRepository>()
            .AddTransient<IModulePermissionRepository, ModulePermissionRepository>()
            .AddTransient<ICategoryModuleRepository, CategoryModuleRepository>()
            .AddTransient<ICourseRepository, CourseRepository>()
            .AddTransient<IClassRepository, ClassRepository>()
            .AddTransient<IStudentRepository, StudentRepository>()
         
            .AddTransient<ICityRepository, CityRepository>()
            .AddTransient<IStateRepository, StateRepository>()
            .AddTransient<IAffiliatedRepository, AffiliatedRepository>()
            .AddTransient<IAcademicYearRepository, AcademicYearRepository>()
       
            .AddTransient<IEventRepository, EventRepository>()
            .AddTransient<IDashboardRepository, DashboardRepository>()
            .AddTransient<ISuperAdminDashboardRepository, SuperAdminDashboardRepository>()
            .AddTransient<IEmployeeDashboardRepository, EmployeeDashboardRepository>()
            .AddTransient<IStudentDashboardRepository, StudentDashboardRepository>()
            .AddTransient<IFacultyRepository, FacultyRepository>()
            .AddTransient<IDepartmentRepository, DepartmentRepository>()
            .AddTransient<IFeeTypeRepository, FeeTypeRepository>()
            .AddTransient<ISchoolRepository, SchoolRepository>()
            .AddTransient<IOrganizationProfileRepository, OrganizationProfileRepository>()
            .AddTransient<ISchoolProfileSettingRepository, SchoolProfileSettingRepository>()
            .AddTransient<IAffiliationBoardRepository, AffiliationBoardRepository>()
            .AddTransient<ISchoolTypeRepository, SchoolTypeRepository>()
            .AddTransient<ISchoolMediumRepository, SchoolMediumRepository>()
            .AddTransient<ISchoolSubscriptionRepository, SchoolSubscriptionRepository>()
            .AddTransient<ISchoolOwnerRepository, SchoolOwnerRepository>()
            .AddTransient<IEmployeeRepository, EmployeeRepository>()
            // Email Module Repositories
            .AddTransient<IEmailServerSettingRepository, EmailServerSettingRepository>()
            .AddTransient<IEmailTemplateRepository, EmailTemplateRepository>()
            .AddTransient<IEmailBrandingRepository, EmailBrandingRepository>()
            .AddTransient<IEmailLogRepository, EmailLogRepository>()
            // HR Master Generic
            .AddScoped(typeof(IHrMasterService<>), typeof(School.Services.Hr.HrMasterService<>))
            
            // Leave Management

            .AddTransient<ILeaveBalanceRepository, School.Infrastructure.Repositories.Hr.LeaveManagement.LeaveBalanceRepository>()
            .AddTransient<ILeaveRequestRepository, School.Infrastructure.Repositories.Hr.LeaveManagement.LeaveRequestRepository>()
            .AddTransient<ILeaveSettingRepository, School.Infrastructure.Repositories.Hr.LeaveManagement.LeaveSettingRepository>()
            .AddTransient<ILeaveTypeRepository, School.Infrastructure.Repositories.Hr.LeaveManagement.LeaveTypeRepository>()
            
            // Attendance
            .AddTransient<IAttendanceRepository, School.Infrastructure.Repositories.Hr.Attendance.AttendanceRepository>()

            .AddTransient<IShiftMasterRepository, School.Infrastructure.Repositories.Hr.Attendance.ShiftMasterRepository>()
            .AddTransient<IWeekOffRepository, School.Infrastructure.Repositories.Hr.Attendance.WeekOffRepository>()
            
            // Timesheet
            .AddTransient<ITimesheetRepository, School.Infrastructure.Repositories.Hr.Timesheet.TimesheetRepository>()
            .AddTransient<IHrmsExpansionRepository, HrmsExpansionRepository>()

            // Academic Module
            .AddTransient<ISubjectEnrollmentRepository, SubjectEnrollmentRepository>()
            .AddTransient<IStudentAttendanceRepository, StudentAttendanceRepository>()
            // Academic Extended (4.3-4.7)
            .AddTransient<ITimetablePeriodRepository, TimetablePeriodRepository>()
            .AddTransient<IHomeworkRepository, HomeworkRepository>()
            .AddTransient<IAssignmentRepository, AssignmentRepository>()
            .AddTransient<IOnlineClassRepository, OnlineClassRepository>()
            .AddTransient<ISyllabusRepository, SyllabusRepository>()
            // Phase 5 – Examination
            .AddTransient<IExamScheduleRepository, ExamScheduleRepository>()
            .AddTransient<IGradeConfigRepository, GradeConfigRepository>()
            .AddTransient<IReportCardRepository, ReportCardRepository>()
            .AddTransient<IStudentPromotionRepository, StudentPromotionRepository>()

            // Fee Collection
            .AddTransient<IFeeInstallmentRepository, FeeInstallmentRepository>()
            .AddTransient<IFeePaymentRepository, FeePaymentRepository>()
            .AddTransient<IFeeStructureRepository, FeeStructureRepository>()
            // Phase 6 – Fee Extended
            .AddTransient<IFeeFineRepository, FeeFineRepository>()
            .AddTransient<IStudentScholarshipRepository, StudentScholarshipRepository>()
            .AddTransient<IFeeRefundRepository, FeeRefundRepository>()
            .AddTransient<IFineRuleRepository, FineRuleRepository>()
            // Transport Module
            .AddTransient<IVehicleRepository, VehicleRepository>()
            .AddTransient<ITransportRouteRepository, TransportRouteRepository>()
            .AddTransient<ITransportAllocationRepository, TransportAllocationRepository>()
            // Parent Portal
            .AddTransient<IParentRepository, ParentRepository>()
            
            // Admission Module Repositories
            .AddTransient<ICampusRepository, CampusRepository>()
            .AddTransient<IEducationLevelRepository, EducationLevelRepository>()
            .AddTransient<IProgramRepository, ProgramRepository>()
            .AddTransient<IBranchRepository, BranchRepository>()
            .AddTransient<IYearSemesterRepository, YearSemesterRepository>()
            .AddTransient<IBatchRepository, BatchRepository>()
            .AddTransient<IAdmissionFormConfigRepository, AdmissionFormConfigRepository>()
            .AddTransient<IAdmissionRuleRepository, AdmissionRuleRepository>()
            .AddTransient<IAdmissionApplicationRepository, AdmissionApplicationRepository>()
            // Library Module Repositories
            .AddTransient<IBookRepository, BookRepository>()
            .AddTransient<IBookIssueLogRepository, BookIssueLogRepository>()
            ;
        }
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();

            // Bind Encryption Config
            services.Configure<School.Utilities.Security.EncryptionConfig>(configuration.GetSection("EncryptionConfig"));
            services.AddScoped<School.Utilities.Security.IEncryptionService, School.Utilities.Security.AESEncryptionService>();

            // Email Background Queue Services
            services.AddSingleton<IEmailQueue, EmailQueue>();
            services.AddHostedService<EmailQueueProcessor>();
            services.AddHostedService<FineCalculationBackgroundJob>();
            services.AddHostedService<ScheduledReportService>();
            services.AddHostedService<WhatsAppQueueProcessor>();

            return services
            .AddSingleton<School.Infrastructure.Email.PlaceholderResolver>()
            .AddSingleton<School.Infrastructure.Email.ITemplateRenderer, School.Infrastructure.Email.EmailTemplateRenderer>()
            .AddScoped<School.Infrastructure.Email.SmtpEmailProvider>()
            .AddScoped<IEmailService, EmailService>()
            .AddScoped<IRdlcReportManager, RdlcReportManager>()
            .AddScoped<IMessageService, MessageService>()
            .AddScoped<IEmailServerSettingService, EmailServerSettingService>()
            .AddScoped<IEmailTemplateService, EmailTemplateService>()
            .AddScoped<IEmailBrandingService, EmailBrandingService>()
            .AddScoped<IEmailLogService, EmailLogService>()
            .AddScoped<ICurrentUserService, CurrentUserService>()
            .AddScoped<ITenantService, TenantService>()
            .AddScoped<IPermissionService, PermissionService>()
            .AddHttpContextAccessor()
            .AddScoped<IJWTAuthenticationManager, JWTAuthenticationManager>()
            .AddScoped<IAccountService, AccountService>()
            .AddScoped<IMenuService, MenuService>()
            .AddScoped<ISubMenuService, SubMenuService>()
            .AddScoped<IMenuPermessionService, MenuPermessionService>()
            .AddScoped<IModuleService, ModuleService>()
            .AddScoped<IModulePermissionService, ModulePermissionService>()
            .AddScoped<ICategoryModuleService, CategoryModuleService>()
            .AddScoped<ICourseService, CourseService>()
            .AddScoped<IClassService, ClassService>()
            .AddScoped<IStudentService, StudentService>()
            .AddScoped<IEmployeeService, EmployeeService>()
           
            .AddScoped<IImageService, ImageService>()
            .AddScoped<IDocumentService, School.Services.DocumentManagement.DocumentService>()
            .AddScoped<ICityService, CityService>()
            .AddScoped<IStateService, StateService>()
            .AddScoped<IAffiliatedService, AffiliatedService>()
            .AddScoped<IMasterService, MasterService>()
            .AddScoped<IAcademicYearService, AcademicYearService>()
           
            .AddScoped<IEventService, EventService>()
            .AddScoped<IDashboardService, DashboardService>()
            .AddScoped<ISuperAdminDashboardService, SuperAdminDashboardService>()
            .AddScoped<IEmployeeDashboardService, EmployeeDashboardService>()
            .AddScoped<IStudentDashboardService, StudentDashboardService>()
            .AddScoped<IFacultyService, FacultyService>()
            .AddScoped<global::School.Services.Interfaces.IDepartmentService, School.Services.DepartmentService>()
            .AddScoped<School.Services.Hr.IDepartmentService, School.Services.Hr.DepartmentService>()
            .AddScoped<IPdfCertificateService, PdfCertificateService>()
            .AddScoped<IRdlcCertificateService, RdlcCertificateService>()
            .AddScoped<IFeeTypeService, FeeTypeService>()
            .AddScoped<School.Services.School.ISchoolServices.ISchoolService, School.Services.School.SchoolService>()
            .AddSingleton<School.Services.School.ISchoolServices.IOrganizationCacheService, School.Services.School.OrganizationCacheService>()
            .AddScoped<School.Services.School.ISchoolServices.ITenantBrandingProvider, School.Services.School.TenantBrandingProvider>()
            .AddScoped<School.Services.School.ISchoolServices.IBrandingService, School.Services.School.BrandingService>()
            .AddScoped<School.Services.School.ISchoolServices.IThemeService, School.Services.School.ThemeService>()
            .AddScoped<School.Services.School.ISchoolServices.ILogoService, School.Services.School.LogoService>()
            .AddScoped<School.Services.School.ISchoolServices.ISignatureService, School.Services.School.SignatureService>()
            .AddScoped<School.Services.School.ISchoolServices.IWatermarkService, School.Services.School.WatermarkService>()
            .AddScoped<School.Services.School.ISchoolServices.IOrganizationProfileService, School.Services.School.OrganizationProfileService>()
            .AddScoped<School.Services.School.ISchoolServices.IReportBrandingService, School.Services.School.ReportBrandingService>()
            .AddScoped<School.Services.School.ISchoolServices.ISchoolProfileSettingService, School.Services.School.SchoolProfileSettingService>()
            .AddScoped<School.Services.School.ISchoolServices.IAffiliationBoardService, School.Services.School.AffiliationBoardService>()
            .AddScoped<School.Services.School.ISchoolServices.ISchoolTypeService, School.Services.School.SchoolTypeService>()
            .AddScoped<School.Services.School.ISchoolServices.ISchoolMediumService, School.Services.School.SchoolMediumService>()
            .AddScoped<School.Services.School.ISchoolServices.ISchoolSubscriptionService, School.Services.School.SchoolSubscriptionService>()
            .AddScoped<School.Services.School.ISchoolServices.ISchoolOwnerService, School.Services.School.SchoolOwnerService>()
            // Leave Management

            .AddScoped<ILeaveBalanceService, School.Services.Hr.LeaveManagement.LeaveBalanceService>()
            .AddScoped<ILeaveRequestService, School.Services.Hr.LeaveManagement.LeaveRequestService>()
            .AddScoped<ILeaveSettingService, School.Services.Hr.LeaveManagement.LeaveSettingService>()
            .AddScoped<ILeaveTypeService, School.Services.Hr.LeaveManagement.LeaveTypeService>()
            
            // Attendance
            .AddScoped<IAttendanceService, School.Services.Hr.Attendance.AttendanceService>()

            .AddScoped<IShiftMasterService, School.Services.Hr.Attendance.ShiftMasterService>()
            .AddScoped<IWeekOffService, School.Services.Hr.Attendance.WeekOffService>()
            
            // Timesheet
            .AddScoped<ITimesheetService, School.Services.Hr.Timesheet.TimesheetService>()
            .AddScoped<ITimesheetEntryService, School.Services.Hr.Timesheet.TimesheetEntryService>()

            // Library
            .AddScoped<ILibraryService, LibraryService>()

            // Payroll
            .AddScoped<ISalaryComponentService, School.Services.Payroll.SalaryComponentService>()
            .AddScoped<IPayrollRunService, School.Services.Payroll.PayrollRunService>()
            .AddScoped<IPayGroupService, School.Services.Payroll.PayGroupService>()
            .AddScoped<ISalaryStructureService, School.Services.Payroll.SalaryStructureService>()
            .AddScoped<IEmployeeSalaryAllocationService, School.Services.Payroll.EmployeeSalaryAllocationService>()
            .AddScoped<IEmployeeLoanService, School.Services.Payroll.EmployeeLoanService>()
            .AddScoped<ISalaryAdvanceService, School.Services.Payroll.SalaryAdvanceService>()
            .AddScoped<IEmployeeBonusService, School.Services.Payroll.EmployeeBonusService>()
            .AddScoped<IReimbursementClaimService, School.Services.Payroll.ReimbursementClaimService>()
            .AddScoped<ISalaryArrearService, School.Services.Payroll.SalaryArrearService>()
            .AddScoped<IStatutoryComplianceConfigService, School.Services.Payroll.StatutoryComplianceConfigService>()
            .AddScoped<IAccountingService, AccountingService>()
            .AddScoped<IProcurementService, ProcurementService>()
            .AddScoped<ICommunicationService, CommunicationService>()
            .AddScoped<IWhatsAppService, WhatsAppService>()
            .AddScoped<IWhatsAppQueueService, WhatsAppQueueService>()
            .AddScoped<IWhatsAppTemplateService, WhatsAppTemplateService>()
            .AddScoped<IWhatsAppWebhookService, WhatsAppWebhookService>()

            // Academic
            .AddScoped<School.Services.Interfaces.Academic.IExamService, School.Services.Academic.ExamService>()
            .AddScoped<IExamResultService, School.Services.Academic.ExamResultService>()
            .AddScoped<School.Services.Interfaces.Academic.ISubjectService, School.Services.Academic.SubjectService>()
            .AddScoped<School.Services.Interfaces.Academic.ITimetableSlotService, School.Services.Academic.TimetableSlotService>()
            .AddScoped<School.Services.Interfaces.Academic.ISubjectEnrollmentService, School.Services.Academic.SubjectEnrollmentService>()
            .AddScoped<School.Services.Interfaces.Academic.IStudentAttendanceService, School.Services.Academic.StudentAttendanceService>()
            // Academic Extended (4.3-4.7)
            .AddScoped<School.Services.Interfaces.Academic.ITimetableService, School.Services.Academic.TimetableService>()
            .AddScoped<School.Services.Interfaces.Academic.IHomeworkService, School.Services.Academic.HomeworkService>()
            .AddScoped<School.Services.Interfaces.Academic.IAssignmentService, School.Services.Academic.AssignmentService>()
            .AddScoped<School.Services.Interfaces.Academic.IOnlineClassService, School.Services.Academic.OnlineClassService>()
            .AddScoped<School.Services.Interfaces.Academic.ISyllabusService, School.Services.Academic.SyllabusService>()
            // Phase 5 – Examination
            .AddScoped<School.Services.Academic.IExamScheduleService, School.Services.Academic.ExamScheduleService>()
            .AddScoped<School.Services.Academic.IGradeConfigService, School.Services.Academic.GradeConfigService>()
            .AddScoped<School.Services.Academic.IReportCardService, School.Services.Academic.ReportCardService>()
            .AddScoped<School.Services.Academic.IStudentPromotionService, School.Services.Academic.StudentPromotionService>()
            .AddScoped<IHrmsExpansionService, HrmsExpansionService>()

            // Fee Collection
            .AddScoped<IFeeCollectionService, School.Services.FeeCollectionService>()
            // Phase 6 – Fee Extended
            .AddScoped<School.Services.Fee.IFeeFineService, School.Services.Fee.FeeFineService>()
            .AddScoped<School.Services.Fee.IScholarshipService, School.Services.Fee.ScholarshipService>()
            .AddScoped<School.Services.Fee.IFeeRefundService, School.Services.Fee.FeeRefundService>()
            .AddScoped<School.Services.Interfaces.IOnlinePaymentService, School.Services.Fee.OnlinePaymentService>()
            .AddScoped<School.Services.Interfaces.IReceiptService, School.Services.Fee.ReceiptService>()
            .AddScoped<School.Services.Interfaces.IFineCalculationService, School.Services.Fee.FineCalculationService>()
            .AddScoped<School.Services.Interfaces.IFeeReportService, School.Services.Fee.FeeReportService>()
            // Transport Module
            .AddScoped<ITransportService, School.Services.Transport.TransportService>()
            // Location Module
            .AddScoped<ICountryService, CountryService>()
            .AddScoped<IStateLocationService, StateLocationService>()
            .AddScoped<ICityLocationService, CityLocationService>()
            
            // Admission & Enrollment Services
            .AddScoped<IAdmissionService, AdmissionService>()
            .AddScoped<IEnrollmentService, EnrollmentService>()

            // Hostel Module
            .AddScoped<IHostelService, HostelService>()
            // Analytics, Administration & AI Modules
            .AddScoped<IAnalyticsService, AnalyticsService>()
            .AddScoped<IAdministrationService, AdministrationService>()
            .AddScoped<IAiService, AiService>()
            .AddScoped<IComplaintService, ComplaintService>()
            .AddScoped<IVisitorService, VisitorService>()
            .AddScoped<ICertificateService, CertificateService>()
            .AddScoped<INotificationLogService, NotificationLogService>()
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








