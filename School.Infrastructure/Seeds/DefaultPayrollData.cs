using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using School.Domain.Payroll;

namespace School.Infrastructure.Seeds
{
    public class DefaultPayrollData
    {
        public static async Task SeedAsync(SchoolDbContext context)
        {
            var schoolId = context.SchoolRegistrations.FirstOrDefault()?.Id ?? 1;

            // 1. Seed Statutory Compliance Configurations
            if (!await context.StatutoryComplianceConfigs.AnyAsync())
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
            if (!await context.SalaryComponents.AnyAsync())
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
            if (!await context.PayGroups.AnyAsync())
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
            if (!await context.SalaryStructures.AnyAsync())
            {
                var payGroup = await context.PayGroups.FirstOrDefaultAsync(p => p.Name == "Monthly Staff Pay Group");
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
                    await context.SaveChangesAsync(); // Auto Id

                    var compBasic = await context.SalaryComponents.FirstOrDefaultAsync(c => c.Name == "Basic");
                    var compHra = await context.SalaryComponents.FirstOrDefaultAsync(c => c.Name == "HRA");
                    var compConveyance = await context.SalaryComponents.FirstOrDefaultAsync(c => c.Name == "Conveyance Allowance");
                    var compMedical = await context.SalaryComponents.FirstOrDefaultAsync(c => c.Name == "Medical Allowance");
                    var compPf = await context.SalaryComponents.FirstOrDefaultAsync(c => c.Name.Contains("PF"));
                    var compEsi = await context.SalaryComponents.FirstOrDefaultAsync(c => c.Name.Contains("ESI"));
                    var compPt = await context.SalaryComponents.FirstOrDefaultAsync(c => c.Name.Contains("PT"));

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
            if (!await context.EmployeeSalaryAllocations.AnyAsync())
            {
                var employees = await context.Employees.Take(5).ToListAsync();
                var salaryStructure = await context.SalaryStructures.FirstOrDefaultAsync(s => s.Name == "Standard Teaching Staff Structure");

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
        }
    }
}
