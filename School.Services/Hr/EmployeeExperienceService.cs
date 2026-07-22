using System.Net;
using Microsoft.EntityFrameworkCore;
using School.Infrastructure.UnitOfWork.Interfaces;
using School_DTOs;
using School_DTOs.Hr;

namespace School.Services.Hr
{
    public class EmployeeExperienceService : IEmployeeExperienceService
    {
        private readonly IRepository<global::School.Domain.Hr.EmployeeExperience> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeExperienceService(IRepository<global::School.Domain.Hr.EmployeeExperience> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse<List<EmployeeExperienceDto>>> GetAllByEmployeeIdAsync(int fkId)
        {
            var data = await _repository.List().Where(x => x.EmployeeId == fkId).Select(x => new EmployeeExperienceDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                Company = x.Company,
                Designation = x.Designation,
                JoiningDate = x.JoiningDate,
                LeavingDate = x.LeavingDate,
                Salary = x.Salary
            }).ToListAsync();

            return new APIResponse<List<EmployeeExperienceDto>> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = data };
        }

        public async Task<APIResponse<EmployeeExperienceDto>> GetByIdAsync(int id)
        {
            var data = await _repository.List().Where(x => x.Id == id).Select(x => new EmployeeExperienceDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                Company = x.Company,
                Designation = x.Designation,
                JoiningDate = x.JoiningDate,
                LeavingDate = x.LeavingDate,
                Salary = x.Salary
            }).FirstOrDefaultAsync();

            if (data == null) return new APIResponse<EmployeeExperienceDto> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            return new APIResponse<EmployeeExperienceDto> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = data };
        }

        public async Task<APIResponse<object>> CreateAsync(CreateEmployeeExperienceDto dto, string username)
        {
            var entity = new global::School.Domain.Hr.EmployeeExperience
            {
                EmployeeId = dto.EmployeeId,
                Company = dto.Company,
                Designation = dto.Designation,
                JoiningDate = dto.JoiningDate,
                LeavingDate = dto.LeavingDate,
                Salary = dto.Salary,
                CreatedBy = username,
                CreatedDate = DateTime.UtcNow
            };
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Created successfully" };
        }

        public async Task<APIResponse<object>> UpdateAsync(int id, UpdateEmployeeExperienceDto dto, string username)
        {
            if (id != dto.Id) return new APIResponse<object> { StatusCode = HttpStatusCode.BadRequest, Message = "Id mismatch" };
            var entity = await _repository.List().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (entity == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };

            entity.EmployeeId = dto.EmployeeId;
            entity.Company = dto.Company;
            entity.Designation = dto.Designation;
            entity.JoiningDate = dto.JoiningDate;
            entity.LeavingDate = dto.LeavingDate;
            entity.Salary = dto.Salary;
            entity.UpdatedBy = username;
            entity.UpdatedDate = DateTime.UtcNow;

            _repository.Update(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Updated successfully" };
        }

        public async Task<APIResponse<object>> DeleteAsync(int id, string username)
        {
            var entity = await _repository.List().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (entity == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            _repository.Delete(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Deleted successfully" };
        }
    }
}