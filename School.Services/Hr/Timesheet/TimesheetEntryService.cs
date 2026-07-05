using Microsoft.EntityFrameworkCore;
using School.Domain.Hr.Timesheet;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork.Interfaces;
using School.Services.Interfaces.Hr.Timesheet;
using School_DTOs.Common;
using School_DTOs.Hr.Timesheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace School.Services.Hr.Timesheet
{
    public class TimesheetEntryService : ITimesheetEntryService
    {
        private readonly IRepository<TimesheetEntry> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public TimesheetEntryService(IRepository<TimesheetEntry> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse<List<TimesheetEntryDto>>> GetAllByTimesheetIdAsync(int foreignKeyId)
        {
            var data = await _repository.GetAll().Where(x => x.TimesheetId == foreignKeyId).Select(x => new TimesheetEntryDto
            {
                Id = x.Id,
                TimesheetId = x.TimesheetId,
                TimesheetId = x.TimesheetId, EntryDate = x.EntryDate, TaskName = x.TaskName, ProjectName = x.ProjectName, HoursWorked = x.HoursWorked, Description = x.Description
            }).ToListAsync();

            return new APIResponse<List<TimesheetEntryDto>>(HttpStatusCode.OK, "Success", data);
        }

        public async Task<APIResponse<TimesheetEntryDto>> GetByIdAsync(int id)
        {
            var data = await _repository.GetAll().Where(x => x.Id == id).Select(x => new TimesheetEntryDto
            {
                Id = x.Id,
                TimesheetId = x.TimesheetId,
                TimesheetId = x.TimesheetId, EntryDate = x.EntryDate, TaskName = x.TaskName, ProjectName = x.ProjectName, HoursWorked = x.HoursWorked, Description = x.Description
            }).FirstOrDefaultAsync();

            if (data == null) return new APIResponse<TimesheetEntryDto>(HttpStatusCode.NotFound, "Not found");
            return new APIResponse<TimesheetEntryDto>(HttpStatusCode.OK, "Success", data);
        }

        public async Task<APIResponse<object>> CreateAsync(CreateTimesheetEntryDto dto, string username)
        {
            var entity = new TimesheetEntry
            {
                TimesheetId = dto.TimesheetId,
                TimesheetId = dto.TimesheetId, EntryDate = dto.EntryDate, TaskName = dto.TaskName, ProjectName = dto.ProjectName, HoursWorked = dto.HoursWorked, Description = dto.Description,
                CreatedBy = username,
                CreatedDate = DateTime.UtcNow
            };
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object>(HttpStatusCode.OK, "Created successfully");
        }

        public async Task<APIResponse<object>> UpdateAsync(int id, UpdateTimesheetEntryDto dto, string username)
        {
            if (id != dto.Id) return new APIResponse<object>(HttpStatusCode.BadRequest, "Id mismatch");
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return new APIResponse<object>(HttpStatusCode.NotFound, "Not found");

            entity.TimesheetId = dto.TimesheetId;
            entity.TimesheetId = dto.TimesheetId;
            entity.EntryDate = dto.EntryDate;
            entity.TaskName = dto.TaskName;
            entity.ProjectName = dto.ProjectName;
            entity.HoursWorked = dto.HoursWorked;
            entity.Description = dto.Description;
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