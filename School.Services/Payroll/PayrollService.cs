using Microsoft.EntityFrameworkCore;
using School.Domain.Payroll;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork.Interfaces;
using School.Services.Interfaces.Payroll;
using School_DTOs.Payroll;
using School_DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
namespace School.Services.Payroll
{
    public class SalaryComponentService : ISalaryComponentService
    {
        private readonly IRepository<SalaryComponent> _repo; private readonly IUnitOfWork _uow;
        public SalaryComponentService(IRepository<SalaryComponent> repo, IUnitOfWork uow){_repo=repo;_uow=uow;}
        public async Task<APIResponse<List<SalaryComponentDto>>> GetAllAsync(){var d=await _repo.List().Select(x=>new SalaryComponentDto{Id=x.Id,Name=x.Name,Type=x.Type,Amount=x.Amount,Status=x.Status}).ToListAsync();return new APIResponse<List<SalaryComponentDto>>{StatusCode=HttpStatusCode.OK,Message="Success",Data=d};}
        public async Task<APIResponse<SalaryComponentDto>> GetByIdAsync(int id){var x=await _repo.List().Where(s=>s.Id==id).Select(x=>new SalaryComponentDto{Id=x.Id,Name=x.Name,Type=x.Type,Amount=x.Amount,Status=x.Status}).FirstOrDefaultAsync();if(x==null)return new APIResponse<SalaryComponentDto>{StatusCode=HttpStatusCode.NotFound,Message="Not found"};return new APIResponse<SalaryComponentDto>{StatusCode=HttpStatusCode.OK,Message="Success",Data=x};}
        public async Task<APIResponse<object>> CreateAsync(CreateSalaryComponentDto dto,string username){await _repo.AddAsync(new SalaryComponent{Name=dto.Name,Type=dto.Type,Amount=dto.Amount,Status=dto.Status,CreatedBy=username,CreatedDate=DateTime.UtcNow});await _uow.CommitAsync();return new APIResponse<object>{StatusCode=HttpStatusCode.OK,Message="Created successfully"};}
        public async Task<APIResponse<object>> UpdateAsync(int id,UpdateSalaryComponentDto dto,string username){var e=await _repo.List().Where(x=>x.Id==id).FirstOrDefaultAsync();if(e==null)return new APIResponse<object>{StatusCode=HttpStatusCode.NotFound,Message="Not found"};e.Name=dto.Name;e.Type=dto.Type;e.Amount=dto.Amount;e.Status=dto.Status;e.UpdatedBy=username;e.UpdatedDate=DateTime.UtcNow;_repo.Update(e);await _uow.CommitAsync();return new APIResponse<object>{StatusCode=HttpStatusCode.OK,Message="Updated successfully"};}
        public async Task<APIResponse<object>> DeleteAsync(int id,string username){var e=await _repo.List().Where(x=>x.Id==id).FirstOrDefaultAsync();if(e==null)return new APIResponse<object>{StatusCode=HttpStatusCode.NotFound,Message="Not found"};_repo.Delete(e);await _uow.CommitAsync();return new APIResponse<object>{StatusCode=HttpStatusCode.OK,Message="Deleted successfully"};}
    }

