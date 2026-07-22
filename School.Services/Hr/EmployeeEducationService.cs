using System.Net;
using Microsoft.EntityFrameworkCore;
using School.Infrastructure.UnitOfWork.Interfaces;
using School_DTOs;
using School_DTOs.Hr;

namespace School.Services.Hr
{
    public class EmployeeEducationService : IEmployeeEducationService
    {
        private readonly IRepository<global::School.Domain.Hr.EmployeeEducation> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeEducationService(IRepository<global::School.Domain.Hr.EmployeeEducation> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse<List<EmployeeEducationDto>>> GetAllByEmployeeIdAsync(int fkId)
        {
            var data = await _repository.List().Where(x => x.EmployeeId == fkId).Select(x => new EmployeeEducationDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                Degree = x.Degree,
                Board = x.Board,
                University = x.University,
                PassingYear = x.PassingYear,
                Percentage = x.Percentage
            }).ToListAsync();

            return new APIResponse<List<EmployeeEducationDto>> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = data };
        }

        public async Task<APIResponse<EmployeeEducationDto>> GetByIdAsync(int id)
        {
            var data = await _repository.List().Where(x => x.Id == id).Select(x => new EmployeeEducationDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                Degree = x.Degree,
                Board = x.Board,
                University = x.University,
                PassingYear = x.PassingYear,
                Percentage = x.Percentage
            }).FirstOrDefaultAsync();

            if (data == null) return new APIResponse<EmployeeEducationDto> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            return new APIResponse<EmployeeEducationDto> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = data };
        }

        public async Task<APIResponse<object>> CreateAsync(CreateEmployeeEducationDto dto, string username)
        {
            var entity = new global::School.Domain.Hr.EmployeeEducation
            {
                EmployeeId = dto.EmployeeId,
                Degree = dto.Degree,
                Board = dto.Board,
                University = dto.University,
                PassingYear = dto.PassingYear,
                Percentage = dto.Percentage,
                CreatedBy = username,
                CreatedDate = DateTime.UtcNow
            };
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Created successfully" };
        }

        public async Task<APIResponse<object>> UpdateAsync(int id, UpdateEmployeeEducationDto dto, string username)
        {
            if (id != dto.Id) return new APIResponse<object> { StatusCode = HttpStatusCode.BadRequest, Message = "Id mismatch" };
            var entity = await _repository.List().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (entity == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };

            entity.EmployeeId = dto.EmployeeId;
            entity.Degree = dto.Degree;
            entity.Board = dto.Board;
            entity.University = dto.University;
            entity.PassingYear = dto.PassingYear;
            entity.Percentage = dto.Percentage;
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