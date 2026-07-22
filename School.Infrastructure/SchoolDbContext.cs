using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using School.Domain;
using School.Domain.Academic;
using School.Domain.AccessControl;
using School.Domain.Administration;
// Duplicate removed
using School.Domain.AI;
using School.Domain.Analytics;
using School.Domain.Auth;
using School.Domain.Communication;
using School.Domain.Communication.Recipients;
using School.Domain.Email;
using School.Domain.Entities;
using School.Domain.FeeManagnment;
using School.Domain.Finance;
using School.Domain.Hostel;
using School.Domain.Hr;
using School.Domain.Hr.Assets;
using School.Domain.Hr.Attendance;
using School.Domain.Hr.LeaveManagement;
using School.Domain.Hr.Performance;
using School.Domain.Hr.Recruitment;
using School.Domain.Hr.Timesheet;
using School.Domain.Hr.Training;
using School.Domain.Inventory;
using School.Domain.Library;
using School.Domain.Location;
using School.Domain.Payroll;
using School.Domain.Reporting;
using School.Domain.School;
using School.Domain.Student;
using School.Domain.Transport;
using School.Infrastructure.Interfaces;
using ReportingTemplate = School.Domain.Reporting.ReportTemplate;

namespace School.Infrastructure
{
    public class SchoolDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly ITenantService _tenantService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public int? CurrentTenantId => _tenantService?.GetTenantId();

