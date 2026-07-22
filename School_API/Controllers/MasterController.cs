using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using School.Models;
using School.Services.Interfaces;
using School.Utilities.Enums;
using School_API.Common.Interface;
using School_DTOs;

namespace School_API.Controllers
{
    public class MasterController : BaseController
    {
        private readonly IMasterService _masterService;
        public MasterController(IMasterService masterService, ICurrentUserService currentUser) : base(currentUser)
        {
            _masterService = masterService;
        }

        [HttpPost]
        public async Task<IActionResult> GetDropdownData([FromBody] DropDownModel model)
        {
            APIResponse<IEnumerable<DropdownDto>> res = new APIResponse<IEnumerable<DropdownDto>>();

            if (string.IsNullOrWhiteSpace(model.Table))
                return Ok(new APIResponse<List<DropdownDto>>
                {
                    Success = false,
                    Message = "Table parameter is required.",
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                });

            if (!Enum.TryParse(model.Table, true, out SourceName dropdownType))
                return Ok(new APIResponse<List<DropdownDto>>
                {
                    Success = false,
                    Message = "Invalid dropdown type.",
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                });

            switch (dropdownType)
            {
                case SourceName.State:
                    if (model.ParentId == null)
                        return Ok(new APIResponse<List<DropdownDto>> { Success = false, Message = "countryId is required for states.", StatusCode = System.Net.HttpStatusCode.BadRequest });
                    res = await _masterService.GetStatesAsync(model.ParentId.Value);
                    break;

                case SourceName.City:
                    if (model.ParentId == null)
                        return Ok(new APIResponse<List<DropdownDto>> { Success = false, Message = "stateId is required for cities.", StatusCode = System.Net.HttpStatusCode.BadRequest });
                    res = await _masterService.GetCitiesAsync(model.ParentId.Value);
                    break;

                case SourceName.Status:
                    res = await _masterService.GetStatusAsync();
                    break;

                case SourceName.Course:
                    if (model.ParentId == null)
                        return Ok(new APIResponse<List<DropdownDto>> { Success = false, Message = "courseTypeId is required for courses.", StatusCode = System.Net.HttpStatusCode.BadRequest });
                    res = await _masterService.GetCoursesAsync(model.ParentId.Value);
                    break;

                case SourceName.AcademicYear:
                    res = await _masterService.GetAcademicYearAsync();
                    break;

                case SourceName.AffiliationBoard:
                    res = await _masterService.GetAffiliationBoardsAsync();
                    break;

                case SourceName.SchoolType:
                    res = await _masterService.GetSchoolTypesAsync();
                    break;

                case SourceName.SchoolMedium:
                    res = await _masterService.GetSchoolMediumsAsync();
                    break;

                case SourceName.Country:
                    res = await _masterService.GetCountriesAsync();
                    break;

                case SourceName.Module:
                    res = await _masterService.GetModulesAsync();
                    break;

                case SourceName.Menu:
                    res = await _masterService.GetMenusAsync();
                    break;

                case SourceName.SubMenu:
                    if (model.ParentId == null)
                        return Ok(new APIResponse<List<DropdownDto>> { Success = false, Message = "menuId is required for submenus.", StatusCode = System.Net.HttpStatusCode.BadRequest });
                    res = await _masterService.GetSubMenusAsync(model.ParentId.Value);
                    break;

                case SourceName.Class:
                    res = await _masterService.GetClassesAsync();
                    break;

                case SourceName.Department:
                    res = await _masterService.GetDepartmentsAsync();
                    break;

                case SourceName.FeeType:
                    res = await _masterService.GetFeeTypesAsync();
                    break;

                case SourceName.Faculty:
                    res = await _masterService.GetFacultiesAsync();
                    break;

                case SourceName.CategoryModule:
                    res = await _masterService.GetCategoryModulesAsync();
                    break;

                case SourceName.Affiliated:
                    res = await _masterService.GetAffiliatedsAsync();
                    break;

                // â”€â”€ HR Master â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
                case SourceName.Designation:
                    res = await _masterService.GetDesignationsAsync();
                    break;

                case SourceName.Employee:
                    res = await _masterService.GetEmployeesAsync();
                    break;

                case SourceName.EmployeeCategory:
                    res = await _masterService.GetEmployeeCategoriesAsync();
                    break;

                case SourceName.EmployeeType:
                    res = await _masterService.GetEmployeeTypesAsync();
                    break;

                case SourceName.EmploymentStatus:
                    res = await _masterService.GetEmploymentStatusesAsync();
                    break;

                case SourceName.SalaryGrade:
                    res = await _masterService.GetSalaryGradesAsync();
                    break;

                case SourceName.BloodGroupMaster:
                    res = await _masterService.GetBloodGroupMastersAsync();
                    break;

                case SourceName.QualificationMaster:
                    res = await _masterService.GetQualificationMastersAsync();
                    break;

                case SourceName.ReligionMaster:
                    res = await _masterService.GetReligionMastersAsync();
                    break;

                case SourceName.Specialization:
                    res = await _masterService.GetSpecializationsAsync();
                    break;

                case SourceName.ShiftMaster:
                    res = await _masterService.GetShiftMastersAsync();
                    break;

                case SourceName.HolidayMaster:
                    res = await _masterService.GetHolidayMastersAsync();
                    break;

                case SourceName.WeekOff:
                    res = await _masterService.GetWeekOffsAsync();
                    break;

                case SourceName.NoticePeriod:
                    res = await _masterService.GetNoticePeriodsAsync();
                    break;

                case SourceName.LeaveType:
                    res = await _masterService.GetLeaveTypesAsync();
                    break;

                case SourceName.LeaveSetting:
                    res = await _masterService.GetLeaveSettingsAsync();
                    break;

                case SourceName.JobPosting:
                    res = await _masterService.GetJobPostingsAsync();
                    break;

                case SourceName.Candidate:
                    res = await _masterService.GetCandidatesAsync();
                    break;

                case SourceName.JobApplication:
                    res = await _masterService.GetJobApplicationsAsync();
                    break;

                case SourceName.PerformanceReview:
                    res = await _masterService.GetPerformanceReviewsAsync();
                    break;

                case SourceName.KpiMetric:
                    res = await _masterService.GetKpiMetricsAsync();
                    break;

                case SourceName.TrainingProgram:
                    res = await _masterService.GetTrainingProgramsAsync();
                    break;

                case SourceName.TrainingEnrollment:
                    res = await _masterService.GetTrainingEnrollmentsAsync();
                    break;

                case SourceName.SchoolAsset:
                    res = await _masterService.GetSchoolAssetsAsync();
                    break;

                case SourceName.AssetAssignment:
                    res = await _masterService.GetAssetAssignmentsAsync();
                    break;

                // â”€â”€ Academic â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
                case SourceName.Subject:
                    res = await _masterService.GetSubjectsAsync();
                    break;

                // â”€â”€ Payroll â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
                case SourceName.SalaryComponent:
                    res = await _masterService.GetSalaryComponentsAsync();
                    break;

                // â”€â”€ Transport â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
                case SourceName.TransportRoute:
                    res = await _masterService.GetTransportRoutesAsync();
                    break;

                case SourceName.Vehicle:
                    res = await _masterService.GetVehiclesAsync();
                    break;

                case SourceName.Student:
                    res = await _masterService.GetStudentsAsync();
                    break;

                case SourceName.SchoolRegistration:
                    res = await _masterService.GetSchoolRegistrationsAsync();
                    break;

                case SourceName.SchoolProfileSetting:
                    res = await _masterService.GetSchoolProfileSettingsAsync();
                    break;

                case SourceName.SchoolSubscription:
                    res = await _masterService.GetSchoolSubscriptionsAsync();
                    break;

                case SourceName.SchoolOwner:
                    res = await _masterService.GetSchoolOwnersAsync();
                    break;

                case SourceName.EducationalDetail:
                    res = await _masterService.GetEducationalDetailsAsync();
                    break;

                case SourceName.EmailServerSetting:
                    res = await _masterService.GetEmailServerSettingsAsync();
                    break;

                case SourceName.EmailTemplate:
                    res = await _masterService.GetEmailTemplatesAsync();
                    break;

                case SourceName.EmailBranding:
                    res = await _masterService.GetEmailBrandingsAsync();
                    break;

                case SourceName.Exam:
                    res = await _masterService.GetExamsAsync();
                    break;

                case SourceName.ExamResult:
                    res = await _masterService.GetExamResultsAsync();
                    break;

                case SourceName.Event:
                    res = await _masterService.GetEventsAsync();
                    break;

                case SourceName.EmployeeDocument:
                    res = await _masterService.GetEmployeeDocumentsAsync();
                    break;

                case SourceName.EmployeeBankDetail:
                    res = await _masterService.GetEmployeeBankDetailsAsync();
                    break;

                case SourceName.EmployeeEducation:
                    res = await _masterService.GetEmployeeEducationsAsync();
                    break;

                case SourceName.EmployeeExperience:
                    res = await _masterService.GetEmployeeExperiencesAsync();
                    break;

                case SourceName.EmployeeSalaryDetail:
                    res = await _masterService.GetEmployeeSalaryDetailsAsync();
                    break;

                case SourceName.EmployeeDetail:
                    res = await _masterService.GetEmployeeDetailsAsync();
                    break;

                case SourceName.LeaveRequest:
                    res = await _masterService.GetLeaveRequestsAsync();
                    break;

                case SourceName.LeaveBalance:
                    res = await _masterService.GetLeaveBalancesAsync();
                    break;

                case SourceName.Attendance:
                    res = await _masterService.GetAttendancesAsync();
                    break;

                case SourceName.AttendanceLog:
                    res = await _masterService.GetAttendanceLogsAsync();
                    break;

                case SourceName.Timesheet:
                    res = await _masterService.GetTimesheetsAsync();
                    break;

                case SourceName.TimesheetEntry:
                    res = await _masterService.GetTimesheetEntriesAsync();
                    break;

                case SourceName.PayrollRun:
                    res = await _masterService.GetPayrollRunsAsync();
                    break;

                case SourceName.PayGroup:
                    res = await _masterService.GetPayGroupsAsync();
                    break;

                case SourceName.SalaryStructure:
                    res = await _masterService.GetSalaryStructuresAsync();
                    break;

                case SourceName.SalaryStructureItem:
                    res = await _masterService.GetSalaryStructureItemsAsync();
                    break;

                case SourceName.EmployeeSalaryAllocation:
                    res = await _masterService.GetEmployeeSalaryAllocationsAsync();
                    break;

                case SourceName.EmployeeLoan:
                    res = await _masterService.GetEmployeeLoansAsync();
                    break;

                case SourceName.LoanRepaymentSchedule:
                    res = await _masterService.GetLoanRepaymentSchedulesAsync();
                    break;

                case SourceName.SalaryAdvance:
                    res = await _masterService.GetSalaryAdvancesAsync();
                    break;

                case SourceName.EmployeeBonus:
                    res = await _masterService.GetEmployeeBonusesAsync();
                    break;

                case SourceName.ReimbursementClaim:
                    res = await _masterService.GetReimbursementClaimsAsync();
                    break;

                case SourceName.SalaryArrear:
                    res = await _masterService.GetSalaryArrearsAsync();
                    break;

                case SourceName.StatutoryComplianceConfig:
                    res = await _masterService.GetStatutoryComplianceConfigsAsync();
                    break;

                case SourceName.PayrollRunDetail:
                    res = await _masterService.GetPayrollRunDetailsAsync();
                    break;

                case SourceName.TransportAllocation:
                    res = await _masterService.GetTransportAllocationsAsync();
                    break;

                case SourceName.TransportStop:
                    res = await _masterService.GetTransportStopsAsync();
                    break;

                case SourceName.RouteStopMapping:
                    res = await _masterService.GetRouteStopMappingsAsync();
                    break;

                case SourceName.Conductor:
                    res = await _masterService.GetConductorsAsync();
                    break;

                case SourceName.RouteAssignment:
                    res = await _masterService.GetRouteAssignmentsAsync();
                    break;

                case SourceName.TransportTrip:
                    res = await _masterService.GetTransportTripsAsync();
                    break;

                case SourceName.VehicleMaintenance:
                    res = await _masterService.GetVehicleMaintenancesAsync();
                    break;

                case SourceName.VehicleIncident:
                    res = await _masterService.GetVehicleIncidentsAsync();
                    break;

                case SourceName.TransportInventory:
                    res = await _masterService.GetTransportInventoriesAsync();
                    break;

                case SourceName.Book:
                    res = await _masterService.GetBooksAsync();
                    break;

                case SourceName.BookIssueLog:
                    res = await _masterService.GetBookIssueLogsAsync();
                    break;

                case SourceName.BookCategory:
                    res = await _masterService.GetBookCategoriesAsync();
                    break;

                case SourceName.BookAuthor:
                    res = await _masterService.GetBookAuthorsAsync();
                    break;

                case SourceName.BookPublisher:
                    res = await _masterService.GetBookPublishersAsync();
                    break;

                case SourceName.BookVendor:
                    res = await _masterService.GetBookVendorsAsync();
                    break;

                case SourceName.LibraryMember:
                    res = await _masterService.GetLibraryMembersAsync();
                    break;

                case SourceName.BookReservation:
                    res = await _masterService.GetBookReservationsAsync();
                    break;

                case SourceName.LibraryFineRule:
                    res = await _masterService.GetLibraryFineRulesAsync();
                    break;

                case SourceName.DigitalResource:
                    res = await _masterService.GetDigitalResourcesAsync();
                    break;

                case SourceName.Campus:
                    res = await _masterService.GetCampusesAsync();
                    break;

                case SourceName.EducationLevel:
                    res = await _masterService.GetEducationLevelsAsync();
                    break;

                case SourceName.YearSemester:
                    res = await _masterService.GetYearSemestersAsync();
                    break;

                case SourceName.Program:
                    res = await _masterService.GetProgramsAsync();
                    break;

                case SourceName.Branch:
                    res = await _masterService.GetBranchesAsync();
                    break;

                case SourceName.Batch:
                    res = await _masterService.GetBatchesAsync();
                    break;

                case SourceName.AdmissionFormConfig:
                    res = await _masterService.GetAdmissionFormConfigsAsync();
                    break;

                case SourceName.AdmissionRule:
                    res = await _masterService.GetAdmissionRulesAsync();
                    break;

                case SourceName.FeeStructure:
                    res = await _masterService.GetFeeStructuresAsync();
                    break;

                case SourceName.FeeStructureItem:
                    res = await _masterService.GetFeeStructureItemsAsync();
                    break;

                case SourceName.AdmissionApplication:
                    res = await _masterService.GetAdmissionApplicationsAsync();
                    break;

                case SourceName.ParentStudentMapping:
                    res = await _masterService.GetParentStudentMappingsAsync();
                    break;

                case SourceName.SubjectEnrollment:
                    res = await _masterService.GetSubjectEnrollmentsAsync();
                    break;

                case SourceName.StudentAttendance:
                    res = await _masterService.GetStudentAttendancesAsync();
                    break;

                case SourceName.TimetablePeriod:
                    res = await _masterService.GetTimetablePeriodsAsync();
                    break;

                case SourceName.Homework:
                    res = await _masterService.GetHomeworksAsync();
                    break;

                case SourceName.HomeworkSubmission:
                    res = await _masterService.GetHomeworkSubmissionsAsync();
                    break;

                case SourceName.Assignment:
                    res = await _masterService.GetAssignmentsAsync();
                    break;

                case SourceName.AssignmentSubmission:
                    res = await _masterService.GetAssignmentSubmissionsAsync();
                    break;

                case SourceName.OnlineClass:
                    res = await _masterService.GetOnlineClassesAsync();
                    break;

                case SourceName.SyllabusChapter:
                    res = await _masterService.GetSyllabusChaptersAsync();
                    break;

                case SourceName.LessonPlan:
                    res = await _masterService.GetLessonPlansAsync();
                    break;

                case SourceName.FeeInstallment:
                    res = await _masterService.GetFeeInstallmentsAsync();
                    break;

                case SourceName.FeePayment:
                    res = await _masterService.GetFeePaymentsAsync();
                    break;

                case SourceName.FeeFine:
                    res = await _masterService.GetFeeFinesAsync();
                    break;

                case SourceName.StudentScholarship:
                    res = await _masterService.GetStudentScholarshipsAsync();
                    break;

                case SourceName.FeeRefund:
                    res = await _masterService.GetFeeRefundsAsync();
                    break;

                case SourceName.PaymentGateway:
                    res = await _masterService.GetPaymentGatewaysAsync();
                    break;

                case SourceName.OnlinePaymentOrder:
                    res = await _masterService.GetOnlinePaymentOrdersAsync();
                    break;

                case SourceName.FineRule:
                    res = await _masterService.GetFineRulesAsync();
                    break;

                case SourceName.ExamSchedule:
                    res = await _masterService.GetExamSchedulesAsync();
                    break;

                case SourceName.GradeConfig:
                    res = await _masterService.GetGradeConfigsAsync();
                    break;

                case SourceName.ReportCard:
                    res = await _masterService.GetReportCardsAsync();
                    break;

                case SourceName.StudentPromotion:
                    res = await _masterService.GetStudentPromotionsAsync();
                    break;

                case SourceName.Hostel:
                    res = await _masterService.GetHostelsAsync();
                    break;

                case SourceName.Building:
                    res = await _masterService.GetBuildingsAsync();
                    break;

                case SourceName.Floor:
                    res = await _masterService.GetFloorsAsync();
                    break;

                case SourceName.RoomCategory:
                    res = await _masterService.GetRoomCategoriesAsync();
                    break;

                case SourceName.Room:
                    res = await _masterService.GetRoomsAsync();
                    break;

                case SourceName.Bed:
                    res = await _masterService.GetBedsAsync();
                    break;

                case SourceName.HostelWarden:
                    res = await _masterService.GetHostelWardensAsync();
                    break;

                case SourceName.HostelAdmission:
                    res = await _masterService.GetHostelAdmissionsAsync();
                    break;

                case SourceName.RoomTransferHistory:
                    res = await _masterService.GetRoomTransferHistoriesAsync();
                    break;

                case SourceName.BedReservation:
                    res = await _masterService.GetBedReservationsAsync();
                    break;

                case SourceName.HostelFeeAllocation:
                    res = await _masterService.GetHostelFeeAllocationsAsync();
                    break;

                case SourceName.HostelFeePayment:
                    res = await _masterService.GetHostelFeePaymentsAsync();
                    break;

                case SourceName.MessMenu:
                    res = await _masterService.GetMessMenusAsync();
                    break;

                case SourceName.MealAttendance:
                    res = await _masterService.GetMealAttendancesAsync();
                    break;

                case SourceName.HostelVisitor:
                    res = await _masterService.GetHostelVisitorsAsync();
                    break;

                case SourceName.HostelGatePass:
                    res = await _masterService.GetHostelGatePassesAsync();
                    break;

                case SourceName.HostelAttendance:
                    res = await _masterService.GetHostelAttendancesAsync();
                    break;

                case SourceName.HostelComplaint:
                    res = await _masterService.GetHostelComplaintsAsync();
                    break;

                case SourceName.HostelMaintenance:
                    res = await _masterService.GetHostelMaintenancesAsync();
                    break;

                case SourceName.LaundryTransaction:
                    res = await _masterService.GetLaundryTransactionsAsync();
                    break;

                case SourceName.HostelInventory:
                    res = await _masterService.GetHostelInventoriesAsync();
                    break;

                case SourceName.HostelMedicalLog:
                    res = await _masterService.GetHostelMedicalLogsAsync();
                    break;

                case SourceName.HostelDiscipline:
                    res = await _masterService.GetHostelDisciplinesAsync();
                    break;

                case SourceName.CoaAccount:
                    res = await _masterService.GetCoaAccountsAsync();
                    break;

                case SourceName.JournalEntry:
                    res = await _masterService.GetJournalEntriesAsync();
                    break;

                case SourceName.JournalEntryLine:
                    res = await _masterService.GetJournalEntryLinesAsync();
                    break;

                case SourceName.CashBankTransaction:
                    res = await _masterService.GetCashBankTransactionsAsync();
                    break;

                case SourceName.BudgetPlan:
                    res = await _masterService.GetBudgetPlansAsync();
                    break;

                case SourceName.TaxConfig:
                    res = await _masterService.GetTaxConfigsAsync();
                    break;

                case SourceName.FinancialYear:
                    res = await _masterService.GetFinancialYearsAsync();
                    break;

                case SourceName.CostCenter:
                    res = await _masterService.GetCostCentersAsync();
                    break;

                case SourceName.ChequeBook:
                    res = await _masterService.GetChequeBooksAsync();
                    break;

                case SourceName.ItemCategory:
                    res = await _masterService.GetItemCategoriesAsync();
                    break;

                case SourceName.InventoryItem:
                    res = await _masterService.GetInventoryItemsAsync();
                    break;

                case SourceName.Vendor:
                    res = await _masterService.GetVendorsAsync();
                    break;

                case SourceName.PurchaseRequisition:
                    res = await _masterService.GetPurchaseRequisitionsAsync();
                    break;

                case SourceName.PurchaseRequisitionItem:
                    res = await _masterService.GetPurchaseRequisitionItemsAsync();
                    break;

                case SourceName.PurchaseOrder:
                    res = await _masterService.GetPurchaseOrdersAsync();
                    break;

                case SourceName.PurchaseOrderItem:
                    res = await _masterService.GetPurchaseOrderItemsAsync();
                    break;

                case SourceName.GoodsReceiptNote:
                    res = await _masterService.GetGoodsReceiptNotesAsync();
                    break;

                case SourceName.GoodsReceiptNoteItem:
                    res = await _masterService.GetGoodsReceiptNoteItemsAsync();
                    break;

                case SourceName.StockTransaction:
                    res = await _masterService.GetStockTransactionsAsync();
                    break;

                case SourceName.Warehouse:
                    res = await _masterService.GetWarehousesAsync();
                    break;

                case SourceName.WarehouseBin:
                    res = await _masterService.GetWarehouseBinsAsync();
                    break;

                case SourceName.Store:
                    res = await _masterService.GetStoresAsync();
                    break;

                case SourceName.RequestForQuotation:
                    res = await _masterService.GetRequestForQuotationsAsync();
                    break;

                case SourceName.VendorQuotation:
                    res = await _masterService.GetVendorQuotationsAsync();
                    break;

                case SourceName.PurchaseReturn:
                    res = await _masterService.GetPurchaseReturnsAsync();
                    break;

                case SourceName.StockIssue:
                    res = await _masterService.GetStockIssuesAsync();
                    break;

                case SourceName.QualityInspection:
                    res = await _masterService.GetQualityInspectionsAsync();
                    break;

                case SourceName.NoticeBoard:
                    res = await _masterService.GetNoticeBoardsAsync();
                    break;

                case SourceName.Circular:
                    res = await _masterService.GetCircularsAsync();
                    break;

                case SourceName.PushNotification:
                    res = await _masterService.GetPushNotificationsAsync();
                    break;

                case SourceName.ParentTeacherChat:
                    res = await _masterService.GetParentTeacherChatsAsync();
                    break;

                case SourceName.FeedbackSurvey:
                    res = await _masterService.GetFeedbackSurveysAsync();
                    break;

                case SourceName.SurveyQuestion:
                    res = await _masterService.GetSurveyQuestionsAsync();
                    break;

                case SourceName.SurveyResponse:
                    res = await _masterService.GetSurveyResponsesAsync();
                    break;

                case SourceName.DashboardConfig:
                    res = await _masterService.GetDashboardConfigsAsync();
                    break;

                case SourceName.DashboardWidget:
                    res = await _masterService.GetDashboardWidgetsAsync();
                    break;

                case SourceName.ReportTemplate:
                    res = await _masterService.GetReportTemplatesAsync();
                    break;

                case SourceName.AnalyticsKpi:
                    res = await _masterService.GetAnalyticsKpisAsync();
                    break;

                case SourceName.SchoolBranch:
                    res = await _masterService.GetSchoolBranchesAsync();
                    break;

                case SourceName.WorkflowDefinition:
                    res = await _masterService.GetWorkflowDefinitionsAsync();
                    break;

                case SourceName.WorkflowStep:
                    res = await _masterService.GetWorkflowStepsAsync();
                    break;

                case SourceName.WorkflowInstance:
                    res = await _masterService.GetWorkflowInstancesAsync();
                    break;

                case SourceName.ApprovalLog:
                    res = await _masterService.GetApprovalLogsAsync();
                    break;

                case SourceName.AiPrediction:
                    res = await _masterService.GetAiPredictionsAsync();
                    break;

                case SourceName.AiGeneration:
                    res = await _masterService.GetAiGenerationsAsync();
                    break;

                case SourceName.AiChatSession:
                    res = await _masterService.GetAiChatSessionsAsync();
                    break;

                case SourceName.AiChatMessage:
                    res = await _masterService.GetAiChatMessagesAsync();
                    break;

                default:
                    return Ok(new APIResponse<List<DropdownDto>>
                    {
                        Success = false,
                        Message = "Invalid dropdown type.",
                        StatusCode = System.Net.HttpStatusCode.BadRequest
                    });
            }

            return Ok(res);
        }

