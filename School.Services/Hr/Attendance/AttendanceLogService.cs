using System.Net;
using Microsoft.EntityFrameworkCore;
using School.Infrastructure.UnitOfWork.Interfaces;
using School.Services.Interfaces.Hr.Attendance;
using School_DTOs;
using School_DTOs.Hr.Attendance;

namespace School.Services.Hr.Attendance
{
    public class AttendanceLogService : IAttendanceLogService
    {
        private readonly IRepository<global::School.Domain.Hr.Attendance.AttendanceLog> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public AttendanceLogService(IRepository<global::School.Domain.Hr.Attendance.AttendanceLog> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse<List<AttendanceLogDto>>> GetAllByEmployeeIdAsync(int fkId)
        {
            var data = await _repository.List().Where(x => x.EmployeeId == fkId).Select(x => new AttendanceLogDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                LogTime = x.LogTime,
                LogType = x.LogType,
                Source = x.Source,
                DeviceId = x.DeviceId
            }).ToListAsync();

            return new APIResponse<List<AttendanceLogDto>> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = data };
        }

        public async Task<APIResponse<AttendanceLogDto>> GetByIdAsync(int id)
        {
            var data = await _repository.List().Where(x => x.Id == id).Select(x => new AttendanceLogDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                LogTime = x.LogTime,
                LogType = x.LogType,
                Source = x.Source,
                DeviceId = x.DeviceId
            }).FirstOrDefaultAsync();

            if (data == null) return new APIResponse<AttendanceLogDto> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            return new APIResponse<AttendanceLogDto> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = data };
        }

        public async Task<APIResponse<object>> CreateAsync(CreateAttendanceLogDto dto, string username)
        {
            var entity = new global::School.Domain.Hr.Attendance.AttendanceLog
            {
                EmployeeId = dto.EmployeeId,
                LogTime = dto.LogTime,
                LogType = dto.LogType,
                Source = dto.Source,
                DeviceId = dto.DeviceId,
                CreatedBy = username,
                CreatedDate = DateTime.UtcNow
            };
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Created successfully" };
        }

        public async Task<APIResponse<object>> UpdateAsync(int id, UpdateAttendanceLogDto dto, string username)
        {
            if (id != dto.Id) return new APIResponse<object> { StatusCode = HttpStatusCode.BadRequest, Message = "Id mismatch" };
            var entity = await _repository.List().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (entity == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };

            entity.EmployeeId = dto.EmployeeId;
            entity.LogTime = dto.LogTime;
            entity.LogType = dto.LogType;
            entity.Source = dto.Source;
            entity.DeviceId = dto.DeviceId;
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