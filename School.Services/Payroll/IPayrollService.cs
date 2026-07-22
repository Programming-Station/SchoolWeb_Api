using School_DTOs;
using School_DTOs.Payroll;

namespace School.Services.Interfaces.Payroll
{
    public interface ISalaryComponentService
    {
        Task<APIResponse<List<SalaryComponentDto>>> GetAllAsync();
        Task<APIResponse<SalaryComponentDto>> GetByIdAsync(int id);
        Task<APIResponse<object>> CreateAsync(CreateSalaryComponentDto dto, string username);
        Task<APIResponse<object>> UpdateAsync(int id, UpdateSalaryComponentDto dto, string username);
        Task<APIResponse<object>> DeleteAsync(int id, string username);
    }

    public interface IPayrollRunService
    {
        Task<APIResponse<List<PayrollRunDto>>> GetAllByEmployeeIdAsync(int employeeId);
        Task<APIResponse<List<PayrollRunDto>>> QueryPayrollAsync(int? payGroupId, string? month, string? status);
        Task<APIResponse<PayrollRunDto>> GetByIdAsync(int id);
        Task<APIResponse<List<PayrollRunDetailDto>>> GetDetailsByRunIdAsync(int runId);
        Task<APIResponse<object>> GeneratePayrollAsync(GeneratePayrollRequestDto dto, string username);
        Task<APIResponse<object>> ProcessPayrollAsync(int id, string username);
        Task<APIResponse<object>> ApprovePayrollAsync(int id, string username);
        Task<APIResponse<object>> LockPayrollAsync(int id, string username);
        Task<APIResponse<object>> RollbackPayrollAsync(int id, string username);
        Task<APIResponse<object>> MarkAsPaidAsync(int id, string paymentMethod, string paymentRef, string username);
        Task<APIResponse<object>> ProcessBulkPaymentAsync(BulkPaymentRequestDto dto, string username);
        Task<APIResponse<object>> GetDashboardStatsAsync();
    }

    public interface IPayGroupService
    {
        Task<APIResponse<List<PayGroupDto>>> GetAllAsync();
        Task<APIResponse<PayGroupDto>> GetByIdAsync(int id);
        Task<APIResponse<object>> CreateAsync(CreatePayGroupDto dto, string username);
        Task<APIResponse<object>> UpdateAsync(int id, UpdatePayGroupDto dto, string username);
        Task<APIResponse<object>> DeleteAsync(int id, string username);
    }

    public interface ISalaryStructureService
    {
        Task<APIResponse<List<SalaryStructureDto>>> GetAllAsync();
        Task<APIResponse<SalaryStructureDto>> GetByIdAsync(int id);
        Task<APIResponse<object>> CreateAsync(CreateSalaryStructureDto dto, string username);
        Task<APIResponse<object>> UpdateAsync(int id, UpdateSalaryStructureDto dto, string username);
        Task<APIResponse<object>> DeleteAsync(int id, string username);
    }

    public interface IEmployeeSalaryAllocationService
    {
        Task<APIResponse<List<EmployeeSalaryAllocationDto>>> GetAllAsync();
        Task<APIResponse<EmployeeSalaryAllocationDto>> GetByEmployeeIdAsync(int employeeId);
        Task<APIResponse<object>> CreateAsync(CreateEmployeeSalaryAllocationDto dto, string username);
        Task<APIResponse<object>> UpdateAsync(int id, UpdateEmployeeSalaryAllocationDto dto, string username);
        Task<APIResponse<object>> DeleteAsync(int id, string username);
    }

    public interface IEmployeeLoanService
    {
        Task<APIResponse<List<EmployeeLoanDto>>> GetAllAsync();
        Task<APIResponse<List<EmployeeLoanDto>>> GetByEmployeeIdAsync(int employeeId);
        Task<APIResponse<object>> CreateAsync(CreateEmployeeLoanDto dto, string username);
        Task<APIResponse<object>> UpdateStatusAsync(UpdateEmployeeLoanStatusDto dto, string username);
        Task<APIResponse<object>> DeleteAsync(int id, string username);
    }

    public interface ISalaryAdvanceService
    {
        Task<APIResponse<List<SalaryAdvanceDto>>> GetAllAsync();
        Task<APIResponse<List<SalaryAdvanceDto>>> GetByEmployeeIdAsync(int employeeId);
        Task<APIResponse<object>> CreateAsync(CreateSalaryAdvanceDto dto, string username);
        Task<APIResponse<object>> UpdateStatusAsync(UpdateSalaryAdvanceStatusDto dto, string username);
        Task<APIResponse<object>> DeleteAsync(int id, string username);
    }

    public interface IEmployeeBonusService
    {
        Task<APIResponse<List<EmployeeBonusDto>>> GetAllAsync();
        Task<APIResponse<List<EmployeeBonusDto>>> GetByEmployeeIdAsync(int employeeId);
        Task<APIResponse<object>> CreateAsync(CreateEmployeeBonusDto dto, string username);
        Task<APIResponse<object>> UpdateAsync(int id, UpdateEmployeeBonusDto dto, string username);
        Task<APIResponse<object>> DeleteAsync(int id, string username);
    }

    public interface IReimbursementClaimService
    {
        Task<APIResponse<List<ReimbursementClaimDto>>> GetAllAsync();
        Task<APIResponse<List<ReimbursementClaimDto>>> GetByEmployeeIdAsync(int employeeId);
        Task<APIResponse<object>> CreateAsync(CreateReimbursementClaimDto dto, string username);
        Task<APIResponse<object>> ApproveClaimAsync(ApproveClaimDto dto, string username);
        Task<APIResponse<object>> DeleteAsync(int id, string username);
    }

    public interface ISalaryArrearService
    {
        Task<APIResponse<List<SalaryArrearDto>>> GetAllAsync();
        Task<APIResponse<List<SalaryArrearDto>>> GetByEmployeeIdAsync(int employeeId);
        Task<APIResponse<object>> CreateAsync(CreateSalaryArrearDto dto, string username);
        Task<APIResponse<object>> DeleteAsync(int id, string username);
    }

    public interface IStatutoryComplianceConfigService
    {
        Task<APIResponse<StatutoryComplianceConfigDto>> GetConfigAsync();
        Task<APIResponse<object>> SaveConfigAsync(StatutoryComplianceConfigDto dto, string username);
    }
}
