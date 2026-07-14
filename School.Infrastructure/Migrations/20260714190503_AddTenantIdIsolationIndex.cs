using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTenantIdIsolationIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_YearSemesters_SchoolRegistrationId",
                table: "YearSemesters");

            migrationBuilder.DropIndex(
                name: "IX_WeekOffs_SchoolRegistrationId",
                table: "WeekOffs");

            migrationBuilder.DropIndex(
                name: "IX_Warehouses_SchoolRegistrationId",
                schema: "Inventory",
                table: "Warehouses");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseBins_SchoolRegistrationId",
                schema: "Inventory",
                table: "WarehouseBins");

            migrationBuilder.DropIndex(
                name: "IX_Vendors_SchoolRegistrationId",
                schema: "Inventory",
                table: "Vendors");

            migrationBuilder.DropIndex(
                name: "IX_VendorQuotations_SchoolRegistrationId",
                schema: "Inventory",
                table: "VendorQuotations");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_SchoolRegistrationId",
                schema: "Transport",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_VehicleMaintenances_SchoolRegistrationId",
                schema: "Transport",
                table: "VehicleMaintenances");

            migrationBuilder.DropIndex(
                name: "IX_VehicleIncidents_SchoolRegistrationId",
                schema: "Transport",
                table: "VehicleIncidents");

            migrationBuilder.DropIndex(
                name: "IX_TransportTrips_SchoolRegistrationId",
                schema: "Transport",
                table: "TransportTrips");

            migrationBuilder.DropIndex(
                name: "IX_TransportStops_SchoolRegistrationId",
                schema: "Transport",
                table: "TransportStops");

            migrationBuilder.DropIndex(
                name: "IX_TransportRoutes_SchoolRegistrationId",
                schema: "Transport",
                table: "TransportRoutes");

            migrationBuilder.DropIndex(
                name: "IX_TransportInventories_SchoolRegistrationId",
                schema: "Transport",
                table: "TransportInventories");

            migrationBuilder.DropIndex(
                name: "IX_TransportGateLogs_SchoolRegistrationId",
                schema: "Transport",
                table: "TransportGateLogs");

            migrationBuilder.DropIndex(
                name: "IX_TransportAllocations_SchoolRegistrationId",
                schema: "Transport",
                table: "TransportAllocations");

            migrationBuilder.DropIndex(
                name: "IX_TrainingPrograms_SchoolRegistrationId",
                table: "TrainingPrograms");

            migrationBuilder.DropIndex(
                name: "IX_TrainingEnrollments_SchoolRegistrationId",
                table: "TrainingEnrollments");

            migrationBuilder.DropIndex(
                name: "IX_TimetableSlots_SchoolRegistrationId",
                table: "TimetableSlots");

            migrationBuilder.DropIndex(
                name: "IX_TimetablePeriods_SchoolRegistrationId",
                table: "TimetablePeriods");

            migrationBuilder.DropIndex(
                name: "IX_Timesheets_SchoolRegistrationId",
                table: "Timesheets");

            migrationBuilder.DropIndex(
                name: "IX_TimesheetEntries_SchoolRegistrationId",
                table: "TimesheetEntries");

            migrationBuilder.DropIndex(
                name: "IX_TaxConfigs_SchoolRegistrationId",
                schema: "Finance",
                table: "TaxConfigs");

            migrationBuilder.DropIndex(
                name: "IX_SyllabusChapters_SchoolRegistrationId",
                table: "SyllabusChapters");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_SchoolRegistrationId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_SubjectEnrollments_SchoolRegistrationId",
                table: "SubjectEnrollments");

            migrationBuilder.DropIndex(
                name: "IX_StudentScholarships_SchoolRegistrationId",
                table: "StudentScholarships");

            migrationBuilder.DropIndex(
                name: "IX_Students_SchoolRegistrationId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_StudentPromotions_SchoolRegistrationId",
                table: "StudentPromotions");

            migrationBuilder.DropIndex(
                name: "IX_StudentAttendances_SchoolRegistrationId",
                table: "StudentAttendances");

            migrationBuilder.DropIndex(
                name: "IX_Stores_SchoolRegistrationId",
                schema: "Inventory",
                table: "Stores");

            migrationBuilder.DropIndex(
                name: "IX_StockTransactions_SchoolRegistrationId",
                schema: "Inventory",
                table: "StockTransactions");

            migrationBuilder.DropIndex(
                name: "IX_StockIssues_SchoolRegistrationId",
                schema: "Inventory",
                table: "StockIssues");

            migrationBuilder.DropIndex(
                name: "IX_StatutoryComplianceConfigs_SchoolRegistrationId",
                schema: "Payroll",
                table: "StatutoryComplianceConfigs");

            migrationBuilder.DropIndex(
                name: "IX_Specializations_SchoolRegistrationId",
                table: "Specializations");

            migrationBuilder.DropIndex(
                name: "IX_ShiftMasters_SchoolRegistrationId",
                table: "ShiftMasters");

            migrationBuilder.DropIndex(
                name: "IX_SchoolSubscriptions_SchoolRegistrationId",
                schema: "School",
                table: "SchoolSubscriptions");

            migrationBuilder.DropIndex(
                name: "IX_SchoolOwners_SchoolRegistrationId",
                schema: "School",
                table: "SchoolOwners");

            migrationBuilder.DropIndex(
                name: "IX_SchoolAssets_SchoolRegistrationId",
                table: "SchoolAssets");

            migrationBuilder.DropIndex(
                name: "IX_SalaryStructures_SchoolRegistrationId",
                schema: "Payroll",
                table: "SalaryStructures");

            migrationBuilder.DropIndex(
                name: "IX_SalaryStructureItems_SchoolRegistrationId",
                schema: "Payroll",
                table: "SalaryStructureItems");

            migrationBuilder.DropIndex(
                name: "IX_SalaryGrades_SchoolRegistrationId",
                table: "SalaryGrades");

            migrationBuilder.DropIndex(
                name: "IX_SalaryComponents_SchoolRegistrationId",
                schema: "Payroll",
                table: "SalaryComponents");

            migrationBuilder.DropIndex(
                name: "IX_SalaryArrears_SchoolRegistrationId",
                schema: "Payroll",
                table: "SalaryArrears");

            migrationBuilder.DropIndex(
                name: "IX_SalaryAdvances_SchoolRegistrationId",
                schema: "Payroll",
                table: "SalaryAdvances");

            migrationBuilder.DropIndex(
                name: "IX_RouteStopMappings_SchoolRegistrationId",
                schema: "Transport",
                table: "RouteStopMappings");

            migrationBuilder.DropIndex(
                name: "IX_RouteAssignments_SchoolRegistrationId",
                schema: "Transport",
                table: "RouteAssignments");

            migrationBuilder.DropIndex(
                name: "IX_RoomTransferHistories_SchoolRegistrationId",
                schema: "Hostel",
                table: "RoomTransferHistories");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_SchoolRegistrationId",
                schema: "Hostel",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_RoomCategories_SchoolRegistrationId",
                schema: "Hostel",
                table: "RoomCategories");

            migrationBuilder.DropIndex(
                name: "IX_RfidScanLogs_SchoolRegistrationId",
                schema: "Transport",
                table: "RfidScanLogs");

            migrationBuilder.DropIndex(
                name: "IX_RequestForQuotations_SchoolRegistrationId",
                schema: "Inventory",
                table: "RequestForQuotations");

            migrationBuilder.DropIndex(
                name: "IX_ReportCards_SchoolRegistrationId",
                table: "ReportCards");

            migrationBuilder.DropIndex(
                name: "IX_ReligionMasters_SchoolRegistrationId",
                table: "ReligionMasters");

            migrationBuilder.DropIndex(
                name: "IX_ReimbursementClaims_SchoolRegistrationId",
                schema: "Payroll",
                table: "ReimbursementClaims");

            migrationBuilder.DropIndex(
                name: "IX_QualityInspections_SchoolRegistrationId",
                schema: "Inventory",
                table: "QualityInspections");

            migrationBuilder.DropIndex(
                name: "IX_QualificationMasters_SchoolRegistrationId",
                table: "QualificationMasters");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseReturns_SchoolRegistrationId",
                schema: "Inventory",
                table: "PurchaseReturns");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequisitions_SchoolRegistrationId",
                schema: "Inventory",
                table: "PurchaseRequisitions");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequisitionItems_SchoolRegistrationId",
                schema: "Inventory",
                table: "PurchaseRequisitionItems");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_SchoolRegistrationId",
                schema: "Inventory",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrderItems_SchoolRegistrationId",
                schema: "Inventory",
                table: "PurchaseOrderItems");

            migrationBuilder.DropIndex(
                name: "IX_Programs_SchoolRegistrationId",
                table: "Programs");

            migrationBuilder.DropIndex(
                name: "IX_PerformanceReviews_SchoolRegistrationId",
                table: "PerformanceReviews");

            migrationBuilder.DropIndex(
                name: "IX_PayrollRuns_SchoolRegistrationId",
                schema: "Payroll",
                table: "PayrollRuns");

            migrationBuilder.DropIndex(
                name: "IX_PayrollRunDetails_SchoolRegistrationId",
                schema: "Payroll",
                table: "PayrollRunDetails");

            migrationBuilder.DropIndex(
                name: "IX_PaymentGateways_SchoolRegistrationId",
                table: "PaymentGateways");

            migrationBuilder.DropIndex(
                name: "IX_PayGroups_SchoolRegistrationId",
                schema: "Payroll",
                table: "PayGroups");

            migrationBuilder.DropIndex(
                name: "IX_ParentStudentMappings_SchoolRegistrationId",
                table: "ParentStudentMappings");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationProfiles_SchoolRegistrationId",
                schema: "School",
                table: "OrganizationProfiles");

            migrationBuilder.DropIndex(
                name: "IX_OnlinePaymentOrders_SchoolRegistrationId",
                table: "OnlinePaymentOrders");

            migrationBuilder.DropIndex(
                name: "IX_OnlineClasses_SchoolRegistrationId",
                table: "OnlineClasses");

            migrationBuilder.DropIndex(
                name: "IX_NoticePeriods_SchoolRegistrationId",
                table: "NoticePeriods");

            migrationBuilder.DropIndex(
                name: "IX_MessMenus_SchoolRegistrationId",
                schema: "Hostel",
                table: "MessMenus");

            migrationBuilder.DropIndex(
                name: "IX_MealAttendances_SchoolRegistrationId",
                schema: "Hostel",
                table: "MealAttendances");

            migrationBuilder.DropIndex(
                name: "IX_LoanRepaymentSchedules_SchoolRegistrationId",
                schema: "Payroll",
                table: "LoanRepaymentSchedules");

            migrationBuilder.DropIndex(
                name: "IX_LibraryMembers_SchoolRegistrationId",
                schema: "Library",
                table: "LibraryMembers");

            migrationBuilder.DropIndex(
                name: "IX_LibraryFineRules_SchoolRegistrationId",
                schema: "Library",
                table: "LibraryFineRules");

            migrationBuilder.DropIndex(
                name: "IX_LibraryBooks_SchoolRegistrationId",
                schema: "Library",
                table: "LibraryBooks");

            migrationBuilder.DropIndex(
                name: "IX_LessonPlans_SchoolRegistrationId",
                table: "LessonPlans");

            migrationBuilder.DropIndex(
                name: "IX_LeaveTypes_SchoolRegistrationId",
                table: "LeaveTypes");

            migrationBuilder.DropIndex(
                name: "IX_LeaveSettings_SchoolRegistrationId",
                table: "LeaveSettings");

            migrationBuilder.DropIndex(
                name: "IX_LeaveRequests_SchoolRegistrationId",
                table: "LeaveRequests");

            migrationBuilder.DropIndex(
                name: "IX_LeaveBalances_SchoolRegistrationId",
                table: "LeaveBalances");

            migrationBuilder.DropIndex(
                name: "IX_LaundryTransactions_SchoolRegistrationId",
                schema: "Hostel",
                table: "LaundryTransactions");

            migrationBuilder.DropIndex(
                name: "IX_KpiMetrics_SchoolRegistrationId",
                table: "KpiMetrics");

            migrationBuilder.DropIndex(
                name: "IX_JournalEntryLines_SchoolRegistrationId",
                schema: "Finance",
                table: "JournalEntryLines");

            migrationBuilder.DropIndex(
                name: "IX_JournalEntries_SchoolRegistrationId",
                schema: "Finance",
                table: "JournalEntries");

            migrationBuilder.DropIndex(
                name: "IX_JobPostings_SchoolRegistrationId",
                table: "JobPostings");

            migrationBuilder.DropIndex(
                name: "IX_JobApplications_SchoolRegistrationId",
                table: "JobApplications");

            migrationBuilder.DropIndex(
                name: "IX_ItemCategories_SchoolRegistrationId",
                schema: "Inventory",
                table: "ItemCategories");

            migrationBuilder.DropIndex(
                name: "IX_InventoryItems_SchoolRegistrationId",
                schema: "Inventory",
                table: "InventoryItems");

            migrationBuilder.DropIndex(
                name: "IX_HostelWardens_SchoolRegistrationId",
                schema: "Hostel",
                table: "HostelWardens");

            migrationBuilder.DropIndex(
                name: "IX_HostelVisitors_SchoolRegistrationId",
                schema: "Hostel",
                table: "HostelVisitors");

            migrationBuilder.DropIndex(
                name: "IX_Hostels_SchoolRegistrationId",
                schema: "Hostel",
                table: "Hostels");

            migrationBuilder.DropIndex(
                name: "IX_HostelMedicalLogs_SchoolRegistrationId",
                schema: "Hostel",
                table: "HostelMedicalLogs");

            migrationBuilder.DropIndex(
                name: "IX_HostelMaintenances_SchoolRegistrationId",
                schema: "Hostel",
                table: "HostelMaintenances");

            migrationBuilder.DropIndex(
                name: "IX_HostelInventories_SchoolRegistrationId",
                schema: "Hostel",
                table: "HostelInventories");

            migrationBuilder.DropIndex(
                name: "IX_HostelGatePasses_SchoolRegistrationId",
                schema: "Hostel",
                table: "HostelGatePasses");

            migrationBuilder.DropIndex(
                name: "IX_HostelFeePayments_SchoolRegistrationId",
                schema: "Hostel",
                table: "HostelFeePayments");

            migrationBuilder.DropIndex(
                name: "IX_HostelFeeAllocations_SchoolRegistrationId",
                schema: "Hostel",
                table: "HostelFeeAllocations");

            migrationBuilder.DropIndex(
                name: "IX_HostelDisciplines_SchoolRegistrationId",
                schema: "Hostel",
                table: "HostelDisciplines");

            migrationBuilder.DropIndex(
                name: "IX_HostelComplaints_SchoolRegistrationId",
                schema: "Hostel",
                table: "HostelComplaints");

            migrationBuilder.DropIndex(
                name: "IX_HostelAttendances_SchoolRegistrationId",
                schema: "Hostel",
                table: "HostelAttendances");

            migrationBuilder.DropIndex(
                name: "IX_HostelAdmissions_SchoolRegistrationId",
                schema: "Hostel",
                table: "HostelAdmissions");

            migrationBuilder.DropIndex(
                name: "IX_HomeworkSubmissions_SchoolRegistrationId",
                table: "HomeworkSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_Homeworks_SchoolRegistrationId",
                table: "Homeworks");

            migrationBuilder.DropIndex(
                name: "IX_HolidayMasters_SchoolRegistrationId",
                table: "HolidayMasters");

            migrationBuilder.DropIndex(
                name: "IX_GradeConfigs_SchoolRegistrationId",
                table: "GradeConfigs");

            migrationBuilder.DropIndex(
                name: "IX_GoodsReceiptNotes_SchoolRegistrationId",
                schema: "Inventory",
                table: "GoodsReceiptNotes");

            migrationBuilder.DropIndex(
                name: "IX_GoodsReceiptNoteItems_SchoolRegistrationId",
                schema: "Inventory",
                table: "GoodsReceiptNoteItems");

            migrationBuilder.DropIndex(
                name: "IX_FuelLogs_SchoolRegistrationId",
                schema: "Transport",
                table: "FuelLogs");

            migrationBuilder.DropIndex(
                name: "IX_Floors_SchoolRegistrationId",
                schema: "Hostel",
                table: "Floors");

            migrationBuilder.DropIndex(
                name: "IX_FineRules_SchoolRegistrationId",
                table: "FineRules");

            migrationBuilder.DropIndex(
                name: "IX_FinancialYears_SchoolRegistrationId",
                schema: "Finance",
                table: "FinancialYears");

            migrationBuilder.DropIndex(
                name: "IX_FeeTypes_SchoolRegistrationId",
                table: "FeeTypes");

            migrationBuilder.DropIndex(
                name: "IX_FeeStructures_SchoolRegistrationId",
                table: "FeeStructures");

            migrationBuilder.DropIndex(
                name: "IX_FeeRefunds_SchoolRegistrationId",
                table: "FeeRefunds");

            migrationBuilder.DropIndex(
                name: "IX_FeePayments_SchoolRegistrationId",
                table: "FeePayments");

            migrationBuilder.DropIndex(
                name: "IX_FeeInstallments_SchoolRegistrationId",
                table: "FeeInstallments");

            migrationBuilder.DropIndex(
                name: "IX_FeeFines_SchoolRegistrationId",
                table: "FeeFines");

            migrationBuilder.DropIndex(
                name: "IX_Faculties_SchoolRegistrationId",
                table: "Faculties");

            migrationBuilder.DropIndex(
                name: "IX_ExamSchedules_SchoolRegistrationId",
                table: "ExamSchedules");

            migrationBuilder.DropIndex(
                name: "IX_Exams_SchoolRegistrationId",
                table: "Exams");

            migrationBuilder.DropIndex(
                name: "IX_ExamResults_SchoolRegistrationId",
                table: "ExamResults");

            migrationBuilder.DropIndex(
                name: "IX_Events_SchoolRegistrationId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_EmploymentStatuses_SchoolRegistrationId",
                table: "EmploymentStatuses");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeTypes_SchoolRegistrationId",
                table: "EmployeeTypes");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeSalaryDetails_SchoolRegistrationId",
                table: "EmployeeSalaryDetails");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeSalaryAllocations_SchoolRegistrationId",
                schema: "Payroll",
                table: "EmployeeSalaryAllocations");

            migrationBuilder.DropIndex(
                name: "IX_Employees_SchoolRegistrationId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeLoans_SchoolRegistrationId",
                schema: "Payroll",
                table: "EmployeeLoans");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeExperiences_SchoolRegistrationId",
                table: "EmployeeExperiences");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeEducations_SchoolRegistrationId",
                table: "EmployeeEducations");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeDocuments_SchoolRegistrationId",
                table: "EmployeeDocuments");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeCategories_SchoolRegistrationId",
                table: "EmployeeCategories");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeBonuses_SchoolRegistrationId",
                schema: "Payroll",
                table: "EmployeeBonuses");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeBankDetails_SchoolRegistrationId",
                table: "EmployeeBankDetails");

            migrationBuilder.DropIndex(
                name: "IX_EmailTemplates_SchoolRegistrationId",
                table: "EmailTemplates");

            migrationBuilder.DropIndex(
                name: "IX_EmailServerSettings_SchoolRegistrationId",
                table: "EmailServerSettings");

            migrationBuilder.DropIndex(
                name: "IX_EmailLogs_SchoolRegistrationId",
                table: "EmailLogs");

            migrationBuilder.DropIndex(
                name: "IX_EmailBrandings_SchoolRegistrationId",
                table: "EmailBrandings");

            migrationBuilder.DropIndex(
                name: "IX_EducationLevels_SchoolRegistrationId",
                table: "EducationLevels");

            migrationBuilder.DropIndex(
                name: "IX_EducationalDetails_SchoolRegistrationId",
                table: "EducationalDetails");

            migrationBuilder.DropIndex(
                name: "IX_DigitalResources_SchoolRegistrationId",
                schema: "Library",
                table: "DigitalResources");

            migrationBuilder.DropIndex(
                name: "IX_Designations_SchoolRegistrationId",
                table: "Designations");

            migrationBuilder.DropIndex(
                name: "IX_Departments_SchoolRegistrationId",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Courses_SchoolRegistrationId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_CostCenters_SchoolRegistrationId",
                schema: "Finance",
                table: "CostCenters");

            migrationBuilder.DropIndex(
                name: "IX_Conductors_SchoolRegistrationId",
                schema: "Transport",
                table: "Conductors");

            migrationBuilder.DropIndex(
                name: "IX_CoaAccounts_SchoolRegistrationId",
                schema: "Finance",
                table: "CoaAccounts");

            migrationBuilder.DropIndex(
                name: "IX_Classes_SchoolRegistrationId",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_ChequeBooks_SchoolRegistrationId",
                schema: "Finance",
                table: "ChequeBooks");

            migrationBuilder.DropIndex(
                name: "IX_CashBankTransactions_SchoolRegistrationId",
                schema: "Finance",
                table: "CashBankTransactions");

            migrationBuilder.DropIndex(
                name: "IX_Candidates_SchoolRegistrationId",
                table: "Candidates");

            migrationBuilder.DropIndex(
                name: "IX_Campuses_SchoolRegistrationId",
                schema: "School",
                table: "Campuses");

            migrationBuilder.DropIndex(
                name: "IX_Buildings_SchoolRegistrationId",
                schema: "Hostel",
                table: "Buildings");

            migrationBuilder.DropIndex(
                name: "IX_BudgetPlans_SchoolRegistrationId",
                schema: "Finance",
                table: "BudgetPlans");

            migrationBuilder.DropIndex(
                name: "IX_Branches_SchoolRegistrationId",
                table: "Branches");

            migrationBuilder.DropIndex(
                name: "IX_BookVendors_SchoolRegistrationId",
                schema: "Library",
                table: "BookVendors");

            migrationBuilder.DropIndex(
                name: "IX_BookReservations_SchoolRegistrationId",
                schema: "Library",
                table: "BookReservations");

            migrationBuilder.DropIndex(
                name: "IX_BookPublishers_SchoolRegistrationId",
                schema: "Library",
                table: "BookPublishers");

            migrationBuilder.DropIndex(
                name: "IX_BookIssueLogs_SchoolRegistrationId",
                schema: "Library",
                table: "BookIssueLogs");

            migrationBuilder.DropIndex(
                name: "IX_BookCategories_SchoolRegistrationId",
                schema: "Library",
                table: "BookCategories");

            migrationBuilder.DropIndex(
                name: "IX_BookAuthors_SchoolRegistrationId",
                schema: "Library",
                table: "BookAuthors");

            migrationBuilder.DropIndex(
                name: "IX_BloodGroupMasters_SchoolRegistrationId",
                table: "BloodGroupMasters");

            migrationBuilder.DropIndex(
                name: "IX_Beds_SchoolRegistrationId",
                schema: "Hostel",
                table: "Beds");

            migrationBuilder.DropIndex(
                name: "IX_BedReservations_SchoolRegistrationId",
                schema: "Hostel",
                table: "BedReservations");

            migrationBuilder.DropIndex(
                name: "IX_Batches_SchoolRegistrationId",
                table: "Batches");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_SchoolRegistrationId",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_AttendanceLogs_SchoolRegistrationId",
                table: "AttendanceLogs");

            migrationBuilder.DropIndex(
                name: "IX_AssignmentSubmissions_SchoolRegistrationId",
                table: "AssignmentSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_SchoolRegistrationId",
                table: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_AssetMaintenanceLogs_SchoolRegistrationId",
                schema: "Inventory",
                table: "AssetMaintenanceLogs");

            migrationBuilder.DropIndex(
                name: "IX_AssetDepreciationLogs_SchoolRegistrationId",
                schema: "Inventory",
                table: "AssetDepreciationLogs");

            migrationBuilder.DropIndex(
                name: "IX_AssetAssignments_SchoolRegistrationId",
                table: "AssetAssignments");

            migrationBuilder.DropIndex(
                name: "IX_Affiliateds_SchoolRegistrationId",
                table: "Affiliateds");

            migrationBuilder.DropIndex(
                name: "IX_AdmissionRules_SchoolRegistrationId",
                table: "AdmissionRules");

            migrationBuilder.DropIndex(
                name: "IX_AdmissionFormConfigs_SchoolRegistrationId",
                table: "AdmissionFormConfigs");

            migrationBuilder.DropIndex(
                name: "IX_AdmissionApplications_SchoolRegistrationId",
                table: "AdmissionApplications");

            migrationBuilder.DropIndex(
                name: "IX_AcademicYears_SchoolRegistrationId",
                table: "AcademicYears");

            migrationBuilder.CreateIndex(
                name: "IX_YearSemesters_SchoolRegistrationId_IsDeleted",
                table: "YearSemesters",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowSteps_SchoolRegistrationId_IsDeleted",
                schema: "Administration",
                table: "WorkflowSteps",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowInstances_SchoolRegistrationId_IsDeleted",
                schema: "Administration",
                table: "WorkflowInstances",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowDefinitions_SchoolRegistrationId_IsDeleted",
                schema: "Administration",
                table: "WorkflowDefinitions",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_WhatsAppLogs_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "WhatsAppLogs",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_WeekOffs_SchoolRegistrationId_IsDeleted",
                table: "WeekOffs",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_SchoolRegistrationId_IsDeleted",
                schema: "Inventory",
                table: "Warehouses",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseBins_SchoolRegistrationId_IsDeleted",
                schema: "Inventory",
                table: "WarehouseBins",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_SchoolRegistrationId_IsDeleted",
                schema: "Inventory",
                table: "Vendors",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_VendorQuotations_SchoolRegistrationId_IsDeleted",
                schema: "Inventory",
                table: "VendorQuotations",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_SchoolRegistrationId_IsDeleted",
                schema: "Transport",
                table: "Vehicles",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_VehicleMaintenances_SchoolRegistrationId_IsDeleted",
                schema: "Transport",
                table: "VehicleMaintenances",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_VehicleIncidents_SchoolRegistrationId_IsDeleted",
                schema: "Transport",
                table: "VehicleIncidents",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_TransportTrips_SchoolRegistrationId_IsDeleted",
                schema: "Transport",
                table: "TransportTrips",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_TransportStops_SchoolRegistrationId_IsDeleted",
                schema: "Transport",
                table: "TransportStops",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_TransportRoutes_SchoolRegistrationId_IsDeleted",
                schema: "Transport",
                table: "TransportRoutes",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_TransportInventories_SchoolRegistrationId_IsDeleted",
                schema: "Transport",
                table: "TransportInventories",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_TransportGateLogs_SchoolRegistrationId_IsDeleted",
                schema: "Transport",
                table: "TransportGateLogs",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_TransportAllocations_SchoolRegistrationId_IsDeleted",
                schema: "Transport",
                table: "TransportAllocations",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPrograms_SchoolRegistrationId_IsDeleted",
                table: "TrainingPrograms",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingEnrollments_SchoolRegistrationId_IsDeleted",
                table: "TrainingEnrollments",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_TimetableSlots_SchoolRegistrationId_IsDeleted",
                table: "TimetableSlots",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_TimetablePeriods_SchoolRegistrationId_IsDeleted",
                table: "TimetablePeriods",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Timesheets_SchoolRegistrationId_IsDeleted",
                table: "Timesheets",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_TimesheetEntries_SchoolRegistrationId_IsDeleted",
                table: "TimesheetEntries",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_TicketResponses_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "TicketResponses",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_TaxConfigs_SchoolRegistrationId_IsDeleted",
                schema: "Finance",
                table: "TaxConfigs",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_SyllabusChapters_SchoolRegistrationId_IsDeleted",
                table: "SyllabusChapters",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_SurveyResponses_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "SurveyResponses",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_SurveyQuestions_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "SurveyQuestions",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_SupportTickets_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "SupportTickets",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_SubMenus_IsDeleted",
                table: "SubMenus",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_SchoolRegistrationId_IsDeleted",
                table: "Subjects",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_SubjectEnrollments_SchoolRegistrationId_IsDeleted",
                table: "SubjectEnrollments",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_StudentScholarships_SchoolRegistrationId_IsDeleted",
                table: "StudentScholarships",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Students_SchoolRegistrationId_IsDeleted",
                table: "Students",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_StudentPromotions_SchoolRegistrationId_IsDeleted",
                table: "StudentPromotions",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendances_SchoolRegistrationId_IsDeleted",
                table: "StudentAttendances",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Stores_SchoolRegistrationId_IsDeleted",
                schema: "Inventory",
                table: "Stores",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_SchoolRegistrationId_IsDeleted",
                schema: "Inventory",
                table: "StockTransactions",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_StockIssues_SchoolRegistrationId_IsDeleted",
                schema: "Inventory",
                table: "StockIssues",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_StatutoryComplianceConfigs_SchoolRegistrationId_IsDeleted",
                schema: "Payroll",
                table: "StatutoryComplianceConfigs",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_States_IsDeleted",
                table: "States",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_SchoolRegistrationId_IsDeleted",
                table: "Specializations",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_SmsLogs_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "SmsLogs",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_ShiftMasters_SchoolRegistrationId_IsDeleted",
                table: "ShiftMasters",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_SharedDocuments_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "SharedDocuments",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_SchoolTypes_IsDeleted",
                schema: "School",
                table: "SchoolTypes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolSubscriptions_SchoolRegistrationId_IsDeleted",
                schema: "School",
                table: "SchoolSubscriptions",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_SchoolRegistrations_IsDeleted",
                schema: "School",
                table: "SchoolRegistrations",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolProfileSettings_SchoolRegistrationId_IsDeleted",
                schema: "School",
                table: "SchoolProfileSettings",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_SchoolOwners_SchoolRegistrationId_IsDeleted",
                schema: "School",
                table: "SchoolOwners",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_SchoolMediums_IsDeleted",
                schema: "School",
                table: "SchoolMediums",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolBranches_SchoolRegistrationId_IsDeleted",
                schema: "Administration",
                table: "SchoolBranches",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_SchoolAssets_SchoolRegistrationId_IsDeleted",
                table: "SchoolAssets",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_SalaryStructures_SchoolRegistrationId_IsDeleted",
                schema: "Payroll",
                table: "SalaryStructures",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_SalaryStructureItems_SchoolRegistrationId_IsDeleted",
                schema: "Payroll",
                table: "SalaryStructureItems",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_SalaryGrades_SchoolRegistrationId_IsDeleted",
                table: "SalaryGrades",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_SalaryComponents_SchoolRegistrationId_IsDeleted",
                schema: "Payroll",
                table: "SalaryComponents",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_SalaryArrears_SchoolRegistrationId_IsDeleted",
                schema: "Payroll",
                table: "SalaryArrears",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_SalaryAdvances_SchoolRegistrationId_IsDeleted",
                schema: "Payroll",
                table: "SalaryAdvances",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_RouteStopMappings_SchoolRegistrationId_IsDeleted",
                schema: "Transport",
                table: "RouteStopMappings",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_RouteAssignments_SchoolRegistrationId_IsDeleted",
                schema: "Transport",
                table: "RouteAssignments",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_RoomTransferHistories_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "RoomTransferHistories",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "Rooms",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_RoomCategories_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "RoomCategories",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_RfidScanLogs_SchoolRegistrationId_IsDeleted",
                schema: "Transport",
                table: "RfidScanLogs",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_RequestForQuotations_SchoolRegistrationId_IsDeleted",
                schema: "Inventory",
                table: "RequestForQuotations",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_ReportTemplates_SchoolRegistrationId_IsDeleted",
                schema: "Analytics",
                table: "ReportTemplates",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_ReportCards_SchoolRegistrationId_IsDeleted",
                table: "ReportCards",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_ReligionMasters_SchoolRegistrationId_IsDeleted",
                table: "ReligionMasters",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_ReimbursementClaims_SchoolRegistrationId_IsDeleted",
                schema: "Payroll",
                table: "ReimbursementClaims",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_QuickPolls_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "QuickPolls",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_QualityInspections_SchoolRegistrationId_IsDeleted",
                schema: "Inventory",
                table: "QualityInspections",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_QualificationMasters_SchoolRegistrationId_IsDeleted",
                table: "QualificationMasters",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_PushNotifications_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "PushNotifications",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReturns_SchoolRegistrationId_IsDeleted",
                schema: "Inventory",
                table: "PurchaseReturns",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequisitions_SchoolRegistrationId_IsDeleted",
                schema: "Inventory",
                table: "PurchaseRequisitions",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequisitionItems_SchoolRegistrationId_IsDeleted",
                schema: "Inventory",
                table: "PurchaseRequisitionItems",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_SchoolRegistrationId_IsDeleted",
                schema: "Inventory",
                table: "PurchaseOrders",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderItems_SchoolRegistrationId_IsDeleted",
                schema: "Inventory",
                table: "PurchaseOrderItems",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Programs_SchoolRegistrationId_IsDeleted",
                table: "Programs",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_PollVotes_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "PollVotes",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceReviews_SchoolRegistrationId_IsDeleted",
                table: "PerformanceReviews",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_PayrollRuns_SchoolRegistrationId_IsDeleted",
                schema: "Payroll",
                table: "PayrollRuns",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_PayrollRunDetails_SchoolRegistrationId_IsDeleted",
                schema: "Payroll",
                table: "PayrollRunDetails",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentGateways_SchoolRegistrationId_IsDeleted",
                table: "PaymentGateways",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_PayGroups_SchoolRegistrationId_IsDeleted",
                schema: "Payroll",
                table: "PayGroups",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_ParentTeacherChats_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "ParentTeacherChats",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_ParentStudentMappings_SchoolRegistrationId_IsDeleted",
                table: "ParentStudentMappings",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationProfiles_SchoolRegistrationId_IsDeleted",
                schema: "School",
                table: "OrganizationProfiles",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationProfileAudits_SchoolRegistrationId_IsDeleted",
                schema: "School",
                table: "OrganizationProfileAudits",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_OnlinePaymentOrders_SchoolRegistrationId_IsDeleted",
                table: "OnlinePaymentOrders",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_OnlineClasses_SchoolRegistrationId_IsDeleted",
                table: "OnlineClasses",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_NoticePeriods_SchoolRegistrationId_IsDeleted",
                table: "NoticePeriods",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_NoticeBoards_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "NoticeBoards",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Modules_IsDeleted",
                table: "Modules",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ModulePermissions_IsDeleted",
                table: "ModulePermissions",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_MessMenus_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "MessMenus",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Menus_IsDeleted",
                table: "Menus",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_MenuPermessions_IsDeleted",
                table: "MenuPermessions",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_MealAttendances_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "MealAttendances",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_LoanRepaymentSchedules_SchoolRegistrationId_IsDeleted",
                schema: "Payroll",
                table: "LoanRepaymentSchedules",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_LibraryMembers_SchoolRegistrationId_IsDeleted",
                schema: "Library",
                table: "LibraryMembers",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_LibraryFineRules_SchoolRegistrationId_IsDeleted",
                schema: "Library",
                table: "LibraryFineRules",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_LibraryBooks_SchoolRegistrationId_IsDeleted",
                schema: "Library",
                table: "LibraryBooks",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_LessonPlans_SchoolRegistrationId_IsDeleted",
                table: "LessonPlans",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_LeaveTypes_SchoolRegistrationId_IsDeleted",
                table: "LeaveTypes",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_LeaveSettings_SchoolRegistrationId_IsDeleted",
                table: "LeaveSettings",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_SchoolRegistrationId_IsDeleted",
                table: "LeaveRequests",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_LeaveBalances_SchoolRegistrationId_IsDeleted",
                table: "LeaveBalances",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_LaundryTransactions_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "LaundryTransactions",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_KpiMetrics_SchoolRegistrationId_IsDeleted",
                table: "KpiMetrics",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryLines_SchoolRegistrationId_IsDeleted",
                schema: "Finance",
                table: "JournalEntryLines",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_SchoolRegistrationId_IsDeleted",
                schema: "Finance",
                table: "JournalEntries",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_JobPostings_SchoolRegistrationId_IsDeleted",
                table: "JobPostings",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_SchoolRegistrationId_IsDeleted",
                table: "JobApplications",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_ItemCategories_SchoolRegistrationId_IsDeleted",
                schema: "Inventory",
                table: "ItemCategories",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_SchoolRegistrationId_IsDeleted",
                schema: "Inventory",
                table: "InventoryItems",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_HostelWardens_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "HostelWardens",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_HostelVisitors_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "HostelVisitors",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Hostels_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "Hostels",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_HostelMedicalLogs_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "HostelMedicalLogs",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_HostelMaintenances_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "HostelMaintenances",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_HostelInventories_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "HostelInventories",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_HostelGatePasses_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "HostelGatePasses",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_HostelFeePayments_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "HostelFeePayments",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_HostelFeeAllocations_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "HostelFeeAllocations",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_HostelDisciplines_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "HostelDisciplines",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_HostelComplaints_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "HostelComplaints",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_HostelAttendances_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "HostelAttendances",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_HostelAdmissions_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "HostelAdmissions",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_HomeworkSubmissions_SchoolRegistrationId_IsDeleted",
                table: "HomeworkSubmissions",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Homeworks_SchoolRegistrationId_IsDeleted",
                table: "Homeworks",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_HolidayMasters_SchoolRegistrationId_IsDeleted",
                table: "HolidayMasters",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_GroupChatRooms_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "GroupChatRooms",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_GroupChatMessages_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "GroupChatMessages",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_GroupChatMembers_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "GroupChatMembers",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_GradeConfigs_SchoolRegistrationId_IsDeleted",
                table: "GradeConfigs",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceiptNotes_SchoolRegistrationId_IsDeleted",
                schema: "Inventory",
                table: "GoodsReceiptNotes",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceiptNoteItems_SchoolRegistrationId_IsDeleted",
                schema: "Inventory",
                table: "GoodsReceiptNoteItems",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_FuelLogs_SchoolRegistrationId_IsDeleted",
                schema: "Transport",
                table: "FuelLogs",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Floors_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "Floors",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_FineRules_SchoolRegistrationId_IsDeleted",
                table: "FineRules",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_FinancialYears_SchoolRegistrationId_IsDeleted",
                schema: "Finance",
                table: "FinancialYears",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_FeeTypes_SchoolRegistrationId_IsDeleted",
                table: "FeeTypes",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_FeeStructures_SchoolRegistrationId_IsDeleted",
                table: "FeeStructures",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_FeeStructureItems_IsDeleted",
                table: "FeeStructureItems",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_FeeRefunds_SchoolRegistrationId_IsDeleted",
                table: "FeeRefunds",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_FeePayments_SchoolRegistrationId_IsDeleted",
                table: "FeePayments",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_FeeInstallments_SchoolRegistrationId_IsDeleted",
                table: "FeeInstallments",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_FeeFines_SchoolRegistrationId_IsDeleted",
                table: "FeeFines",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackSurveys_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "FeedbackSurveys",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Faculties_SchoolRegistrationId_IsDeleted",
                table: "Faculties",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_ExamSchedules_SchoolRegistrationId_IsDeleted",
                table: "ExamSchedules",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Exams_SchoolRegistrationId_IsDeleted",
                table: "Exams",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_ExamResults_SchoolRegistrationId_IsDeleted",
                table: "ExamResults",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Events_SchoolRegistrationId_IsDeleted",
                table: "Events",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_EmploymentStatuses_SchoolRegistrationId_IsDeleted",
                table: "EmploymentStatuses",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTypes_SchoolRegistrationId_IsDeleted",
                table: "EmployeeTypes",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSalaryDetails_SchoolRegistrationId_IsDeleted",
                table: "EmployeeSalaryDetails",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSalaryAllocations_SchoolRegistrationId_IsDeleted",
                schema: "Payroll",
                table: "EmployeeSalaryAllocations",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_SchoolRegistrationId_IsDeleted",
                table: "Employees",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLoans_SchoolRegistrationId_IsDeleted",
                schema: "Payroll",
                table: "EmployeeLoans",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeExperiences_SchoolRegistrationId_IsDeleted",
                table: "EmployeeExperiences",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEducations_SchoolRegistrationId_IsDeleted",
                table: "EmployeeEducations",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDocuments_SchoolRegistrationId_IsDeleted",
                table: "EmployeeDocuments",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDetails_IsDeleted",
                table: "EmployeeDetails",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeCategories_SchoolRegistrationId_IsDeleted",
                table: "EmployeeCategories",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeBonuses_SchoolRegistrationId_IsDeleted",
                schema: "Payroll",
                table: "EmployeeBonuses",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeBankDetails_SchoolRegistrationId_IsDeleted",
                table: "EmployeeBankDetails",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplates_SchoolRegistrationId_IsDeleted",
                table: "EmailTemplates",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_EmailServerSettings_SchoolRegistrationId_IsDeleted",
                table: "EmailServerSettings",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_EmailLogs_SchoolRegistrationId_IsDeleted",
                table: "EmailLogs",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_EmailBrandings_SchoolRegistrationId_IsDeleted",
                table: "EmailBrandings",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_EducationLevels_SchoolRegistrationId_IsDeleted",
                table: "EducationLevels",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_EducationalDetails_SchoolRegistrationId_IsDeleted",
                table: "EducationalDetails",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_DigitalResources_SchoolRegistrationId_IsDeleted",
                schema: "Library",
                table: "DigitalResources",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Designations_SchoolRegistrationId_IsDeleted",
                table: "Designations",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Departments_SchoolRegistrationId_IsDeleted",
                table: "Departments",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_DashboardWidgets_SchoolRegistrationId_IsDeleted",
                schema: "Analytics",
                table: "DashboardWidgets",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_DashboardConfigs_SchoolRegistrationId_IsDeleted",
                schema: "Analytics",
                table: "DashboardConfigs",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_SchoolRegistrationId_IsDeleted",
                table: "Courses",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Countries_IsDeleted",
                table: "Countries",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_CostCenters_SchoolRegistrationId_IsDeleted",
                schema: "Finance",
                table: "CostCenters",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Conductors_SchoolRegistrationId_IsDeleted",
                schema: "Transport",
                table: "Conductors",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationTemplates_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "CommunicationTemplates",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationMeetings_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "CommunicationMeetings",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_CoaAccounts_SchoolRegistrationId_IsDeleted",
                schema: "Finance",
                table: "CoaAccounts",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Classes_SchoolRegistrationId_IsDeleted",
                table: "Classes",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Cities_IsDeleted",
                table: "Cities",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Circulars_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "Circulars",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_ChequeBooks_SchoolRegistrationId_IsDeleted",
                schema: "Finance",
                table: "ChequeBooks",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_CentralNotifications_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "CentralNotifications",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryModules_IsDeleted",
                table: "CategoryModules",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_CashBankTransactions_SchoolRegistrationId_IsDeleted",
                schema: "Finance",
                table: "CashBankTransactions",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_SchoolRegistrationId_IsDeleted",
                table: "Candidates",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Campuses_SchoolRegistrationId_IsDeleted",
                schema: "School",
                table: "Campuses",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "Buildings",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPlans_SchoolRegistrationId_IsDeleted",
                schema: "Finance",
                table: "BudgetPlans",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Branches_SchoolRegistrationId_IsDeleted",
                table: "Branches",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_BookVendors_SchoolRegistrationId_IsDeleted",
                schema: "Library",
                table: "BookVendors",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_BookReservations_SchoolRegistrationId_IsDeleted",
                schema: "Library",
                table: "BookReservations",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_BookPublishers_SchoolRegistrationId_IsDeleted",
                schema: "Library",
                table: "BookPublishers",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_BookIssueLogs_SchoolRegistrationId_IsDeleted",
                schema: "Library",
                table: "BookIssueLogs",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_BookCategories_SchoolRegistrationId_IsDeleted",
                schema: "Library",
                table: "BookCategories",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_BookAuthors_SchoolRegistrationId_IsDeleted",
                schema: "Library",
                table: "BookAuthors",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_BloodGroupMasters_SchoolRegistrationId_IsDeleted",
                table: "BloodGroupMasters",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Beds_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "Beds",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_BedReservations_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "BedReservations",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Batches_SchoolRegistrationId_IsDeleted",
                table: "Batches",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_SchoolRegistrationId_IsDeleted",
                table: "Attendances",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceLogs_SchoolRegistrationId_IsDeleted",
                table: "AttendanceLogs",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentSubmissions_SchoolRegistrationId_IsDeleted",
                table: "AssignmentSubmissions",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_SchoolRegistrationId_IsDeleted",
                table: "Assignments",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_AssetMaintenanceLogs_SchoolRegistrationId_IsDeleted",
                schema: "Inventory",
                table: "AssetMaintenanceLogs",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_AssetDepreciationLogs_SchoolRegistrationId_IsDeleted",
                schema: "Inventory",
                table: "AssetDepreciationLogs",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_AssetAssignments_SchoolRegistrationId_IsDeleted",
                table: "AssetAssignments",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalLogs_SchoolRegistrationId_IsDeleted",
                schema: "Administration",
                table: "ApprovalLogs",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "Announcements",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_AnalyticsKpis_SchoolRegistrationId_IsDeleted",
                schema: "Analytics",
                table: "AnalyticsKpis",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_AiPredictions_SchoolRegistrationId_IsDeleted",
                schema: "AI",
                table: "AiPredictions",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_AiGenerations_SchoolRegistrationId_IsDeleted",
                schema: "AI",
                table: "AiGenerations",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_AiChatSessions_SchoolRegistrationId_IsDeleted",
                schema: "AI",
                table: "AiChatSessions",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_AiChatMessages_SchoolRegistrationId_IsDeleted",
                schema: "AI",
                table: "AiChatMessages",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_AffiliationBoards_IsDeleted",
                schema: "School",
                table: "AffiliationBoards",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Affiliateds_SchoolRegistrationId_IsDeleted",
                table: "Affiliateds",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionRules_SchoolRegistrationId_IsDeleted",
                table: "AdmissionRules",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionFormConfigs_SchoolRegistrationId_IsDeleted",
                table: "AdmissionFormConfigs",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionAuditLogs_IsDeleted",
                table: "AdmissionAuditLogs",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionApplications_SchoolRegistrationId_IsDeleted",
                table: "AdmissionApplications",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_AdminAuditLogs_SchoolRegistrationId_IsDeleted",
                schema: "Administration",
                table: "AdminAuditLogs",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_AcademicYears_SchoolRegistrationId_IsDeleted",
                table: "AcademicYears",
                columns: new[] { "SchoolRegistrationId", "IsDeleted" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_YearSemesters_SchoolRegistrationId_IsDeleted",
                table: "YearSemesters");

            migrationBuilder.DropIndex(
                name: "IX_WorkflowSteps_SchoolRegistrationId_IsDeleted",
                schema: "Administration",
                table: "WorkflowSteps");

            migrationBuilder.DropIndex(
                name: "IX_WorkflowInstances_SchoolRegistrationId_IsDeleted",
                schema: "Administration",
                table: "WorkflowInstances");

            migrationBuilder.DropIndex(
                name: "IX_WorkflowDefinitions_SchoolRegistrationId_IsDeleted",
                schema: "Administration",
                table: "WorkflowDefinitions");

            migrationBuilder.DropIndex(
                name: "IX_WhatsAppLogs_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "WhatsAppLogs");

            migrationBuilder.DropIndex(
                name: "IX_WeekOffs_SchoolRegistrationId_IsDeleted",
                table: "WeekOffs");

            migrationBuilder.DropIndex(
                name: "IX_Warehouses_SchoolRegistrationId_IsDeleted",
                schema: "Inventory",
                table: "Warehouses");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseBins_SchoolRegistrationId_IsDeleted",
                schema: "Inventory",
                table: "WarehouseBins");

            migrationBuilder.DropIndex(
                name: "IX_Vendors_SchoolRegistrationId_IsDeleted",
                schema: "Inventory",
                table: "Vendors");

            migrationBuilder.DropIndex(
                name: "IX_VendorQuotations_SchoolRegistrationId_IsDeleted",
                schema: "Inventory",
                table: "VendorQuotations");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_SchoolRegistrationId_IsDeleted",
                schema: "Transport",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_VehicleMaintenances_SchoolRegistrationId_IsDeleted",
                schema: "Transport",
                table: "VehicleMaintenances");

            migrationBuilder.DropIndex(
                name: "IX_VehicleIncidents_SchoolRegistrationId_IsDeleted",
                schema: "Transport",
                table: "VehicleIncidents");

            migrationBuilder.DropIndex(
                name: "IX_TransportTrips_SchoolRegistrationId_IsDeleted",
                schema: "Transport",
                table: "TransportTrips");

            migrationBuilder.DropIndex(
                name: "IX_TransportStops_SchoolRegistrationId_IsDeleted",
                schema: "Transport",
                table: "TransportStops");

            migrationBuilder.DropIndex(
                name: "IX_TransportRoutes_SchoolRegistrationId_IsDeleted",
                schema: "Transport",
                table: "TransportRoutes");

            migrationBuilder.DropIndex(
                name: "IX_TransportInventories_SchoolRegistrationId_IsDeleted",
                schema: "Transport",
                table: "TransportInventories");

            migrationBuilder.DropIndex(
                name: "IX_TransportGateLogs_SchoolRegistrationId_IsDeleted",
                schema: "Transport",
                table: "TransportGateLogs");

            migrationBuilder.DropIndex(
                name: "IX_TransportAllocations_SchoolRegistrationId_IsDeleted",
                schema: "Transport",
                table: "TransportAllocations");

            migrationBuilder.DropIndex(
                name: "IX_TrainingPrograms_SchoolRegistrationId_IsDeleted",
                table: "TrainingPrograms");

            migrationBuilder.DropIndex(
                name: "IX_TrainingEnrollments_SchoolRegistrationId_IsDeleted",
                table: "TrainingEnrollments");

            migrationBuilder.DropIndex(
                name: "IX_TimetableSlots_SchoolRegistrationId_IsDeleted",
                table: "TimetableSlots");

            migrationBuilder.DropIndex(
                name: "IX_TimetablePeriods_SchoolRegistrationId_IsDeleted",
                table: "TimetablePeriods");

            migrationBuilder.DropIndex(
                name: "IX_Timesheets_SchoolRegistrationId_IsDeleted",
                table: "Timesheets");

            migrationBuilder.DropIndex(
                name: "IX_TimesheetEntries_SchoolRegistrationId_IsDeleted",
                table: "TimesheetEntries");

            migrationBuilder.DropIndex(
                name: "IX_TicketResponses_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "TicketResponses");

            migrationBuilder.DropIndex(
                name: "IX_TaxConfigs_SchoolRegistrationId_IsDeleted",
                schema: "Finance",
                table: "TaxConfigs");

            migrationBuilder.DropIndex(
                name: "IX_SyllabusChapters_SchoolRegistrationId_IsDeleted",
                table: "SyllabusChapters");

            migrationBuilder.DropIndex(
                name: "IX_SurveyResponses_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "SurveyResponses");

            migrationBuilder.DropIndex(
                name: "IX_SurveyQuestions_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "SurveyQuestions");

            migrationBuilder.DropIndex(
                name: "IX_SupportTickets_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "SupportTickets");

            migrationBuilder.DropIndex(
                name: "IX_SubMenus_IsDeleted",
                table: "SubMenus");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_SchoolRegistrationId_IsDeleted",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_SubjectEnrollments_SchoolRegistrationId_IsDeleted",
                table: "SubjectEnrollments");

            migrationBuilder.DropIndex(
                name: "IX_StudentScholarships_SchoolRegistrationId_IsDeleted",
                table: "StudentScholarships");

            migrationBuilder.DropIndex(
                name: "IX_Students_SchoolRegistrationId_IsDeleted",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_StudentPromotions_SchoolRegistrationId_IsDeleted",
                table: "StudentPromotions");

            migrationBuilder.DropIndex(
                name: "IX_StudentAttendances_SchoolRegistrationId_IsDeleted",
                table: "StudentAttendances");

            migrationBuilder.DropIndex(
                name: "IX_Stores_SchoolRegistrationId_IsDeleted",
                schema: "Inventory",
                table: "Stores");

            migrationBuilder.DropIndex(
                name: "IX_StockTransactions_SchoolRegistrationId_IsDeleted",
                schema: "Inventory",
                table: "StockTransactions");

            migrationBuilder.DropIndex(
                name: "IX_StockIssues_SchoolRegistrationId_IsDeleted",
                schema: "Inventory",
                table: "StockIssues");

            migrationBuilder.DropIndex(
                name: "IX_StatutoryComplianceConfigs_SchoolRegistrationId_IsDeleted",
                schema: "Payroll",
                table: "StatutoryComplianceConfigs");

            migrationBuilder.DropIndex(
                name: "IX_States_IsDeleted",
                table: "States");

            migrationBuilder.DropIndex(
                name: "IX_Specializations_SchoolRegistrationId_IsDeleted",
                table: "Specializations");

            migrationBuilder.DropIndex(
                name: "IX_SmsLogs_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "SmsLogs");

            migrationBuilder.DropIndex(
                name: "IX_ShiftMasters_SchoolRegistrationId_IsDeleted",
                table: "ShiftMasters");

            migrationBuilder.DropIndex(
                name: "IX_SharedDocuments_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "SharedDocuments");

            migrationBuilder.DropIndex(
                name: "IX_SchoolTypes_IsDeleted",
                schema: "School",
                table: "SchoolTypes");

            migrationBuilder.DropIndex(
                name: "IX_SchoolSubscriptions_SchoolRegistrationId_IsDeleted",
                schema: "School",
                table: "SchoolSubscriptions");

            migrationBuilder.DropIndex(
                name: "IX_SchoolRegistrations_IsDeleted",
                schema: "School",
                table: "SchoolRegistrations");

            migrationBuilder.DropIndex(
                name: "IX_SchoolProfileSettings_SchoolRegistrationId_IsDeleted",
                schema: "School",
                table: "SchoolProfileSettings");

            migrationBuilder.DropIndex(
                name: "IX_SchoolOwners_SchoolRegistrationId_IsDeleted",
                schema: "School",
                table: "SchoolOwners");

            migrationBuilder.DropIndex(
                name: "IX_SchoolMediums_IsDeleted",
                schema: "School",
                table: "SchoolMediums");

            migrationBuilder.DropIndex(
                name: "IX_SchoolBranches_SchoolRegistrationId_IsDeleted",
                schema: "Administration",
                table: "SchoolBranches");

            migrationBuilder.DropIndex(
                name: "IX_SchoolAssets_SchoolRegistrationId_IsDeleted",
                table: "SchoolAssets");

            migrationBuilder.DropIndex(
                name: "IX_SalaryStructures_SchoolRegistrationId_IsDeleted",
                schema: "Payroll",
                table: "SalaryStructures");

            migrationBuilder.DropIndex(
                name: "IX_SalaryStructureItems_SchoolRegistrationId_IsDeleted",
                schema: "Payroll",
                table: "SalaryStructureItems");

            migrationBuilder.DropIndex(
                name: "IX_SalaryGrades_SchoolRegistrationId_IsDeleted",
                table: "SalaryGrades");

            migrationBuilder.DropIndex(
                name: "IX_SalaryComponents_SchoolRegistrationId_IsDeleted",
                schema: "Payroll",
                table: "SalaryComponents");

            migrationBuilder.DropIndex(
                name: "IX_SalaryArrears_SchoolRegistrationId_IsDeleted",
                schema: "Payroll",
                table: "SalaryArrears");

            migrationBuilder.DropIndex(
                name: "IX_SalaryAdvances_SchoolRegistrationId_IsDeleted",
                schema: "Payroll",
                table: "SalaryAdvances");

            migrationBuilder.DropIndex(
                name: "IX_RouteStopMappings_SchoolRegistrationId_IsDeleted",
                schema: "Transport",
                table: "RouteStopMappings");

            migrationBuilder.DropIndex(
                name: "IX_RouteAssignments_SchoolRegistrationId_IsDeleted",
                schema: "Transport",
                table: "RouteAssignments");

            migrationBuilder.DropIndex(
                name: "IX_RoomTransferHistories_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "RoomTransferHistories");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_RoomCategories_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "RoomCategories");

            migrationBuilder.DropIndex(
                name: "IX_RfidScanLogs_SchoolRegistrationId_IsDeleted",
                schema: "Transport",
                table: "RfidScanLogs");

            migrationBuilder.DropIndex(
                name: "IX_RequestForQuotations_SchoolRegistrationId_IsDeleted",
                schema: "Inventory",
                table: "RequestForQuotations");

            migrationBuilder.DropIndex(
                name: "IX_ReportTemplates_SchoolRegistrationId_IsDeleted",
                schema: "Analytics",
                table: "ReportTemplates");

            migrationBuilder.DropIndex(
                name: "IX_ReportCards_SchoolRegistrationId_IsDeleted",
                table: "ReportCards");

            migrationBuilder.DropIndex(
                name: "IX_ReligionMasters_SchoolRegistrationId_IsDeleted",
                table: "ReligionMasters");

            migrationBuilder.DropIndex(
                name: "IX_ReimbursementClaims_SchoolRegistrationId_IsDeleted",
                schema: "Payroll",
                table: "ReimbursementClaims");

            migrationBuilder.DropIndex(
                name: "IX_QuickPolls_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "QuickPolls");

            migrationBuilder.DropIndex(
                name: "IX_QualityInspections_SchoolRegistrationId_IsDeleted",
                schema: "Inventory",
                table: "QualityInspections");

            migrationBuilder.DropIndex(
                name: "IX_QualificationMasters_SchoolRegistrationId_IsDeleted",
                table: "QualificationMasters");

            migrationBuilder.DropIndex(
                name: "IX_PushNotifications_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "PushNotifications");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseReturns_SchoolRegistrationId_IsDeleted",
                schema: "Inventory",
                table: "PurchaseReturns");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequisitions_SchoolRegistrationId_IsDeleted",
                schema: "Inventory",
                table: "PurchaseRequisitions");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequisitionItems_SchoolRegistrationId_IsDeleted",
                schema: "Inventory",
                table: "PurchaseRequisitionItems");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_SchoolRegistrationId_IsDeleted",
                schema: "Inventory",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrderItems_SchoolRegistrationId_IsDeleted",
                schema: "Inventory",
                table: "PurchaseOrderItems");

            migrationBuilder.DropIndex(
                name: "IX_Programs_SchoolRegistrationId_IsDeleted",
                table: "Programs");

            migrationBuilder.DropIndex(
                name: "IX_PollVotes_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "PollVotes");

            migrationBuilder.DropIndex(
                name: "IX_PerformanceReviews_SchoolRegistrationId_IsDeleted",
                table: "PerformanceReviews");

            migrationBuilder.DropIndex(
                name: "IX_PayrollRuns_SchoolRegistrationId_IsDeleted",
                schema: "Payroll",
                table: "PayrollRuns");

            migrationBuilder.DropIndex(
                name: "IX_PayrollRunDetails_SchoolRegistrationId_IsDeleted",
                schema: "Payroll",
                table: "PayrollRunDetails");

            migrationBuilder.DropIndex(
                name: "IX_PaymentGateways_SchoolRegistrationId_IsDeleted",
                table: "PaymentGateways");

            migrationBuilder.DropIndex(
                name: "IX_PayGroups_SchoolRegistrationId_IsDeleted",
                schema: "Payroll",
                table: "PayGroups");

            migrationBuilder.DropIndex(
                name: "IX_ParentTeacherChats_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "ParentTeacherChats");

            migrationBuilder.DropIndex(
                name: "IX_ParentStudentMappings_SchoolRegistrationId_IsDeleted",
                table: "ParentStudentMappings");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationProfiles_SchoolRegistrationId_IsDeleted",
                schema: "School",
                table: "OrganizationProfiles");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationProfileAudits_SchoolRegistrationId_IsDeleted",
                schema: "School",
                table: "OrganizationProfileAudits");

            migrationBuilder.DropIndex(
                name: "IX_OnlinePaymentOrders_SchoolRegistrationId_IsDeleted",
                table: "OnlinePaymentOrders");

            migrationBuilder.DropIndex(
                name: "IX_OnlineClasses_SchoolRegistrationId_IsDeleted",
                table: "OnlineClasses");

            migrationBuilder.DropIndex(
                name: "IX_NoticePeriods_SchoolRegistrationId_IsDeleted",
                table: "NoticePeriods");

            migrationBuilder.DropIndex(
                name: "IX_NoticeBoards_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "NoticeBoards");

            migrationBuilder.DropIndex(
                name: "IX_Modules_IsDeleted",
                table: "Modules");

            migrationBuilder.DropIndex(
                name: "IX_ModulePermissions_IsDeleted",
                table: "ModulePermissions");

            migrationBuilder.DropIndex(
                name: "IX_MessMenus_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "MessMenus");

            migrationBuilder.DropIndex(
                name: "IX_Menus_IsDeleted",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_MenuPermessions_IsDeleted",
                table: "MenuPermessions");

            migrationBuilder.DropIndex(
                name: "IX_MealAttendances_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "MealAttendances");

            migrationBuilder.DropIndex(
                name: "IX_LoanRepaymentSchedules_SchoolRegistrationId_IsDeleted",
                schema: "Payroll",
                table: "LoanRepaymentSchedules");

            migrationBuilder.DropIndex(
                name: "IX_LibraryMembers_SchoolRegistrationId_IsDeleted",
                schema: "Library",
                table: "LibraryMembers");

            migrationBuilder.DropIndex(
                name: "IX_LibraryFineRules_SchoolRegistrationId_IsDeleted",
                schema: "Library",
                table: "LibraryFineRules");

            migrationBuilder.DropIndex(
                name: "IX_LibraryBooks_SchoolRegistrationId_IsDeleted",
                schema: "Library",
                table: "LibraryBooks");

            migrationBuilder.DropIndex(
                name: "IX_LessonPlans_SchoolRegistrationId_IsDeleted",
                table: "LessonPlans");

            migrationBuilder.DropIndex(
                name: "IX_LeaveTypes_SchoolRegistrationId_IsDeleted",
                table: "LeaveTypes");

            migrationBuilder.DropIndex(
                name: "IX_LeaveSettings_SchoolRegistrationId_IsDeleted",
                table: "LeaveSettings");

            migrationBuilder.DropIndex(
                name: "IX_LeaveRequests_SchoolRegistrationId_IsDeleted",
                table: "LeaveRequests");

            migrationBuilder.DropIndex(
                name: "IX_LeaveBalances_SchoolRegistrationId_IsDeleted",
                table: "LeaveBalances");

            migrationBuilder.DropIndex(
                name: "IX_LaundryTransactions_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "LaundryTransactions");

            migrationBuilder.DropIndex(
                name: "IX_KpiMetrics_SchoolRegistrationId_IsDeleted",
                table: "KpiMetrics");

            migrationBuilder.DropIndex(
                name: "IX_JournalEntryLines_SchoolRegistrationId_IsDeleted",
                schema: "Finance",
                table: "JournalEntryLines");

            migrationBuilder.DropIndex(
                name: "IX_JournalEntries_SchoolRegistrationId_IsDeleted",
                schema: "Finance",
                table: "JournalEntries");

            migrationBuilder.DropIndex(
                name: "IX_JobPostings_SchoolRegistrationId_IsDeleted",
                table: "JobPostings");

            migrationBuilder.DropIndex(
                name: "IX_JobApplications_SchoolRegistrationId_IsDeleted",
                table: "JobApplications");

            migrationBuilder.DropIndex(
                name: "IX_ItemCategories_SchoolRegistrationId_IsDeleted",
                schema: "Inventory",
                table: "ItemCategories");

            migrationBuilder.DropIndex(
                name: "IX_InventoryItems_SchoolRegistrationId_IsDeleted",
                schema: "Inventory",
                table: "InventoryItems");

            migrationBuilder.DropIndex(
                name: "IX_HostelWardens_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "HostelWardens");

            migrationBuilder.DropIndex(
                name: "IX_HostelVisitors_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "HostelVisitors");

            migrationBuilder.DropIndex(
                name: "IX_Hostels_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "Hostels");

            migrationBuilder.DropIndex(
                name: "IX_HostelMedicalLogs_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "HostelMedicalLogs");

            migrationBuilder.DropIndex(
                name: "IX_HostelMaintenances_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "HostelMaintenances");

            migrationBuilder.DropIndex(
                name: "IX_HostelInventories_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "HostelInventories");

            migrationBuilder.DropIndex(
                name: "IX_HostelGatePasses_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "HostelGatePasses");

            migrationBuilder.DropIndex(
                name: "IX_HostelFeePayments_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "HostelFeePayments");

            migrationBuilder.DropIndex(
                name: "IX_HostelFeeAllocations_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "HostelFeeAllocations");

            migrationBuilder.DropIndex(
                name: "IX_HostelDisciplines_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "HostelDisciplines");

            migrationBuilder.DropIndex(
                name: "IX_HostelComplaints_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "HostelComplaints");

            migrationBuilder.DropIndex(
                name: "IX_HostelAttendances_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "HostelAttendances");

            migrationBuilder.DropIndex(
                name: "IX_HostelAdmissions_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "HostelAdmissions");

            migrationBuilder.DropIndex(
                name: "IX_HomeworkSubmissions_SchoolRegistrationId_IsDeleted",
                table: "HomeworkSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_Homeworks_SchoolRegistrationId_IsDeleted",
                table: "Homeworks");

            migrationBuilder.DropIndex(
                name: "IX_HolidayMasters_SchoolRegistrationId_IsDeleted",
                table: "HolidayMasters");

            migrationBuilder.DropIndex(
                name: "IX_GroupChatRooms_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "GroupChatRooms");

            migrationBuilder.DropIndex(
                name: "IX_GroupChatMessages_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "GroupChatMessages");

            migrationBuilder.DropIndex(
                name: "IX_GroupChatMembers_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "GroupChatMembers");

            migrationBuilder.DropIndex(
                name: "IX_GradeConfigs_SchoolRegistrationId_IsDeleted",
                table: "GradeConfigs");

            migrationBuilder.DropIndex(
                name: "IX_GoodsReceiptNotes_SchoolRegistrationId_IsDeleted",
                schema: "Inventory",
                table: "GoodsReceiptNotes");

            migrationBuilder.DropIndex(
                name: "IX_GoodsReceiptNoteItems_SchoolRegistrationId_IsDeleted",
                schema: "Inventory",
                table: "GoodsReceiptNoteItems");

            migrationBuilder.DropIndex(
                name: "IX_FuelLogs_SchoolRegistrationId_IsDeleted",
                schema: "Transport",
                table: "FuelLogs");

            migrationBuilder.DropIndex(
                name: "IX_Floors_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "Floors");

            migrationBuilder.DropIndex(
                name: "IX_FineRules_SchoolRegistrationId_IsDeleted",
                table: "FineRules");

            migrationBuilder.DropIndex(
                name: "IX_FinancialYears_SchoolRegistrationId_IsDeleted",
                schema: "Finance",
                table: "FinancialYears");

            migrationBuilder.DropIndex(
                name: "IX_FeeTypes_SchoolRegistrationId_IsDeleted",
                table: "FeeTypes");

            migrationBuilder.DropIndex(
                name: "IX_FeeStructures_SchoolRegistrationId_IsDeleted",
                table: "FeeStructures");

            migrationBuilder.DropIndex(
                name: "IX_FeeStructureItems_IsDeleted",
                table: "FeeStructureItems");

            migrationBuilder.DropIndex(
                name: "IX_FeeRefunds_SchoolRegistrationId_IsDeleted",
                table: "FeeRefunds");

            migrationBuilder.DropIndex(
                name: "IX_FeePayments_SchoolRegistrationId_IsDeleted",
                table: "FeePayments");

            migrationBuilder.DropIndex(
                name: "IX_FeeInstallments_SchoolRegistrationId_IsDeleted",
                table: "FeeInstallments");

            migrationBuilder.DropIndex(
                name: "IX_FeeFines_SchoolRegistrationId_IsDeleted",
                table: "FeeFines");

            migrationBuilder.DropIndex(
                name: "IX_FeedbackSurveys_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "FeedbackSurveys");

            migrationBuilder.DropIndex(
                name: "IX_Faculties_SchoolRegistrationId_IsDeleted",
                table: "Faculties");

            migrationBuilder.DropIndex(
                name: "IX_ExamSchedules_SchoolRegistrationId_IsDeleted",
                table: "ExamSchedules");

            migrationBuilder.DropIndex(
                name: "IX_Exams_SchoolRegistrationId_IsDeleted",
                table: "Exams");

            migrationBuilder.DropIndex(
                name: "IX_ExamResults_SchoolRegistrationId_IsDeleted",
                table: "ExamResults");

            migrationBuilder.DropIndex(
                name: "IX_Events_SchoolRegistrationId_IsDeleted",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_EmploymentStatuses_SchoolRegistrationId_IsDeleted",
                table: "EmploymentStatuses");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeTypes_SchoolRegistrationId_IsDeleted",
                table: "EmployeeTypes");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeSalaryDetails_SchoolRegistrationId_IsDeleted",
                table: "EmployeeSalaryDetails");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeSalaryAllocations_SchoolRegistrationId_IsDeleted",
                schema: "Payroll",
                table: "EmployeeSalaryAllocations");

            migrationBuilder.DropIndex(
                name: "IX_Employees_SchoolRegistrationId_IsDeleted",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeLoans_SchoolRegistrationId_IsDeleted",
                schema: "Payroll",
                table: "EmployeeLoans");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeExperiences_SchoolRegistrationId_IsDeleted",
                table: "EmployeeExperiences");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeEducations_SchoolRegistrationId_IsDeleted",
                table: "EmployeeEducations");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeDocuments_SchoolRegistrationId_IsDeleted",
                table: "EmployeeDocuments");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeDetails_IsDeleted",
                table: "EmployeeDetails");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeCategories_SchoolRegistrationId_IsDeleted",
                table: "EmployeeCategories");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeBonuses_SchoolRegistrationId_IsDeleted",
                schema: "Payroll",
                table: "EmployeeBonuses");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeBankDetails_SchoolRegistrationId_IsDeleted",
                table: "EmployeeBankDetails");

            migrationBuilder.DropIndex(
                name: "IX_EmailTemplates_SchoolRegistrationId_IsDeleted",
                table: "EmailTemplates");

            migrationBuilder.DropIndex(
                name: "IX_EmailServerSettings_SchoolRegistrationId_IsDeleted",
                table: "EmailServerSettings");

            migrationBuilder.DropIndex(
                name: "IX_EmailLogs_SchoolRegistrationId_IsDeleted",
                table: "EmailLogs");

            migrationBuilder.DropIndex(
                name: "IX_EmailBrandings_SchoolRegistrationId_IsDeleted",
                table: "EmailBrandings");

            migrationBuilder.DropIndex(
                name: "IX_EducationLevels_SchoolRegistrationId_IsDeleted",
                table: "EducationLevels");

            migrationBuilder.DropIndex(
                name: "IX_EducationalDetails_SchoolRegistrationId_IsDeleted",
                table: "EducationalDetails");

            migrationBuilder.DropIndex(
                name: "IX_DigitalResources_SchoolRegistrationId_IsDeleted",
                schema: "Library",
                table: "DigitalResources");

            migrationBuilder.DropIndex(
                name: "IX_Designations_SchoolRegistrationId_IsDeleted",
                table: "Designations");

            migrationBuilder.DropIndex(
                name: "IX_Departments_SchoolRegistrationId_IsDeleted",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_DashboardWidgets_SchoolRegistrationId_IsDeleted",
                schema: "Analytics",
                table: "DashboardWidgets");

            migrationBuilder.DropIndex(
                name: "IX_DashboardConfigs_SchoolRegistrationId_IsDeleted",
                schema: "Analytics",
                table: "DashboardConfigs");

            migrationBuilder.DropIndex(
                name: "IX_Courses_SchoolRegistrationId_IsDeleted",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Countries_IsDeleted",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_CostCenters_SchoolRegistrationId_IsDeleted",
                schema: "Finance",
                table: "CostCenters");

            migrationBuilder.DropIndex(
                name: "IX_Conductors_SchoolRegistrationId_IsDeleted",
                schema: "Transport",
                table: "Conductors");

            migrationBuilder.DropIndex(
                name: "IX_CommunicationTemplates_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "CommunicationTemplates");

            migrationBuilder.DropIndex(
                name: "IX_CommunicationMeetings_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "CommunicationMeetings");

            migrationBuilder.DropIndex(
                name: "IX_CoaAccounts_SchoolRegistrationId_IsDeleted",
                schema: "Finance",
                table: "CoaAccounts");

            migrationBuilder.DropIndex(
                name: "IX_Classes_SchoolRegistrationId_IsDeleted",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Cities_IsDeleted",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Circulars_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "Circulars");

            migrationBuilder.DropIndex(
                name: "IX_ChequeBooks_SchoolRegistrationId_IsDeleted",
                schema: "Finance",
                table: "ChequeBooks");

            migrationBuilder.DropIndex(
                name: "IX_CentralNotifications_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "CentralNotifications");

            migrationBuilder.DropIndex(
                name: "IX_CategoryModules_IsDeleted",
                table: "CategoryModules");

            migrationBuilder.DropIndex(
                name: "IX_CashBankTransactions_SchoolRegistrationId_IsDeleted",
                schema: "Finance",
                table: "CashBankTransactions");

            migrationBuilder.DropIndex(
                name: "IX_Candidates_SchoolRegistrationId_IsDeleted",
                table: "Candidates");

            migrationBuilder.DropIndex(
                name: "IX_Campuses_SchoolRegistrationId_IsDeleted",
                schema: "School",
                table: "Campuses");

            migrationBuilder.DropIndex(
                name: "IX_Buildings_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "Buildings");

            migrationBuilder.DropIndex(
                name: "IX_BudgetPlans_SchoolRegistrationId_IsDeleted",
                schema: "Finance",
                table: "BudgetPlans");

            migrationBuilder.DropIndex(
                name: "IX_Branches_SchoolRegistrationId_IsDeleted",
                table: "Branches");

            migrationBuilder.DropIndex(
                name: "IX_BookVendors_SchoolRegistrationId_IsDeleted",
                schema: "Library",
                table: "BookVendors");

            migrationBuilder.DropIndex(
                name: "IX_BookReservations_SchoolRegistrationId_IsDeleted",
                schema: "Library",
                table: "BookReservations");

            migrationBuilder.DropIndex(
                name: "IX_BookPublishers_SchoolRegistrationId_IsDeleted",
                schema: "Library",
                table: "BookPublishers");

            migrationBuilder.DropIndex(
                name: "IX_BookIssueLogs_SchoolRegistrationId_IsDeleted",
                schema: "Library",
                table: "BookIssueLogs");

            migrationBuilder.DropIndex(
                name: "IX_BookCategories_SchoolRegistrationId_IsDeleted",
                schema: "Library",
                table: "BookCategories");

            migrationBuilder.DropIndex(
                name: "IX_BookAuthors_SchoolRegistrationId_IsDeleted",
                schema: "Library",
                table: "BookAuthors");

            migrationBuilder.DropIndex(
                name: "IX_BloodGroupMasters_SchoolRegistrationId_IsDeleted",
                table: "BloodGroupMasters");

            migrationBuilder.DropIndex(
                name: "IX_Beds_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "Beds");

            migrationBuilder.DropIndex(
                name: "IX_BedReservations_SchoolRegistrationId_IsDeleted",
                schema: "Hostel",
                table: "BedReservations");

            migrationBuilder.DropIndex(
                name: "IX_Batches_SchoolRegistrationId_IsDeleted",
                table: "Batches");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_SchoolRegistrationId_IsDeleted",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_AttendanceLogs_SchoolRegistrationId_IsDeleted",
                table: "AttendanceLogs");

            migrationBuilder.DropIndex(
                name: "IX_AssignmentSubmissions_SchoolRegistrationId_IsDeleted",
                table: "AssignmentSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_SchoolRegistrationId_IsDeleted",
                table: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_AssetMaintenanceLogs_SchoolRegistrationId_IsDeleted",
                schema: "Inventory",
                table: "AssetMaintenanceLogs");

            migrationBuilder.DropIndex(
                name: "IX_AssetDepreciationLogs_SchoolRegistrationId_IsDeleted",
                schema: "Inventory",
                table: "AssetDepreciationLogs");

            migrationBuilder.DropIndex(
                name: "IX_AssetAssignments_SchoolRegistrationId_IsDeleted",
                table: "AssetAssignments");

            migrationBuilder.DropIndex(
                name: "IX_ApprovalLogs_SchoolRegistrationId_IsDeleted",
                schema: "Administration",
                table: "ApprovalLogs");

            migrationBuilder.DropIndex(
                name: "IX_Announcements_SchoolRegistrationId_IsDeleted",
                schema: "Communication",
                table: "Announcements");

            migrationBuilder.DropIndex(
                name: "IX_AnalyticsKpis_SchoolRegistrationId_IsDeleted",
                schema: "Analytics",
                table: "AnalyticsKpis");

            migrationBuilder.DropIndex(
                name: "IX_AiPredictions_SchoolRegistrationId_IsDeleted",
                schema: "AI",
                table: "AiPredictions");

            migrationBuilder.DropIndex(
                name: "IX_AiGenerations_SchoolRegistrationId_IsDeleted",
                schema: "AI",
                table: "AiGenerations");

            migrationBuilder.DropIndex(
                name: "IX_AiChatSessions_SchoolRegistrationId_IsDeleted",
                schema: "AI",
                table: "AiChatSessions");

            migrationBuilder.DropIndex(
                name: "IX_AiChatMessages_SchoolRegistrationId_IsDeleted",
                schema: "AI",
                table: "AiChatMessages");

            migrationBuilder.DropIndex(
                name: "IX_AffiliationBoards_IsDeleted",
                schema: "School",
                table: "AffiliationBoards");

            migrationBuilder.DropIndex(
                name: "IX_Affiliateds_SchoolRegistrationId_IsDeleted",
                table: "Affiliateds");

            migrationBuilder.DropIndex(
                name: "IX_AdmissionRules_SchoolRegistrationId_IsDeleted",
                table: "AdmissionRules");

            migrationBuilder.DropIndex(
                name: "IX_AdmissionFormConfigs_SchoolRegistrationId_IsDeleted",
                table: "AdmissionFormConfigs");

            migrationBuilder.DropIndex(
                name: "IX_AdmissionAuditLogs_IsDeleted",
                table: "AdmissionAuditLogs");

            migrationBuilder.DropIndex(
                name: "IX_AdmissionApplications_SchoolRegistrationId_IsDeleted",
                table: "AdmissionApplications");

            migrationBuilder.DropIndex(
                name: "IX_AdminAuditLogs_SchoolRegistrationId_IsDeleted",
                schema: "Administration",
                table: "AdminAuditLogs");

            migrationBuilder.DropIndex(
                name: "IX_AcademicYears_SchoolRegistrationId_IsDeleted",
                table: "AcademicYears");

            migrationBuilder.CreateIndex(
                name: "IX_YearSemesters_SchoolRegistrationId",
                table: "YearSemesters",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_WeekOffs_SchoolRegistrationId",
                table: "WeekOffs",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_SchoolRegistrationId",
                schema: "Inventory",
                table: "Warehouses",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseBins_SchoolRegistrationId",
                schema: "Inventory",
                table: "WarehouseBins",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_SchoolRegistrationId",
                schema: "Inventory",
                table: "Vendors",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorQuotations_SchoolRegistrationId",
                schema: "Inventory",
                table: "VendorQuotations",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_SchoolRegistrationId",
                schema: "Transport",
                table: "Vehicles",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleMaintenances_SchoolRegistrationId",
                schema: "Transport",
                table: "VehicleMaintenances",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleIncidents_SchoolRegistrationId",
                schema: "Transport",
                table: "VehicleIncidents",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportTrips_SchoolRegistrationId",
                schema: "Transport",
                table: "TransportTrips",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportStops_SchoolRegistrationId",
                schema: "Transport",
                table: "TransportStops",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportRoutes_SchoolRegistrationId",
                schema: "Transport",
                table: "TransportRoutes",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportInventories_SchoolRegistrationId",
                schema: "Transport",
                table: "TransportInventories",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportGateLogs_SchoolRegistrationId",
                schema: "Transport",
                table: "TransportGateLogs",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportAllocations_SchoolRegistrationId",
                schema: "Transport",
                table: "TransportAllocations",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPrograms_SchoolRegistrationId",
                table: "TrainingPrograms",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingEnrollments_SchoolRegistrationId",
                table: "TrainingEnrollments",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_TimetableSlots_SchoolRegistrationId",
                table: "TimetableSlots",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_TimetablePeriods_SchoolRegistrationId",
                table: "TimetablePeriods",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Timesheets_SchoolRegistrationId",
                table: "Timesheets",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_TimesheetEntries_SchoolRegistrationId",
                table: "TimesheetEntries",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxConfigs_SchoolRegistrationId",
                schema: "Finance",
                table: "TaxConfigs",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_SyllabusChapters_SchoolRegistrationId",
                table: "SyllabusChapters",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_SchoolRegistrationId",
                table: "Subjects",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectEnrollments_SchoolRegistrationId",
                table: "SubjectEnrollments",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentScholarships_SchoolRegistrationId",
                table: "StudentScholarships",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_SchoolRegistrationId",
                table: "Students",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentPromotions_SchoolRegistrationId",
                table: "StudentPromotions",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendances_SchoolRegistrationId",
                table: "StudentAttendances",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_SchoolRegistrationId",
                schema: "Inventory",
                table: "Stores",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_SchoolRegistrationId",
                schema: "Inventory",
                table: "StockTransactions",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_StockIssues_SchoolRegistrationId",
                schema: "Inventory",
                table: "StockIssues",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_StatutoryComplianceConfigs_SchoolRegistrationId",
                schema: "Payroll",
                table: "StatutoryComplianceConfigs",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_SchoolRegistrationId",
                table: "Specializations",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftMasters_SchoolRegistrationId",
                table: "ShiftMasters",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolSubscriptions_SchoolRegistrationId",
                schema: "School",
                table: "SchoolSubscriptions",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolOwners_SchoolRegistrationId",
                schema: "School",
                table: "SchoolOwners",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolAssets_SchoolRegistrationId",
                table: "SchoolAssets",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryStructures_SchoolRegistrationId",
                schema: "Payroll",
                table: "SalaryStructures",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryStructureItems_SchoolRegistrationId",
                schema: "Payroll",
                table: "SalaryStructureItems",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryGrades_SchoolRegistrationId",
                table: "SalaryGrades",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryComponents_SchoolRegistrationId",
                schema: "Payroll",
                table: "SalaryComponents",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryArrears_SchoolRegistrationId",
                schema: "Payroll",
                table: "SalaryArrears",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryAdvances_SchoolRegistrationId",
                schema: "Payroll",
                table: "SalaryAdvances",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteStopMappings_SchoolRegistrationId",
                schema: "Transport",
                table: "RouteStopMappings",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteAssignments_SchoolRegistrationId",
                schema: "Transport",
                table: "RouteAssignments",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomTransferHistories_SchoolRegistrationId",
                schema: "Hostel",
                table: "RoomTransferHistories",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_SchoolRegistrationId",
                schema: "Hostel",
                table: "Rooms",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomCategories_SchoolRegistrationId",
                schema: "Hostel",
                table: "RoomCategories",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_RfidScanLogs_SchoolRegistrationId",
                schema: "Transport",
                table: "RfidScanLogs",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestForQuotations_SchoolRegistrationId",
                schema: "Inventory",
                table: "RequestForQuotations",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCards_SchoolRegistrationId",
                table: "ReportCards",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_ReligionMasters_SchoolRegistrationId",
                table: "ReligionMasters",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_ReimbursementClaims_SchoolRegistrationId",
                schema: "Payroll",
                table: "ReimbursementClaims",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_QualityInspections_SchoolRegistrationId",
                schema: "Inventory",
                table: "QualityInspections",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_QualificationMasters_SchoolRegistrationId",
                table: "QualificationMasters",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReturns_SchoolRegistrationId",
                schema: "Inventory",
                table: "PurchaseReturns",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequisitions_SchoolRegistrationId",
                schema: "Inventory",
                table: "PurchaseRequisitions",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequisitionItems_SchoolRegistrationId",
                schema: "Inventory",
                table: "PurchaseRequisitionItems",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_SchoolRegistrationId",
                schema: "Inventory",
                table: "PurchaseOrders",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderItems_SchoolRegistrationId",
                schema: "Inventory",
                table: "PurchaseOrderItems",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Programs_SchoolRegistrationId",
                table: "Programs",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceReviews_SchoolRegistrationId",
                table: "PerformanceReviews",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollRuns_SchoolRegistrationId",
                schema: "Payroll",
                table: "PayrollRuns",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollRunDetails_SchoolRegistrationId",
                schema: "Payroll",
                table: "PayrollRunDetails",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentGateways_SchoolRegistrationId",
                table: "PaymentGateways",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_PayGroups_SchoolRegistrationId",
                schema: "Payroll",
                table: "PayGroups",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentStudentMappings_SchoolRegistrationId",
                table: "ParentStudentMappings",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationProfiles_SchoolRegistrationId",
                schema: "School",
                table: "OrganizationProfiles",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_OnlinePaymentOrders_SchoolRegistrationId",
                table: "OnlinePaymentOrders",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_OnlineClasses_SchoolRegistrationId",
                table: "OnlineClasses",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_NoticePeriods_SchoolRegistrationId",
                table: "NoticePeriods",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_MessMenus_SchoolRegistrationId",
                schema: "Hostel",
                table: "MessMenus",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_MealAttendances_SchoolRegistrationId",
                schema: "Hostel",
                table: "MealAttendances",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanRepaymentSchedules_SchoolRegistrationId",
                schema: "Payroll",
                table: "LoanRepaymentSchedules",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryMembers_SchoolRegistrationId",
                schema: "Library",
                table: "LibraryMembers",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryFineRules_SchoolRegistrationId",
                schema: "Library",
                table: "LibraryFineRules",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryBooks_SchoolRegistrationId",
                schema: "Library",
                table: "LibraryBooks",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonPlans_SchoolRegistrationId",
                table: "LessonPlans",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveTypes_SchoolRegistrationId",
                table: "LeaveTypes",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveSettings_SchoolRegistrationId",
                table: "LeaveSettings",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_SchoolRegistrationId",
                table: "LeaveRequests",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveBalances_SchoolRegistrationId",
                table: "LeaveBalances",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_LaundryTransactions_SchoolRegistrationId",
                schema: "Hostel",
                table: "LaundryTransactions",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_KpiMetrics_SchoolRegistrationId",
                table: "KpiMetrics",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryLines_SchoolRegistrationId",
                schema: "Finance",
                table: "JournalEntryLines",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_SchoolRegistrationId",
                schema: "Finance",
                table: "JournalEntries",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_JobPostings_SchoolRegistrationId",
                table: "JobPostings",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_SchoolRegistrationId",
                table: "JobApplications",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemCategories_SchoolRegistrationId",
                schema: "Inventory",
                table: "ItemCategories",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_SchoolRegistrationId",
                schema: "Inventory",
                table: "InventoryItems",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelWardens_SchoolRegistrationId",
                schema: "Hostel",
                table: "HostelWardens",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelVisitors_SchoolRegistrationId",
                schema: "Hostel",
                table: "HostelVisitors",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Hostels_SchoolRegistrationId",
                schema: "Hostel",
                table: "Hostels",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelMedicalLogs_SchoolRegistrationId",
                schema: "Hostel",
                table: "HostelMedicalLogs",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelMaintenances_SchoolRegistrationId",
                schema: "Hostel",
                table: "HostelMaintenances",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelInventories_SchoolRegistrationId",
                schema: "Hostel",
                table: "HostelInventories",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelGatePasses_SchoolRegistrationId",
                schema: "Hostel",
                table: "HostelGatePasses",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelFeePayments_SchoolRegistrationId",
                schema: "Hostel",
                table: "HostelFeePayments",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelFeeAllocations_SchoolRegistrationId",
                schema: "Hostel",
                table: "HostelFeeAllocations",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelDisciplines_SchoolRegistrationId",
                schema: "Hostel",
                table: "HostelDisciplines",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelComplaints_SchoolRegistrationId",
                schema: "Hostel",
                table: "HostelComplaints",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelAttendances_SchoolRegistrationId",
                schema: "Hostel",
                table: "HostelAttendances",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_HostelAdmissions_SchoolRegistrationId",
                schema: "Hostel",
                table: "HostelAdmissions",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeworkSubmissions_SchoolRegistrationId",
                table: "HomeworkSubmissions",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Homeworks_SchoolRegistrationId",
                table: "Homeworks",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_HolidayMasters_SchoolRegistrationId",
                table: "HolidayMasters",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeConfigs_SchoolRegistrationId",
                table: "GradeConfigs",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceiptNotes_SchoolRegistrationId",
                schema: "Inventory",
                table: "GoodsReceiptNotes",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceiptNoteItems_SchoolRegistrationId",
                schema: "Inventory",
                table: "GoodsReceiptNoteItems",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_FuelLogs_SchoolRegistrationId",
                schema: "Transport",
                table: "FuelLogs",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Floors_SchoolRegistrationId",
                schema: "Hostel",
                table: "Floors",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_FineRules_SchoolRegistrationId",
                table: "FineRules",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialYears_SchoolRegistrationId",
                schema: "Finance",
                table: "FinancialYears",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeTypes_SchoolRegistrationId",
                table: "FeeTypes",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeStructures_SchoolRegistrationId",
                table: "FeeStructures",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeRefunds_SchoolRegistrationId",
                table: "FeeRefunds",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_FeePayments_SchoolRegistrationId",
                table: "FeePayments",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeInstallments_SchoolRegistrationId",
                table: "FeeInstallments",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeFines_SchoolRegistrationId",
                table: "FeeFines",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Faculties_SchoolRegistrationId",
                table: "Faculties",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSchedules_SchoolRegistrationId",
                table: "ExamSchedules",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_SchoolRegistrationId",
                table: "Exams",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamResults_SchoolRegistrationId",
                table: "ExamResults",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_SchoolRegistrationId",
                table: "Events",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmploymentStatuses_SchoolRegistrationId",
                table: "EmploymentStatuses",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTypes_SchoolRegistrationId",
                table: "EmployeeTypes",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSalaryDetails_SchoolRegistrationId",
                table: "EmployeeSalaryDetails",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSalaryAllocations_SchoolRegistrationId",
                schema: "Payroll",
                table: "EmployeeSalaryAllocations",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_SchoolRegistrationId",
                table: "Employees",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLoans_SchoolRegistrationId",
                schema: "Payroll",
                table: "EmployeeLoans",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeExperiences_SchoolRegistrationId",
                table: "EmployeeExperiences",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEducations_SchoolRegistrationId",
                table: "EmployeeEducations",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDocuments_SchoolRegistrationId",
                table: "EmployeeDocuments",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeCategories_SchoolRegistrationId",
                table: "EmployeeCategories",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeBonuses_SchoolRegistrationId",
                schema: "Payroll",
                table: "EmployeeBonuses",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeBankDetails_SchoolRegistrationId",
                table: "EmployeeBankDetails",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplates_SchoolRegistrationId",
                table: "EmailTemplates",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailServerSettings_SchoolRegistrationId",
                table: "EmailServerSettings",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailLogs_SchoolRegistrationId",
                table: "EmailLogs",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailBrandings_SchoolRegistrationId",
                table: "EmailBrandings",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationLevels_SchoolRegistrationId",
                table: "EducationLevels",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationalDetails_SchoolRegistrationId",
                table: "EducationalDetails",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_DigitalResources_SchoolRegistrationId",
                schema: "Library",
                table: "DigitalResources",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Designations_SchoolRegistrationId",
                table: "Designations",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_SchoolRegistrationId",
                table: "Departments",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_SchoolRegistrationId",
                table: "Courses",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_CostCenters_SchoolRegistrationId",
                schema: "Finance",
                table: "CostCenters",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Conductors_SchoolRegistrationId",
                schema: "Transport",
                table: "Conductors",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_CoaAccounts_SchoolRegistrationId",
                schema: "Finance",
                table: "CoaAccounts",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_SchoolRegistrationId",
                table: "Classes",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_ChequeBooks_SchoolRegistrationId",
                schema: "Finance",
                table: "ChequeBooks",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_CashBankTransactions_SchoolRegistrationId",
                schema: "Finance",
                table: "CashBankTransactions",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_SchoolRegistrationId",
                table: "Candidates",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Campuses_SchoolRegistrationId",
                schema: "School",
                table: "Campuses",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_SchoolRegistrationId",
                schema: "Hostel",
                table: "Buildings",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPlans_SchoolRegistrationId",
                schema: "Finance",
                table: "BudgetPlans",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_SchoolRegistrationId",
                table: "Branches",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_BookVendors_SchoolRegistrationId",
                schema: "Library",
                table: "BookVendors",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_BookReservations_SchoolRegistrationId",
                schema: "Library",
                table: "BookReservations",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_BookPublishers_SchoolRegistrationId",
                schema: "Library",
                table: "BookPublishers",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_BookIssueLogs_SchoolRegistrationId",
                schema: "Library",
                table: "BookIssueLogs",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_BookCategories_SchoolRegistrationId",
                schema: "Library",
                table: "BookCategories",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_BookAuthors_SchoolRegistrationId",
                schema: "Library",
                table: "BookAuthors",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_BloodGroupMasters_SchoolRegistrationId",
                table: "BloodGroupMasters",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Beds_SchoolRegistrationId",
                schema: "Hostel",
                table: "Beds",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_BedReservations_SchoolRegistrationId",
                schema: "Hostel",
                table: "BedReservations",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Batches_SchoolRegistrationId",
                table: "Batches",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_SchoolRegistrationId",
                table: "Attendances",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceLogs_SchoolRegistrationId",
                table: "AttendanceLogs",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentSubmissions_SchoolRegistrationId",
                table: "AssignmentSubmissions",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_SchoolRegistrationId",
                table: "Assignments",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetMaintenanceLogs_SchoolRegistrationId",
                schema: "Inventory",
                table: "AssetMaintenanceLogs",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetDepreciationLogs_SchoolRegistrationId",
                schema: "Inventory",
                table: "AssetDepreciationLogs",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetAssignments_SchoolRegistrationId",
                table: "AssetAssignments",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Affiliateds_SchoolRegistrationId",
                table: "Affiliateds",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionRules_SchoolRegistrationId",
                table: "AdmissionRules",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionFormConfigs_SchoolRegistrationId",
                table: "AdmissionFormConfigs",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionApplications_SchoolRegistrationId",
                table: "AdmissionApplications",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicYears_SchoolRegistrationId",
                table: "AcademicYears",
                column: "SchoolRegistrationId");
        }
    }
}
