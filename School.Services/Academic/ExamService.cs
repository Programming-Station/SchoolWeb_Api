using School_DTOs;
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
    public class ExamService : IExamService
    {
        private readonly IRepository<Exam> _repo; private readonly IUnitOfWork _uow;
        public ExamService(IRepository<Exam> repo, IUnitOfWork uow){_repo=repo;_uow=uow;}
        public async Task<APIResponse<List<ExamDto>>> GetAllAsync(){var d=await _repo.List().Select(x=>new ExamDto{Id=x.Id,Name=x.Name,ExamType=x.ExamType,StartDate=x.StartDate,EndDate=x.EndDate,Status=x.Status,Description=x.Description}).ToListAsync();return new APIResponse<List<ExamDto>>{StatusCode=HttpStatusCode.OK,Message="Success",Data=d};}
        public async Task<APIResponse<ExamDto>> GetByIdAsync(int id){var x=await _repo.List().Where(e=>e.Id==id).Select(x=>new ExamDto{Id=x.Id,Name=x.Name,ExamType=x.ExamType,StartDate=x.StartDate,EndDate=x.EndDate,Status=x.Status,Description=x.Description}).FirstOrDefaultAsync();if(x==null)return new APIResponse<ExamDto>{StatusCode=HttpStatusCode.NotFound,Message="Not found"};return new APIResponse<ExamDto>{StatusCode=HttpStatusCode.OK,Message="Success",Data=x};}
        public async Task<APIResponse<object>> CreateAsync(CreateExamDto dto,string username){await _repo.AddAsync(new Exam{Name=dto.Name,ExamType=dto.ExamType,StartDate=dto.StartDate,EndDate=dto.EndDate,Status=dto.Status,Description=dto.Description,CreatedBy=username,CreatedDate=DateTime.UtcNow});await _uow.CommitAsync();return new APIResponse<object>{StatusCode=HttpStatusCode.OK,Message="Created successfully"};}
        public async Task<APIResponse<object>> UpdateAsync(int id,UpdateExamDto dto,string username){var e=await _repo.List().Where(x=>x.Id==id).FirstOrDefaultAsync();if(e==null)return new APIResponse<object>{StatusCode=HttpStatusCode.NotFound,Message="Not found"};e.Name=dto.Name;e.ExamType=dto.ExamType;e.StartDate=dto.StartDate;e.EndDate=dto.EndDate;e.Status=dto.Status;e.Description=dto.Description;e.UpdatedBy=username;e.UpdatedDate=DateTime.UtcNow;_repo.Update(e);await _uow.CommitAsync();return new APIResponse<object>{StatusCode=HttpStatusCode.OK,Message="Updated successfully"};}
        public async Task<APIResponse<object>> DeleteAsync(int id,string username){var e=await _repo.List().Where(x=>x.Id==id).FirstOrDefaultAsync();if(e==null)return new APIResponse<object>{StatusCode=HttpStatusCode.NotFound,Message="Not found"};_repo.Delete(e);await _uow.CommitAsync();return new APIResponse<object>{StatusCode=HttpStatusCode.OK,Message="Deleted successfully"};}
    }

    public class ExamResultService : IExamResultService
    {
        private readonly IRepository<ExamResult> _repo; private readonly IUnitOfWork _uow;
        public ExamResultService(IRepository<ExamResult> repo, IUnitOfWork uow){_repo=repo;_uow=uow;}
        public async Task<APIResponse<List<ExamResultDto>>> GetAllByExamIdAsync(int examId){var d=await _repo.List().Where(x=>x.ExamId==examId).Select(x=>new ExamResultDto{Id=x.Id,ExamId=x.ExamId,StudentId=x.StudentId,SubjectId=x.SubjectId,MarksObtained=x.MarksObtained,TotalMarks=x.TotalMarks,Grade=x.Grade,Status=x.Status}).ToListAsync();return new APIResponse<List<ExamResultDto>>{StatusCode=HttpStatusCode.OK,Message="Success",Data=d};}
        public async Task<APIResponse<ExamResultDto>> GetByIdAsync(int id){var x=await _repo.List().Where(r=>r.Id==id).Select(x=>new ExamResultDto{Id=x.Id,ExamId=x.ExamId,StudentId=x.StudentId,SubjectId=x.SubjectId,MarksObtained=x.MarksObtained,TotalMarks=x.TotalMarks,Grade=x.Grade,Status=x.Status}).FirstOrDefaultAsync();if(x==null)return new APIResponse<ExamResultDto>{StatusCode=HttpStatusCode.NotFound,Message="Not found"};return new APIResponse<ExamResultDto>{StatusCode=HttpStatusCode.OK,Message="Success",Data=x};}
        public async Task<APIResponse<object>> CreateAsync(CreateExamResultDto dto,string username){await _repo.AddAsync(new ExamResult{ExamId=dto.ExamId,StudentId=dto.StudentId,SubjectId=dto.SubjectId,MarksObtained=dto.MarksObtained,TotalMarks=dto.TotalMarks,Grade=dto.Grade,Status=dto.Status,CreatedBy=username,CreatedDate=DateTime.UtcNow});await _uow.CommitAsync();return new APIResponse<object>{StatusCode=HttpStatusCode.OK,Message="Created successfully"};}
        public async Task<APIResponse<object>> UpdateAsync(int id,UpdateExamResultDto dto,string username){var e=await _repo.List().Where(x=>x.Id==id).FirstOrDefaultAsync();if(e==null)return new APIResponse<object>{StatusCode=HttpStatusCode.NotFound,Message="Not found"};e.ExamId=dto.ExamId;e.StudentId=dto.StudentId;e.SubjectId=dto.SubjectId;e.MarksObtained=dto.MarksObtained;e.TotalMarks=dto.TotalMarks;e.Grade=dto.Grade;e.Status=dto.Status;e.UpdatedBy=username;e.UpdatedDate=DateTime.UtcNow;_repo.Update(e);await _uow.CommitAsync();return new APIResponse<object>{StatusCode=HttpStatusCode.OK,Message="Updated successfully"};}
        public async Task<APIResponse<object>> DeleteAsync(int id,string username){var e=await _repo.List().Where(x=>x.Id==id).FirstOrDefaultAsync();if(e==null)return new APIResponse<object>{StatusCode=HttpStatusCode.NotFound,Message="Not found"};_repo.Delete(e);await _uow.CommitAsync();return new APIResponse<object>{StatusCode=HttpStatusCode.OK,Message="Deleted successfully"};}
    }
}

