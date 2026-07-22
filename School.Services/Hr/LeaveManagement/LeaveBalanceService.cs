using System.Net;
using Microsoft.EntityFrameworkCore;
using School.Infrastructure.UnitOfWork.Interfaces;
using School.Services.Interfaces.Hr.LeaveManagement;
using School_DTOs;
using School_DTOs.Hr.LeaveManagement;

namespace School.Services.Hr.LeaveManagement
{
    public class LeaveBalanceService : ILeaveBalanceService
    {
        private readonly IRepository<global::School.Domain.Hr.LeaveManagement.LeaveBalance> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public LeaveBalanceService(IRepository<global::School.Domain.Hr.LeaveManagement.LeaveBalance> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse<List<LeaveBalanceDto>>> GetAllByEmployeeIdAsync(int fkId)
        {
            var data = await _repository.List().Where(x => x.EmployeeId == fkId).Select(x => new LeaveBalanceDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                LeaveTypeId = x.LeaveTypeId,
                Year = x.Year,
                TotalLeaves = x.TotalLeaves,
                UsedLeaves = x.UsedLeaves,
                AvailableLeaves = x.AvailableLeaves
            }).ToListAsync();

            return new APIResponse<List<LeaveBalanceDto>> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = data };
        }

        public async Task<APIResponse<LeaveBalanceDto>> GetByIdAsync(int id)
        {
            var data = await _repository.List().Where(x => x.Id == id).Select(x => new LeaveBalanceDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                LeaveTypeId = x.LeaveTypeId,
                Year = x.Year,
                TotalLeaves = x.TotalLeaves,
                UsedLeaves = x.UsedLeaves,
                AvailableLeaves = x.AvailableLeaves
            }).FirstOrDefaultAsync();

            if (data == null) return new APIResponse<LeaveBalanceDto> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            return new APIResponse<LeaveBalanceDto> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = data };
        }

        public async Task<APIResponse<object>> CreateAsync(CreateLeaveBalanceDto dto, string username)
        {
            var entity = new global::School.Domain.Hr.LeaveManagement.LeaveBalance
            {
                EmployeeId = dto.EmployeeId,
                LeaveTypeId = dto.LeaveTypeId,
                Year = dto.Year,
                TotalLeaves = dto.TotalLeaves,
                UsedLeaves = dto.UsedLeaves,
                AvailableLeaves = dto.AvailableLeaves,
                CreatedBy = username,
                CreatedDate = DateTime.UtcNow
            };
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Created successfully" };
        }

        public async Task<APIResponse<object>> UpdateAsync(int id, UpdateLeaveBalanceDto dto, string username)
        {
            if (id != dto.Id) return new APIResponse<object> { StatusCode = HttpStatusCode.BadRequest, Message = "Id mismatch" };
            var entity = await _repository.List().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (entity == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };

            entity.EmployeeId = dto.EmployeeId;
            entity.LeaveTypeId = dto.LeaveTypeId;
            entity.Year = dto.Year;
            entity.TotalLeaves = dto.TotalLeaves;
            entity.UsedLeaves = dto.UsedLeaves;
            entity.AvailableLeaves = dto.AvailableLeaves;
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