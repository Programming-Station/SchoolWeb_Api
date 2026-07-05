using School_DTOs.Common;
using School_DTOs;
using School_DTOs.Hr;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace School.Services.Hr
{
    public interface IEmployeeDocumentService
    {
        Task<APIResponse<List<EmployeeDocumentDto>>> GetAllByEmployeeIdAsync(int employeeId);
        Task<APIResponse<EmployeeDocumentDto>> GetByIdAsync(int id);
        Task<APIResponse<object>> CreateAsync(CreateEmployeeDocumentDto dto, string username);
        Task<APIResponse<object>> UpdateAsync(int id, UpdateEmployeeDocumentDto dto, string username);
        Task<APIResponse<object>> DeleteAsync(int id, string username);
    }
}