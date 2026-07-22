namespace School_DTOs.Payroll
{
    // 1. PayGroup DTOs
    public class PayGroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Frequency { get; set; } = "Monthly";
        public string Currency { get; set; } = "INR";
        public bool IsActive { get; set; } = true;
    }

    public class CreatePayGroupDto
    {
        public string Name { get; set; } = null!;
        public string Frequency { get; set; } = "Monthly";
        public string Currency { get; set; } = "INR";
        public bool IsActive { get; set; } = true;
    }

    public class UpdatePayGroupDto : CreatePayGroupDto
    {
        public int Id { get; set; }
    }

    // 2. SalaryStructure DTOs
    public class SalaryStructureDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int PayGroupId { get; set; }
        public string? PayGroupName { get; set; }
        public bool IsActive { get; set; } = true;
        public List<SalaryStructureItemDto> Items { get; set; } = new();
    }

    public class CreateSalaryStructureDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int PayGroupId { get; set; }
        public bool IsActive { get; set; } = true;
        public List<CreateSalaryStructureItemDto> Items { get; set; } = new();
    }

    public class UpdateSalaryStructureDto : CreateSalaryStructureDto
    {
        public int Id { get; set; }
    }

    // 3. SalaryStructureItem DTOs
    public class SalaryStructureItemDto
    {
        public int Id { get; set; }
        public int SalaryStructureId { get; set; }
        public int SalaryComponentId { get; set; }
        public string? SalaryComponentName { get; set; }
        public string? SalaryComponentType { get; set; } // Earning, Deduction
        public string CalculationType { get; set; } = "Fixed"; // Fixed, PercentageOfBasic, Formula
        public decimal Value { get; set; }
        public string? Formula { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class CreateSalaryStructureItemDto
    {
        public int SalaryComponentId { get; set; }
        public string CalculationType { get; set; } = "Fixed";
        public decimal Value { get; set; }
        public string? Formula { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class UpdateSalaryStructureItemDto : CreateSalaryStructureItemDto
    {
        public int Id { get; set; }
    }

    // 4. EmployeeSalaryAllocation DTOs
    public class EmployeeSalaryAllocationDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeCode { get; set; }
        public int SalaryStructureId { get; set; }
        public string? SalaryStructureName { get; set; }
        public DateTime EffectiveDate { get; set; }
        public decimal BaseSalary { get; set; }
        public string? Remarks { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateEmployeeSalaryAllocationDto
    {
        public int EmployeeId { get; set; }
        public int SalaryStructureId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public decimal BaseSalary { get; set; }
        public string? Remarks { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class UpdateEmployeeSalaryAllocationDto : CreateEmployeeSalaryAllocationDto
    {
        public int Id { get; set; }
    }

    // 5. EmployeeLoan DTOs
    public class EmployeeLoanDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeCode { get; set; }
        public decimal PrincipalAmount { get; set; }
        public decimal InterestRate { get; set; }
        public int TotalInstallments { get; set; }
        public decimal MonthlyInstallment { get; set; }
        public decimal BalanceAmount { get; set; }
        public string? Purpose { get; set; }
        public string Status { get; set; } = "Pending";
        public string? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public List<LoanRepaymentScheduleDto> Repayments { get; set; } = new();
    }

    public class CreateEmployeeLoanDto
    {
        public int EmployeeId { get; set; }
        public decimal PrincipalAmount { get; set; }
        public decimal InterestRate { get; set; }
        public int TotalInstallments { get; set; }
        public decimal MonthlyInstallment { get; set; }
        public string? Purpose { get; set; }
    }

    public class UpdateEmployeeLoanStatusDto
    {
        public int Id { get; set; }
        public string Status { get; set; } = "Approved"; // Approved, Rejected, Terminated
        public string? Remarks { get; set; }
    }

    // 6. LoanRepaymentSchedule DTOs
    public class LoanRepaymentScheduleDto
    {
        public int Id { get; set; }
        public int EmployeeLoanId { get; set; }
        public int InstallmentNo { get; set; }
        public DateTime DueDate { get; set; }
        public decimal PrincipalComponent { get; set; }
        public decimal InterestComponent { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime? PaidDate { get; set; }
        public string Status { get; set; } = "Pending";
    }

    // 7. SalaryAdvance DTOs
    public class SalaryAdvanceDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeCode { get; set; }
        public decimal AdvanceAmount { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; } = "Pending";
        public string? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string? TargetRecoveryMonth { get; set; }
    }

    public class CreateSalaryAdvanceDto
    {
        public int EmployeeId { get; set; }
        public decimal AdvanceAmount { get; set; }
        public string? TargetRecoveryMonth { get; set; }
    }

    public class UpdateSalaryAdvanceStatusDto
    {
        public int Id { get; set; }
        public string Status { get; set; } = "Approved";
    }

    // 8. EmployeeBonus DTOs
    public class EmployeeBonusDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeCode { get; set; }
        public string BonusType { get; set; } = null!;
        public decimal Amount { get; set; }
        public string PayoutMonth { get; set; } = null!;
        public string? Remarks { get; set; }
        public string Status { get; set; } = "Pending";
    }

    public class CreateEmployeeBonusDto
    {
        public int EmployeeId { get; set; }
        public string BonusType { get; set; } = null!;
        public decimal Amount { get; set; }
        public string PayoutMonth { get; set; } = null!;
        public string? Remarks { get; set; }
    }

    public class UpdateEmployeeBonusDto : CreateEmployeeBonusDto
    {
        public int Id { get; set; }
    }

    // 9. ReimbursementClaim DTOs
    public class ReimbursementClaimDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeCode { get; set; }
        public string ClaimType { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime ClaimDate { get; set; }
        public string? Description { get; set; }
        public string? AttachmentPath { get; set; }
        public string Status { get; set; } = "Pending";
        public string? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string? SettlementRef { get; set; }
    }

    public class CreateReimbursementClaimDto
    {
        public int EmployeeId { get; set; }
        public string ClaimType { get; set; } = null!;
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public string? AttachmentPath { get; set; }
    }

    public class ApproveClaimDto
    {
        public int Id { get; set; }
        public string Status { get; set; } = "Approved"; // Approved, Rejected, Settled
        public string? SettlementRef { get; set; }
    }

    // 10. SalaryArrear DTOs
    public class SalaryArrearDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeCode { get; set; }
        public decimal Amount { get; set; }
        public string ArrearMonth { get; set; } = null!;
        public string? PaidInMonth { get; set; }
        public string? Reason { get; set; }
        public string Status { get; set; } = "Pending";
    }

    public class CreateSalaryArrearDto
    {
        public int EmployeeId { get; set; }
        public decimal Amount { get; set; }
        public string ArrearMonth { get; set; } = null!;
        public string? Reason { get; set; }
    }

    // 11. StatutoryComplianceConfig DTOs
    public class StatutoryComplianceConfigDto
    {
        public int Id { get; set; }
        public decimal PfEmployerRate { get; set; }
        public decimal PfEmployeeRate { get; set; }
        public decimal PfMaxBasicLimit { get; set; }
        public decimal EsiEmployerRate { get; set; }
        public decimal EsiEmployeeRate { get; set; }
        public decimal EsiMaxGrossLimit { get; set; }
        public string? ProfessionalTaxSlabJson { get; set; }
        public bool EnableGratuity { get; set; }
    }

    // 12. PayrollRunDetail DTO
    public class PayrollRunDetailDto
    {
        public int Id { get; set; }
        public int PayrollRunId { get; set; }
        public int SalaryComponentId { get; set; }
        public string? SalaryComponentName { get; set; }
        public string? SalaryComponentType { get; set; }
        public decimal Amount { get; set; }
    }

    // 13. Payroll Generation payload
    public class GeneratePayrollRequestDto
    {
        public int PayGroupId { get; set; }
        public string Month { get; set; } = null!; // e.g. "2026-02"
    }

    // 14. Bulk Payment payload
    public class BulkPaymentRequestDto
    {
        public List<int> PayrollRunIds { get; set; } = new();
        public string PaymentMethod { get; set; } = "BankTransfer";
        public string? PaymentRef { get; set; }
    }
}