        // â”€â”€ Location â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

        [HttpGet]
        public async Task<IActionResult> GetStates([Required] int countryId)
        {
            var res = await _masterService.GetStatesAsync(countryId);
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetCities([Required] int stateId)
        {
            var res = await _masterService.GetCitiesAsync(stateId);
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetCountries()
        {
            var res = await _masterService.GetCountriesAsync();
            return Ok(res);
        }

        // â”€â”€ General â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

        [HttpGet]
        public async Task<IActionResult> GetStatues()
        {
            var res = await _masterService.GetStatusAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var res = await _masterService.GetRolesAsync();
            return Ok(res);
        }

        // â”€â”€ School â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

        [HttpGet]
        public async Task<IActionResult> GetAffiliationBoards()
        {
            var res = await _masterService.GetAffiliationBoardsAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetSchoolTypes()
        {
            var res = await _masterService.GetSchoolTypesAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetSchoolMediums()
        {
            var res = await _masterService.GetSchoolMediumsAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetAffiliateds()
        {
            var res = await _masterService.GetAffiliatedsAsync();
            return Ok(res);
        }

        // â”€â”€ Access Control â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

        [HttpGet]
        public async Task<IActionResult> GetModules()
        {
            var res = await _masterService.GetModulesAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetMenus()
        {
            var res = await _masterService.GetMenusAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetSubMenus([Required] int menuId)
        {
            var res = await _masterService.GetSubMenusAsync(menuId);
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryModules()
        {
            var res = await _masterService.GetCategoryModulesAsync();
            return Ok(res);
        }

        // â”€â”€ Academic â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

        [HttpGet]
        public async Task<IActionResult> GetAcademicYears()
        {
            var res = await _masterService.GetAcademicYearAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetClasses()
        {
            var res = await _masterService.GetClassesAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetSubjects()
        {
            var res = await _masterService.GetSubjectsAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetFaculties()
        {
            var res = await _masterService.GetFacultiesAsync();
            return Ok(res);
        }

        // â”€â”€ Fee â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

        [HttpGet]
        public async Task<IActionResult> GetFeeTypes()
        {
            var res = await _masterService.GetFeeTypesAsync();
            return Ok(res);
        }

        // â”€â”€ HR Master â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

        [HttpGet]
        public async Task<IActionResult> GetDepartments()
        {
            var res = await _masterService.GetDepartmentsAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetDesignations()
        {
            var res = await _masterService.GetDesignationsAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var res = await _masterService.GetEmployeesAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeeCategories()
        {
            var res = await _masterService.GetEmployeeCategoriesAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeeTypes()
        {
            var res = await _masterService.GetEmployeeTypesAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetEmploymentStatuses()
        {
            var res = await _masterService.GetEmploymentStatusesAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetSalaryGrades()
        {
            var res = await _masterService.GetSalaryGradesAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetBloodGroupMasters()
        {
            var res = await _masterService.GetBloodGroupMastersAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetQualificationMasters()
        {
            var res = await _masterService.GetQualificationMastersAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetReligionMasters()
        {
            var res = await _masterService.GetReligionMastersAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetSpecializations()
        {
            var res = await _masterService.GetSpecializationsAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetShiftMasters()
        {
            var res = await _masterService.GetShiftMastersAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetHolidayMasters()
        {
            var res = await _masterService.GetHolidayMastersAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetWeekOffs()
        {
            var res = await _masterService.GetWeekOffsAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetNoticePeriods()
        {
            var res = await _masterService.GetNoticePeriodsAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetLeaveTypes()
        {
            var res = await _masterService.GetLeaveTypesAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetLeaveSettings()
        {
            var res = await _masterService.GetLeaveSettingsAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetJobPostings()
        {
            var res = await _masterService.GetJobPostingsAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetCandidates()
        {
            var res = await _masterService.GetCandidatesAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetJobApplications()
        {
            var res = await _masterService.GetJobApplicationsAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetPerformanceReviews()
        {
            var res = await _masterService.GetPerformanceReviewsAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetKpiMetrics()
        {
            var res = await _masterService.GetKpiMetricsAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetTrainingPrograms()
        {
            var res = await _masterService.GetTrainingProgramsAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetTrainingEnrollments()
        {
            var res = await _masterService.GetTrainingEnrollmentsAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetSchoolAssets()
        {
            var res = await _masterService.GetSchoolAssetsAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetAssetAssignments()
        {
            var res = await _masterService.GetAssetAssignmentsAsync();
            return Ok(res);
        }

        // â”€â”€ Payroll â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

        [HttpGet]
        public async Task<IActionResult> GetSalaryComponents()
        {
            var res = await _masterService.GetSalaryComponentsAsync();
            return Ok(res);
        }

        // â”€â”€ Transport â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

        [HttpGet]
        public async Task<IActionResult> GetTransportRoutes()
        {
            var res = await _masterService.GetTransportRoutesAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetVehicles()
        {
            var res = await _masterService.GetVehiclesAsync();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            var res = await _masterService.GetStudentsAsync();
            return Ok(res);
        }

        [HttpPost("bulk-delete")]
        public async Task<IActionResult> BulkDelete([FromQuery] global::School.Utilities.Enums.SourceName table, [FromBody] System.Collections.Generic.IEnumerable<int> ids)
        {
            var res = await _masterService.BulkDeleteAsync(table, ids, UserName);
            return StatusCode((int)res.StatusCode, res);
        }

        [HttpPost("bulk-status-change")]
        public async Task<IActionResult> BulkStatusChange([FromQuery] global::School.Utilities.Enums.SourceName table, [FromBody] School_DTOs.Hr.BulkStatusChangeDto dto)
        {
            var res = await _masterService.BulkStatusChangeAsync(table, dto.Ids, dto.Status, UserName);
            return StatusCode((int)res.StatusCode, res);
        }
    }
}

