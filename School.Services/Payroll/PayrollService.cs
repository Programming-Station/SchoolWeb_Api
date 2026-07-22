using System.Net;
using Microsoft.EntityFrameworkCore;
using School.Domain.Payroll;
using School.Infrastructure;
using School.Infrastructure.UnitOfWork.Interfaces;
using School.Services.Interfaces;
using School.Services.Interfaces.Payroll;
using School_DTOs;
using School_DTOs.Payroll;

namespace School.Services.Payroll
{
    // 1. SalaryComponentService
    public class SalaryComponentService : ISalaryComponentService
    {
        private readonly IRepository<SalaryComponent> _repo;
        private readonly IUnitOfWork _uow;

        public SalaryComponentService(IRepository<SalaryComponent> repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<APIResponse<List<SalaryComponentDto>>> GetAllAsync()
        {
            var d = await _repo.List()
                .Select(x => new SalaryComponentDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Type = x.Type,
                    Amount = x.Amount,
                    Status = x.Status
                }).ToListAsync();

            return new APIResponse<List<SalaryComponentDto>> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = d };
        }

        public async Task<APIResponse<SalaryComponentDto>> GetByIdAsync(int id)
        {
            var x = await _repo.List()
                .Where(s => s.Id == id)
                .Select(x => new SalaryComponentDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Type = x.Type,
                    Amount = x.Amount,
                    Status = x.Status
                }).FirstOrDefaultAsync();

