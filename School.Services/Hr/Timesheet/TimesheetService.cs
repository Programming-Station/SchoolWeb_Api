using System.Net;
using Microsoft.EntityFrameworkCore;
using School.Infrastructure.UnitOfWork.Interfaces;
using School.Services.Interfaces.Hr.Timesheet;
using School_DTOs;
using School_DTOs.Hr.Timesheet;

namespace School.Services.Hr.Timesheet
{
    public class TimesheetService : ITimesheetService
    {
        private readonly IRepository<global::School.Domain.Hr.Timesheet.Timesheet> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public TimesheetService(IRepository<global::School.Domain.Hr.Timesheet.Timesheet> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse<List<TimesheetDto>>> GetAllByEmployeeIdAsync(int fkId)
        {
            var data = await _repository.List().Where(x => x.EmployeeId == fkId).Select(x => new TimesheetDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Status = x.Status,
                ApprovedById = x.ApprovedById,
                TotalHours = x.TotalHours
            }).ToListAsync();

            return new APIResponse<List<TimesheetDto>> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = data };
        }

        public async Task<APIResponse<TimesheetDto>> GetByIdAsync(int id)
        {
            var data = await _repository.List().Where(x => x.Id == id).Select(x => new TimesheetDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Status = x.Status,
                ApprovedById = x.ApprovedById,
                TotalHours = x.TotalHours
            }).FirstOrDefaultAsync();

            if (data == null) return new APIResponse<TimesheetDto> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            return new APIResponse<TimesheetDto> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = data };
        }

        public async Task<APIResponse<object>> CreateAsync(CreateTimesheetDto dto, string username)
        {
            var entity = new global::School.Domain.Hr.Timesheet.Timesheet
            {
                EmployeeId = dto.EmployeeId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = dto.Status,
                ApprovedById = dto.ApprovedById,
                TotalHours = dto.TotalHours,
                CreatedBy = username,
                CreatedDate = DateTime.UtcNow
            };
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Created successfully" };
        }

        public async Task<APIResponse<object>> UpdateAsync(int id, UpdateTimesheetDto dto, string username)
        {
            if (id != dto.Id) return new APIResponse<object> { StatusCode = HttpStatusCode.BadRequest, Message = "Id mismatch" };
            var entity = await _repository.List().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (entity == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };

            entity.EmployeeId = dto.EmployeeId;
            entity.StartDate = dto.StartDate;
            entity.EndDate = dto.EndDate;
            entity.Status = dto.Status;
            entity.ApprovedById = dto.ApprovedById;
            entity.TotalHours = dto.TotalHours;
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

        public async Task<APIResponse<object>> SubmitTimesheetAsync(int id, string username)
        {
            var entity = await _repository.List()
                .Include(x => x.Entries)
                .Where(x => x.Id == id).FirstOrDefaultAsync();
            if (entity == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Timesheet not found" };
            if (entity.Status != "Draft") return new APIResponse<object> { StatusCode = HttpStatusCode.BadRequest, Message = "Only draft timesheets can be submitted" };
            if (!entity.Entries.Any()) return new APIResponse<object> { StatusCode = HttpStatusCode.BadRequest, Message = "Cannot submit a timesheet with no entries" };

            entity.TotalHours = entity.Entries.Sum(e => e.HoursWorked);
            entity.Status = "Submitted";
            entity.UpdatedBy = username;
            entity.UpdatedDate = DateTime.UtcNow;

            _repository.Update(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = $"Timesheet submitted. Total hours: {entity.TotalHours}" };
        }

        public async Task<APIResponse<object>> ApproveTimesheetAsync(int id, int approverEmployeeId, string username)
        {
            var entity = await _repository.List().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (entity == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Timesheet not found" };
            if (entity.Status != "Submitted") return new APIResponse<object> { StatusCode = HttpStatusCode.BadRequest, Message = "Only submitted timesheets can be approved" };

            entity.Status = "Approved";
            entity.ApprovedById = approverEmployeeId;
            entity.UpdatedBy = username;
            entity.UpdatedDate = DateTime.UtcNow;

            _repository.Update(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Timesheet approved successfully" };
        }

        public async Task<APIResponse<object>> RejectTimesheetAsync(int id, int approverEmployeeId, string reason, string username)
        {
            var entity = await _repository.List().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (entity == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Timesheet not found" };
            if (entity.Status != "Submitted") return new APIResponse<object> { StatusCode = HttpStatusCode.BadRequest, Message = "Only submitted timesheets can be rejected" };

            entity.Status = "Rejected";
            entity.ApprovedById = approverEmployeeId;
            entity.UpdatedBy = username;
            entity.UpdatedDate = DateTime.UtcNow;

            _repository.Update(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Timesheet rejected" };
        }
    }
}