        public SchoolDbContext(
            DbContextOptions<SchoolDbContext> options,
            ITenantService tenantService = null,
            IHttpContextAccessor httpContextAccessor = null) : base(options)
        {
            _tenantService = tenantService;
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<AutoNumberSetting> AutoNumberSettings { get; set; } = null!;
        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
        public DbSet<AuditLog> AuditLogs { get; set; } = null!;
        public DbSet<Menu> Menus { get; set; } = null!;
        public DbSet<SubMenu> SubMenus { get; set; } = null!;
        public DbSet<Status> Statuses { get; set; } = null!;
        public DbSet<MenuPermession> MenuPermessions { get; set; } = null!;
        public DbSet<LoginHistory> LoginHistories { get; set; } = null!;
        public DbSet<Module> Modules { get; set; } = null!;
        public DbSet<ModulePermission> ModulePermissions { get; set; } = null!;
        public DbSet<Permission> Permissions { get; set; } = null!;
        public DbSet<RolePermission> RolePermissions { get; set; } = null!;
        public DbSet<CategoryModule> CategoryModules { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Class> Classes { get; set; } = null!;
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<State> States { get; set; } = null!;
        public DbSet<Country> Countries { get; set; } = null!;
        public DbSet<Affiliated> Affiliateds { get; set; } = null!;
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<SchoolRegistration> SchoolRegistrations { get; set; } = null!;
        public DbSet<OrganizationProfile> OrganizationProfiles { get; set; } = null!;
        public DbSet<OrganizationProfileAudit> OrganizationProfileAudits { get; set; } = null!;
        public DbSet<AffiliationBoard> AffiliationBoards { get; set; } = null!;
        public DbSet<SchoolType> SchoolTypes { get; set; } = null!;
        public DbSet<SchoolMedium> SchoolMediums { get; set; } = null!;
        public DbSet<SchoolProfileSetting> SchoolProfileSettings { get; set; } = null!;
        public DbSet<SchoolSubscription> SchoolSubscriptions { get; set; } = null!;
        public DbSet<SchoolOwner> SchoolOwners { get; set; } = null!;
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
        public DbSet<PayrollRun> PayrollRuns { get; set; } = null!;
        public DbSet<PayGroup> PayGroups { get; set; } = null!;
        public DbSet<SalaryStructure> SalaryStructures { get; set; } = null!;
        public DbSet<SalaryStructureItem> SalaryStructureItems { get; set; } = null!;
        public DbSet<EmployeeSalaryAllocation> EmployeeSalaryAllocations { get; set; } = null!;
        public DbSet<EmployeeLoan> EmployeeLoans { get; set; } = null!;
        public DbSet<LoanRepaymentSchedule> LoanRepaymentSchedules { get; set; } = null!;
        public DbSet<SalaryAdvance> SalaryAdvances { get; set; } = null!;
        public DbSet<EmployeeBonus> EmployeeBonuses { get; set; } = null!;
        public DbSet<ReimbursementClaim> ReimbursementClaims { get; set; } = null!;
        public DbSet<SalaryArrear> SalaryArrears { get; set; } = null!;
        public DbSet<StatutoryComplianceConfig> StatutoryComplianceConfigs { get; set; } = null!;
        public DbSet<PayrollRunDetail> PayrollRunDetails { get; set; } = null!;

        // Transport
        public DbSet<TransportRoute> TransportRoutes { get; set; } = null!;
        public DbSet<Vehicle> Vehicles { get; set; } = null!;
        public DbSet<TransportAllocation> TransportAllocations { get; set; } = null!;
        public DbSet<TransportStop> TransportStops { get; set; } = null!;
        public DbSet<RouteStopMapping> RouteStopMappings { get; set; } = null!;
        public DbSet<Conductor> Conductors { get; set; } = null!;
        public DbSet<RouteAssignment> RouteAssignments { get; set; } = null!;
        public DbSet<TransportTrip> TransportTrips { get; set; } = null!;
        public DbSet<RfidScanLog> RfidScanLogs { get; set; } = null!;
        public DbSet<FuelLog> FuelLogs { get; set; } = null!;
        public DbSet<VehicleMaintenance> VehicleMaintenances { get; set; } = null!;
        public DbSet<VehicleIncident> VehicleIncidents { get; set; } = null!;
        public DbSet<TransportInventory> TransportInventories { get; set; } = null!;
        public DbSet<TransportGateLog> TransportGateLogs { get; set; } = null!;

        // Library — Core
        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<BookIssueLog> BookIssueLogs { get; set; } = null!;
        public DbSet<BookCategory> BookCategories { get; set; } = null!;
        public DbSet<BookAuthor> BookAuthors { get; set; } = null!;
        public DbSet<BookPublisher> BookPublishers { get; set; } = null!;
        public DbSet<BookVendor> BookVendors { get; set; } = null!;
        // Library — Members & Transactions
        public DbSet<LibraryMember> LibraryMembers { get; set; } = null!;
        public DbSet<BookReservation> BookReservations { get; set; } = null!;
        public DbSet<LibraryFineRule> LibraryFineRules { get; set; } = null!;
        // Library — Digital
        public DbSet<DigitalResource> DigitalResources { get; set; } = null!;

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
        public DbSet<ParentStudentMapping> ParentStudentMappings { get; set; } = null!;

        // Academic Module
        public DbSet<School.Domain.Academic.SubjectEnrollment> SubjectEnrollments { get; set; } = null!;
        public DbSet<School.Domain.Academic.StudentAttendance> StudentAttendances { get; set; } = null!;
        // 4.3 Timetable
        public DbSet<School.Domain.Academic.TimetablePeriod> TimetablePeriods { get; set; } = null!;
        // 4.4 Homework
        public DbSet<School.Domain.Academic.Homework> Homeworks { get; set; } = null!;
        public DbSet<School.Domain.Academic.HomeworkSubmission> HomeworkSubmissions { get; set; } = null!;
        // 4.5 Assignment
        public DbSet<School.Domain.Academic.Assignment> Assignments { get; set; } = null!;
        public DbSet<School.Domain.Academic.AssignmentSubmission> AssignmentSubmissions { get; set; } = null!;
        // 4.6 Online Classes
        public DbSet<School.Domain.Academic.OnlineClass> OnlineClasses { get; set; } = null!;
        // 4.7 Syllabus Tracking
        public DbSet<School.Domain.Academic.SyllabusChapter> SyllabusChapters { get; set; } = null!;
        public DbSet<School.Domain.Academic.LessonPlan> LessonPlans { get; set; } = null!;

        // Fee Collection Module
        public DbSet<School.Domain.FeeManagnment.FeeInstallment> FeeInstallments { get; set; } = null!;
        public DbSet<School.Domain.FeeManagnment.FeePayment> FeePayments { get; set; } = null!;
        // 6.4 Fine | 6.5 Scholarship | 6.6 Refund
        public DbSet<School.Domain.FeeManagnment.FeeFine> FeeFines { get; set; } = null!;
        public DbSet<School.Domain.FeeManagnment.StudentScholarship> StudentScholarships { get; set; } = null!;
        public DbSet<School.Domain.FeeManagnment.FeeRefund> FeeRefunds { get; set; } = null!;
        public DbSet<School.Domain.FeeManagnment.PaymentGateway> PaymentGateways { get; set; } = null!;
        public DbSet<School.Domain.FeeManagnment.OnlinePaymentOrder> OnlinePaymentOrders { get; set; } = null!;
        public DbSet<School.Domain.FeeManagnment.FineRule> FineRules { get; set; } = null!;

        // Phase 5 – Examination
        // 5.1 Exam Schedule
        public DbSet<School.Domain.Academic.ExamSchedule> ExamSchedules { get; set; } = null!;
        // 5.3 Grade Config
        public DbSet<School.Domain.Academic.GradeConfig> GradeConfigs { get; set; } = null!;
        // 5.4 Report Card
        public DbSet<School.Domain.Academic.ReportCard> ReportCards { get; set; } = null!;
        // 5.6 Promotion
        public DbSet<School.Domain.Academic.StudentPromotion> StudentPromotions { get; set; } = null!;

        // Hostel Module
        public DbSet<Hostel> Hostels { get; set; } = null!;
        public DbSet<Building> Buildings { get; set; } = null!;
        public DbSet<Floor> Floors { get; set; } = null!;
        public DbSet<RoomCategory> RoomCategories { get; set; } = null!;
        public DbSet<Room> Rooms { get; set; } = null!;
        public DbSet<Bed> Beds { get; set; } = null!;
        public DbSet<HostelWarden> HostelWardens { get; set; } = null!;
        public DbSet<HostelAdmission> HostelAdmissions { get; set; } = null!;
        public DbSet<RoomTransferHistory> RoomTransferHistories { get; set; } = null!;
        public DbSet<BedReservation> BedReservations { get; set; } = null!;
        public DbSet<HostelFeeAllocation> HostelFeeAllocations { get; set; } = null!;
        public DbSet<HostelFeePayment> HostelFeePayments { get; set; } = null!;
        public DbSet<MessMenu> MessMenus { get; set; } = null!;
        public DbSet<MealAttendance> MealAttendances { get; set; } = null!;
        public DbSet<HostelVisitor> HostelVisitors { get; set; } = null!;
        public DbSet<HostelGatePass> HostelGatePasses { get; set; } = null!;
        public DbSet<HostelAttendance> HostelAttendances { get; set; } = null!;
        public DbSet<HostelComplaint> HostelComplaints { get; set; } = null!;
        public DbSet<HostelMaintenance> HostelMaintenances { get; set; } = null!;
        public DbSet<LaundryTransaction> LaundryTransactions { get; set; } = null!;
        public DbSet<HostelInventory> HostelInventories { get; set; } = null!;
        public DbSet<HostelMedicalLog> HostelMedicalLogs { get; set; } = null!;
        public DbSet<HostelDiscipline> HostelDisciplines { get; set; } = null!;

        // Finance Schema
        public DbSet<CoaAccount> CoaAccounts { get; set; } = null!;
        public DbSet<JournalEntry> JournalEntries { get; set; } = null!;
        public DbSet<JournalEntryLine> JournalEntryLines { get; set; } = null!;
        public DbSet<CashBankTransaction> CashBankTransactions { get; set; } = null!;
        public DbSet<BudgetPlan> BudgetPlans { get; set; } = null!;
        public DbSet<TaxConfig> TaxConfigs { get; set; } = null!;
        public DbSet<FinancialYear> FinancialYears { get; set; } = null!;
        public DbSet<CostCenter> CostCenters { get; set; } = null!;
        public DbSet<ChequeBook> ChequeBooks { get; set; } = null!;

        // Inventory Schema
        public DbSet<ItemCategory> ItemCategories { get; set; } = null!;
        public DbSet<InventoryItem> InventoryItems { get; set; } = null!;
        public DbSet<Vendor> Vendors { get; set; } = null!;
        public DbSet<PurchaseRequisition> PurchaseRequisitions { get; set; } = null!;
        public DbSet<PurchaseRequisitionItem> PurchaseRequisitionItems { get; set; } = null!;
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; } = null!;
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; } = null!;
        public DbSet<GoodsReceiptNote> GoodsReceiptNotes { get; set; } = null!;
        public DbSet<GoodsReceiptNoteItem> GoodsReceiptNoteItems { get; set; } = null!;
        public DbSet<StockTransaction> StockTransactions { get; set; } = null!;
        public DbSet<AssetDepreciationLog> AssetDepreciationLogs { get; set; } = null!;
        public DbSet<AssetMaintenanceLog> AssetMaintenanceLogs { get; set; } = null!;
        public DbSet<Warehouse> Warehouses { get; set; } = null!;
        public DbSet<WarehouseBin> WarehouseBins { get; set; } = null!;
        public DbSet<Store> Stores { get; set; } = null!;
        public DbSet<RequestForQuotation> RequestForQuotations { get; set; } = null!;
        public DbSet<VendorQuotation> VendorQuotations { get; set; } = null!;
        public DbSet<PurchaseReturn> PurchaseReturns { get; set; } = null!;
        public DbSet<StockIssue> StockIssues { get; set; } = null!;
        public DbSet<QualityInspection> QualityInspections { get; set; } = null!;

