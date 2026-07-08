using School.Models;
using School.Services.Interfaces;
using School.Utilities.Enums;
using School_API.Common.Interface;
using School_DTOs;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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

                // ── HR Master ───────────────────────────────────────────────────
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

                // ── Academic ────────────────────────────────────────────────────
                case SourceName.Subject:
                    res = await _masterService.GetSubjectsAsync();
                    break;

                // ── Payroll ─────────────────────────────────────────────────────
                case SourceName.SalaryComponent:
                    res = await _masterService.GetSalaryComponentsAsync();
                    break;

                // ── Transport ───────────────────────────────────────────────────
                case SourceName.TransportRoute:
                    res = await _masterService.GetTransportRoutesAsync();
                    break;

                case SourceName.Vehicle:
                    res = await _masterService.GetVehiclesAsync();
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

        // ── Location ─────────────────────────────────────────────────────────────

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

        // ── General ──────────────────────────────────────────────────────────────

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

        // ── School ───────────────────────────────────────────────────────────────

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

        // ── Access Control ───────────────────────────────────────────────────────

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

        // ── Academic ─────────────────────────────────────────────────────────────

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

        // ── Fee ──────────────────────────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> GetFeeTypes()
        {
            var res = await _masterService.GetFeeTypesAsync();
            return Ok(res);
        }

        // ── HR Master ────────────────────────────────────────────────────────────

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

        // ── Payroll ──────────────────────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> GetSalaryComponents()
        {
            var res = await _masterService.GetSalaryComponentsAsync();
            return Ok(res);
        }

        // ── Transport ────────────────────────────────────────────────────────────

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
    }
}
