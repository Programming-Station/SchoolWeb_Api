using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using School.Domain;
using School.Domain.Finance;
using School.Domain.School;

namespace School.Infrastructure.Seeds
{
    public class DefaultFinanceData
    {
        public static async Task SeedAsync(SchoolDbContext context)
        {
            // Seed a default school registration if none exist to prevent tenant constraint issues
            var school = await context.SchoolRegistrations.IgnoreQueryFilters().FirstOrDefaultAsync();
            if (school == null) return;

            int schoolId = school.Id;

            // 1. Chart of Accounts
            if (!await context.CoaAccounts.IgnoreQueryFilters().AnyAsync(a => a.SchoolRegistrationId == schoolId))
            {
                var accounts = new List<CoaAccount>
                {
                    new CoaAccount { Code = "10000", Name = "Cash in Hand", AccountType = "Asset", IsActive = true, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new CoaAccount { Code = "11000", Name = "HDFC Operational Bank", AccountType = "Asset", IsActive = true, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new CoaAccount { Code = "12000", Name = "Inventory Stock Ledger", AccountType = "Asset", IsActive = true, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new CoaAccount { Code = "20000", Name = "Accounts Payable (Creditors)", AccountType = "Liability", IsActive = true, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new CoaAccount { Code = "21000", Name = "GST Payable Ledger", AccountType = "Liability", IsActive = true, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new CoaAccount { Code = "30000", Name = "Capital Equity Account", AccountType = "Equity", IsActive = true, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new CoaAccount { Code = "40000", Name = "Tuition Fees Income", AccountType = "Revenue", IsActive = true, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new CoaAccount { Code = "50000", Name = "Staff Salaries Expense", AccountType = "Expense", IsActive = true, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new CoaAccount { Code = "51000", Name = "Fixed Asset Depreciation Expense", AccountType = "Expense", IsActive = true, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" }
                };

                context.CoaAccounts.AddRange(accounts);
                await context.SaveChangesAsync();
            }

            var coaList = await context.CoaAccounts.IgnoreQueryFilters().Where(a => a.SchoolRegistrationId == schoolId).ToListAsync();
            var bankAccount = coaList.FirstOrDefault(a => a.Code == "11000");
            var cashAccount = coaList.FirstOrDefault(a => a.Code == "10000");
            var salaryAccount = coaList.FirstOrDefault(a => a.Code == "50000");
            var tuitionAccount = coaList.FirstOrDefault(a => a.Code == "40000");
            var gstAccount = coaList.FirstOrDefault(a => a.Code == "21000");

            // 2. Financial Years
            if (!await context.FinancialYears.IgnoreQueryFilters().AnyAsync(y => y.SchoolRegistrationId == schoolId))
            {
                var years = new List<FinancialYear>
                {
                    new FinancialYear
                    {
                        YearName = "2024-2025",
                        StartDate = new DateTime(2024, 4, 1, 0, 0, 0, DateTimeKind.Utc),
                        EndDate = new DateTime(2025, 3, 31, 23, 59, 59, DateTimeKind.Utc),
                        IsClosed = true,
                        IsLocked = true,
                        SchoolRegistrationId = schoolId,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = "seed"
                    },
                    new FinancialYear
                    {
                        YearName = "2025-2026",
                        StartDate = new DateTime(2025, 4, 1, 0, 0, 0, DateTimeKind.Utc),
                        EndDate = new DateTime(2026, 3, 31, 23, 59, 59, DateTimeKind.Utc),
                        IsClosed = false,
                        IsLocked = false,
                        SchoolRegistrationId = schoolId,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = "seed"
                    },
                    new FinancialYear
                    {
                        YearName = "2026-2027",
                        StartDate = new DateTime(2026, 4, 1, 0, 0, 0, DateTimeKind.Utc),
                        EndDate = new DateTime(2027, 3, 31, 23, 59, 59, DateTimeKind.Utc),
                        IsClosed = false,
                        IsLocked = false,
                        SchoolRegistrationId = schoolId,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = "seed"
                    }
                };
                context.FinancialYears.AddRange(years);
                await context.SaveChangesAsync();
            }

            // 3. Cost Centers
            if (!await context.CostCenters.IgnoreQueryFilters().AnyAsync(c => c.SchoolRegistrationId == schoolId))
            {
                var costCenters = new List<CostCenter>
                {
                    new CostCenter { Code = "CC-ADM", Name = "Administration Department", IsActive = true, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new CostCenter { Code = "CC-IT", Name = "IT Department", IsActive = true, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new CostCenter { Code = "CC-LIB", Name = "Library Management", IsActive = true, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" }
                };
                context.CostCenters.AddRange(costCenters);
                await context.SaveChangesAsync();
            }

            var ccList = await context.CostCenters.IgnoreQueryFilters().Where(c => c.SchoolRegistrationId == schoolId).ToListAsync();
            var itCostCenter = ccList.FirstOrDefault(c => c.Code == "CC-IT");

            // 4. Cheque Books
            if (bankAccount != null && !await context.ChequeBooks.IgnoreQueryFilters().AnyAsync(b => b.SchoolRegistrationId == schoolId))
            {
                var book = new ChequeBook
                {
                    BankAccountId = bankAccount.Id,
                    SeriesPrefix = "HDFC",
                    StartChequeNo = 100001,
                    EndChequeNo = 100100,
                    NextChequeNo = 100001,
                    IsExhausted = false,
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "seed"
                };
                context.ChequeBooks.Add(book);
                await context.SaveChangesAsync();
            }

            // 5. Budget Plans
            var dept = await context.Departments.IgnoreQueryFilters().FirstOrDefaultAsync();
            if (dept != null && salaryAccount != null && !await context.BudgetPlans.IgnoreQueryFilters().AnyAsync(b => b.SchoolRegistrationId == schoolId))
            {
                var budget = new BudgetPlan
                {
                    FinancialYear = "2025-2026",
                    DepartmentId = dept.Id,
                    AccountId = salaryAccount.Id,
                    AllocatedAmount = 5000000m,
                    UtilizedAmount = 1500000m,
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "seed"
                };
                context.BudgetPlans.Add(budget);
                await context.SaveChangesAsync();
            }

            // 6. Tax Configurations
            if (gstAccount != null && !await context.TaxConfigs.IgnoreQueryFilters().AnyAsync(t => t.SchoolRegistrationId == schoolId))
            {
                var taxes = new List<TaxConfig>
                {
                    new TaxConfig { TaxName = "CGST 9%", Percentage = 9.00m, AccountId = gstAccount.Id, IsActive = true, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new TaxConfig { TaxName = "SGST 9%", Percentage = 9.00m, AccountId = gstAccount.Id, IsActive = true, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" },
                    new TaxConfig { TaxName = "IGST 18%", Percentage = 18.00m, AccountId = gstAccount.Id, IsActive = true, SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow, CreatedBy = "seed" }
                };
                context.TaxConfigs.AddRange(taxes);
                await context.SaveChangesAsync();
            }

            // 7. Journal Entries (Mock Vouchers)
            if (cashAccount != null && tuitionAccount != null && !await context.JournalEntries.IgnoreQueryFilters().AnyAsync(j => j.SchoolRegistrationId == schoolId))
            {
                var entry = new JournalEntry
                {
                    VoucherNo = "JV-2025-00001",
                    EntryDate = DateTime.UtcNow.AddDays(-10),
                    Narration = "Opening Tuition Fees collection recorded",
                    Reference = "REF-FEES-001",
                    Source = "Manual",
                    VoucherType = "Journal",
                    Status = "Approved",
                    IsPosted = true,
                    ApprovedBy = "seed",
                    ApprovedDate = DateTime.UtcNow.AddDays(-10),
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddDays(-10),
                    CreatedBy = "seed"
                };

                entry.Lines.Add(new JournalEntryLine
                {
                    AccountId = cashAccount.Id,
                    DebitAmount = 15000m,
                    CreditAmount = 0m,
                    Description = "Fees received in cash",
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddDays(-10),
                    CreatedBy = "seed"
                });

                entry.Lines.Add(new JournalEntryLine
                {
                    AccountId = tuitionAccount.Id,
                    DebitAmount = 0m,
                    CreditAmount = 15000m,
                    Description = "Tuition Fees Income credited",
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddDays(-10),
                    CreatedBy = "seed"
                });

                context.JournalEntries.Add(entry);

                // Update ledger balances accordingly
                cashAccount.CurrentBalance += 15000m;
                tuitionAccount.CurrentBalance += 15000m;

                await context.SaveChangesAsync();

                // 8. Cash Bank Transactions
                if (bankAccount != null && !await context.CashBankTransactions.IgnoreQueryFilters().AnyAsync(t => t.SchoolRegistrationId == schoolId))
                {
                    var txn = new CashBankTransaction
                    {
                        JournalEntryId = entry.Id,
                        AccountId = bankAccount.Id,
                        TransactionType = "Deposit",
                        PaymentMode = "BankTransfer",
                        ReferenceNo = "TXN-FEES-100458",
                        TransactionDate = DateTime.UtcNow.AddDays(-10),
                        Amount = 15000m,
                        IsReconciled = true,
                        ReconciledDate = DateTime.UtcNow.AddDays(-9),
                        ClearedDate = DateTime.UtcNow.AddDays(-9),
                        SchoolRegistrationId = schoolId,
                        CreatedDate = DateTime.UtcNow.AddDays(-10),
                        CreatedBy = "seed"
                    };
                    context.CashBankTransactions.Add(txn);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
