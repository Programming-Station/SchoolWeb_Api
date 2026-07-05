using School_DTOs.Common;
using School_DTOs;
using School_DTOs.Hr;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace School.Services.Interfaces.Hr
{
    public interface IEmployeeExperienceService
    {
        Task<APIResponse<List<EmployeeExperienceDto>>> GetAllByEmployeeIdAsync(int employeeId);
        Task<APIResponse<EmployeeExperienceDto>> GetByIdAsync(int id);
        Task<APIResponse<object>> CreateAsync(CreateEmployeeExperienceDto dto, string username);
        Task<APIResponse<object>> UpdateAsync(int id, UpdateEmployeeExperienceDto dto, string username);
        Task<APIResponse<object>> DeleteAsync(int id, string username);
    }
}