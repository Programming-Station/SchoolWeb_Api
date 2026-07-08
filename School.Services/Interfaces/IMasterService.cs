using School_DTOs;
using School_DTOs.Account;

namespace School.Services.Interfaces
{
    public interface IMasterService
    { 
        Task<APIResponse<IEnumerable<DropdownDto>>> GetStatesAsync(int id);
        Task<APIResponse<IEnumerable<DropdownDto>>> GetCitiesAsync(int id);
        Task<APIResponse<IEnumerable<DropdownDto>>> GetStatusAsync();
        Task<APIResponse<IEnumerable<DropdownDto>>> GetAcademicYearAsync();
        Task<APIResponse<IEnumerable<DropdownDto>>> GetCoursesAsync(int id);
        Task<APIResponse<IEnumerable<RoleDto>>> GetRolesAsync();
        Task<APIResponse<IEnumerable<DropdownDto>>> GetAffiliationBoardsAsync();
        Task<APIResponse<IEnumerable<DropdownDto>>> GetSchoolTypesAsync();
        Task<APIResponse<IEnumerable<DropdownDto>>> GetSchoolMediumsAsync();
        Task<APIResponse<IEnumerable<DropdownDto>>> GetCountriesAsync();
        Task<APIResponse<IEnumerable<DropdownDto>>> GetModulesAsync();
        Task<APIResponse<IEnumerable<DropdownDto>>> GetMenusAsync();
        Task<APIResponse<IEnumerable<DropdownDto>>> GetSubMenusAsync(int menuId);
        Task<APIResponse<IEnumerable<DropdownDto>>> GetClassesAsync();
        Task<APIResponse<IEnumerable<DropdownDto>>> GetDepartmentsAsync();
        Task<APIResponse<IEnumerable<DropdownDto>>> GetFeeTypesAsync();
        Task<APIResponse<IEnumerable<DropdownDto>>> GetFacultiesAsync();
        Task<APIResponse<IEnumerable<DropdownDto>>> GetCategoryModulesAsync();
        Task<APIResponse<IEnumerable<DropdownDto>>> GetAffiliatedsAsync();

        // HR Master dropdowns
        Task<APIResponse<IEnumerable<DropdownDto>>> GetDesignationsAsync();
        Task<APIResponse<IEnumerable<DropdownDto>>> GetEmployeesAsync();
        Task<APIResponse<IEnumerable<DropdownDto>>> GetEmployeeCategoriesAsync();
        Task<APIResponse<IEnumerable<DropdownDto>>> GetEmployeeTypesAsync();
        Task<APIResponse<IEnumerable<DropdownDto>>> GetEmploymentStatusesAsync();
        Task<APIResponse<IEnumerable<DropdownDto>>> GetSalaryGradesAsync();
        Task<APIResponse<IEnumerable<DropdownDto>>> GetBloodGroupMastersAsync();
        Task<APIResponse<IEnumerable<DropdownDto>>> GetQualificationMastersAsync();
        Task<APIResponse<IEnumerable<DropdownDto>>> GetReligionMastersAsync();
        Task<APIResponse<IEnumerable<DropdownDto>>> GetSpecializationsAsync();
        Task<APIResponse<IEnumerable<DropdownDto>>> GetShiftMastersAsync();
        Task<APIResponse<IEnumerable<DropdownDto>>> GetHolidayMastersAsync();
        Task<APIResponse<IEnumerable<DropdownDto>>> GetWeekOffsAsync();
        Task<APIResponse<IEnumerable<DropdownDto>>> GetNoticePeriodsAsync();
        Task<APIResponse<IEnumerable<DropdownDto>>> GetLeaveTypesAsync();
        Task<APIResponse<IEnumerable<DropdownDto>>> GetLeaveSettingsAsync();
        Task<APIResponse<IEnumerable<DropdownDto>>> GetJobPostingsAsync();
        Task<APIResponse<IEnumerable<DropdownDto>>> GetCandidatesAsync();
        Task<APIResponse<IEnumerable<DropdownDto>>> GetJobApplicationsAsync();
        Task<APIResponse<IEnumerable<DropdownDto>>> GetPerformanceReviewsAsync();
        Task<APIResponse<IEnumerable<DropdownDto>>> GetKpiMetricsAsync();
        Task<APIResponse<IEnumerable<DropdownDto>>> GetTrainingProgramsAsync();
        Task<APIResponse<IEnumerable<DropdownDto>>> GetTrainingEnrollmentsAsync();
        Task<APIResponse<IEnumerable<DropdownDto>>> GetSchoolAssetsAsync();
        Task<APIResponse<IEnumerable<DropdownDto>>> GetAssetAssignmentsAsync();

        // Academic
        Task<APIResponse<IEnumerable<DropdownDto>>> GetSubjectsAsync();

        // Payroll
        Task<APIResponse<IEnumerable<DropdownDto>>> GetSalaryComponentsAsync();

        // Transport
        Task<APIResponse<IEnumerable<DropdownDto>>> GetTransportRoutesAsync();
        Task<APIResponse<IEnumerable<DropdownDto>>> GetVehiclesAsync();
    }
}

