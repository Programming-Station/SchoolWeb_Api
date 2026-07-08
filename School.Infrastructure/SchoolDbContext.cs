using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using School.Domain.Auth;
using School.Domain;
using School.Domain.Student;
using School.Domain.FeeManagnment;
using School.Domain.School;
using School.Domain.AccessControl;
using School.Domain.Location;
using School.Domain.Hr;
using School.Domain.Hr.LeaveManagement;
using School.Domain.Hr.Timesheet;
using School.Domain.Hr.Attendance;
using School.Infrastructure.Interfaces;
using School.Domain.Email;
using School.Domain.Academic;
using School.Domain.Hr.Recruitment;
using School.Domain.Hr.Performance;
using School.Domain.Hr.Training;
using School.Domain.Hr.Assets;
using School.Domain.Payroll;
using School.Domain.Transport;

namespace School.Infrastructure
{
    public class SchoolDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly ITenantService _tenantService;
        public int? CurrentTenantId => _tenantService?.GetTenantId();

        public SchoolDbContext(DbContextOptions<SchoolDbContext> options, ITenantService tenantService = null) : base(options)
        {
            _tenantService = tenantService;
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
        public DbSet<Menu> Menus { get; set; } = null!;
        public DbSet<SubMenu> SubMenus { get; set; } = null!;
        public DbSet<Status> Statuses { get; set; } = null!;
        public DbSet<MenuPermession> MenuPermessions { get; set; } = null!;
        public DbSet<LoginHistory> LoginHistories { get; set; } = null!;
        public DbSet<Module> Modules { get; set; } = null!;
        public DbSet<ModulePermission> ModulePermissions { get; set; } = null!;
        public DbSet<CategoryModule> CategoryModules { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Class> Classes { get; set; } = null!;
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<State> States { get; set; } = null!;
        public DbSet<Country> Countries { get; set; } = null!;
        public DbSet<Affiliated> Affiliateds { get; set; } = null!;
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<StudentRegistration> StudentRegistrations { get; set; } = null!;
        public DbSet<SchoolRegistration> SchoolRegistrations { get; set; } = null!;
        public DbSet<AffiliationBoard> AffiliationBoards { get; set; } = null!;
        public DbSet<SchoolType> SchoolTypes { get; set; } = null!;
        public DbSet<SchoolMedium> SchoolMediums { get; set; } = null!;
        public DbSet<SchoolProfileSetting> SchoolProfileSettings { get; set; } = null!;
        public DbSet<SchoolSubscription> SchoolSubscriptions { get; set; } = null!;
        public DbSet<SchoolOwner> SchoolOwners { get; set; } = null!;
        public DbSet<StudentExperienceCertificate> StudentExperienceCertificates { get; set; } = null!;
        public DbSet<EducationalDetail> EducationalDetails { get; set; } = null!;
        public DbSet<AcademicYear> AcademicYears { get; set; } = null!;
        public DbSet<EmailServerSetting> EmailServerSettings { get; set; } = null!;
        public DbSet<EmailTemplate> EmailTemplates { get; set; } = null!;
        public DbSet<EmailBranding> EmailBrandings { get; set; } = null!;
        public DbSet<EmailLog> EmailLogs { get; set; } = null!;
        
        public DbSet<Subject> Subjects { get; set; } = null!;
        public DbSet<Exam> Exams { get; set; } = null!;
        public DbSet<ExamResult> ExamResults { get; set; } = null!;
        public DbSet<TimetableSlot> TimetableSlots { get; set; } = null!;

        public DbSet<Event> Events { get; set; } = null!;
        public DbSet<Faculty> Faculties { get; set; } = null!;
        public DbSet<Department> Departments { get; set; } = null!;
        public DbSet<FeeType> FeeTypes { get; set; } = null!;

        // HR Module
        public DbSet<Designation> Designations { get; set; } = null!;
        public DbSet<Specialization> Specializations { get; set; } = null!;
        public DbSet<BloodGroupMaster> BloodGroupMasters { get; set; } = null!;
        public DbSet<ReligionMaster> ReligionMasters { get; set; } = null!;
        public DbSet<QualificationMaster> QualificationMasters { get; set; } = null!;
        public DbSet<EmployeeCategory> EmployeeCategories { get; set; } = null!;
        public DbSet<EmployeeType> EmployeeTypes { get; set; } = null!;
        public DbSet<EmploymentStatus> EmploymentStatuses { get; set; } = null!;
        public DbSet<SalaryGrade> SalaryGrades { get; set; } = null!;
        public DbSet<ShiftMaster> ShiftMasters { get; set; } = null!;
        public DbSet<HolidayMaster> HolidayMasters { get; set; } = null!;
        public DbSet<WeekOff> WeekOffs { get; set; } = null!;
        public DbSet<NoticePeriod> NoticePeriods { get; set; } = null!;
        public DbSet<LeaveType> LeaveTypes { get; set; } = null!;
        public DbSet<LeaveSetting> LeaveSettings { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<EmployeeDocument> EmployeeDocuments { get; set; } = null!;
        public DbSet<EmployeeBankDetail> EmployeeBankDetails { get; set; } = null!;
        public DbSet<EmployeeEducation> EmployeeEducations { get; set; } = null!;
        public DbSet<EmployeeExperience> EmployeeExperiences { get; set; } = null!;
        public DbSet<EmployeeSalaryDetail> EmployeeSalaryDetails { get; set; } = null!;
        public DbSet<EmployeeDetail> EmployeeDetails { get; set; } = null!;
        public DbSet<LeaveRequest> LeaveRequests { get; set; } = null!;
        public DbSet<LeaveBalance> LeaveBalances { get; set; } = null!;
        public DbSet<Attendance> Attendances { get; set; } = null!;
        public DbSet<AttendanceLog> AttendanceLogs { get; set; } = null!;
        public DbSet<Timesheet> Timesheets { get; set; } = null!;

        // Expanded HRMS entities
        public DbSet<JobPosting> JobPostings { get; set; } = null!;
        public DbSet<Candidate> Candidates { get; set; } = null!;
        public DbSet<JobApplication> JobApplications { get; set; } = null!;
        public DbSet<PerformanceReview> PerformanceReviews { get; set; } = null!;
        public DbSet<KpiMetric> KpiMetrics { get; set; } = null!;
        public DbSet<TrainingProgram> TrainingPrograms { get; set; } = null!;
        public DbSet<TrainingEnrollment> TrainingEnrollments { get; set; } = null!;
        public DbSet<SchoolAsset> SchoolAssets { get; set; } = null!;
        public DbSet<AssetAssignment> AssetAssignments { get; set; } = null!;
        public DbSet<TimesheetEntry> TimesheetEntries { get; set; } = null!;

        // Payroll
        public DbSet<SalaryComponent> SalaryComponents { get; set; } = null!;

        // Transport
        public DbSet<TransportRoute> TransportRoutes { get; set; } = null!;
        public DbSet<Vehicle> Vehicles { get; set; } = null!;

        // Admission Module
        public DbSet<Campus> Campuses { get; set; } = null!;
        public DbSet<EducationLevel> EducationLevels { get; set; } = null!;
        public DbSet<YearSemester> YearSemesters { get; set; } = null!;
        public DbSet<Program> Programs { get; set; } = null!;
        public DbSet<Branch> Branches { get; set; } = null!;
        public DbSet<Batch> Batches { get; set; } = null!;
        public DbSet<AdmissionFormConfig> AdmissionFormConfigs { get; set; } = null!;
        public DbSet<AdmissionRule> AdmissionRules { get; set; } = null!;
        public DbSet<FeeStructure> FeeStructures { get; set; } = null!;
        public DbSet<FeeStructureItem> FeeStructureItems { get; set; } = null!;
        public DbSet<AdmissionApplication> AdmissionApplications { get; set; } = null!;
        public DbSet<AdmissionAuditLog> AdmissionAuditLogs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "Users");
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.UserName).IsRequired().HasMaxLength(256);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(256);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasIndex(e => e.EmployeeCode).IsUnique();
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasIndex(e => e.EnrollmentNumber).IsUnique();
            });

            modelBuilder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Roles");
            });

            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });

            modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });

            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });

            modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });

            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("RefreshTokens");
                entity.HasKey(rt => rt.Id);
                entity.HasOne(rt => rt.User)
                    .WithMany(u => u.RefreshTokens)
                    .HasForeignKey(rt => rt.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<LoginHistory>(entity =>
            {
                entity.ToTable("LoginHistories");
                entity.HasKey(lh => lh.Id);
                entity.HasIndex(lh => lh.UserId);
                entity.HasIndex(lh => lh.LoginTime);
                entity.HasIndex(lh => new { lh.UserId, lh.LoginTime });
                entity.HasIndex(lh => lh.SessionId);
                entity.HasIndex(lh => lh.IsActive);

                entity.Property(lh => lh.UserId).IsRequired();
                entity.Property(lh => lh.LoginTime).IsRequired();
                entity.Property(lh => lh.LoginMethod).IsRequired().HasMaxLength(50);
                entity.Property(lh => lh.SessionId).HasMaxLength(500);
                entity.Property(lh => lh.LogoutReason).HasMaxLength(100);
                entity.Property(lh => lh.IpAddress).HasMaxLength(50);
                entity.Property(lh => lh.DeviceType).HasMaxLength(50);
                entity.Property(lh => lh.Browser).HasMaxLength(100);
                entity.Property(lh => lh.BrowserVersion).HasMaxLength(50);
                entity.Property(lh => lh.OperatingSystem).HasMaxLength(100);
                entity.Property(lh => lh.OperatingSystemVersion).HasMaxLength(50);
                entity.Property(lh => lh.DeviceModel).HasMaxLength(200);
                entity.Property(lh => lh.UserAgent).HasMaxLength(1000);
                entity.Property(lh => lh.Country).HasMaxLength(100);
                entity.Property(lh => lh.City).HasMaxLength(100);
                entity.Property(lh => lh.Region).HasMaxLength(100);
                entity.Property(lh => lh.FailureReason).HasMaxLength(500);
                entity.Property(lh => lh.SecurityNotes).HasMaxLength(1000);

                entity.HasOne(lh => lh.User)
                    .WithMany()
                    .HasForeignKey(lh => lh.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Disable cascade delete globally to prevent multiple cascade paths (we use soft delete anyway)
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
            {
                // Leave Identity related cascade deletes intact
                if (fk.DeclaringEntityType.ClrType.Namespace != null &&
                    !fk.DeclaringEntityType.ClrType.Namespace.Contains("Microsoft.AspNetCore.Identity") &&
                    fk.DeclaringEntityType.ClrType != typeof(RefreshToken) &&
                    fk.DeclaringEntityType.ClrType != typeof(LoginHistory))
                {
                    fk.DeleteBehavior = DeleteBehavior.NoAction;
                }
            }

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(School.Domain.BaseEntity.IDeleteEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(SchoolDbContext)
                        .GetMethod(nameof(SetGlobalQueryFilterForSoftDelete))
                        ?.MakeGenericMethod(entityType.ClrType);
                    method?.Invoke(this, new object[] { modelBuilder });
                }
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetTenantId();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            SetTenantId();
            return base.SaveChanges();
        }

        private void SetTenantId()
        {
            var tenantId = CurrentTenantId;
            if (!tenantId.HasValue) return;

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added)
                {
                    if (entry.Entity is BaseEntity.ITenantEntity tenantEntity)
                    {
                        if (tenantEntity.SchoolRegistrationId == 0)
                        {
                            tenantEntity.SchoolRegistrationId = tenantId.Value;
                        }
                    }
                }
            }
        }

        private void SetGlobalQueryFilterForSoftDelete<T>(ModelBuilder builder) where T : class, BaseEntity.IDeleteEntity
        {
            if (typeof(BaseEntity.ITenantEntity).IsAssignableFrom(typeof(T)))
            {
                builder.Entity<T>().HasIndex("SchoolRegistrationId", "IsDeleted");
                builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted && (CurrentTenantId == null || ((BaseEntity.ITenantEntity)e).SchoolRegistrationId == CurrentTenantId));
            }
            else
            {
                builder.Entity<T>().HasIndex("IsDeleted");
                builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
            }
        }





    }
}




