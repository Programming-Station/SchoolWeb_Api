using Microsoft.EntityFrameworkCore;
using School.Domain.Academic;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork.Interfaces;
using School.Services.Interfaces.Academic;
using School_DTOs.Academic;
using School_DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace School.Services.Academic
{
    public class SubjectService : ISubjectService
    {
        private readonly IRepository<Subject> _repo;
        private readonly IUnitOfWork _uow;
        public SubjectService(IRepository<Subject> repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }

        public async Task<APIResponse<List<SubjectDto>>> GetAllAsync()
        {
            var data = await _repo.List().Select(x => new SubjectDto { Id=x.Id, Name=x.Name, Code=x.Code, Description=x.Description, DisplayOrder=x.DisplayOrder, Status=x.Status }).ToListAsync();
            return new APIResponse<List<SubjectDto>> { StatusCode=HttpStatusCode.OK, Message="Success", Data=data };
        }
        public async Task<APIResponse<SubjectDto>> GetByIdAsync(int id)
        {
            var x = await _repo.List().Where(s=>s.Id==id).Select(x => new SubjectDto { Id=x.Id, Name=x.Name, Code=x.Code, Description=x.Description, DisplayOrder=x.DisplayOrder, Status=x.Status }).FirstOrDefaultAsync();
            if(x==null) return new APIResponse<SubjectDto>{StatusCode=HttpStatusCode.NotFound,Message="Not found"};
            return new APIResponse<SubjectDto>{StatusCode=HttpStatusCode.OK,Message="Success",Data=x};
        }
        public async Task<APIResponse<object>> CreateAsync(CreateSubjectDto dto, string username)
        {
            await _repo.AddAsync(new Subject { Name=dto.Name,Code=dto.Code,Description=dto.Description,DisplayOrder=dto.DisplayOrder,Status=dto.Status,CreatedBy=username,CreatedDate=DateTime.UtcNow });
            await _uow.CommitAsync();
            return new APIResponse<object>{StatusCode=HttpStatusCode.OK,Message="Created successfully"};
        }
        public async Task<APIResponse<object>> UpdateAsync(int id, UpdateSubjectDto dto, string username)
        {
            var e=await _repo.List().Where(x=>x.Id==id).FirstOrDefaultAsync();
            if(e==null) return new APIResponse<object>{StatusCode=HttpStatusCode.NotFound,Message="Not found"};
            e.Name=dto.Name;e.Code=dto.Code;e.Description=dto.Description;e.DisplayOrder=dto.DisplayOrder;e.Status=dto.Status;e.UpdatedBy=username;e.UpdatedDate=DateTime.UtcNow;
            _repo.Update(e); await _uow.CommitAsync();
            return new APIResponse<object>{StatusCode=HttpStatusCode.OK,Message="Updated successfully"};
        }
        public async Task<APIResponse<object>> DeleteAsync(int id, string username)
        {
            var e=await _repo.List().Where(x=>x.Id==id).FirstOrDefaultAsync();
            if(e==null) return new APIResponse<object>{StatusCode=HttpStatusCode.NotFound,Message="Not found"};
            _repo.Delete(e); await _uow.CommitAsync();
            return new APIResponse<object>{StatusCode=HttpStatusCode.OK,Message="Deleted successfully"};
        }
    }
}
