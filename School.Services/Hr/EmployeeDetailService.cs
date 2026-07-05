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
    public class EmployeeDetailService : IEmployeeDetailService
    {
        private readonly IRepository<EmployeeDetail> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeDetailService(IRepository<EmployeeDetail> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse<List<EmployeeDetailDto>>> GetAllByEmployeeIdAsync(int employeeId)
        {
            var data = await _repository.GetAll().Where(x => x.EmployeeId == employeeId).Select(x => new EmployeeDetailDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                FatherName = x.FatherName, MotherName = x.MotherName, SpouseName = x.SpouseName, AadhaarNumber = x.AadhaarNumber, PanNumber = x.PanNumber
            }).ToListAsync();

            return new APIResponse<List<EmployeeDetailDto>>(HttpStatusCode.OK, "Success", data);
        }

        public async Task<APIResponse<EmployeeDetailDto>> GetByIdAsync(int id)
        {
            var data = await _repository.GetAll().Where(x => x.Id == id).Select(x => new EmployeeDetailDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                FatherName = x.FatherName, MotherName = x.MotherName, SpouseName = x.SpouseName, AadhaarNumber = x.AadhaarNumber, PanNumber = x.PanNumber
            }).FirstOrDefaultAsync();

            if (data == null) return new APIResponse<EmployeeDetailDto>(HttpStatusCode.NotFound, "Not found");
            return new APIResponse<EmployeeDetailDto>(HttpStatusCode.OK, "Success", data);
        }

        public async Task<APIResponse<object>> CreateAsync(CreateEmployeeDetailDto dto, string username)
        {
            var entity = new EmployeeDetail
            {
                EmployeeId = dto.EmployeeId,
                FatherName = dto.FatherName, MotherName = dto.MotherName, SpouseName = dto.SpouseName, AadhaarNumber = dto.AadhaarNumber, PanNumber = dto.PanNumber,
                CreatedBy = username,
                CreatedDate = DateTime.UtcNow
            };
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object>(HttpStatusCode.OK, "Created successfully");
        }

        public async Task<APIResponse<object>> UpdateAsync(int id, UpdateEmployeeDetailDto dto, string username)
        {
            if (id != dto.Id) return new APIResponse<object>(HttpStatusCode.BadRequest, "Id mismatch");
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return new APIResponse<object>(HttpStatusCode.NotFound, "Not found");

            entity.EmployeeId = dto.EmployeeId;
            entity.FatherName = dto.FatherName;
            entity.MotherName = dto.MotherName;
            entity.SpouseName = dto.SpouseName;
            entity.AadhaarNumber = dto.AadhaarNumber;
            entity.PanNumber = dto.PanNumber;
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