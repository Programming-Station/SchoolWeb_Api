using System.Net;
using Microsoft.EntityFrameworkCore;
using School.Infrastructure.UnitOfWork.Interfaces;
using School_DTOs;
using School_DTOs.Hr;

namespace School.Services.Hr
{
    public class EmployeeBankDetailService : IEmployeeBankDetailService
    {
        private readonly IRepository<global::School.Domain.Hr.EmployeeBankDetail> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeBankDetailService(IRepository<global::School.Domain.Hr.EmployeeBankDetail> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse<List<EmployeeBankDetailDto>>> GetAllByEmployeeIdAsync(int fkId)
        {
            var data = await _repository.List().Where(x => x.EmployeeId == fkId).Select(x => new EmployeeBankDetailDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                BankName = x.BankName,
                AccountNumber = x.AccountNumber,
                IFSC = x.IFSC,
                Branch = x.Branch
            }).ToListAsync();

            return new APIResponse<List<EmployeeBankDetailDto>> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = data };
        }

        public async Task<APIResponse<EmployeeBankDetailDto>> GetByIdAsync(int id)
        {
            var data = await _repository.List().Where(x => x.Id == id).Select(x => new EmployeeBankDetailDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                BankName = x.BankName,
                AccountNumber = x.AccountNumber,
                IFSC = x.IFSC,
                Branch = x.Branch
            }).FirstOrDefaultAsync();

            if (data == null) return new APIResponse<EmployeeBankDetailDto> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            return new APIResponse<EmployeeBankDetailDto> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = data };
        }

        public async Task<APIResponse<object>> CreateAsync(CreateEmployeeBankDetailDto dto, string username)
        {
            var entity = new global::School.Domain.Hr.EmployeeBankDetail
            {
                EmployeeId = dto.EmployeeId,
                BankName = dto.BankName,
                AccountNumber = dto.AccountNumber,
                IFSC = dto.IFSC,
                Branch = dto.Branch,
                CreatedBy = username,
                CreatedDate = DateTime.UtcNow
            };
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Created successfully" };
        }

        public async Task<APIResponse<object>> UpdateAsync(int id, UpdateEmployeeBankDetailDto dto, string username)
        {
            if (id != dto.Id) return new APIResponse<object> { StatusCode = HttpStatusCode.BadRequest, Message = "Id mismatch" };
            var entity = await _repository.List().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (entity == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };

            entity.EmployeeId = dto.EmployeeId;
            entity.BankName = dto.BankName;
            entity.AccountNumber = dto.AccountNumber;
            entity.IFSC = dto.IFSC;
            entity.Branch = dto.Branch;
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