    public class PayrollRunService : IPayrollRunService
    {
        private readonly IRepository<PayrollRun> _repo; private readonly IUnitOfWork _uow;
        public PayrollRunService(IRepository<PayrollRun> repo, IUnitOfWork uow){_repo=repo;_uow=uow;}
        public async Task<APIResponse<List<PayrollRunDto>>> GetAllByEmployeeIdAsync(int empId){var d=await _repo.List().Where(x=>x.EmployeeId==empId).Select(x=>new PayrollRunDto{Id=x.Id,EmployeeId=x.EmployeeId,Month=x.Month,GrossSalary=x.GrossSalary,TotalDeductions=x.TotalDeductions,NetSalary=x.NetSalary,Status=x.Status}).ToListAsync();return new APIResponse<List<PayrollRunDto>>{StatusCode=HttpStatusCode.OK,Message="Success",Data=d};}
        public async Task<APIResponse<PayrollRunDto>> GetByIdAsync(int id){var x=await _repo.List().Where(r=>r.Id==id).Select(x=>new PayrollRunDto{Id=x.Id,EmployeeId=x.EmployeeId,Month=x.Month,GrossSalary=x.GrossSalary,TotalDeductions=x.TotalDeductions,NetSalary=x.NetSalary,Status=x.Status}).FirstOrDefaultAsync();if(x==null)return new APIResponse<PayrollRunDto>{StatusCode=HttpStatusCode.NotFound,Message="Not found"};return new APIResponse<PayrollRunDto>{StatusCode=HttpStatusCode.OK,Message="Success",Data=x};}
        public async Task<APIResponse<object>> CreateAsync(CreatePayrollRunDto dto,string username){await _repo.AddAsync(new PayrollRun{EmployeeId=dto.EmployeeId,Month=dto.Month,GrossSalary=dto.GrossSalary,TotalDeductions=dto.TotalDeductions,NetSalary=dto.NetSalary,Status="Draft",CreatedBy=username,CreatedDate=DateTime.UtcNow});await _uow.CommitAsync();return new APIResponse<object>{StatusCode=HttpStatusCode.OK,Message="Created successfully"};}
        public async Task<APIResponse<object>> UpdateAsync(int id,UpdatePayrollRunDto dto,string username){var e=await _repo.List().Where(x=>x.Id==id).FirstOrDefaultAsync();if(e==null)return new APIResponse<object>{StatusCode=HttpStatusCode.NotFound,Message="Not found"};e.EmployeeId=dto.EmployeeId;e.Month=dto.Month;e.GrossSalary=dto.GrossSalary;e.TotalDeductions=dto.TotalDeductions;e.NetSalary=dto.NetSalary;e.UpdatedBy=username;e.UpdatedDate=DateTime.UtcNow;_repo.Update(e);await _uow.CommitAsync();return new APIResponse<object>{StatusCode=HttpStatusCode.OK,Message="Updated successfully"};}
        public async Task<APIResponse<object>> DeleteAsync(int id,string username){var e=await _repo.List().Where(x=>x.Id==id).FirstOrDefaultAsync();if(e==null)return new APIResponse<object>{StatusCode=HttpStatusCode.NotFound,Message="Not found"};_repo.Delete(e);await _uow.CommitAsync();return new APIResponse<object>{StatusCode=HttpStatusCode.OK,Message="Deleted successfully"};}
        public async Task<APIResponse<object>> ProcessPayrollAsync(int id,string username){var e=await _repo.List().Where(x=>x.Id==id).FirstOrDefaultAsync();if(e==null)return new APIResponse<object>{StatusCode=HttpStatusCode.NotFound,Message="Not found"};if(e.Status!="Draft")return new APIResponse<object>{StatusCode=HttpStatusCode.BadRequest,Message="Only draft payrolls can be processed"};e.Status="Processed";e.UpdatedBy=username;e.UpdatedDate=DateTime.UtcNow;_repo.Update(e);await _uow.CommitAsync();return new APIResponse<object>{StatusCode=HttpStatusCode.OK,Message="Payroll processed successfully"};}
        public async Task<APIResponse<object>> MarkAsPaidAsync(int id,string username){var e=await _repo.List().Where(x=>x.Id==id).FirstOrDefaultAsync();if(e==null)return new APIResponse<object>{StatusCode=HttpStatusCode.NotFound,Message="Not found"};if(e.Status!="Processed")return new APIResponse<object>{StatusCode=HttpStatusCode.BadRequest,Message="Only processed payrolls can be marked as paid"};e.Status="Paid";e.UpdatedBy=username;e.UpdatedDate=DateTime.UtcNow;_repo.Update(e);await _uow.CommitAsync();return new APIResponse<object>{StatusCode=HttpStatusCode.OK,Message="Payroll marked as paid"};}
    }
}
