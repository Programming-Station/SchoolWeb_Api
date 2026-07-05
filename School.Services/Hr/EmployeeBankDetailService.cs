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
    public class EmployeeBankDetailService : IEmployeeBankDetailService
    {
        private readonly IRepository<EmployeeBankDetail> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeBankDetailService(IRepository<EmployeeBankDetail> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse<List<EmployeeBankDetailDto>>> GetAllByEmployeeIdAsync(int employeeId)
        {
            var data = await _repository.GetAll().Where(x => x.EmployeeId == employeeId).Select(x => new EmployeeBankDetailDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                BankName = x.BankName, AccountNumber = x.AccountNumber, IfscCode = x.IfscCode, Branch = x.Branch
            }).ToListAsync();

            return new APIResponse<List<EmployeeBankDetailDto>>(HttpStatusCode.OK, "Success", data);
        }

        public async Task<APIResponse<EmployeeBankDetailDto>> GetByIdAsync(int id)
        {
            var data = await _repository.GetAll().Where(x => x.Id == id).Select(x => new EmployeeBankDetailDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                BankName = x.BankName, AccountNumber = x.AccountNumber, IfscCode = x.IfscCode, Branch = x.Branch
            }).FirstOrDefaultAsync();

            if (data == null) return new APIResponse<EmployeeBankDetailDto>(HttpStatusCode.NotFound, "Not found");
            return new APIResponse<EmployeeBankDetailDto>(HttpStatusCode.OK, "Success", data);
        }

        public async Task<APIResponse<object>> CreateAsync(CreateEmployeeBankDetailDto dto, string username)
        {
            var entity = new EmployeeBankDetail
            {
                EmployeeId = dto.EmployeeId,
                BankName = dto.BankName, AccountNumber = dto.AccountNumber, IfscCode = dto.IfscCode, Branch = dto.Branch,
                CreatedBy = username,
                CreatedDate = DateTime.UtcNow
            };
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object>(HttpStatusCode.OK, "Created successfully");
        }

        public async Task<APIResponse<object>> UpdateAsync(int id, UpdateEmployeeBankDetailDto dto, string username)
        {
            if (id != dto.Id) return new APIResponse<object>(HttpStatusCode.BadRequest, "Id mismatch");
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return new APIResponse<object>(HttpStatusCode.NotFound, "Not found");

            entity.EmployeeId = dto.EmployeeId;
            entity.BankName = dto.BankName;
            entity.AccountNumber = dto.AccountNumber;
            entity.IfscCode = dto.IfscCode;
            entity.Branch = dto.Branch;
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