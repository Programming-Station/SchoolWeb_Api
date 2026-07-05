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
    public class EmployeeEducationService : IEmployeeEducationService
    {
        private readonly IRepository<EmployeeEducation> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeEducationService(IRepository<EmployeeEducation> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse<List<EmployeeEducationDto>>> GetAllByEmployeeIdAsync(int employeeId)
        {
            var data = await _repository.GetAll().Where(x => x.EmployeeId == employeeId).Select(x => new EmployeeEducationDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                Degree = x.Degree, Board = x.Board, University = x.University, PassingYear = x.PassingYear, Percentage = x.Percentage
            }).ToListAsync();

            return new APIResponse<List<EmployeeEducationDto>>(HttpStatusCode.OK, "Success", data);
        }

        public async Task<APIResponse<EmployeeEducationDto>> GetByIdAsync(int id)
        {
            var data = await _repository.GetAll().Where(x => x.Id == id).Select(x => new EmployeeEducationDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                Degree = x.Degree, Board = x.Board, University = x.University, PassingYear = x.PassingYear, Percentage = x.Percentage
            }).FirstOrDefaultAsync();

            if (data == null) return new APIResponse<EmployeeEducationDto>(HttpStatusCode.NotFound, "Not found");
            return new APIResponse<EmployeeEducationDto>(HttpStatusCode.OK, "Success", data);
        }

        public async Task<APIResponse<object>> CreateAsync(CreateEmployeeEducationDto dto, string username)
        {
            var entity = new EmployeeEducation
            {
                EmployeeId = dto.EmployeeId,
                Degree = dto.Degree, Board = dto.Board, University = dto.University, PassingYear = dto.PassingYear, Percentage = dto.Percentage,
                CreatedBy = username,
                CreatedDate = DateTime.UtcNow
            };
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object>(HttpStatusCode.OK, "Created successfully");
        }

        public async Task<APIResponse<object>> UpdateAsync(int id, UpdateEmployeeEducationDto dto, string username)
        {
            if (id != dto.Id) return new APIResponse<object>(HttpStatusCode.BadRequest, "Id mismatch");
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return new APIResponse<object>(HttpStatusCode.NotFound, "Not found");

            entity.EmployeeId = dto.EmployeeId;
            entity.Degree = dto.Degree;
            entity.Board = dto.Board;
            entity.University = dto.University;
            entity.PassingYear = dto.PassingYear;
            entity.Percentage = dto.Percentage;
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