        // Communication Schema
        public DbSet<NoticeBoard> NoticeBoards { get; set; } = null!;
        public DbSet<Circular> Circulars { get; set; } = null!;
        public DbSet<SmsLog> SmsLogs { get; set; } = null!;
        public DbSet<WhatsAppLog> WhatsAppLogs { get; set; } = null!;
        public DbSet<WhatsAppAccount> WhatsAppAccounts { get; set; } = null!;
        public DbSet<WhatsAppTemplate> WhatsAppTemplates { get; set; } = null!;
        public DbSet<WhatsAppMessage> WhatsAppMessages { get; set; } = null!;
        public DbSet<WhatsAppQueue> WhatsAppQueues { get; set; } = null!;
        public DbSet<WhatsAppDeliveryLog> WhatsAppDeliveryLogs { get; set; } = null!;
        public DbSet<WhatsAppWebhookEvent> WhatsAppWebhookEvents { get; set; } = null!;
        public DbSet<WhatsAppConversation> WhatsAppConversations { get; set; } = null!;
        public DbSet<WhatsAppMedia> WhatsAppMediaFiles { get; set; } = null!;
        public DbSet<WhatsAppAuditLog> WhatsAppAuditLogs { get; set; } = null!;
        public DbSet<PushNotification> PushNotifications { get; set; } = null!;
        public DbSet<ParentTeacherChat> ParentTeacherChats { get; set; } = null!;
        public DbSet<FeedbackSurvey> FeedbackSurveys { get; set; } = null!;
        public DbSet<SurveyQuestion> SurveyQuestions { get; set; } = null!;
        public DbSet<SurveyResponse> SurveyResponses { get; set; } = null!;
        public DbSet<Announcement> Announcements { get; set; } = null!;
        public DbSet<CommunicationMeeting> CommunicationMeetings { get; set; } = null!;
        public DbSet<SupportTicket> SupportTickets { get; set; } = null!;
        public DbSet<TicketResponse> TicketResponses { get; set; } = null!;
        public DbSet<QuickPoll> QuickPolls { get; set; } = null!;
        public DbSet<PollVote> PollVotes { get; set; } = null!;
        public DbSet<SharedDocument> SharedDocuments { get; set; } = null!;
        public DbSet<CommunicationTemplate> CommunicationTemplates { get; set; } = null!;
        public DbSet<GroupChatRoom> GroupChatRooms { get; set; } = null!;
        public DbSet<GroupChatMember> GroupChatMembers { get; set; } = null!;
        public DbSet<GroupChatMessage> GroupChatMessages { get; set; } = null!;
        public DbSet<CentralNotification> CentralNotifications { get; set; } = null!;

