using System.Net;
using Microsoft.EntityFrameworkCore;
using School.Infrastructure.UnitOfWork.Interfaces;
using School_DTOs;
using School_DTOs.Hr;

namespace School.Services.Hr
{
    public class EmployeeSalaryDetailService : IEmployeeSalaryDetailService
    {
        private readonly IRepository<global::School.Domain.Hr.EmployeeSalaryDetail> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeSalaryDetailService(IRepository<global::School.Domain.Hr.EmployeeSalaryDetail> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse<List<EmployeeSalaryDetailDto>>> GetAllByEmployeeIdAsync(int fkId)
        {
            var data = await _repository.List().Where(x => x.EmployeeId == fkId).Select(x => new EmployeeSalaryDetailDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                Basic = x.Basic,
                HRA = x.HRA,
                DA = x.DA,
                PF = x.PF,
                ESI = x.ESI,
                NetSalary = x.NetSalary
            }).ToListAsync();

            return new APIResponse<List<EmployeeSalaryDetailDto>> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = data };
        }

        public async Task<APIResponse<EmployeeSalaryDetailDto>> GetByIdAsync(int id)
        {
            var data = await _repository.List().Where(x => x.Id == id).Select(x => new EmployeeSalaryDetailDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                Basic = x.Basic,
                HRA = x.HRA,
                DA = x.DA,
                PF = x.PF,
                ESI = x.ESI,
                NetSalary = x.NetSalary
            }).FirstOrDefaultAsync();

            if (data == null) return new APIResponse<EmployeeSalaryDetailDto> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            return new APIResponse<EmployeeSalaryDetailDto> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = data };
        }

        public async Task<APIResponse<object>> CreateAsync(CreateEmployeeSalaryDetailDto dto, string username)
        {
            var entity = new global::School.Domain.Hr.EmployeeSalaryDetail
            {
                EmployeeId = dto.EmployeeId,
                Basic = dto.Basic,
                HRA = dto.HRA,
                DA = dto.DA,
                PF = dto.PF,
                ESI = dto.ESI,
                NetSalary = dto.NetSalary,
                CreatedBy = username,
                CreatedDate = DateTime.UtcNow
            };
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Created successfully" };
        }

        public async Task<APIResponse<object>> UpdateAsync(int id, UpdateEmployeeSalaryDetailDto dto, string username)
        {
            if (id != dto.Id) return new APIResponse<object> { StatusCode = HttpStatusCode.BadRequest, Message = "Id mismatch" };
            var entity = await _repository.List().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (entity == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };

            entity.EmployeeId = dto.EmployeeId;
            entity.Basic = dto.Basic;
            entity.HRA = dto.HRA;
            entity.DA = dto.DA;
            entity.PF = dto.PF;
            entity.ESI = dto.ESI;
            entity.NetSalary = dto.NetSalary;
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