using Microsoft.AspNetCore.Mvc;
using School.Services.Interfaces.Payroll;
using School_API.Common.Interface;
using School_DTOs.Payroll;
using System.Threading.Tasks;
namespace School_API.Controllers.Payroll
{
    [Route("api/[controller]")][ApiController]
    public class SalaryComponentController : BaseController
    {
        private readonly ISalaryComponentService _svc;
        public SalaryComponentController(ISalaryComponentService svc,ICurrentUserService cur):base(cur){_svc=svc;}
        [HttpGet] public async Task<IActionResult> GetAll(){var r=await _svc.GetAllAsync();return r.Success?Ok(r):StatusCode((int)r.StatusCode,r);}
        [HttpGet("{id}")] public async Task<IActionResult> GetById(int id){var r=await _svc.GetByIdAsync(id);return r.Success?Ok(r):StatusCode((int)r.StatusCode,r);}
        [HttpPost] public async Task<IActionResult> Create([FromBody]CreateSalaryComponentDto m){var r=await _svc.CreateAsync(m,UserName);return StatusCode((int)r.StatusCode,r);}
        [HttpPut] public async Task<IActionResult> Update([FromBody]UpdateSalaryComponentDto m){var r=await _svc.UpdateAsync(m.Id,m,UserName);return r.Success?Ok(r):StatusCode((int)r.StatusCode,r);}
        [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id){var r=await _svc.DeleteAsync(id,UserName);return r.Success?Ok(r):StatusCode((int)r.StatusCode,r);}
    }

    [Route("api/[controller]")][ApiController]
    public class PayrollController : BaseController
    {
        private readonly IPayrollRunService _svc;
        public PayrollController(IPayrollRunService svc,ICurrentUserService cur):base(cur){_svc=svc;}
        [HttpGet("employee/{empId}")] public async Task<IActionResult> GetByEmployee(int empId){var r=await _svc.GetAllByEmployeeIdAsync(empId);return r.Success?Ok(r):StatusCode((int)r.StatusCode,r);}
        [HttpGet("{id}")] public async Task<IActionResult> GetById(int id){var r=await _svc.GetByIdAsync(id);return r.Success?Ok(r):StatusCode((int)r.StatusCode,r);}
        [HttpPost] public async Task<IActionResult> Create([FromBody]CreatePayrollRunDto m){var r=await _svc.CreateAsync(m,UserName);return StatusCode((int)r.StatusCode,r);}
        [HttpPut] public async Task<IActionResult> Update([FromBody]UpdatePayrollRunDto m){var r=await _svc.UpdateAsync(m.Id,m,UserName);return r.Success?Ok(r):StatusCode((int)r.StatusCode,r);}
        [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id){var r=await _svc.DeleteAsync(id,UserName);return r.Success?Ok(r):StatusCode((int)r.StatusCode,r);}
        [HttpPut("{id}/process")] public async Task<IActionResult> Process(int id){var r=await _svc.ProcessPayrollAsync(id,UserName);return r.Success?Ok(r):StatusCode((int)r.StatusCode,r);}
        [HttpPut("{id}/mark-paid")] public async Task<IActionResult> MarkPaid(int id){var r=await _svc.MarkAsPaidAsync(id,UserName);return r.Success?Ok(r):StatusCode((int)r.StatusCode,r);}
    }
}