        // Analytics Schema
        public DbSet<DashboardConfig> DashboardConfigs { get; set; } = null!;
        public DbSet<DashboardWidget> DashboardWidgets { get; set; } = null!;
        public DbSet<AnalyticsKpi> AnalyticsKpis { get; set; } = null!;

        // ─── Enterprise Reporting Engine ──────────────────────────────────────
        public DbSet<ReportCategory> ReportCategories { get; set; } = null!;
        public DbSet<ReportingTemplate> ReportTemplates { get; set; } = null!;
        public DbSet<ReportParameter> ReportParameters { get; set; } = null!;
        public DbSet<ReportBranding> ReportBrandings { get; set; } = null!;
        public DbSet<ReportHistory> ReportHistories { get; set; } = null!;
        public DbSet<ReportSchedule> ReportSchedules { get; set; } = null!;
        public DbSet<ReportPermission> ReportPermissions { get; set; } = null!;
        public DbSet<ReportExecution> ReportExecutions { get; set; } = null!;

        // Communication - Recipients
        public DbSet<Recipient> Recipients { get; set; } = null!;
        public DbSet<RecipientCategory> RecipientCategories { get; set; } = null!;
        public DbSet<RecipientGroup> RecipientGroups { get; set; } = null!;
        public DbSet<RecipientGroupMember> RecipientGroupMembers { get; set; } = null!;
        public DbSet<RecipientTag> RecipientTags { get; set; } = null!;
        public DbSet<RecipientPreference> RecipientPreferences { get; set; } = null!;
        public DbSet<RecipientHistory> RecipientHistories { get; set; } = null!;
        public DbSet<RecipientBlacklist> RecipientBlacklists { get; set; } = null!;
        public DbSet<RecipientActivity> RecipientActivities { get; set; } = null!;
        public DbSet<DistributionList> DistributionLists { get; set; } = null!;
        public DbSet<EmailRecipient> EmailRecipients { get; set; } = null!;
        public DbSet<EmailAttachment> EmailAttachments { get; set; } = null!;

