using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using School.Domain.Auth;
using School.Domain;
using School.Domain.Website;
using School.Domain.Student;
using School.Domain.FeeManagnment;

namespace School.Infrastructure
{
    public class SchoolDbContext : IdentityDbContext<ApplicationUser>
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options)
        {
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
        public DbSet<Affiliated> Affiliateds { get; set; } = null!;
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<StudentRegistration> StudentRegistrations { get; set; } = null!;
        public DbSet<StudentExperienceCertificate> StudentExperienceCertificates { get; set; } = null!;
        public DbSet<EducationalDetail> EducationalDetails { get; set; } = null!;
        public DbSet<AcademicYear> AcademicYears { get; set; } = null!;

        // Website Management Entities
        public DbSet<SliderImage> SliderImages { get; set; } = null!;
        public DbSet<HeroSection> HeroSections { get; set; } = null!;
        public DbSet<NoticeBar> NoticeBars { get; set; } = null!;
        public DbSet<WelcomeSection> WelcomeSections { get; set; } = null!;
        public DbSet<AboutSection> AboutSections { get; set; } = null!;
        public DbSet<AboutPage> AboutPages { get; set; } = null!;
        public DbSet<ContactInfo> ContactInfos { get; set; } = null!;
        public DbSet<GalleryImage> GalleryImages { get; set; } = null!;
        public DbSet<Achievement> Achievements { get; set; } = null!;
        public DbSet<TeamMember> TeamMembers { get; set; } = null!;
        public DbSet<Enquiry> Enquiries { get; set; } = null!;
        public DbSet<SchoolRegistration> SchoolRegistrations { get; set; } = null!;
        public DbSet<Event> Events { get; set; } = null!;
        public DbSet<Teacher> Teachers { get; set; } = null!;
        public DbSet<Faculty> Faculties { get; set; } = null!;
        public DbSet<Department> Departments { get; set; } = null!;
        public DbSet<FeeType> FeeTypes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure ApplicationUser
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "Users");
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.UserName).IsRequired().HasMaxLength(256);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(256);
            });

            // Configure Identity tables
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

            // Configure Teacher-ApplicationUser relationship
            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.HasOne(t => t.User)
                    .WithMany()
                    .HasForeignKey(t => t.UserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();
            });
            modelBuilder.Entity<Teacher>()
                .HasOne(t => t.State)
                .WithMany()
                .HasForeignKey(t => t.StateId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Teacher>()
                .HasOne(t => t.City)
                .WithMany()
                .HasForeignKey(t => t.CityId)
                .OnDelete(DeleteBehavior.Cascade);
            // Configure RefreshToken
            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("RefreshTokens");
                entity.HasKey(rt => rt.Id);
                entity.HasOne(rt => rt.User)
                    .WithMany(u => u.RefreshTokens)
                    .HasForeignKey(rt => rt.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure LoginHistory
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

            // Configure Website Entities
            //ConfigureWebsiteEntities(modelBuilder);


            // Seed Data - Using DbInitializer.Seed() in Program.cs instead
            // SeedData(modelBuilder);

            // Apply global query filters for soft delete
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

        private void SetGlobalQueryFilterForSoftDelete<T>(ModelBuilder builder) where T : class, BaseEntity.IDeleteEntity
        {
            builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
        }

        // Seed Data method - Using DbInitializer.Seed() in Program.cs instead
        // private void SeedData(ModelBuilder modelBuilder)
        // {
        //     // Seed Roles
        //     List<IdentityRole> roles = DefaultRoles.IdentityRoleList();
        //     modelBuilder.Entity<IdentityRole>().HasData(roles);
        //
        //     // Seed Status
        //     List<Status> statuses = DefaultStatusList.StatusList();
        //     modelBuilder.Entity<Status>().HasData(statuses);
        //
        //     // Seed Users
        //     List<ApplicationUser> users = DefaultUser.IdentityBasicUserList();
        //     modelBuilder.Entity<ApplicationUser>().HasData(users);
        //
        //     // Map User to Role
        //     var identityUserRoles = MappingUserRole.IdentityUserRoleList();
        //     modelBuilder.Entity<IdentityUserRole<string>>().HasData(identityUserRoles);
        // }

        //public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        //{
        //    UpdateAuditFields();
        //    return base.SaveChangesAsync(cancellationToken);
        //}

        //private void UpdateAuditFields()
        //{
        //    var entries = ChangeTracker.Entries()
        //        .Where(e => e.Entity is B2B.Domain.BaseEntity.IAuditEntity && 
        //                   (e.State == EntityState.Added || e.State == EntityState.Modified));

        //    foreach (var entry in entries)
        //    {
        //        var entity = (B2B.Domain.BaseEntity.IAuditEntity)entry.Entity;
        //        var now = DateTime.Now;

        //        if (entry.State == EntityState.Added)
        //        {
        //            entity.CreatedDate = now;
        //        }
        //        else if (entry.State == EntityState.Modified)
        //        {
        //            entity.UpdatedDate = now;
        //        }
        //    }
        //}
    }
}

