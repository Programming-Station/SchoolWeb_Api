using Microsoft.EntityFrameworkCore;
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
using School_DTOs;

namespace School.Services.Hr.Attendance
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IRepository<global::School.Domain.Hr.Attendance.Attendance> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public AttendanceService(IRepository<global::School.Domain.Hr.Attendance.Attendance> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse<List<AttendanceDto>>> GetAllByEmployeeIdAsync(int fkId)
        {
            var data = await _repository.List().Where(x => x.EmployeeId == fkId).Select(x => new AttendanceDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                AttendanceDate = x.AttendanceDate, CheckInTime = x.CheckInTime, CheckOutTime = x.CheckOutTime, Status = x.Status, Remarks = x.Remarks
            }).ToListAsync();

            return new APIResponse<List<AttendanceDto>> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = data };
        }

        public async Task<APIResponse<AttendanceDto>> GetByIdAsync(int id)
        {
            var data = await _repository.List().Where(x => x.Id == id).Select(x => new AttendanceDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                AttendanceDate = x.AttendanceDate, CheckInTime = x.CheckInTime, CheckOutTime = x.CheckOutTime, Status = x.Status, Remarks = x.Remarks
            }).FirstOrDefaultAsync();

            if (data == null) return new APIResponse<AttendanceDto> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            return new APIResponse<AttendanceDto> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = data };
        }

        public async Task<APIResponse<object>> CreateAsync(CreateAttendanceDto dto, string username)
        {
            var entity = new global::School.Domain.Hr.Attendance.Attendance
            {
                EmployeeId = dto.EmployeeId,
                AttendanceDate = dto.AttendanceDate, CheckInTime = dto.CheckInTime, CheckOutTime = dto.CheckOutTime, Status = dto.Status, Remarks = dto.Remarks,
                CreatedBy = username,
                CreatedDate = DateTime.UtcNow
            };
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Created successfully" };
        }

        public async Task<APIResponse<object>> UpdateAsync(int id, UpdateAttendanceDto dto, string username)
        {
            if (id != dto.Id) return new APIResponse<object> { StatusCode = HttpStatusCode.BadRequest, Message = "Id mismatch" };
            var entity = await _repository.List().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (entity == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };

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

        public async Task<APIResponse<object>> PunchInAsync(int employeeId, string username)
        {
            var today = DateTime.UtcNow.Date;
            var existing = await _repository.List().Where(x => x.EmployeeId == employeeId && x.AttendanceDate == today).FirstOrDefaultAsync();
            if (existing != null) return new APIResponse<object> { StatusCode = HttpStatusCode.BadRequest, Message = "Already punched in today" };

            var entity = new global::School.Domain.Hr.Attendance.Attendance
            {
                EmployeeId = employeeId,
                AttendanceDate = today,
                CheckInTime = DateTime.UtcNow.TimeOfDay,
                Status = "Present",
                CreatedBy = username,
                CreatedDate = DateTime.UtcNow
            };
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Punched in successfully" };
        }

        public async Task<APIResponse<object>> PunchOutAsync(int employeeId, string username)
        {
            var today = DateTime.UtcNow.Date;
            var entity = await _repository.List().Where(x => x.EmployeeId == employeeId && x.AttendanceDate == today).FirstOrDefaultAsync();
            if (entity == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "No punch in record found for today" };
            if (entity.CheckOutTime != null) return new APIResponse<object> { StatusCode = HttpStatusCode.BadRequest, Message = "Already punched out" };

            entity.CheckOutTime = DateTime.UtcNow.TimeOfDay;
            entity.UpdatedBy = username;
            entity.UpdatedDate = DateTime.UtcNow;

            _repository.Update(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Punched out successfully" };
        }
    }
}