            if (x == null) return new APIResponse<SalaryComponentDto> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            return new APIResponse<SalaryComponentDto> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = x };
        }

        public async Task<APIResponse<object>> CreateAsync(CreateSalaryComponentDto dto, string username)
        {
            await _repo.AddAsync(new SalaryComponent
            {
                Name = dto.Name,
                Type = dto.Type,
                Amount = dto.Amount,
                Status = dto.Status,
                CreatedBy = username,
                CreatedDate = DateTime.UtcNow
            });
            await _uow.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Created successfully" };
        }

        public async Task<APIResponse<object>> UpdateAsync(int id, UpdateSalaryComponentDto dto, string username)
        {
            var e = await _repo.List().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (e == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            e.Name = dto.Name;
            e.Type = dto.Type;
            e.Amount = dto.Amount;
            e.Status = dto.Status;
            e.UpdatedBy = username;
            e.UpdatedDate = DateTime.UtcNow;
            _repo.Update(e);
            await _uow.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Updated successfully" };
        }

        public async Task<APIResponse<object>> DeleteAsync(int id, string username)
        {
            var e = await _repo.List().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (e == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            _repo.Delete(e);
            await _uow.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Deleted successfully" };
        }
    }

    // 2. PayrollRunService
    public class PayrollRunService : IPayrollRunService
    {
        private readonly IRepository<PayrollRun> _repo;
        private readonly IUnitOfWork _uow;
        private readonly IEmailService _emailService;
        private readonly SchoolDbContext _dbContext;

        public PayrollRunService(
            IRepository<PayrollRun> repo,
            IUnitOfWork uow,
            IEmailService emailService,
            SchoolDbContext dbContext)
        {
            _repo = repo;
            _uow = uow;
            _emailService = emailService;
            _dbContext = dbContext;
        }

        public async Task<APIResponse<List<PayrollRunDto>>> GetAllByEmployeeIdAsync(int employeeId)
        {
            var d = await _repo.List().Where(x => x.EmployeeId == employeeId)
                .Select(x => new PayrollRunDto
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    Month = x.Month,
                    GrossSalary = x.GrossSalary,
                    TotalDeductions = x.TotalDeductions,
                    NetSalary = x.NetSalary,
                    Status = x.Status
                }).ToListAsync();

            return new APIResponse<List<PayrollRunDto>> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = d };
        }

        public async Task<APIResponse<List<PayrollRunDto>>> QueryPayrollAsync(int? payGroupId, string? month, string? status)
        {
            var query = _repo.List().Include(x => x.Employee).AsQueryable();
            if (payGroupId.HasValue) query = query.Where(x => x.PayGroupId == payGroupId.Value);
            if (!string.IsNullOrEmpty(month)) query = query.Where(x => x.Month == month);
            if (!string.IsNullOrEmpty(status)) query = query.Where(x => x.Status == status);

            var d = await query.Select(x => new PayrollRunDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                EmployeeName = x.Employee.FirstName + " " + x.Employee.LastName,
                EmployeeCode = x.Employee.EmployeeCode,
                Month = x.Month,
                GrossSalary = x.GrossSalary,
                TotalDeductions = x.TotalDeductions,
                NetSalary = x.NetSalary,
                Status = x.Status
            }).ToListAsync();

            return new APIResponse<List<PayrollRunDto>> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = d };
        }

        public async Task<APIResponse<PayrollRunDto>> GetByIdAsync(int id)
        {
            var x = await _repo.List().Include(x => x.Employee).Where(r => r.Id == id)
                .Select(x => new PayrollRunDto
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    EmployeeName = x.Employee.FirstName + " " + x.Employee.LastName,
                    EmployeeCode = x.Employee.EmployeeCode,
                    Month = x.Month,
                    GrossSalary = x.GrossSalary,
                    TotalDeductions = x.TotalDeductions,
                    NetSalary = x.NetSalary,
                    Status = x.Status,
                    PaymentMethod = x.PaymentMethod,
                    PaymentRef = x.PaymentRef,
                    PaidDate = x.PaidDate,
                    LockedDate = x.LockedDate,
                    LockedBy = x.LockedBy,
                    ApprovedBy = x.ApprovedBy,
                    ApprovedDate = x.ApprovedDate
                }).FirstOrDefaultAsync();

            if (x == null) return new APIResponse<PayrollRunDto> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            return new APIResponse<PayrollRunDto> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = x };
        }

        public async Task<APIResponse<List<PayrollRunDetailDto>>> GetDetailsByRunIdAsync(int runId)
        {
            var list = await _dbContext.PayrollRunDetails.Include(x => x.SalaryComponent)
                .Where(x => x.PayrollRunId == runId)
                .Select(x => new PayrollRunDetailDto
                {
                    Id = x.Id,
                    PayrollRunId = x.PayrollRunId,
                    SalaryComponentId = x.SalaryComponentId,
                    SalaryComponentName = x.SalaryComponent.Name,
                    SalaryComponentType = x.SalaryComponent.Type,
                    Amount = x.Amount
                }).ToListAsync();

            return new APIResponse<List<PayrollRunDetailDto>> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = list };
        }

        public async Task<APIResponse<object>> GeneratePayrollAsync(GeneratePayrollRequestDto dto, string username)
        {
            var schoolId = _dbContext.CurrentTenantId ?? 1;

            // 1. Fetch allocated employees under this Pay Group
            var allocations = await _dbContext.EmployeeSalaryAllocations
                .Include(x => x.Employee)
                .Include(x => x.SalaryStructure)
                .ThenInclude(x => x.Items)
                .ThenInclude(x => x.SalaryComponent)
                .Where(x => x.SalaryStructure.PayGroupId == dto.PayGroupId && x.IsActive)
                .ToListAsync();

            if (!allocations.Any())
            {
                return new APIResponse<object> { StatusCode = HttpStatusCode.BadRequest, Message = "No employees allocated to this Pay Group." };
            }

            // Parse month date e.g. "2026-02"
            if (!DateTime.TryParse(dto.Month + "-01", out DateTime monthDate))
            {
                return new APIResponse<object> { StatusCode = HttpStatusCode.BadRequest, Message = "Invalid Month format. Use YYYY-MM." };
            }

            int daysInMonth = DateTime.DaysInMonth(monthDate.Year, monthDate.Month);

            // 2. Fetch compliance configs
            var compliance = await _dbContext.StatutoryComplianceConfigs.FirstOrDefaultAsync()
                             ?? new StatutoryComplianceConfig();

            using var transaction = await _uow.BeginTransactionAsync();
            try
            {
                foreach (var alloc in allocations)
                {
                    // Check if already processed and locked
                    var existing = await _dbContext.PayrollRuns
                        .Where(x => x.EmployeeId == alloc.EmployeeId && x.Month == dto.Month)
                        .FirstOrDefaultAsync();

                    if (existing != null)
                    {
                        if (existing.Status == "Locked" || existing.Status == "Paid")
                        {
                            continue; // Skip already locked or paid payslips
                        }
                        // Remove draft/processed details for reprocessing
                        var oldDetails = await _dbContext.PayrollRunDetails.Where(x => x.PayrollRunId == existing.Id).ToListAsync();
                        _dbContext.PayrollRunDetails.RemoveRange(oldDetails);
                        _dbContext.PayrollRuns.Remove(existing);
                    }

                    // 3. Attendance integration
                    var attendance = await _dbContext.Attendances
                        .Where(x => x.EmployeeId == alloc.EmployeeId && x.AttendanceDate.Month == monthDate.Month && x.AttendanceDate.Year == monthDate.Year)
                        .ToListAsync();

                    decimal lopDays = 0;
                    if (attendance.Any())
                    {
                        decimal absentCount = attendance.Count(a => a.Status.Equals("Absent", StringComparison.OrdinalIgnoreCase));
                        decimal halfDayCount = attendance.Count(a => a.Status.Equals("HalfDay", StringComparison.OrdinalIgnoreCase));
                        lopDays = absentCount + (halfDayCount * 0.5m);
                    }

                    decimal paidDays = Math.Max(0, daysInMonth - lopDays);

                    // 4. Calculate Earnings & Deductions
                    decimal basicSalary = alloc.BaseSalary;
                    decimal calculatedBasic = Math.Round(basicSalary * (paidDays / daysInMonth), 2);

                    decimal grossSalary = 0;
                    decimal totalDeductions = 0;

                    var detailsList = new List<PayrollRunDetail>();

                    // Calculate Earnings first to build Gross Salary
                    foreach (var item in alloc.SalaryStructure.Items.Where(x => x.SalaryComponent.Type == "Earning").OrderBy(x => x.DisplayOrder))
                    {
                        decimal amount = 0;
                        if (item.SalaryComponent.Name.Equals("Basic", StringComparison.OrdinalIgnoreCase))
                        {
                            amount = calculatedBasic;
                        }
                        else if (item.CalculationType == "Fixed")
                        {
                            amount = item.Value;
                        }
                        else if (item.CalculationType == "PercentageOfBasic")
                        {
                            amount = Math.Round(calculatedBasic * (item.Value / 100m), 2);
                        }

                        grossSalary += amount;

                        detailsList.Add(new PayrollRunDetail
                        {
                            SalaryComponentId = item.SalaryComponentId,
                            Amount = amount,
                            SchoolRegistrationId = schoolId
                        });
                    }

                    // Add dynamic Bonuses
                    var bonuses = await _dbContext.EmployeeBonuses
                        .Where(x => x.EmployeeId == alloc.EmployeeId && x.PayoutMonth == dto.Month && x.Status == "Approved")
                        .ToListAsync();
                    foreach (var bonus in bonuses)
                    {
                        grossSalary += bonus.Amount;
                        bonus.Status = "Processed";
                    }

                    // Add Arrears
                    var arrears = await _dbContext.SalaryArrears
                        .Where(x => x.EmployeeId == alloc.EmployeeId && x.ArrearMonth == dto.Month && x.Status == "Pending")
                        .ToListAsync();
                    foreach (var arrear in arrears)
                    {
                        grossSalary += arrear.Amount;
                        arrear.Status = "Paid";
                        arrear.PaidInMonth = dto.Month;
                    }

                    // Add Claims / Reimbursements
                    var claims = await _dbContext.ReimbursementClaims
                        .Where(x => x.EmployeeId == alloc.EmployeeId && x.Status == "Approved")
                        .ToListAsync();
                    foreach (var claim in claims)
                    {
                        grossSalary += claim.Amount;
                        claim.Status = "Settled";
                    }

                    // Compute Deductions
                    foreach (var item in alloc.SalaryStructure.Items.Where(x => x.SalaryComponent.Type == "Deduction").OrderBy(x => x.DisplayOrder))
                    {
                        decimal amount = 0;
                        if (item.SalaryComponent.Name.Contains("PF", StringComparison.OrdinalIgnoreCase))
                        {
                            decimal pfBase = Math.Min(calculatedBasic, compliance.PfMaxBasicLimit);
                            amount = Math.Round(pfBase * (compliance.PfEmployeeRate / 100m), 2);
                        }
                        else if (item.SalaryComponent.Name.Contains("ESI", StringComparison.OrdinalIgnoreCase))
                        {
                            if (grossSalary <= compliance.EsiMaxGrossLimit)
                            {
                                amount = Math.Round(grossSalary * (compliance.EsiEmployeeRate / 100m), 2);
                            }
                        }
                        else if (item.CalculationType == "Fixed")
                        {
                            amount = item.Value;
                        }
                        else if (item.CalculationType == "PercentageOfBasic")
                        {
                            amount = Math.Round(calculatedBasic * (item.Value / 100m), 2);
                        }

                        totalDeductions += amount;

                        detailsList.Add(new PayrollRunDetail
                        {
                            SalaryComponentId = item.SalaryComponentId,
                            Amount = amount,
                            SchoolRegistrationId = schoolId
                        });
                    }

                    // Recover Loans
                    var loanInstallments = await _dbContext.LoanRepaymentSchedules
                        .Include(x => x.EmployeeLoan)
                        .Where(x => x.EmployeeLoan.EmployeeId == alloc.EmployeeId && x.Status == "Pending" && x.DueDate <= monthDate)
                        .ToListAsync();

                    foreach (var inst in loanInstallments)
                    {
                        totalDeductions += inst.TotalAmount;
                        inst.Status = "Paid";
                        inst.PaidAmount = inst.TotalAmount;
                        inst.PaidDate = DateTime.UtcNow;
                        inst.EmployeeLoan.BalanceAmount = Math.Max(0, inst.EmployeeLoan.BalanceAmount - inst.TotalAmount);
                        if (inst.EmployeeLoan.BalanceAmount <= 0)
                        {
                            inst.EmployeeLoan.Status = "Completed";
                        }
                    }

                    // Recover Advances
                    var advances = await _dbContext.SalaryAdvances
                        .Where(x => x.EmployeeId == alloc.EmployeeId && x.Status == "Approved" && x.TargetRecoveryMonth == dto.Month)
                        .ToListAsync();

                    foreach (var adv in advances)
                    {
                        totalDeductions += adv.AdvanceAmount;
                        adv.Status = "Recovered";
                    }

                    decimal netSalary = Math.Max(0, grossSalary - totalDeductions);

                    // Save Payroll Run (Payslip Master)
                    var run = new PayrollRun
                    {
                        EmployeeId = alloc.EmployeeId,
                        PayGroupId = dto.PayGroupId,
                        Month = dto.Month,
                        GrossSalary = grossSalary,
                        TotalDeductions = totalDeductions,
                        NetSalary = netSalary,
                        Status = "Draft",
                        SchoolRegistrationId = schoolId,
                        CreatedBy = username,
                        CreatedDate = DateTime.UtcNow
                    };

                    await _dbContext.PayrollRuns.AddAsync(run);
                    await _dbContext.SaveChangesAsync(); // Fetch Id

                    // Save component details
                    foreach (var details in detailsList)
                    {
                        details.PayrollRunId = run.Id;
                        await _dbContext.PayrollRunDetails.AddAsync(details);
                    }
                }

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }

            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Payroll runs generated successfully in Draft mode." };
        }

        public async Task<APIResponse<object>> ProcessPayrollAsync(int id, string username)
        {
            var e = await _repo.List().Include(x => x.Employee).Where(x => x.Id == id).FirstOrDefaultAsync();
            if (e == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            if (e.Status != "Draft") return new APIResponse<object> { StatusCode = HttpStatusCode.BadRequest, Message = "Only draft payrolls can be processed" };

            e.Status = "Processed";
            e.UpdatedBy = username;
            e.UpdatedDate = DateTime.UtcNow;
            _repo.Update(e);
            await _uow.CommitAsync();

            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Payroll processed successfully" };
        }

        public async Task<APIResponse<object>> ApprovePayrollAsync(int id, string username)
        {
            var e = await _repo.List().Include(x => x.Employee).Where(x => x.Id == id).FirstOrDefaultAsync();
            if (e == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            if (e.Status != "Processed" && e.Status != "Draft") return new APIResponse<object> { StatusCode = HttpStatusCode.BadRequest, Message = "Payroll must be processed before approval" };

            e.Status = "Approved";
            e.ApprovedBy = username;
            e.ApprovedDate = DateTime.UtcNow;
            e.UpdatedBy = username;
            e.UpdatedDate = DateTime.UtcNow;
            _repo.Update(e);
            await _uow.CommitAsync();

            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Payroll approved successfully" };
        }

        public async Task<APIResponse<object>> LockPayrollAsync(int id, string username)
        {
            var e = await _repo.List().Include(x => x.Employee).Where(x => x.Id == id).FirstOrDefaultAsync();
            if (e == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            if (e.Status != "Approved") return new APIResponse<object> { StatusCode = HttpStatusCode.BadRequest, Message = "Only approved payrolls can be locked" };

            e.Status = "Locked";
            e.LockedBy = username;
            e.LockedDate = DateTime.UtcNow;
            e.UpdatedBy = username;
            e.UpdatedDate = DateTime.UtcNow;
            _repo.Update(e);
            await _uow.CommitAsync();

            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Payroll locked successfully" };
        }

        public async Task<APIResponse<object>> RollbackPayrollAsync(int id, string username)
        {
            var e = await _repo.List().Include(x => x.Employee).Where(x => x.Id == id).FirstOrDefaultAsync();
            if (e == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            if (e.Status == "Paid") return new APIResponse<object> { StatusCode = HttpStatusCode.BadRequest, Message = "Paid payrolls cannot be rolled back" };

            e.Status = "Draft";
            e.UpdatedBy = username;
            e.UpdatedDate = DateTime.UtcNow;
            _repo.Update(e);
            await _uow.CommitAsync();

            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Payroll rolled back to draft successfully" };
        }

        public async Task<APIResponse<object>> MarkAsPaidAsync(int id, string paymentMethod, string paymentRef, string username)
        {
            var e = await _repo.List().Include(x => x.Employee).Where(x => x.Id == id).FirstOrDefaultAsync();
            if (e == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            if (e.Status != "Locked" && e.Status != "Approved") return new APIResponse<object> { StatusCode = HttpStatusCode.BadRequest, Message = "Only approved or locked payrolls can be marked as paid" };

            e.Status = "Paid";
            e.PaymentMethod = paymentMethod;
            e.PaymentRef = paymentRef;
            e.PaidDate = DateTime.UtcNow;
            e.UpdatedBy = username;
            e.UpdatedDate = DateTime.UtcNow;
            _repo.Update(e);
            await _uow.CommitAsync();

            // Send Salary Paid email to employee
            if (e.Employee != null && !string.IsNullOrWhiteSpace(e.Employee.Email))
            {
                _ = _emailService.SendGenericTemplateAsync(e.Employee.Email, "Salary Paid", new Dictionary<string, string>
                {
                    { "SchoolName", "School" },
                    { "FirstName", e.Employee.FirstName },
                    { "LastName", e.Employee.LastName },
                    { "EmployeeCode", e.Employee.EmployeeCode },
                    { "Month", e.Month },
                    { "GrossSalary", e.GrossSalary.ToString("N2") },
                    { "TotalDeductions", e.TotalDeductions.ToString("N2") },
                    { "NetSalary", e.NetSalary.ToString("N2") },
                    { "LoginUrl", "#" }
                });
            }

            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Payroll marked as paid" };
        }

        public async Task<APIResponse<object>> ProcessBulkPaymentAsync(BulkPaymentRequestDto dto, string username)
        {
            if (dto.PayrollRunIds == null || !dto.PayrollRunIds.Any())
            {
                return new APIResponse<object> { StatusCode = HttpStatusCode.BadRequest, Message = "No payroll records selected." };
            }

            var records = await _repo.List().Include(x => x.Employee)
                .Where(x => dto.PayrollRunIds.Contains(x.Id))
                .ToListAsync();

            using var transaction = await _uow.BeginTransactionAsync();
            try
            {
                foreach (var e in records)
                {
                    if (e.Status == "Locked" || e.Status == "Approved")
                    {
                        e.Status = "Paid";
                        e.PaymentMethod = dto.PaymentMethod;
                        e.PaymentRef = dto.PaymentRef;
                        e.PaidDate = DateTime.UtcNow;
                        e.UpdatedBy = username;
                        e.UpdatedDate = DateTime.UtcNow;
                        _repo.Update(e);

                        // Send email
                        if (e.Employee != null && !string.IsNullOrWhiteSpace(e.Employee.Email))
                        {
                            _ = _emailService.SendGenericTemplateAsync(e.Employee.Email, "Salary Paid", new Dictionary<string, string>
                            {
                                { "SchoolName", "School" },
                                { "FirstName", e.Employee.FirstName },
                                { "LastName", e.Employee.LastName },
                                { "EmployeeCode", e.Employee.EmployeeCode },
                                { "Month", e.Month },
                                { "GrossSalary", e.GrossSalary.ToString("N2") },
                                { "TotalDeductions", e.TotalDeductions.ToString("N2") },
                                { "NetSalary", e.NetSalary.ToString("N2") },
                                { "LoginUrl", "#" }
                            });
                        }
                    }
                }
                await _uow.CommitAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }

            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Bulk payment processed successfully." };
        }

        public async Task<APIResponse<object>> GetDashboardStatsAsync()
        {
            var tenantId = _dbContext.CurrentTenantId ?? 1;

            var runs = await _repo.List().Where(x => x.SchoolRegistrationId == tenantId).ToListAsync();
            var totalEmployees = await _dbContext.Employees.CountAsync(x => x.SchoolRegistrationId == tenantId && x.Status == "active");
            var activeLoans = await _dbContext.EmployeeLoans.CountAsync(x => x.SchoolRegistrationId == tenantId && x.Status == "Running");

            var gross = runs.Sum(x => x.GrossSalary);
            var deductions = runs.Sum(x => x.TotalDeductions);
            var net = runs.Sum(x => x.NetSalary);

            var pending = runs.Count(x => x.Status == "Draft" || x.Status == "Processed");
            var paid = runs.Count(x => x.Status == "Paid");

            var stats = new
            {
                TotalEmployees = totalEmployees,
                ActiveLoans = activeLoans,
                TotalGross = gross,
                TotalDeductions = deductions,
                TotalNet = net,
                PendingRuns = pending,
                PaidRuns = paid
            };

            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = stats };
        }
    }

    // 3. PayGroupService
    public class PayGroupService : IPayGroupService
    {
        private readonly IRepository<PayGroup> _repo;
        private readonly IUnitOfWork _uow;

        public PayGroupService(IRepository<PayGroup> repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<APIResponse<List<PayGroupDto>>> GetAllAsync()
        {
            var d = await _repo.List()
                .Select(x => new PayGroupDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Frequency = x.Frequency,
                    Currency = x.Currency,
                    IsActive = x.IsActive
                }).ToListAsync();

            return new APIResponse<List<PayGroupDto>> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = d };
        }

        public async Task<APIResponse<PayGroupDto>> GetByIdAsync(int id)
        {
            var x = await _repo.List().Where(p => p.Id == id)
                .Select(x => new PayGroupDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Frequency = x.Frequency,
                    Currency = x.Currency,
                    IsActive = x.IsActive
                }).FirstOrDefaultAsync();

            if (x == null) return new APIResponse<PayGroupDto> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            return new APIResponse<PayGroupDto> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = x };
        }

        public async Task<APIResponse<object>> CreateAsync(CreatePayGroupDto dto, string username)
        {
            await _repo.AddAsync(new PayGroup
            {
                Name = dto.Name,
                Frequency = dto.Frequency,
                Currency = dto.Currency,
                IsActive = dto.IsActive,
                CreatedBy = username,
                CreatedDate = DateTime.UtcNow
            });
            await _uow.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Created successfully" };
        }

        public async Task<APIResponse<object>> UpdateAsync(int id, UpdatePayGroupDto dto, string username)
        {
            var e = await _repo.List().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (e == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            e.Name = dto.Name;
            e.Frequency = dto.Frequency;
            e.Currency = dto.Currency;
            e.IsActive = dto.IsActive;
            e.UpdatedBy = username;
            e.UpdatedDate = DateTime.UtcNow;
            _repo.Update(e);
            await _uow.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Updated successfully" };
        }

        public async Task<APIResponse<object>> DeleteAsync(int id, string username)
        {
            var e = await _repo.List().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (e == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            _repo.Delete(e);
            await _uow.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Deleted successfully" };
        }
    }

    // 4. SalaryStructureService
    public class SalaryStructureService : ISalaryStructureService
    {
        private readonly IRepository<SalaryStructure> _repo;
        private readonly IUnitOfWork _uow;
        private readonly SchoolDbContext _dbContext;

        public SalaryStructureService(IRepository<SalaryStructure> repo, IUnitOfWork uow, SchoolDbContext dbContext)
        {
            _repo = repo;
            _uow = uow;
            _dbContext = dbContext;
        }

        public async Task<APIResponse<List<SalaryStructureDto>>> GetAllAsync()
        {
            var d = await _repo.List().Include(x => x.PayGroup)
                .Select(x => new SalaryStructureDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    PayGroupId = x.PayGroupId,
                    PayGroupName = x.PayGroup.Name,
                    IsActive = x.IsActive
                }).ToListAsync();

            return new APIResponse<List<SalaryStructureDto>> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = d };
        }

        public async Task<APIResponse<SalaryStructureDto>> GetByIdAsync(int id)
        {
            var x = await _repo.List().Include(x => x.PayGroup).Where(s => s.Id == id).FirstOrDefaultAsync();
            if (x == null) return new APIResponse<SalaryStructureDto> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };

            var items = await _dbContext.SalaryStructureItems.Include(i => i.SalaryComponent)
                .Where(i => i.SalaryStructureId == id)
                .Select(i => new SalaryStructureItemDto
                {
                    Id = i.Id,
                    SalaryStructureId = i.SalaryStructureId,
                    SalaryComponentId = i.SalaryComponentId,
                    SalaryComponentName = i.SalaryComponent.Name,
                    SalaryComponentType = i.SalaryComponent.Type,
                    CalculationType = i.CalculationType,
                    Value = i.Value,
                    Formula = i.Formula,
                    DisplayOrder = i.DisplayOrder
                }).ToListAsync();

            var dto = new SalaryStructureDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                PayGroupId = x.PayGroupId,
                PayGroupName = x.PayGroup.Name,
                IsActive = x.IsActive,
                Items = items
            };

            return new APIResponse<SalaryStructureDto> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = dto };
        }

        public async Task<APIResponse<object>> CreateAsync(CreateSalaryStructureDto dto, string username)
        {
            var schoolId = _dbContext.CurrentTenantId ?? 1;

            using var transaction = await _uow.BeginTransactionAsync();
            try
            {
                var structEntity = new SalaryStructure
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    PayGroupId = dto.PayGroupId,
                    IsActive = dto.IsActive,
                    CreatedBy = username,
                    CreatedDate = DateTime.UtcNow,
                    SchoolRegistrationId = schoolId
                };

                await _dbContext.SalaryStructures.AddAsync(structEntity);
                await _dbContext.SaveChangesAsync(); // Auto generate Id

                foreach (var item in dto.Items)
                {
                    var structItem = new SalaryStructureItem
                    {
                        SalaryStructureId = structEntity.Id,
                        SalaryComponentId = item.SalaryComponentId,
                        CalculationType = item.CalculationType,
                        Value = item.Value,
                        Formula = item.Formula,
                        DisplayOrder = item.DisplayOrder,
                        CreatedBy = username,
                        CreatedDate = DateTime.UtcNow,
                        SchoolRegistrationId = schoolId
                    };
                    await _dbContext.SalaryStructureItems.AddAsync(structItem);
                }

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }

            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Created successfully" };
        }

        public async Task<APIResponse<object>> UpdateAsync(int id, UpdateSalaryStructureDto dto, string username)
        {
            var schoolId = _dbContext.CurrentTenantId ?? 1;
            var e = await _repo.List().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (e == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };

            using var transaction = await _uow.BeginTransactionAsync();
            try
            {
                e.Name = dto.Name;
                e.Description = dto.Description;
                e.PayGroupId = dto.PayGroupId;
                e.IsActive = dto.IsActive;
                e.UpdatedBy = username;
                e.UpdatedDate = DateTime.UtcNow;
                _repo.Update(e);

                // Remove existing items and rebuild
                var existingItems = await _dbContext.SalaryStructureItems.Where(x => x.SalaryStructureId == id).ToListAsync();
                _dbContext.SalaryStructureItems.RemoveRange(existingItems);

                foreach (var item in dto.Items)
                {
                    var structItem = new SalaryStructureItem
                    {
                        SalaryStructureId = id,
                        SalaryComponentId = item.SalaryComponentId,
                        CalculationType = item.CalculationType,
                        Value = item.Value,
                        Formula = item.Formula,
                        DisplayOrder = item.DisplayOrder,
                        CreatedBy = username,
                        CreatedDate = DateTime.UtcNow,
                        SchoolRegistrationId = schoolId
                    };
                    await _dbContext.SalaryStructureItems.AddAsync(structItem);
                }

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }

            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Updated successfully" };
        }

        public async Task<APIResponse<object>> DeleteAsync(int id, string username)
        {
            var e = await _repo.List().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (e == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };

            using var transaction = await _uow.BeginTransactionAsync();
            try
            {
                var existingItems = await _dbContext.SalaryStructureItems.Where(x => x.SalaryStructureId == id).ToListAsync();
                _dbContext.SalaryStructureItems.RemoveRange(existingItems);

                _repo.Delete(e);

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }

            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Deleted successfully" };
        }
    }

    // 5. EmployeeSalaryAllocationService
    public class EmployeeSalaryAllocationService : IEmployeeSalaryAllocationService
    {
        private readonly IRepository<EmployeeSalaryAllocation> _repo;
        private readonly IUnitOfWork _uow;

        public EmployeeSalaryAllocationService(IRepository<EmployeeSalaryAllocation> repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<APIResponse<List<EmployeeSalaryAllocationDto>>> GetAllAsync()
        {
            var d = await _repo.List()
                .Include(x => x.Employee)
                .Include(x => x.SalaryStructure)
                .Select(x => new EmployeeSalaryAllocationDto
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    EmployeeName = x.Employee.FirstName + " " + x.Employee.LastName,
                    EmployeeCode = x.Employee.EmployeeCode,
                    SalaryStructureId = x.SalaryStructureId,
                    SalaryStructureName = x.SalaryStructure.Name,
                    EffectiveDate = x.EffectiveDate,
                    BaseSalary = x.BaseSalary,
                    Remarks = x.Remarks,
                    IsActive = x.IsActive
                }).ToListAsync();

            return new APIResponse<List<EmployeeSalaryAllocationDto>> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = d };
        }

        public async Task<APIResponse<EmployeeSalaryAllocationDto>> GetByEmployeeIdAsync(int employeeId)
        {
            var x = await _repo.List()
                .Include(x => x.Employee)
                .Include(x => x.SalaryStructure)
                .Where(a => a.EmployeeId == employeeId && a.IsActive)
                .OrderByDescending(a => a.EffectiveDate)
                .Select(x => new EmployeeSalaryAllocationDto
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    EmployeeName = x.Employee.FirstName + " " + x.Employee.LastName,
                    EmployeeCode = x.Employee.EmployeeCode,
                    SalaryStructureId = x.SalaryStructureId,
                    SalaryStructureName = x.SalaryStructure.Name,
                    EffectiveDate = x.EffectiveDate,
                    BaseSalary = x.BaseSalary,
                    Remarks = x.Remarks,
                    IsActive = x.IsActive
                }).FirstOrDefaultAsync();

            if (x == null) return new APIResponse<EmployeeSalaryAllocationDto> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            return new APIResponse<EmployeeSalaryAllocationDto> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = x };
        }

        public async Task<APIResponse<object>> CreateAsync(CreateEmployeeSalaryAllocationDto dto, string username)
        {
            // Deactivate previous active allocations for employee
            var previousAllocations = await _repo.List()
                .Where(x => x.EmployeeId == dto.EmployeeId && x.IsActive)
                .ToListAsync();

            foreach (var p in previousAllocations)
            {
                p.IsActive = false;
                _repo.Update(p);
            }

            await _repo.AddAsync(new EmployeeSalaryAllocation
            {
                EmployeeId = dto.EmployeeId,
                SalaryStructureId = dto.SalaryStructureId,
                EffectiveDate = dto.EffectiveDate,
                BaseSalary = dto.BaseSalary,
                Remarks = dto.Remarks,
                IsActive = dto.IsActive,
                CreatedBy = username,
                CreatedDate = DateTime.UtcNow
            });

            await _uow.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Salary structure allocated successfully" };
        }

        public async Task<APIResponse<object>> UpdateAsync(int id, UpdateEmployeeSalaryAllocationDto dto, string username)
        {
            var e = await _repo.List().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (e == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };

            e.SalaryStructureId = dto.SalaryStructureId;
            e.EffectiveDate = dto.EffectiveDate;
            e.BaseSalary = dto.BaseSalary;
            e.Remarks = dto.Remarks;
            e.IsActive = dto.IsActive;
            e.UpdatedBy = username;
            e.UpdatedDate = DateTime.UtcNow;

            _repo.Update(e);
            await _uow.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Allocation updated successfully" };
        }

        public async Task<APIResponse<object>> DeleteAsync(int id, string username)
        {
            var e = await _repo.List().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (e == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            _repo.Delete(e);
            await _uow.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Deleted successfully" };
        }
    }

    // 6. EmployeeLoanService
    public class EmployeeLoanService : IEmployeeLoanService
    {
        private readonly IRepository<EmployeeLoan> _repo;
        private readonly IUnitOfWork _uow;
        private readonly SchoolDbContext _dbContext;

        public EmployeeLoanService(IRepository<EmployeeLoan> repo, IUnitOfWork uow, SchoolDbContext dbContext)
        {
            _repo = repo;
            _uow = uow;
            _dbContext = dbContext;
        }

        public async Task<APIResponse<List<EmployeeLoanDto>>> GetAllAsync()
        {
            var d = await _repo.List().Include(x => x.Employee)
                .Select(x => new EmployeeLoanDto
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    EmployeeName = x.Employee.FirstName + " " + x.Employee.LastName,
                    EmployeeCode = x.Employee.EmployeeCode,
                    PrincipalAmount = x.PrincipalAmount,
                    InterestRate = x.InterestRate,
                    TotalInstallments = x.TotalInstallments,
                    MonthlyInstallment = x.MonthlyInstallment,
                    BalanceAmount = x.BalanceAmount,
                    Purpose = x.Purpose,
                    Status = x.Status,
                    ApprovedBy = x.ApprovedBy,
                    ApprovedDate = x.ApprovedDate
                }).ToListAsync();

            return new APIResponse<List<EmployeeLoanDto>> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = d };
        }

        public async Task<APIResponse<List<EmployeeLoanDto>>> GetByEmployeeIdAsync(int employeeId)
        {
            var d = await _repo.List().Include(x => x.Employee)
                .Where(x => x.EmployeeId == employeeId)
                .Select(x => new EmployeeLoanDto
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    EmployeeName = x.Employee.FirstName + " " + x.Employee.LastName,
                    EmployeeCode = x.Employee.EmployeeCode,
                    PrincipalAmount = x.PrincipalAmount,
                    InterestRate = x.InterestRate,
                    TotalInstallments = x.TotalInstallments,
                    MonthlyInstallment = x.MonthlyInstallment,
                    BalanceAmount = x.BalanceAmount,
                    Purpose = x.Purpose,
                    Status = x.Status
                }).ToListAsync();

            return new APIResponse<List<EmployeeLoanDto>> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = d };
        }

        public async Task<APIResponse<object>> CreateAsync(CreateEmployeeLoanDto dto, string username)
        {
            var schoolId = _dbContext.CurrentTenantId ?? 1;

            var loan = new EmployeeLoan
            {
                EmployeeId = dto.EmployeeId,
                PrincipalAmount = dto.PrincipalAmount,
                InterestRate = dto.InterestRate,
                TotalInstallments = dto.TotalInstallments,
                MonthlyInstallment = dto.MonthlyInstallment,
                BalanceAmount = dto.PrincipalAmount,
                Purpose = dto.Purpose,
                Status = "Pending",
                SchoolRegistrationId = schoolId,
                CreatedBy = username,
                CreatedDate = DateTime.UtcNow
            };

            await _repo.AddAsync(loan);
            await _uow.CommitAsync();

            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Loan request submitted successfully" };
        }

        public async Task<APIResponse<object>> UpdateStatusAsync(UpdateEmployeeLoanStatusDto dto, string username)
        {
            var schoolId = _dbContext.CurrentTenantId ?? 1;
            var e = await _repo.List().Where(x => x.Id == dto.Id).FirstOrDefaultAsync();
            if (e == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };

            using var transaction = await _uow.BeginTransactionAsync();
            try
            {
                e.Status = dto.Status;
                e.ApprovedBy = username;
                e.ApprovedDate = DateTime.UtcNow;
                e.UpdatedBy = username;
                e.UpdatedDate = DateTime.UtcNow;
                _repo.Update(e);

                if (dto.Status.Equals("Approved", StringComparison.OrdinalIgnoreCase))
                {
                    e.Status = "Running";

                    // Rebuild repayment schedule
                    var today = DateTime.Today;
                    for (int i = 1; i <= e.TotalInstallments; i++)
                    {
                        var repayment = new LoanRepaymentSchedule
                        {
                            EmployeeLoanId = e.Id,
                            InstallmentNo = i,
                            DueDate = today.AddMonths(i),
                            PrincipalComponent = Math.Round(e.PrincipalAmount / e.TotalInstallments, 2),
                            InterestComponent = 0, // Assuming interest-free or factored into principal
                            TotalAmount = e.MonthlyInstallment,
                            PaidAmount = 0,
                            Status = "Pending",
                            SchoolRegistrationId = schoolId,
                            CreatedBy = username,
                            CreatedDate = DateTime.UtcNow
                        };
                        await _dbContext.LoanRepaymentSchedules.AddAsync(repayment);
                    }
                }

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }

            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = $"Loan status updated to {dto.Status}" };
        }

        public async Task<APIResponse<object>> DeleteAsync(int id, string username)
        {
            var e = await _repo.List().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (e == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            _repo.Delete(e);
            await _uow.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Deleted successfully" };
        }
    }

    // 7. SalaryAdvanceService
    public class SalaryAdvanceService : ISalaryAdvanceService
    {
        private readonly IRepository<SalaryAdvance> _repo;
        private readonly IUnitOfWork _uow;

        public SalaryAdvanceService(IRepository<SalaryAdvance> repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<APIResponse<List<SalaryAdvanceDto>>> GetAllAsync()
        {
            var d = await _repo.List().Include(x => x.Employee)
                .Select(x => new SalaryAdvanceDto
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    EmployeeName = x.Employee.FirstName + " " + x.Employee.LastName,
                    EmployeeCode = x.Employee.EmployeeCode,
                    AdvanceAmount = x.AdvanceAmount,
                    RequestDate = x.RequestDate,
                    Status = x.Status,
                    ApprovedBy = x.ApprovedBy,
                    ApprovedDate = x.ApprovedDate,
                    TargetRecoveryMonth = x.TargetRecoveryMonth
                }).ToListAsync();

            return new APIResponse<List<SalaryAdvanceDto>> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = d };
        }

        public async Task<APIResponse<List<SalaryAdvanceDto>>> GetByEmployeeIdAsync(int employeeId)
        {
            var d = await _repo.List().Include(x => x.Employee)
                .Where(x => x.EmployeeId == employeeId)
                .Select(x => new SalaryAdvanceDto
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    EmployeeName = x.Employee.FirstName + " " + x.Employee.LastName,
                    EmployeeCode = x.Employee.EmployeeCode,
                    AdvanceAmount = x.AdvanceAmount,
                    RequestDate = x.RequestDate,
                    Status = x.Status
                }).ToListAsync();

            return new APIResponse<List<SalaryAdvanceDto>> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = d };
        }

        public async Task<APIResponse<object>> CreateAsync(CreateSalaryAdvanceDto dto, string username)
        {
            await _repo.AddAsync(new SalaryAdvance
            {
                EmployeeId = dto.EmployeeId,
                AdvanceAmount = dto.AdvanceAmount,
                TargetRecoveryMonth = dto.TargetRecoveryMonth,
                Status = "Pending",
                RequestDate = DateTime.UtcNow,
                CreatedBy = username,
                CreatedDate = DateTime.UtcNow
            });
            await _uow.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Salary advance request submitted" };
        }

        public async Task<APIResponse<object>> UpdateStatusAsync(UpdateSalaryAdvanceStatusDto dto, string username)
        {
            var e = await _repo.List().Where(x => x.Id == dto.Id).FirstOrDefaultAsync();
            if (e == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };

            e.Status = dto.Status;
            e.ApprovedBy = username;
            e.ApprovedDate = DateTime.UtcNow;
            e.UpdatedBy = username;
            e.UpdatedDate = DateTime.UtcNow;

            _repo.Update(e);
            await _uow.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = $"Advance request {dto.Status}" };
        }

        public async Task<APIResponse<object>> DeleteAsync(int id, string username)
        {
            var e = await _repo.List().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (e == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            _repo.Delete(e);
            await _uow.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Deleted successfully" };
        }
    }

    // 8. EmployeeBonusService
    public class EmployeeBonusService : IEmployeeBonusService
    {
        private readonly IRepository<EmployeeBonus> _repo;
        private readonly IUnitOfWork _uow;

        public EmployeeBonusService(IRepository<EmployeeBonus> repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<APIResponse<List<EmployeeBonusDto>>> GetAllAsync()
        {
            var d = await _repo.List().Include(x => x.Employee)
                .Select(x => new EmployeeBonusDto
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    EmployeeName = x.Employee.FirstName + " " + x.Employee.LastName,
                    EmployeeCode = x.Employee.EmployeeCode,
                    BonusType = x.BonusType,
                    Amount = x.Amount,
                    PayoutMonth = x.PayoutMonth,
                    Remarks = x.Remarks,
                    Status = x.Status
                }).ToListAsync();

            return new APIResponse<List<EmployeeBonusDto>> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = d };
        }

        public async Task<APIResponse<List<EmployeeBonusDto>>> GetByEmployeeIdAsync(int employeeId)
        {
            var d = await _repo.List().Include(x => x.Employee)
                .Where(x => x.EmployeeId == employeeId)
                .Select(x => new EmployeeBonusDto
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    EmployeeName = x.Employee.FirstName + " " + x.Employee.LastName,
                    EmployeeCode = x.Employee.EmployeeCode,
                    BonusType = x.BonusType,
                    Amount = x.Amount,
                    PayoutMonth = x.PayoutMonth,
                    Remarks = x.Remarks,
                    Status = x.Status
                }).ToListAsync();

            return new APIResponse<List<EmployeeBonusDto>> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = d };
        }

        public async Task<APIResponse<object>> CreateAsync(CreateEmployeeBonusDto dto, string username)
        {
            await _repo.AddAsync(new EmployeeBonus
            {
                EmployeeId = dto.EmployeeId,
                BonusType = dto.BonusType,
                Amount = dto.Amount,
                PayoutMonth = dto.PayoutMonth,
                Remarks = dto.Remarks,
                Status = "Pending",
                CreatedBy = username,
                CreatedDate = DateTime.UtcNow
            });
            await _uow.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Bonus request submitted" };
        }

        public async Task<APIResponse<object>> UpdateAsync(int id, UpdateEmployeeBonusDto dto, string username)
        {
            var e = await _repo.List().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (e == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };

            e.BonusType = dto.BonusType;
            e.Amount = dto.Amount;
            e.PayoutMonth = dto.PayoutMonth;
            e.Remarks = dto.Remarks;
            e.UpdatedBy = username;
            e.UpdatedDate = DateTime.UtcNow;

            _repo.Update(e);
            await _uow.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Bonus updated successfully" };
        }

        public async Task<APIResponse<object>> DeleteAsync(int id, string username)
        {
            var e = await _repo.List().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (e == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            _repo.Delete(e);
            await _uow.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Deleted successfully" };
        }
    }

    // 9. ReimbursementClaimService
    public class ReimbursementClaimService : IReimbursementClaimService
    {
        private readonly IRepository<ReimbursementClaim> _repo;
        private readonly IUnitOfWork _uow;
        private readonly IAccountingService _accountingService;

        public ReimbursementClaimService(IRepository<ReimbursementClaim> repo, IUnitOfWork uow, IAccountingService accountingService)
        {
            _repo = repo;
            _uow = uow;
            _accountingService = accountingService;
        }

        public async Task<APIResponse<List<ReimbursementClaimDto>>> GetAllAsync()
        {
            var d = await _repo.List().Include(x => x.Employee)
                .Select(x => new ReimbursementClaimDto
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    EmployeeName = x.Employee.FirstName + " " + x.Employee.LastName,
                    EmployeeCode = x.Employee.EmployeeCode,
                    ClaimType = x.ClaimType,
                    Amount = x.Amount,
                    ClaimDate = x.ClaimDate,
                    Description = x.Description,
                    AttachmentPath = x.AttachmentPath,
                    Status = x.Status,
                    ApprovedBy = x.ApprovedBy,
                    ApprovedDate = x.ApprovedDate,
                    SettlementRef = x.SettlementRef
                }).ToListAsync();

            return new APIResponse<List<ReimbursementClaimDto>> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = d };
        }

        public async Task<APIResponse<List<ReimbursementClaimDto>>> GetByEmployeeIdAsync(int employeeId)
        {
            var d = await _repo.List().Include(x => x.Employee)
                .Where(x => x.EmployeeId == employeeId)
                .Select(x => new ReimbursementClaimDto
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    EmployeeName = x.Employee.FirstName + " " + x.Employee.LastName,
                    EmployeeCode = x.Employee.EmployeeCode,
                    ClaimType = x.ClaimType,
                    Amount = x.Amount,
                    ClaimDate = x.ClaimDate,
                    Description = x.Description,
                    AttachmentPath = x.AttachmentPath,
                    Status = x.Status
                }).ToListAsync();

            return new APIResponse<List<ReimbursementClaimDto>> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = d };
        }

        public async Task<APIResponse<object>> CreateAsync(CreateReimbursementClaimDto dto, string username)
        {
            await _repo.AddAsync(new ReimbursementClaim
            {
                EmployeeId = dto.EmployeeId,
                ClaimType = dto.ClaimType,
                Amount = dto.Amount,
                Description = dto.Description,
                AttachmentPath = dto.AttachmentPath,
                Status = "Pending",
                ClaimDate = DateTime.UtcNow,
                CreatedBy = username,
                CreatedDate = DateTime.UtcNow
            });
            await _uow.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Reimbursement claim submitted" };
        }

        public async Task<APIResponse<object>> ApproveClaimAsync(ApproveClaimDto dto, string username)
        {
            var e = await _repo.List().Where(x => x.Id == dto.Id).FirstOrDefaultAsync();
            if (e == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };

            e.Status = dto.Status;
            e.ApprovedBy = username;
            e.ApprovedDate = DateTime.UtcNow;
            e.SettlementRef = dto.SettlementRef;
            e.UpdatedBy = username;
            e.UpdatedDate = DateTime.UtcNow;

            _repo.Update(e);
            await _uow.CommitAsync();

            // Post General Ledger Voucher automatically when settled
            if (dto.Status.Equals("Settled", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    var coaResponse = await _accountingService.GetChartOfAccountsAsync(e.SchoolRegistrationId);
                    var coaList = coaResponse?.Data;

                    var expenseAcc = coaList?.FirstOrDefault(a => a.Code == "50000") ?? coaList?.FirstOrDefault(a => a.AccountType.Equals("Expense", StringComparison.OrdinalIgnoreCase));
                    var assetAcc = coaList?.FirstOrDefault(a => a.Code == "11000") ?? coaList?.FirstOrDefault(a => a.Code == "10000") ?? coaList?.FirstOrDefault(a => a.AccountType.Equals("Asset", StringComparison.OrdinalIgnoreCase));

                    if (expenseAcc != null && assetAcc != null)
                    {
                        var journalDto = new School_DTOs.Finance.CreateJournalEntryDto
                        {
                            EntryDate = DateTime.UtcNow,
                            Narration = $"Settlement of Reimbursement Claim #{e.Id} - {e.ClaimType} for employee #{e.EmployeeId}. Ref: {dto.SettlementRef}",
                            Reference = $"CLAIM-{e.Id}",
                            Source = "Payroll",
                            VoucherType = "Payment",
                            Lines = new List<School_DTOs.Finance.CreateJournalEntryLineDto>
                            {
                                new School_DTOs.Finance.CreateJournalEntryLineDto
                                {
                                    AccountId = expenseAcc.Id,
                                    DebitAmount = e.Amount,
                                    CreditAmount = 0,
                                    Description = $"Debit Reimbursement Expense: {e.Description ?? e.ClaimType}"
                                },
                                new School_DTOs.Finance.CreateJournalEntryLineDto
                                {
                                    AccountId = assetAcc.Id,
                                    DebitAmount = 0,
                                    CreditAmount = e.Amount,
                                    Description = $"Credit Cash/Bank settlement Account"
                                }
                            }
                        };

                        await _accountingService.PostJournalEntryAsync(e.SchoolRegistrationId, journalDto, username);
                    }
                }
                catch (Exception ex)
                {
                    // Log or handle the exception to prevent rollback of status update if ledger fails
                    Console.WriteLine("Ledger posting failed: " + ex.Message);
                }
            }

            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = $"Claim status updated to {dto.Status}" };
        }

        public async Task<APIResponse<object>> DeleteAsync(int id, string username)
        {
            var e = await _repo.List().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (e == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            _repo.Delete(e);
            await _uow.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Deleted successfully" };
        }
    }

    // 10. SalaryArrearService
    public class SalaryArrearService : ISalaryArrearService
    {
        private readonly IRepository<SalaryArrear> _repo;
        private readonly IUnitOfWork _uow;

        public SalaryArrearService(IRepository<SalaryArrear> repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<APIResponse<List<SalaryArrearDto>>> GetAllAsync()
        {
            var d = await _repo.List().Include(x => x.Employee)
                .Select(x => new SalaryArrearDto
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    EmployeeName = x.Employee.FirstName + " " + x.Employee.LastName,
                    EmployeeCode = x.Employee.EmployeeCode,
                    Amount = x.Amount,
                    ArrearMonth = x.ArrearMonth,
                    PaidInMonth = x.PaidInMonth,
                    Reason = x.Reason,
                    Status = x.Status
                }).ToListAsync();

            return new APIResponse<List<SalaryArrearDto>> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = d };
        }

        public async Task<APIResponse<List<SalaryArrearDto>>> GetByEmployeeIdAsync(int employeeId)
        {
            var d = await _repo.List().Include(x => x.Employee)
                .Where(x => x.EmployeeId == employeeId)
                .Select(x => new SalaryArrearDto
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    EmployeeName = x.Employee.FirstName + " " + x.Employee.LastName,
                    EmployeeCode = x.Employee.EmployeeCode,
                    Amount = x.Amount,
                    ArrearMonth = x.ArrearMonth,
                    PaidInMonth = x.PaidInMonth,
                    Reason = x.Reason,
                    Status = x.Status
                }).ToListAsync();

            return new APIResponse<List<SalaryArrearDto>> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = d };
        }

        public async Task<APIResponse<object>> CreateAsync(CreateSalaryArrearDto dto, string username)
        {
            await _repo.AddAsync(new SalaryArrear
            {
                EmployeeId = dto.EmployeeId,
                Amount = dto.Amount,
                ArrearMonth = dto.ArrearMonth,
                Reason = dto.Reason,
                Status = "Pending",
                CreatedBy = username,
                CreatedDate = DateTime.UtcNow
            });
            await _uow.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Arrear record created successfully" };
        }

        public async Task<APIResponse<object>> DeleteAsync(int id, string username)
        {
            var e = await _repo.List().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (e == null) return new APIResponse<object> { StatusCode = HttpStatusCode.NotFound, Message = "Not found" };
            _repo.Delete(e);
            await _uow.CommitAsync();
            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Deleted successfully" };
        }
    }

    // 11. StatutoryComplianceConfigService
    public class StatutoryComplianceConfigService : IStatutoryComplianceConfigService
    {
        private readonly IRepository<StatutoryComplianceConfig> _repo;
        private readonly IUnitOfWork _uow;
        private readonly SchoolDbContext _dbContext;

        public StatutoryComplianceConfigService(IRepository<StatutoryComplianceConfig> repo, IUnitOfWork uow, SchoolDbContext dbContext)
        {
            _repo = repo;
            _uow = uow;
            _dbContext = dbContext;
        }

        public async Task<APIResponse<StatutoryComplianceConfigDto>> GetConfigAsync()
        {
            var tenantId = _dbContext.CurrentTenantId ?? 1;
            var x = await _repo.List().Where(c => c.SchoolRegistrationId == tenantId).FirstOrDefaultAsync();

            if (x == null)
            {
                // Create a default one if none exists
                x = new StatutoryComplianceConfig
                {
                    SchoolRegistrationId = tenantId,
                    PfEmployerRate = 12.00m,
                    PfEmployeeRate = 12.00m,
                    PfMaxBasicLimit = 15000.00m,
                    EsiEmployerRate = 3.25m,
                    EsiEmployeeRate = 0.75m,
                    EsiMaxGrossLimit = 21000.00m,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow
                };
                await _repo.AddAsync(x);
                await _uow.CommitAsync();
            }

            var dto = new StatutoryComplianceConfigDto
            {
                Id = x.Id,
                PfEmployerRate = x.PfEmployerRate,
                PfEmployeeRate = x.PfEmployeeRate,
                PfMaxBasicLimit = x.PfMaxBasicLimit,
                EsiEmployerRate = x.EsiEmployerRate,
                EsiEmployeeRate = x.EsiEmployeeRate,
                EsiMaxGrossLimit = x.EsiMaxGrossLimit,
                ProfessionalTaxSlabJson = x.ProfessionalTaxSlabJson,
                EnableGratuity = x.EnableGratuity
            };

            return new APIResponse<StatutoryComplianceConfigDto> { StatusCode = HttpStatusCode.OK, Message = "Success", Data = dto };
        }

        public async Task<APIResponse<object>> SaveConfigAsync(StatutoryComplianceConfigDto dto, string username)
        {
            var tenantId = _dbContext.CurrentTenantId ?? 1;
            var x = await _repo.List().Where(c => c.SchoolRegistrationId == tenantId).FirstOrDefaultAsync();

            if (x == null)
            {
                x = new StatutoryComplianceConfig { SchoolRegistrationId = tenantId };
                await _repo.AddAsync(x);
            }

            x.PfEmployerRate = dto.PfEmployerRate;
            x.PfEmployeeRate = dto.PfEmployeeRate;
            x.PfMaxBasicLimit = dto.PfMaxBasicLimit;
            x.EsiEmployerRate = dto.EsiEmployerRate;
            x.EsiEmployeeRate = dto.EsiEmployeeRate;
            x.EsiMaxGrossLimit = dto.EsiMaxGrossLimit;
            x.ProfessionalTaxSlabJson = dto.ProfessionalTaxSlabJson;
            x.EnableGratuity = dto.EnableGratuity;
            x.UpdatedBy = username;
            x.UpdatedDate = DateTime.UtcNow;

            _repo.Update(x);
            await _uow.CommitAsync();

            return new APIResponse<object> { StatusCode = HttpStatusCode.OK, Message = "Compliance configurations updated" };
        }
    }
}
