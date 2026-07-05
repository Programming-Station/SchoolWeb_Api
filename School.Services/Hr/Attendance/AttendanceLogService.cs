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
    public class AttendanceLogService : IAttendanceLogService
    {
        private readonly IRepository<AttendanceLog> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public AttendanceLogService(IRepository<AttendanceLog> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse<List<AttendanceLogDto>>> GetAllByEmployeeIdAsync(int foreignKeyId)
        {
            var data = await _repository.GetAll().Where(x => x.EmployeeId == foreignKeyId).Select(x => new AttendanceLogDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                LogTime = x.LogTime, LogType = x.LogType, Source = x.Source, DeviceId = x.DeviceId
            }).ToListAsync();

            return new APIResponse<List<AttendanceLogDto>>(HttpStatusCode.OK, "Success", data);
        }

        public async Task<APIResponse<AttendanceLogDto>> GetByIdAsync(int id)
        {
            var data = await _repository.GetAll().Where(x => x.Id == id).Select(x => new AttendanceLogDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                LogTime = x.LogTime, LogType = x.LogType, Source = x.Source, DeviceId = x.DeviceId
            }).FirstOrDefaultAsync();

            if (data == null) return new APIResponse<AttendanceLogDto>(HttpStatusCode.NotFound, "Not found");
            return new APIResponse<AttendanceLogDto>(HttpStatusCode.OK, "Success", data);
        }

        public async Task<APIResponse<object>> CreateAsync(CreateAttendanceLogDto dto, string username)
        {
            var entity = new AttendanceLog
            {
                EmployeeId = dto.EmployeeId,
                LogTime = dto.LogTime, LogType = dto.LogType, Source = dto.Source, DeviceId = dto.DeviceId,
                CreatedBy = username,
                CreatedDate = DateTime.UtcNow
            };
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object>(HttpStatusCode.OK, "Created successfully");
        }

        public async Task<APIResponse<object>> UpdateAsync(int id, UpdateAttendanceLogDto dto, string username)
        {
            if (id != dto.Id) return new APIResponse<object>(HttpStatusCode.BadRequest, "Id mismatch");
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return new APIResponse<object>(HttpStatusCode.NotFound, "Not found");

            entity.EmployeeId = dto.EmployeeId;
            entity.LogTime = dto.LogTime;
            entity.LogType = dto.LogType;
            entity.Source = dto.Source;
            entity.DeviceId = dto.DeviceId;
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