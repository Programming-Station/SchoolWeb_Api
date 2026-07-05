using Microsoft.EntityFrameworkCore;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork.Interfaces;
using School.Services.Interfaces.Hr;
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
    public class EmployeeDocumentService : IEmployeeDocumentService
    {
        private readonly IRepository<global::School.Domain.Hr.EmployeeDocument> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeDocumentService(IRepository<global::School.Domain.Hr.EmployeeDocument> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse<List<EmployeeDocumentDto>>> GetAllByEmployeeIdAsync(int fkId)
        {
            var data = await _repository.List().Where(x => x.EmployeeId == fkId).Select(x => new EmployeeDocumentDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                DocumentName = x.DocumentName, DocumentType = x.DocumentType, FilePath = x.FilePath
            }).ToListAsync();

            return new APIResponse<List<EmployeeDocumentDto>> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = data };
        }

        public async Task<APIResponse<EmployeeDocumentDto>> GetByIdAsync(int id)
        {
            var data = await _repository.List().Where(x => x.Id == id).Select(x => new EmployeeDocumentDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                DocumentName = x.DocumentName, DocumentType = x.DocumentType, FilePath = x.FilePath
            }).FirstOrDefaultAsync();

            if (data == null) return new APIResponse<EmployeeDocumentDto> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            return new APIResponse<EmployeeDocumentDto> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = data };
        }

        public async Task<APIResponse<object>> CreateAsync(CreateEmployeeDocumentDto dto, string username)
        {
            var entity = new global::School.Domain.Hr.EmployeeDocument
            {
                EmployeeId = dto.EmployeeId,
                DocumentName = dto.DocumentName, DocumentType = dto.DocumentType, FilePath = dto.FilePath,
                CreatedBy = username,
                CreatedDate = DateTime.UtcNow
            };
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Created successfully" };
        }

        public async Task<APIResponse<object>> UpdateAsync(int id, UpdateEmployeeDocumentDto dto, string username)
        {
            if (id != dto.Id) return new APIResponse<object> { StatusCode = HttpStatusCode.BadRequest, Message = "Id mismatch" };
            var entity = await _repository.List().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (entity == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };

            entity.EmployeeId = dto.EmployeeId;
            entity.DocumentName = dto.DocumentName;
            entity.DocumentType = dto.DocumentType;
            entity.FilePath = dto.FilePath;
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