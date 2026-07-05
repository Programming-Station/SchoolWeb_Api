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
    }
}
