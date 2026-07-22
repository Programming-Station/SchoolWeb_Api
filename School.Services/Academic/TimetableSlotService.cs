using System.Net;
using Microsoft.EntityFrameworkCore;
using School.Domain.Academic;
using School.Infrastructure.UnitOfWork.Interfaces;
using School.Services.Interfaces.Academic;
using School_DTOs;
using School_DTOs.Academic;
namespace School.Services.Academic
{
    public class TimetableSlotService : ITimetableSlotService
    {
        private readonly IRepository<TimetableSlot> _repo; private readonly IUnitOfWork _uow;
        public TimetableSlotService(IRepository<TimetableSlot> repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }
        public async Task<APIResponse<List<TimetableSlotDto>>> GetAllAsync() { var d = await _repo.List().Select(x => new TimetableSlotDto { Id = x.Id, SubjectId = x.SubjectId, DayOfWeek = x.DayOfWeek, StartTime = x.StartTime, EndTime = x.EndTime, Room = x.Room, TeacherName = x.TeacherName, Status = x.Status }).ToListAsync(); return new APIResponse<List<TimetableSlotDto>> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Success", Data = d }; }
        public async Task<APIResponse<TimetableSlotDto>> GetByIdAsync(int id) { var x = await _repo.List().Where(s => s.Id == id).Select(x => new TimetableSlotDto { Id = x.Id, SubjectId = x.SubjectId, DayOfWeek = x.DayOfWeek, StartTime = x.StartTime, EndTime = x.EndTime, Room = x.Room, TeacherName = x.TeacherName, Status = x.Status }).FirstOrDefaultAsync(); if (x == null) return new APIResponse<TimetableSlotDto> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" }; return new APIResponse<TimetableSlotDto> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Success", Data = x }; }
        public async Task<APIResponse<object>> CreateAsync(CreateTimetableSlotDto dto, string username) { await _repo.AddAsync(new TimetableSlot { SubjectId = dto.SubjectId, DayOfWeek = dto.DayOfWeek, StartTime = dto.StartTime, EndTime = dto.EndTime, Room = dto.Room, TeacherName = dto.TeacherName, Status = dto.Status, CreatedBy = username, CreatedDate = DateTime.UtcNow }); await _uow.CommitAsync(); return new APIResponse<object> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Created successfully" }; }
        public async Task<APIResponse<object>> UpdateAsync(int id, UpdateTimetableSlotDto dto, string username) { var e = await _repo.List().Where(x => x.Id == id).FirstOrDefaultAsync(); if (e == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" }; e.SubjectId = dto.SubjectId; e.DayOfWeek = dto.DayOfWeek; e.StartTime = dto.StartTime; e.EndTime = dto.EndTime; e.Room = dto.Room; e.TeacherName = dto.TeacherName; e.Status = dto.Status; e.UpdatedBy = username; e.UpdatedDate = DateTime.UtcNow; _repo.Update(e); await _uow.CommitAsync(); return new APIResponse<object> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Updated successfully" }; }
        public async Task<APIResponse<object>> DeleteAsync(int id, string username) { var e = await _repo.List().Where(x => x.Id == id).FirstOrDefaultAsync(); if (e == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" }; _repo.Delete(e); await _uow.CommitAsync(); return new APIResponse<object> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Deleted successfully" }; }
    }
}