        // Administration Schema
        public DbSet<SchoolBranch> SchoolBranches { get; set; } = null!;
        public DbSet<WorkflowDefinition> WorkflowDefinitions { get; set; } = null!;
        public DbSet<WorkflowStep> WorkflowSteps { get; set; } = null!;
        public DbSet<WorkflowInstance> WorkflowInstances { get; set; } = null!;
        public DbSet<ApprovalLog> ApprovalLogs { get; set; } = null!;
        public DbSet<AdminAuditLog> AdminAuditLogs { get; set; } = null!;
        public DbSet<Complaint> Complaints { get; set; } = null!;
        public DbSet<Visitor> Visitors { get; set; } = null!;
        public DbSet<CertificateIssuanceLog> CertificateIssuanceLogs { get; set; } = null!;

        // Communication Schema (continued)
        public DbSet<NotificationLog> NotificationLogs { get; set; } = null!;

        // AI Schema
        public DbSet<AiPrediction> AiPredictions { get; set; } = null!;
        public DbSet<AiGeneration> AiGenerations { get; set; } = null!;
        public DbSet<AiChatSession> AiChatSessions { get; set; } = null!;
        public DbSet<AiChatMessage> AiChatMessages { get; set; } = null!;

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

            modelBuilder.Entity<ParentStudentMapping>(entity =>
            {
                entity.ToTable("ParentStudentMappings");
                entity.HasIndex(e => new { e.ParentUserId, e.StudentId }).IsUnique();
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
                var isDeleteEntity = typeof(School.Domain.BaseEntity.IDeleteEntity).IsAssignableFrom(entityType.ClrType);
                var isTenantEntity = typeof(School.Domain.BaseEntity.ITenantEntity).IsAssignableFrom(entityType.ClrType);

                if (isDeleteEntity && isTenantEntity)
                {
                    var method = typeof(SchoolDbContext)
                        .GetMethod(nameof(ConfigureTenantAndSoftDeleteFilter))
                        ?.MakeGenericMethod(entityType.ClrType);
                    method?.Invoke(this, new object[] { modelBuilder });
                }
                else if (isDeleteEntity)
                {
                    var method = typeof(SchoolDbContext)
                        .GetMethod(nameof(ConfigureSoftDeleteFilterOnly))
                        ?.MakeGenericMethod(entityType.ClrType);
                    method?.Invoke(this, new object[] { modelBuilder });
                }
                else if (isTenantEntity)
                {
                    var method = typeof(SchoolDbContext)
                        .GetMethod(nameof(ConfigureTenantFilterOnly))
                        ?.MakeGenericMethod(entityType.ClrType);
                    method?.Invoke(this, new object[] { modelBuilder });
                }
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetAuditProperties();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            SetAuditProperties();
            return base.SaveChanges();
        }

        private void SetAuditProperties()
        {
            var username = _httpContextAccessor?.HttpContext?.User?.FindFirst(School.Utilities.Constants.ClaimConstants.UserName)?.Value ?? "System";
            var tenantId = CurrentTenantId;
            int resolvedTenantId = tenantId ?? 1;

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added)
                {
                    if (entry.Entity is BaseEntity.ITenantEntity tenantEntity)
                    {
                        if (tenantEntity.SchoolRegistrationId == 0)
                        {
                            tenantEntity.SchoolRegistrationId = resolvedTenantId;
                        }
                    }
                    if (entry.Entity is BaseEntity.IAuditEntity auditEntity)
                    {
                        auditEntity.CreatedDate = DateTime.Now;
                        auditEntity.CreatedBy = username;
                    }
                }
                else if (entry.State == EntityState.Modified)
                {
                    if (entry.Entity is BaseEntity.IAuditEntity auditEntity)
                    {
                        auditEntity.UpdatedDate = DateTime.Now;
                        auditEntity.UpdatedBy = username;
                    }
                    if (entry.Entity is BaseEntity.IDeleteEntity deleteEntity && deleteEntity.IsDeleted && deleteEntity.DeletedDate == null)
                    {
                        deleteEntity.DeletedDate = DateTime.Now;
                        deleteEntity.DeletedBy = username;
                    }
                }
                else if (entry.State == EntityState.Deleted)
                {
                    if (entry.Entity is BaseEntity.IDeleteEntity deleteEntity)
                    {
                        entry.State = EntityState.Modified;
                        deleteEntity.IsDeleted = true;
                        deleteEntity.DeletedDate = DateTime.Now;
                        deleteEntity.DeletedBy = username;
                    }
                }
            }
        }

        public void ConfigureTenantAndSoftDeleteFilter<T>(ModelBuilder builder) where T : class, BaseEntity.IDeleteEntity, BaseEntity.ITenantEntity
        {
            builder.Entity<T>().HasIndex("SchoolRegistrationId", "IsDeleted");
            builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted && (CurrentTenantId == null || e.SchoolRegistrationId == CurrentTenantId));
        }

        public void ConfigureSoftDeleteFilterOnly<T>(ModelBuilder builder) where T : class, BaseEntity.IDeleteEntity
        {
            builder.Entity<T>().HasIndex("IsDeleted");
            builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
        }

        public void ConfigureTenantFilterOnly<T>(ModelBuilder builder) where T : class, BaseEntity.ITenantEntity
        {
            builder.Entity<T>().HasIndex("SchoolRegistrationId");
            builder.Entity<T>().HasQueryFilter(e => CurrentTenantId == null || e.SchoolRegistrationId == CurrentTenantId);
        }





    }
}




