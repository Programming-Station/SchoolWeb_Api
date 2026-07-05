using School_DTOs.Common;
using School_DTOs;
using School_DTOs.Hr;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace School.Services.Hr
{
    public interface IEmployeeEducationService
    {
        Task<APIResponse<List<EmployeeEducationDto>>> GetAllByEmployeeIdAsync(int employeeId);
        Task<APIResponse<EmployeeEducationDto>> GetByIdAsync(int id);
        Task<APIResponse<object>> CreateAsync(CreateEmployeeEducationDto dto, string username);
        Task<APIResponse<object>> UpdateAsync(int id, UpdateEmployeeEducationDto dto, string username);
        Task<APIResponse<object>> DeleteAsync(int id, string username);
    }
}