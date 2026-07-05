using School_DTOs.Academic;
using School_DTOs.Common;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace School.Services.Interfaces.Academic
{
    public interface ISubjectService
    {
        Task<APIResponse<List<SubjectDto>>> GetAllAsync();
        Task<APIResponse<SubjectDto>> GetByIdAsync(int id);
        Task<APIResponse<object>> CreateAsync(CreateSubjectDto dto, string username);
        Task<APIResponse<object>> UpdateAsync(int id, UpdateSubjectDto dto, string username);
        Task<APIResponse<object>> DeleteAsync(int id, string username);
    }
}
