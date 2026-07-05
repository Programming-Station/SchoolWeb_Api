using Microsoft.EntityFrameworkCore;
using School.Domain.Hr.Attendance;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork.Interfaces;
using School.Services.Interfaces.Hr.Attendance;
using School_DTOs.Common;
using School_DTOs.Hr.Attendance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace School.Services.Hr.Attendance
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IRepository<Attendance> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public AttendanceService(IRepository<Attendance> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse<List<AttendanceDto>>> GetAllByEmployeeIdAsync(int foreignKeyId)
        {
            var data = await _repository.GetAll().Where(x => x.EmployeeId == foreignKeyId).Select(x => new AttendanceDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                AttendanceDate = x.AttendanceDate, CheckInTime = x.CheckInTime, CheckOutTime = x.CheckOutTime, Status = x.Status, Remarks = x.Remarks
            }).ToListAsync();

            return new APIResponse<List<AttendanceDto>>(HttpStatusCode.OK, "Success", data);
        }

        public async Task<APIResponse<AttendanceDto>> GetByIdAsync(int id)
        {
            var data = await _repository.GetAll().Where(x => x.Id == id).Select(x => new AttendanceDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                AttendanceDate = x.AttendanceDate, CheckInTime = x.CheckInTime, CheckOutTime = x.CheckOutTime, Status = x.Status, Remarks = x.Remarks
            }).FirstOrDefaultAsync();

            if (data == null) return new APIResponse<AttendanceDto>(HttpStatusCode.NotFound, "Not found");
            return new APIResponse<AttendanceDto>(HttpStatusCode.OK, "Success", data);
        }

        public async Task<APIResponse<object>> CreateAsync(CreateAttendanceDto dto, string username)
        {
            var entity = new Attendance
            {
                EmployeeId = dto.EmployeeId,
                AttendanceDate = dto.AttendanceDate, CheckInTime = dto.CheckInTime, CheckOutTime = dto.CheckOutTime, Status = dto.Status, Remarks = dto.Remarks,
                CreatedBy = username,
                CreatedDate = DateTime.UtcNow
            };
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object>(HttpStatusCode.OK, "Created successfully");
        }

        public async Task<APIResponse<object>> UpdateAsync(int id, UpdateAttendanceDto dto, string username)
        {
            if (id != dto.Id) return new APIResponse<object>(HttpStatusCode.BadRequest, "Id mismatch");
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return new APIResponse<object>(HttpStatusCode.NotFound, "Not found");

            entity.EmployeeId = dto.EmployeeId;
            entity.AttendanceDate = dto.AttendanceDate;
            entity.CheckInTime = dto.CheckInTime;
            entity.CheckOutTime = dto.CheckOutTime;
            entity.Status = dto.Status;
            entity.Remarks = dto.Remarks;
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