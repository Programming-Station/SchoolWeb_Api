using School.Domain.School;
using School.Models.School;
using School_DTOs;
using School_DTOs.School;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Services.School.ISchoolServices
{
    public interface ISchoolService
    {
        // Define methods for school-related data access operations
        Task<APIResponse<SchoolRegistrationDto>> AddAsync(SchoolRegistrationModel model);
        Task<APIResponse<IEnumerable<SchoolRegistrationDto>>> GetAllsAsync();

        Task<APIResponse<SchoolRegistrationDto>> GetByIdAsync(int id);

       
        Task<APIResponse<SchoolRegistrationDto>> EditAsync(SchoolRegistrationModel model);

        Task<APIResponse> DeleteAsync(int Id);
    }
}
