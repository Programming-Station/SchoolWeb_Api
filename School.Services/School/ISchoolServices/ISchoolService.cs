using School.Models.School;
using School_DTOs;
using School_DTOs.School;

namespace School.Services.School.ISchoolServices
{
    public interface ISchoolService
    {
        Task<APIResponse<SchoolRegistrationDto>> AddAsync(SchoolRegistrationModel model);
        Task<PagedResponse<SchoolRegistrationDto>> GetAllsAsync(int pageNumber = 1, int pageSize = 10, string? searchTerm = null, bool? isActive = null);
        Task<APIResponse<SchoolRegistrationDto>> GetByIdAsync(int id);


        Task<APIResponse<SchoolRegistrationDto>> EditAsync(SchoolRegistrationModel model);

        Task<APIResponse> DeleteAsync(int Id);
    }
}
