using Microsoft.EntityFrameworkCore;
using School.Domain.Payroll;

namespace School.Infrastructure.Seeds
{
    public class DefaultPayrollData
    {
        public static async Task SeedAsync(SchoolDbContext context)
        {
            var school = await context.SchoolRegistrations.IgnoreQueryFilters().FirstOrDefaultAsync();
            if (school == null) return;

            int schoolId = school.Id;

            // 1. Seed Statutory Compliance Configurations
            if (!await context.StatutoryComplianceConfigs.IgnoreQueryFilters().AnyAsync(c => c.SchoolRegistrationId == schoolId))
            {
                var config = new StatutoryComplianceConfig
                {
                    SchoolRegistrationId = schoolId,
                    PfEmployerRate = 12.00m,
                    PfEmployeeRate = 12.00m,
                    PfMaxBasicLimit = 15000.00m,
                    EsiEmployerRate = 3.25m,
                    EsiEmployeeRate = 0.75m,
                    EsiMaxGrossLimit = 21000.00m,
                    ProfessionalTaxSlabJson = "[{\"min\":0,\"max\":10000,\"tax\":0},{\"min\":10001,\"max\":15000,\"tax\":150},{\"min\":15001,\"max\":9999999,\"tax\":200}]",
                    EnableGratuity = true,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow
                };
                await context.StatutoryComplianceConfigs.AddAsync(config);
                await context.SaveChangesAsync();
            }

            // 2. Seed Salary Components
            if (!await context.SalaryComponents.IgnoreQueryFilters().AnyAsync())
            {
                var components = new List<SalaryComponent>
                {
                    new SalaryComponent { Name = "Basic", Type = "Earning", Amount = 0, Status = "active", CreatedBy = "System", CreatedDate = DateTime.UtcNow },
                    new SalaryComponent { Name = "HRA", Type = "Earning", Amount = 0, Status = "active", CreatedBy = "System", CreatedDate = DateTime.UtcNow },
                    new SalaryComponent { Name = "Conveyance Allowance", Type = "Earning", Amount = 1600, Status = "active", CreatedBy = "System", CreatedDate = DateTime.UtcNow },
                    new SalaryComponent { Name = "Medical Allowance", Type = "Earning", Amount = 1250, Status = "active", CreatedBy = "System", CreatedDate = DateTime.UtcNow },
                    new SalaryComponent { Name = "Special Allowance", Type = "Earning", Amount = 0, Status = "active", CreatedBy = "System", CreatedDate = DateTime.UtcNow },
                    new SalaryComponent { Name = "Provident Fund (PF)", Type = "Deduction", Amount = 0, Status = "active", CreatedBy = "System", CreatedDate = DateTime.UtcNow },
                    new SalaryComponent { Name = "Employee State Insurance (ESI)", Type = "Deduction", Amount = 0, Status = "active", CreatedBy = "System", CreatedDate = DateTime.UtcNow },
                    new SalaryComponent { Name = "Professional Tax (PT)", Type = "Deduction", Amount = 0, Status = "active", CreatedBy = "System", CreatedDate = DateTime.UtcNow },
                    new SalaryComponent { Name = "TDS / Income Tax", Type = "Deduction", Amount = 0, Status = "active", CreatedBy = "System", CreatedDate = DateTime.UtcNow }
                };
                await context.SalaryComponents.AddRangeAsync(components);
                await context.SaveChangesAsync();
            }

            // 3. Seed Pay Groups
            if (!await context.PayGroups.IgnoreQueryFilters().AnyAsync(p => p.SchoolRegistrationId == schoolId))
            {
                var payGroups = new List<PayGroup>
                {
                    new PayGroup { Name = "Monthly Staff Pay Group", Frequency = "Monthly", Currency = "INR", IsActive = true, SchoolRegistrationId = schoolId, CreatedBy = "System", CreatedDate = DateTime.UtcNow },
                    new PayGroup { Name = "Weekly Wage Pay Group", Frequency = "Weekly", Currency = "INR", IsActive = true, SchoolRegistrationId = schoolId, CreatedBy = "System", CreatedDate = DateTime.UtcNow }
                };
                await context.PayGroups.AddRangeAsync(payGroups);
                await context.SaveChangesAsync();
            }

            // 4. Seed Salary Structure & Items
            if (!await context.SalaryStructures.IgnoreQueryFilters().AnyAsync(s => s.SchoolRegistrationId == schoolId))
            {
                var payGroup = await context.PayGroups.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.SchoolRegistrationId == schoolId && p.Name == "Monthly Staff Pay Group");
                if (payGroup != null)
                {
                    var salaryStructure = new SalaryStructure
                    {
                        Name = "Standard Teaching Staff Structure",
                        Description = "Standard salary structure for primary and high school teachers",
                        PayGroupId = payGroup.Id,
                        IsActive = true,
                        SchoolRegistrationId = schoolId,
                        CreatedBy = "System",
                        CreatedDate = DateTime.UtcNow
                    };
                    await context.SalaryStructures.AddAsync(salaryStructure);
                    await context.SaveChangesAsync();

                    var compBasic = await context.SalaryComponents.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.Name == "Basic");
                    var compHra = await context.SalaryComponents.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.Name == "HRA");
                    var compConveyance = await context.SalaryComponents.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.Name == "Conveyance Allowance");
                    var compMedical = await context.SalaryComponents.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.Name == "Medical Allowance");
                    var compPf = await context.SalaryComponents.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.Name.Contains("PF"));
                    var compEsi = await context.SalaryComponents.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.Name.Contains("ESI"));
                    var compPt = await context.SalaryComponents.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.Name.Contains("PT"));

                    var items = new List<SalaryStructureItem>();

                    if (compBasic != null)
                        items.Add(new SalaryStructureItem { SalaryStructureId = salaryStructure.Id, SalaryComponentId = compBasic.Id, CalculationType = "PercentageOfBasic", Value = 50.00m, DisplayOrder = 1, SchoolRegistrationId = schoolId, CreatedBy = "System", CreatedDate = DateTime.UtcNow });
                    if (compHra != null)
                        items.Add(new SalaryStructureItem { SalaryStructureId = salaryStructure.Id, SalaryComponentId = compHra.Id, CalculationType = "PercentageOfBasic", Value = 20.00m, DisplayOrder = 2, SchoolRegistrationId = schoolId, CreatedBy = "System", CreatedDate = DateTime.UtcNow });
                    if (compConveyance != null)
                        items.Add(new SalaryStructureItem { SalaryStructureId = salaryStructure.Id, SalaryComponentId = compConveyance.Id, CalculationType = "Fixed", Value = 1600.00m, DisplayOrder = 3, SchoolRegistrationId = schoolId, CreatedBy = "System", CreatedDate = DateTime.UtcNow });
                    if (compMedical != null)
                        items.Add(new SalaryStructureItem { SalaryStructureId = salaryStructure.Id, SalaryComponentId = compMedical.Id, CalculationType = "Fixed", Value = 1250.00m, DisplayOrder = 4, SchoolRegistrationId = schoolId, CreatedBy = "System", CreatedDate = DateTime.UtcNow });
                    if (compPf != null)
                        items.Add(new SalaryStructureItem { SalaryStructureId = salaryStructure.Id, SalaryComponentId = compPf.Id, CalculationType = "Formula", Value = 12.00m, Formula = "Basic * 0.12", DisplayOrder = 5, SchoolRegistrationId = schoolId, CreatedBy = "System", CreatedDate = DateTime.UtcNow });
                    if (compEsi != null)
                        items.Add(new SalaryStructureItem { SalaryStructureId = salaryStructure.Id, SalaryComponentId = compEsi.Id, CalculationType = "Formula", Value = 0.75m, Formula = "Gross * 0.0075", DisplayOrder = 6, SchoolRegistrationId = schoolId, CreatedBy = "System", CreatedDate = DateTime.UtcNow });
                    if (compPt != null)
                        items.Add(new SalaryStructureItem { SalaryStructureId = salaryStructure.Id, SalaryComponentId = compPt.Id, CalculationType = "Fixed", Value = 200.00m, DisplayOrder = 7, SchoolRegistrationId = schoolId, CreatedBy = "System", CreatedDate = DateTime.UtcNow });

                    await context.SalaryStructureItems.AddRangeAsync(items);
                    await context.SaveChangesAsync();
                }
            }

            // 5. Seed Employee Salary Allocations
            if (!await context.EmployeeSalaryAllocations.IgnoreQueryFilters().AnyAsync(a => a.SchoolRegistrationId == schoolId))
            {
                var employees = await context.Employees.IgnoreQueryFilters().Where(e => e.SchoolRegistrationId == schoolId).Take(5).ToListAsync();
                var salaryStructure = await context.SalaryStructures.IgnoreQueryFilters().FirstOrDefaultAsync(s => s.SchoolRegistrationId == schoolId && s.Name == "Standard Teaching Staff Structure");

                if (salaryStructure != null && employees.Any())
                {
                    decimal[] baseSalaries = { 45000m, 52000m, 38000m, 62000m, 48000m };
                    int index = 0;

                    foreach (var emp in employees)
                    {
                        var alloc = new EmployeeSalaryAllocation
                        {
                            EmployeeId = emp.Id,
                            SalaryStructureId = salaryStructure.Id,
                            EffectiveDate = new DateTime(2026, 1, 1),
                            BaseSalary = baseSalaries[index % baseSalaries.Length],
                            Remarks = "Initial Structure Allocation",
                            IsActive = true,
                            SchoolRegistrationId = schoolId,
                            CreatedBy = "System",
                            CreatedDate = DateTime.UtcNow
                        };
                        await context.EmployeeSalaryAllocations.AddAsync(alloc);
                        index++;
                    }
                    await context.SaveChangesAsync();
                }
            }

            // 6. Seed Employee Bonuses
            var primaryEmp = await context.Employees.IgnoreQueryFilters().FirstOrDefaultAsync(e => e.SchoolRegistrationId == schoolId);
            if (primaryEmp != null)
            {
                if (!await context.EmployeeBonuses.IgnoreQueryFilters().AnyAsync(b => b.SchoolRegistrationId == schoolId))
                {
                    var bonus = new EmployeeBonus
                    {
                        EmployeeId = primaryEmp.Id,
                        BonusType = "Festival",
                        Amount = 5000.00m,
                        PayoutMonth = "2026-03",
                        Remarks = "Holi Festival Bonus allocation.",
                        Status = "Approved",
                        SchoolRegistrationId = schoolId,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = "System"
                    };
                    context.EmployeeBonuses.Add(bonus);
                }

                // 7. Seed Employee Loans & Repayment Schedules
                if (!await context.EmployeeLoans.IgnoreQueryFilters().AnyAsync(l => l.SchoolRegistrationId == schoolId))
                {
                    var loan = new EmployeeLoan
                    {
                        EmployeeId = primaryEmp.Id,
                        PrincipalAmount = 50000.00m,
                        InterestRate = 0.00m,
                        TotalInstallments = 10,
                        MonthlyInstallment = 5000.00m,
                        BalanceAmount = 45000.00m,
                        Purpose = "Personal Home Renovation Loan",
                        Status = "Running",
                        ApprovedBy = "System Admin",
                        ApprovedDate = DateTime.UtcNow.AddDays(-30),
                        SchoolRegistrationId = schoolId,
                        CreatedDate = DateTime.UtcNow.AddDays(-30),
                        CreatedBy = "System"
                    };
                    context.EmployeeLoans.Add(loan);
                    await context.SaveChangesAsync();

                    for (int i = 1; i <= 10; i++)
                    {
                        var sched = new LoanRepaymentSchedule
                        {
                            EmployeeLoanId = loan.Id,
                            InstallmentNo = i,
                            DueDate = DateTime.UtcNow.AddDays(-30 + (i * 30)),
                            PrincipalComponent = 5000.00m,
                            InterestComponent = 0.00m,
                            TotalAmount = 5000.00m,
                            PaidAmount = i == 1 ? 5000.00m : 0.00m,
                            PaidDate = i == 1 ? DateTime.UtcNow.AddDays(-5) : null,
                            Status = i == 1 ? "Paid" : "Pending",
                            SchoolRegistrationId = schoolId,
                            CreatedDate = DateTime.UtcNow.AddDays(-30),
                            CreatedBy = "System"
                        };
                        context.LoanRepaymentSchedules.Add(sched);
                    }
                }

                // 8. Seed Reimbursement Claims
                if (!await context.ReimbursementClaims.IgnoreQueryFilters().AnyAsync(r => r.SchoolRegistrationId == schoolId))
                {
                    var claim = new ReimbursementClaim
                    {
                        EmployeeId = primaryEmp.Id,
                        ClaimType = "Travel",
                        Amount = 2450.00m,
                        ClaimDate = DateTime.UtcNow.AddDays(-12),
                        Description = "Outstation travel expenses for CBSE board meeting conference.",
                        AttachmentPath = "/uploads/claims/travel_receipt_cbse.pdf",
                        Status = "Approved",
                        ApprovedBy = "System Admin",
                        ApprovedDate = DateTime.UtcNow.AddDays(-10),
                        SettlementRef = "SET-BANK-99887",
                        SchoolRegistrationId = schoolId,
                        CreatedDate = DateTime.UtcNow.AddDays(-12),
                        CreatedBy = "System"
                    };
                    context.ReimbursementClaims.Add(claim);
                }

                // 9. Seed Salary Advances
                if (!await context.SalaryAdvances.IgnoreQueryFilters().AnyAsync(a => a.SchoolRegistrationId == schoolId))
                {
                    var advance = new SalaryAdvance
                    {
                        EmployeeId = primaryEmp.Id,
                        AdvanceAmount = 10000.00m,
                        RequestDate = DateTime.UtcNow.AddDays(-15),
                        Status = "Approved",
                        ApprovedBy = "System Admin",
                        ApprovedDate = DateTime.UtcNow.AddDays(-12),
                        TargetRecoveryMonth = "2026-03",
                        SchoolRegistrationId = schoolId,
                        CreatedDate = DateTime.UtcNow.AddDays(-15),
                        CreatedBy = "System"
                    };
                    context.SalaryAdvances.Add(advance);
                }

                // 10. Seed Salary Arrears
                if (!await context.SalaryArrears.IgnoreQueryFilters().AnyAsync(a => a.SchoolRegistrationId == schoolId))
                {
                    var arrear = new SalaryArrear
                    {
                        EmployeeId = primaryEmp.Id,
                        Amount = 4800.00m,
                        ArrearMonth = "2026-01",
                        PaidInMonth = "2026-02",
                        Reason = "Partial join days increment adjustment.",
                        Status = "Paid",
                        SchoolRegistrationId = schoolId,
                        CreatedDate = DateTime.UtcNow.AddDays(-15),
                        CreatedBy = "System"
                    };
                    context.SalaryArrears.Add(arrear);
                }

                // 11. Seed Payroll Run & Details
                if (!await context.PayrollRuns.IgnoreQueryFilters().AnyAsync(p => p.SchoolRegistrationId == schoolId))
                {
                    var payGroup = await context.PayGroups.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.SchoolRegistrationId == schoolId);
                    var payroll = new PayrollRun
                    {
                        EmployeeId = primaryEmp.Id,
                        PayGroupId = payGroup?.Id,
                        Month = "2026-02",
                        GrossSalary = 45000.00m,
                        TotalDeductions = 1000.00m,
                        NetSalary = 44000.00m,
                        Status = "Paid",
                        PaymentMethod = "BankTransfer",
                        PaymentRef = "FT-SALARY-102548",
                        PaidDate = DateTime.UtcNow.AddDays(-2),
                        SchoolRegistrationId = schoolId,
                        CreatedDate = DateTime.UtcNow.AddDays(-2),
                        CreatedBy = "System"
                    };
                    context.PayrollRuns.Add(payroll);
                    await context.SaveChangesAsync();

                    var comps = await context.SalaryComponents.ToListAsync();
                    var basicComp = comps.FirstOrDefault(c => c.Name == "Basic");
                    var hraComp = comps.FirstOrDefault(c => c.Name == "HRA");
                    var medicalComp = comps.FirstOrDefault(c => c.Name == "Medical Allowance");
                    var ptComp = comps.FirstOrDefault(c => c.Name.Contains("PT"));

                    if (basicComp != null)
                        context.PayrollRunDetails.Add(new PayrollRunDetail { PayrollRunId = payroll.Id, SalaryComponentId = basicComp.Id, Amount = 30000.00m, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow });
                    if (hraComp != null)
                        context.PayrollRunDetails.Add(new PayrollRunDetail { PayrollRunId = payroll.Id, SalaryComponentId = hraComp.Id, Amount = 13750.00m, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow });
                    if (medicalComp != null)
                        context.PayrollRunDetails.Add(new PayrollRunDetail { PayrollRunId = payroll.Id, SalaryComponentId = medicalComp.Id, Amount = 1250.00m, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow });
                    if (ptComp != null)
                        context.PayrollRunDetails.Add(new PayrollRunDetail { PayrollRunId = payroll.Id, SalaryComponentId = ptComp.Id, Amount = 200.00m, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow });
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
