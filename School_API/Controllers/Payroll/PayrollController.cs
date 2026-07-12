using Microsoft.AspNetCore.Mvc;
using School.Services.Interfaces.Payroll;
using School_API.Common.Interface;
using School_DTOs.Payroll;
using System.Threading.Tasks;

namespace School_API.Controllers.Payroll
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryComponentController : BaseController
    {
        private readonly ISalaryComponentService _svc;
        public SalaryComponentController(ISalaryComponentService svc, ICurrentUserService cur) : base(cur) { _svc = svc; }

        [HttpGet] public async Task<IActionResult> GetAll() { var r = await _svc.GetAllAsync(); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpGet("{id}")] public async Task<IActionResult> GetById(int id) { var r = await _svc.GetByIdAsync(id); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpPost] public async Task<IActionResult> Create([FromBody] CreateSalaryComponentDto m) { var r = await _svc.CreateAsync(m, UserName); return StatusCode((int)r.StatusCode, r); }
        [HttpPut] public async Task<IActionResult> Update([FromBody] UpdateSalaryComponentDto m) { var r = await _svc.UpdateAsync(m.Id, m, UserName); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id) { var r = await _svc.DeleteAsync(id, UserName); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class PayGroupController : BaseController
    {
        private readonly IPayGroupService _svc;
        public PayGroupController(IPayGroupService svc, ICurrentUserService cur) : base(cur) { _svc = svc; }

        [HttpGet] public async Task<IActionResult> GetAll() { var r = await _svc.GetAllAsync(); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpGet("{id}")] public async Task<IActionResult> GetById(int id) { var r = await _svc.GetByIdAsync(id); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpPost] public async Task<IActionResult> Create([FromBody] CreatePayGroupDto m) { var r = await _svc.CreateAsync(m, UserName); return StatusCode((int)r.StatusCode, r); }
        [HttpPut] public async Task<IActionResult> Update([FromBody] UpdatePayGroupDto m) { var r = await _svc.UpdateAsync(m.Id, m, UserName); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id) { var r = await _svc.DeleteAsync(id, UserName); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class SalaryStructureController : BaseController
    {
        private readonly ISalaryStructureService _svc;
        public SalaryStructureController(ISalaryStructureService svc, ICurrentUserService cur) : base(cur) { _svc = svc; }

        [HttpGet] public async Task<IActionResult> GetAll() { var r = await _svc.GetAllAsync(); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpGet("{id}")] public async Task<IActionResult> GetById(int id) { var r = await _svc.GetByIdAsync(id); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpPost] public async Task<IActionResult> Create([FromBody] CreateSalaryStructureDto m) { var r = await _svc.CreateAsync(m, UserName); return StatusCode((int)r.StatusCode, r); }
        [HttpPut] public async Task<IActionResult> Update([FromBody] UpdateSalaryStructureDto m) { var r = await _svc.UpdateAsync(m.Id, m, UserName); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id) { var r = await _svc.DeleteAsync(id, UserName); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeSalaryAllocationController : BaseController
    {
        private readonly IEmployeeSalaryAllocationService _svc;
        public EmployeeSalaryAllocationController(IEmployeeSalaryAllocationService svc, ICurrentUserService cur) : base(cur) { _svc = svc; }

        [HttpGet] public async Task<IActionResult> GetAll() { var r = await _svc.GetAllAsync(); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpGet("employee/{employeeId}")] public async Task<IActionResult> GetByEmployee(int employeeId) { var r = await _svc.GetByEmployeeIdAsync(employeeId); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpPost] public async Task<IActionResult> Create([FromBody] CreateEmployeeSalaryAllocationDto m) { var r = await _svc.CreateAsync(m, UserName); return StatusCode((int)r.StatusCode, r); }
        [HttpPut] public async Task<IActionResult> Update([FromBody] UpdateEmployeeSalaryAllocationDto m) { var r = await _svc.UpdateAsync(m.Id, m, UserName); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id) { var r = await _svc.DeleteAsync(id, UserName); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeLoanController : BaseController
    {
        private readonly IEmployeeLoanService _svc;
        public EmployeeLoanController(IEmployeeLoanService svc, ICurrentUserService cur) : base(cur) { _svc = svc; }

        [HttpGet] public async Task<IActionResult> GetAll() { var r = await _svc.GetAllAsync(); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpGet("employee/{employeeId}")] public async Task<IActionResult> GetByEmployee(int employeeId) { var r = await _svc.GetByEmployeeIdAsync(employeeId); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpPost] public async Task<IActionResult> Create([FromBody] CreateEmployeeLoanDto m) { var r = await _svc.CreateAsync(m, UserName); return StatusCode((int)r.StatusCode, r); }
        [HttpPut("status")] public async Task<IActionResult> UpdateStatus([FromBody] UpdateEmployeeLoanStatusDto m) { var r = await _svc.UpdateStatusAsync(m, UserName); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id) { var r = await _svc.DeleteAsync(id, UserName); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class SalaryAdvanceController : BaseController
    {
        private readonly ISalaryAdvanceService _svc;
        public SalaryAdvanceController(ISalaryAdvanceService svc, ICurrentUserService cur) : base(cur) { _svc = svc; }

        [HttpGet] public async Task<IActionResult> GetAll() { var r = await _svc.GetAllAsync(); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpGet("employee/{employeeId}")] public async Task<IActionResult> GetByEmployee(int employeeId) { var r = await _svc.GetByEmployeeIdAsync(employeeId); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpPost] public async Task<IActionResult> Create([FromBody] CreateSalaryAdvanceDto m) { var r = await _svc.CreateAsync(m, UserName); return StatusCode((int)r.StatusCode, r); }
        [HttpPut("status")] public async Task<IActionResult> UpdateStatus([FromBody] UpdateSalaryAdvanceStatusDto m) { var r = await _svc.UpdateStatusAsync(m, UserName); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id) { var r = await _svc.DeleteAsync(id, UserName); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeBonusController : BaseController
    {
        private readonly IEmployeeBonusService _svc;
        public EmployeeBonusController(IEmployeeBonusService svc, ICurrentUserService cur) : base(cur) { _svc = svc; }

        [HttpGet] public async Task<IActionResult> GetAll() { var r = await _svc.GetAllAsync(); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpGet("employee/{employeeId}")] public async Task<IActionResult> GetByEmployee(int employeeId) { var r = await _svc.GetByEmployeeIdAsync(employeeId); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpPost] public async Task<IActionResult> Create([FromBody] CreateEmployeeBonusDto m) { var r = await _svc.CreateAsync(m, UserName); return StatusCode((int)r.StatusCode, r); }
        [HttpPut] public async Task<IActionResult> Update([FromBody] UpdateEmployeeBonusDto m) { var r = await _svc.UpdateAsync(m.Id, m, UserName); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id) { var r = await _svc.DeleteAsync(id, UserName); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ReimbursementClaimController : BaseController
    {
        private readonly IReimbursementClaimService _svc;
        public ReimbursementClaimController(IReimbursementClaimService svc, ICurrentUserService cur) : base(cur) { _svc = svc; }

        [HttpGet] public async Task<IActionResult> GetAll() { var r = await _svc.GetAllAsync(); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpGet("employee/{employeeId}")] public async Task<IActionResult> GetByEmployee(int employeeId) { var r = await _svc.GetByEmployeeIdAsync(employeeId); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpPost] public async Task<IActionResult> Create([FromBody] CreateReimbursementClaimDto m) { var r = await _svc.CreateAsync(m, UserName); return StatusCode((int)r.StatusCode, r); }
        [HttpPut("approve")] public async Task<IActionResult> Approve([FromBody] ApproveClaimDto m) { var r = await _svc.ApproveClaimAsync(m, UserName); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id) { var r = await _svc.DeleteAsync(id, UserName); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class SalaryArrearController : BaseController
    {
        private readonly ISalaryArrearService _svc;
        public SalaryArrearController(ISalaryArrearService svc, ICurrentUserService cur) : base(cur) { _svc = svc; }

        [HttpGet] public async Task<IActionResult> GetAll() { var r = await _svc.GetAllAsync(); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpGet("employee/{employeeId}")] public async Task<IActionResult> GetByEmployee(int employeeId) { var r = await _svc.GetByEmployeeIdAsync(employeeId); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpPost] public async Task<IActionResult> Create([FromBody] CreateSalaryArrearDto m) { var r = await _svc.CreateAsync(m, UserName); return StatusCode((int)r.StatusCode, r); }
        [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id) { var r = await _svc.DeleteAsync(id, UserName); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class StatutoryComplianceController : BaseController
    {
        private readonly IStatutoryComplianceConfigService _svc;
        public StatutoryComplianceController(IStatutoryComplianceConfigService svc, ICurrentUserService cur) : base(cur) { _svc = svc; }

        [HttpGet] public async Task<IActionResult> GetConfig() { var r = await _svc.GetConfigAsync(); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpPost] public async Task<IActionResult> SaveConfig([FromBody] StatutoryComplianceConfigDto m) { var r = await _svc.SaveConfigAsync(m, UserName); return StatusCode((int)r.StatusCode, r); }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class PayrollController : BaseController
    {
        private readonly IPayrollRunService _svc;
        public PayrollController(IPayrollRunService svc, ICurrentUserService cur) : base(cur) { _svc = svc; }

        [HttpGet("query")] public async Task<IActionResult> QueryPayroll([FromQuery] int? payGroupId, [FromQuery] string? month, [FromQuery] string? status) { var r = await _svc.QueryPayrollAsync(payGroupId, month, status); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpGet("employee/{empId}")] public async Task<IActionResult> GetByEmployee(int empId) { var r = await _svc.GetAllByEmployeeIdAsync(empId); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpGet("{id}")] public async Task<IActionResult> GetById(int id) { var r = await _svc.GetByIdAsync(id); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpGet("{id}/details")] public async Task<IActionResult> GetDetails(int id) { var r = await _svc.GetDetailsByRunIdAsync(id); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpPost("generate")] public async Task<IActionResult> Generate([FromBody] GeneratePayrollRequestDto m) { var r = await _svc.GeneratePayrollAsync(m, UserName); return StatusCode((int)r.StatusCode, r); }
        [HttpPut("{id}/process")] public async Task<IActionResult> Process(int id) { var r = await _svc.ProcessPayrollAsync(id, UserName); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpPut("{id}/approve")] public async Task<IActionResult> Approve(int id) { var r = await _svc.ApprovePayrollAsync(id, UserName); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpPut("{id}/lock")] public async Task<IActionResult> Lock(int id) { var r = await _svc.LockPayrollAsync(id, UserName); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpPut("{id}/rollback")] public async Task<IActionResult> Rollback(int id) { var r = await _svc.RollbackPayrollAsync(id, UserName); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpPut("{id}/mark-paid")] public async Task<IActionResult> MarkPaid(int id, [FromQuery] string method, [FromQuery] string refNo) { var r = await _svc.MarkAsPaidAsync(id, method, refNo, UserName); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
        [HttpPost("bulk-pay")] public async Task<IActionResult> BulkPay([FromBody] BulkPaymentRequestDto m) { var r = await _svc.ProcessBulkPaymentAsync(m, UserName); return StatusCode((int)r.StatusCode, r); }
        [HttpGet("dashboard-stats")] public async Task<IActionResult> GetStats() { var r = await _svc.GetDashboardStatsAsync(); return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r); }
    }
}
