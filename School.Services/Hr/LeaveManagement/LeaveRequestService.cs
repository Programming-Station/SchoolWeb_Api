using Microsoft.EntityFrameworkCore;
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
using School_DTOs;

namespace School.Services.Hr.LeaveManagement
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly IRepository<global::School.Domain.Hr.LeaveManagement.LeaveRequest> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public LeaveRequestService(IRepository<global::School.Domain.Hr.LeaveManagement.LeaveRequest> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse<List<LeaveRequestDto>>> GetAllByEmployeeIdAsync(int fkId)
        {
            var data = await _repository.List().Where(x => x.EmployeeId == fkId).Select(x => new LeaveRequestDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                LeaveTypeId = x.LeaveTypeId, StartDate = x.StartDate, EndDate = x.EndDate, TotalDays = x.TotalDays, Reason = x.Reason, Status = x.Status, ApprovedById = x.ApprovedById, Remarks = x.Remarks
            }).ToListAsync();

            return new APIResponse<List<LeaveRequestDto>> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = data };
        }

        public async Task<APIResponse<LeaveRequestDto>> GetByIdAsync(int id)
        {
            var data = await _repository.List().Where(x => x.Id == id).Select(x => new LeaveRequestDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                LeaveTypeId = x.LeaveTypeId, StartDate = x.StartDate, EndDate = x.EndDate, TotalDays = x.TotalDays, Reason = x.Reason, Status = x.Status, ApprovedById = x.ApprovedById, Remarks = x.Remarks
            }).FirstOrDefaultAsync();

            if (data == null) return new APIResponse<LeaveRequestDto> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            return new APIResponse<LeaveRequestDto> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = data };
        }

        public async Task<APIResponse<object>> CreateAsync(CreateLeaveRequestDto dto, string username)
        {
            var entity = new global::School.Domain.Hr.LeaveManagement.LeaveRequest
            {
                EmployeeId = dto.EmployeeId,
                LeaveTypeId = dto.LeaveTypeId, StartDate = dto.StartDate, EndDate = dto.EndDate, TotalDays = dto.TotalDays, Reason = dto.Reason, Status = dto.Status, ApprovedById = dto.ApprovedById, Remarks = dto.Remarks,
                CreatedBy = username,
                CreatedDate = DateTime.UtcNow
            };
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Created successfully" };
        }

        public async Task<APIResponse<object>> UpdateAsync(int id, UpdateLeaveRequestDto dto, string username)
        {
            if (id != dto.Id) return new APIResponse<object> { StatusCode = HttpStatusCode.BadRequest, Message = "Id mismatch" };
            var entity = await _repository.List().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (entity == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };

            entity.EmployeeId = dto.EmployeeId;
            entity.LeaveTypeId = dto.LeaveTypeId;
            entity.StartDate = dto.StartDate;
            entity.EndDate = dto.EndDate;
            entity.TotalDays = dto.TotalDays;
            entity.Reason = dto.Reason;
            entity.Status = dto.Status;
            entity.ApprovedById = dto.ApprovedById;
            entity.Remarks = dto.Remarks;
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