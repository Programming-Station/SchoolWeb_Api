using Microsoft.EntityFrameworkCore;
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
using School_DTOs;

namespace School.Services.Hr
{
    public class EmployeeDetailService : IEmployeeDetailService
    {
        private readonly IRepository<global::School.Domain.Hr.EmployeeDetail> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeDetailService(IRepository<global::School.Domain.Hr.EmployeeDetail> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse<List<EmployeeDetailDto>>> GetAllByEmployeeIdAsync(int fkId)
        {
            var data = await _repository.List().Where(x => x.EmployeeId == fkId).Select(x => new EmployeeDetailDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                FatherName = x.FatherName, MotherName = x.MotherName, AadhaarNumber = x.AadhaarNumber, PANNumber = x.PANNumber
            }).ToListAsync();

            return new APIResponse<List<EmployeeDetailDto>> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = data };
        }

        public async Task<APIResponse<EmployeeDetailDto>> GetByIdAsync(int id)
        {
            var data = await _repository.List().Where(x => x.Id == id).Select(x => new EmployeeDetailDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                FatherName = x.FatherName, MotherName = x.MotherName, AadhaarNumber = x.AadhaarNumber, PANNumber = x.PANNumber
            }).FirstOrDefaultAsync();

            if (data == null) return new APIResponse<EmployeeDetailDto> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            return new APIResponse<EmployeeDetailDto> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = data };
        }

        public async Task<APIResponse<object>> CreateAsync(CreateEmployeeDetailDto dto, string username)
        {
            var entity = new global::School.Domain.Hr.EmployeeDetail
            {
                EmployeeId = dto.EmployeeId,
                FatherName = dto.FatherName, MotherName = dto.MotherName, AadhaarNumber = dto.AadhaarNumber, PANNumber = dto.PANNumber,
                CreatedBy = username,
                CreatedDate = DateTime.UtcNow
            };
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Created successfully" };
        }

        public async Task<APIResponse<object>> UpdateAsync(int id, UpdateEmployeeDetailDto dto, string username)
        {
            if (id != dto.Id) return new APIResponse<object> { StatusCode = HttpStatusCode.BadRequest, Message = "Id mismatch" };
            var entity = await _repository.List().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (entity == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };

            entity.EmployeeId = dto.EmployeeId;
            entity.FatherName = dto.FatherName;
            entity.MotherName = dto.MotherName;
                        entity.AadhaarNumber = dto.AadhaarNumber;
            entity.PANNumber = dto.PANNumber;
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