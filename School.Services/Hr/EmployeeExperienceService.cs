using Microsoft.EntityFrameworkCore;
using School.Domain;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork.Interfaces;
using School.Services.Interfaces.Hr;
using School_DTOs.Common;
using School_DTOs.Hr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace School.Services.Hr
{
    public class EmployeeExperienceService : IEmployeeExperienceService
    {
        private readonly IRepository<EmployeeExperience> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeExperienceService(IRepository<EmployeeExperience> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse<List<EmployeeExperienceDto>>> GetAllByEmployeeIdAsync(int employeeId)
        {
            var data = await _repository.GetAll().Where(x => x.EmployeeId == employeeId).Select(x => new EmployeeExperienceDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                Company = x.Company, Designation = x.Designation, JoiningDate = x.JoiningDate, LeavingDate = x.LeavingDate, Salary = x.Salary
            }).ToListAsync();

            return new APIResponse<List<EmployeeExperienceDto>>(HttpStatusCode.OK, "Success", data);
        }

        public async Task<APIResponse<EmployeeExperienceDto>> GetByIdAsync(int id)
        {
            var data = await _repository.GetAll().Where(x => x.Id == id).Select(x => new EmployeeExperienceDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                Company = x.Company, Designation = x.Designation, JoiningDate = x.JoiningDate, LeavingDate = x.LeavingDate, Salary = x.Salary
            }).FirstOrDefaultAsync();

            if (data == null) return new APIResponse<EmployeeExperienceDto>(HttpStatusCode.NotFound, "Not found");
            return new APIResponse<EmployeeExperienceDto>(HttpStatusCode.OK, "Success", data);
        }

        public async Task<APIResponse<object>> CreateAsync(CreateEmployeeExperienceDto dto, string username)
        {
            var entity = new EmployeeExperience
            {
                EmployeeId = dto.EmployeeId,
                Company = dto.Company, Designation = dto.Designation, JoiningDate = dto.JoiningDate, LeavingDate = dto.LeavingDate, Salary = dto.Salary,
                CreatedBy = username,
                CreatedDate = DateTime.UtcNow
            };
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object>(HttpStatusCode.OK, "Created successfully");
        }

        public async Task<APIResponse<object>> UpdateAsync(int id, UpdateEmployeeExperienceDto dto, string username)
        {
            if (id != dto.Id) return new APIResponse<object>(HttpStatusCode.BadRequest, "Id mismatch");
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return new APIResponse<object>(HttpStatusCode.NotFound, "Not found");

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
            return new APIResponse<object>(HttpStatusCode.OK, "Updated successfully");
        }

        public async Task<APIResponse<object>> DeleteAsync(int id, string username)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return new APIResponse<object>(HttpStatusCode.NotFound, "Not found");
            _repository.Remove(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object>(HttpStatusCode.OK, "Deleted successfully");
        }
    }
}