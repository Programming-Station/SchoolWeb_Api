using AutoMapper;
using Microsoft.EntityFrameworkCore;
using School.Domain;
using School.Domain.Hr;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork.Interfaces;
using School_DTOs.Common;
using School_DTOs.Hr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using School_DTOs;

namespace School.Services.Hr
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IRepository<Department> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentService(IRepository<Department> repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<APIResponse<PagedResponse<DepartmentDto>>> GetAllAsync(PaginationFilterDto filter)
        {
            var query = _repository.List().Include(x => x.Faculty).AsNoTracking();
            if (!string.IsNullOrEmpty(filter.SearchText))
                query = query.Where(x => x.Name.Contains(filter.SearchText) || (x.Code != null && x.Code.Contains(filter.SearchText)));

            // if (!string.IsNullOrEmpty(filter.Status))
                // query = query.Where(x => x.Status == filter.Status);

            var totalRecords = await query.CountAsync();
            var data = await query.OrderBy(x => x.DisplayOrder).ThenBy(x => x.Name)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new DepartmentDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    FacultyId = x.FacultyId,
                    FacultyName = x.Faculty.Name,
                    Description = x.Description,
                    DisplayOrder = x.DisplayOrder,
                    Status = x.Status
                })
                .ToListAsync();

            return new APIResponse<PagedResponse<DepartmentDto>> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = new PagedResponse<DepartmentDto> { Data = data, TotalRecords = totalRecords, CurrentPage = filter.PageNumber, PageSize = filter.PageSize } };
        }

        public async Task<APIResponse<DepartmentDto>> GetByIdAsync(int id)
        {
            var data = await _repository.List().Include(x => x.Faculty).Where(x => x.Id == id).Select(x => new DepartmentDto
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                FacultyId = x.FacultyId,
                FacultyName = x.Faculty.Name,
                Description = x.Description,
                DisplayOrder = x.DisplayOrder,
                Status = x.Status
            }).FirstOrDefaultAsync();

            if (data == null)
                return new APIResponse<DepartmentDto> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            return new APIResponse<DepartmentDto> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = data };
        }

        public async Task<APIResponse<object>> CreateAsync(CreateDepartmentDto dto, string username)
        {
            var entity = new Department
            {
                Name = dto.Name,
                Code = dto.Code,
                FacultyId = dto.FacultyId,
                Description = dto.Description,
                DisplayOrder = dto.DisplayOrder,
                Status = dto.Status,
                CreatedBy = username,
                CreatedDate = DateTime.UtcNow
            };
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Created successfully" };
        }

        public async Task<APIResponse<object>> UpdateAsync(int id, UpdateDepartmentDto dto, string username)
        {
            if (id != dto.Id) return new APIResponse<object> { StatusCode = HttpStatusCode.BadRequest, Message = "Id mismatch" };
            var entity = await _repository.List().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (entity == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };

            entity.Name = dto.Name;
            entity.Code = dto.Code;
            entity.FacultyId = dto.FacultyId;
            entity.Description = dto.Description;
            entity.DisplayOrder = dto.DisplayOrder;
            entity.Status = dto.Status;
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

        public async Task<APIResponse<object>> ToggleStatusAsync(int id, string username)
        {
            var entity = await _repository.List().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (entity == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            entity.Status = entity.Status == "active" ? "inactive" : "active";
            entity.UpdatedBy = username;
            entity.UpdatedDate = DateTime.UtcNow;
            _repository.Update(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Status toggled successfully" };
        }
    }
}
