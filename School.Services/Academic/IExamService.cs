using School_DTOs;
using School_DTOs.Academic;
namespace School.Services.Interfaces.Academic
{
    public interface IExamService
    {
        Task<APIResponse<List<ExamDto>>> GetAllAsync();
        Task<APIResponse<ExamDto>> GetByIdAsync(int id);
        Task<APIResponse<object>> CreateAsync(CreateExamDto dto, string username);
        Task<APIResponse<object>> UpdateAsync(int id, UpdateExamDto dto, string username);
        Task<APIResponse<object>> DeleteAsync(int id, string username);
        Task<APIResponse<object>> PublishResultAsync(int examId, string publishedBy);
    }
    public interface IExamResultService
    {
        Task<APIResponse<List<ExamResultDto>>> GetAllByExamIdAsync(int examId);
        Task<APIResponse<ExamResultDto>> GetByIdAsync(int id);
        Task<APIResponse<object>> CreateAsync(CreateExamResultDto dto, string username);
        Task<APIResponse<object>> UpdateAsync(int id, UpdateExamResultDto dto, string username);
        Task<APIResponse<object>> DeleteAsync(int id, string username);
    }
}

