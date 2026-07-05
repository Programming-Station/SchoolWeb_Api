using Microsoft.EntityFrameworkCore;
using School.Domain.Hr.LeaveManagement;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork.Interfaces;
using School.Services.Interfaces.Hr.LeaveManagement;
using School_DTOs.Common;
using School_DTOs.Hr.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace School.Services.Hr.LeaveManagement
{
    public class LeaveBalanceService : ILeaveBalanceService
    {
        private readonly IRepository<LeaveBalance> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public LeaveBalanceService(IRepository<LeaveBalance> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse<List<LeaveBalanceDto>>> GetAllByEmployeeIdAsync(int foreignKeyId)
        {
            var data = await _repository.GetAll().Where(x => x.EmployeeId == foreignKeyId).Select(x => new LeaveBalanceDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                LeaveTypeId = x.LeaveTypeId, Year = x.Year, TotalLeaves = x.TotalLeaves, UsedLeaves = x.UsedLeaves, AvailableLeaves = x.AvailableLeaves
            }).ToListAsync();

            return new APIResponse<List<LeaveBalanceDto>>(HttpStatusCode.OK, "Success", data);
        }

        public async Task<APIResponse<LeaveBalanceDto>> GetByIdAsync(int id)
        {
            var data = await _repository.GetAll().Where(x => x.Id == id).Select(x => new LeaveBalanceDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                LeaveTypeId = x.LeaveTypeId, Year = x.Year, TotalLeaves = x.TotalLeaves, UsedLeaves = x.UsedLeaves, AvailableLeaves = x.AvailableLeaves
            }).FirstOrDefaultAsync();

            if (data == null) return new APIResponse<LeaveBalanceDto>(HttpStatusCode.NotFound, "Not found");
            return new APIResponse<LeaveBalanceDto>(HttpStatusCode.OK, "Success", data);
        }

        public async Task<APIResponse<object>> CreateAsync(CreateLeaveBalanceDto dto, string username)
        {
            var entity = new LeaveBalance
            {
                EmployeeId = dto.EmployeeId,
                LeaveTypeId = dto.LeaveTypeId, Year = dto.Year, TotalLeaves = dto.TotalLeaves, UsedLeaves = dto.UsedLeaves, AvailableLeaves = dto.AvailableLeaves,
                CreatedBy = username,
                CreatedDate = DateTime.UtcNow
            };
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object>(HttpStatusCode.OK, "Created successfully");
        }

        public async Task<APIResponse<object>> UpdateAsync(int id, UpdateLeaveBalanceDto dto, string username)
        {
            if (id != dto.Id) return new APIResponse<object>(HttpStatusCode.BadRequest, "Id mismatch");
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return new APIResponse<object>(HttpStatusCode.NotFound, "Not found");

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