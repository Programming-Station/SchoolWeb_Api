using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using School.Domain.Finance;
using School.Infrastructure;
using School.Services.Interfaces;
using School_DTOs;
using School_DTOs.Finance;

namespace School.Services.Finance
{
    public class AccountingService : IAccountingService
    {
        private readonly SchoolDbContext _context;

        public AccountingService(SchoolDbContext context)
        {
            _context = context;
        }

        // --- Chart of Accounts ---
        public async Task<APIResponse<List<CoaAccountDto>>> GetChartOfAccountsAsync(int schoolId)
        {
            var accounts = await _context.CoaAccounts
                .Include(a => a.ParentAccount)
                .Where(a => a.SchoolRegistrationId == schoolId && !a.IsDeleted)
                .OrderBy(a => a.Code)
                .ToListAsync();

            var list = accounts.Select(a => new CoaAccountDto
            {
                Id = a.Id,
                Code = a.Code,
                Name = a.Name,
                AccountType = a.AccountType,
                ParentAccountId = a.ParentAccountId,
                ParentAccountName = a.ParentAccount?.Name,
                CurrentBalance = a.CurrentBalance,
                IsActive = a.IsActive
            }).ToList();

            return new APIResponse<List<CoaAccountDto>>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = list
            };
        }

        public async Task<APIResponse<CoaAccountDto>> CreateAccountAsync(int schoolId, CreateCoaAccountDto dto, string user)
        {
            // Verify if code is unique
            var exists = await _context.CoaAccounts
                .AnyAsync(a => a.SchoolRegistrationId == schoolId && a.Code == dto.Code && !a.IsDeleted);

            if (exists)
            {
                return new APIResponse<CoaAccountDto>
                {
                    Success = false,
                    Message = $"Account code '{dto.Code}' already exists.",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            var account = new CoaAccount
            {
                Code = dto.Code,
                Name = dto.Name,
                AccountType = dto.AccountType,
                ParentAccountId = dto.ParentAccountId,
                CurrentBalance = 0,
                IsActive = dto.IsActive,
                SchoolRegistrationId = schoolId,
                CreatedBy = user,
                CreatedDate = DateTime.UtcNow
            };

            _context.CoaAccounts.Add(account);
            await _context.SaveChangesAsync();

            return new APIResponse<CoaAccountDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.Created,
                Message = "Chart of Accounts entry created successfully",
                Data = new CoaAccountDto
                {
                    Id = account.Id,
                    Code = account.Code,
                    Name = account.Name,
                    AccountType = account.AccountType,
                    ParentAccountId = account.ParentAccountId,
                    CurrentBalance = account.CurrentBalance,
                    IsActive = account.IsActive
                }
            };
        }

        public async Task<APIResponse<bool>> DeactivateAccountAsync(int schoolId, int accountId, string user)
        {
            var account = await _context.CoaAccounts
                .FirstOrDefaultAsync(a => a.SchoolRegistrationId == schoolId && a.Id == accountId && !a.IsDeleted);

            if (account == null)
            {
                return new APIResponse<bool> { Success = false, Message = "Account not found", StatusCode = HttpStatusCode.NotFound };
            }

            account.IsActive = false;
            account.UpdatedBy = user;
            account.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new APIResponse<bool>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Message = "Account deactivated",
                Data = true
            };
        }

        // --- Journal Entries ---
        public async Task<APIResponse<List<JournalEntryDto>>> GetJournalEntriesAsync(int schoolId, DateTime? fromDate, DateTime? toDate)
        {
            var query = _context.JournalEntries
                .Include(j => j.Lines)
                .ThenInclude(l => l.Account)
                .Where(j => j.SchoolRegistrationId == schoolId && !j.IsDeleted);

            if (fromDate.HasValue) query = query.Where(j => j.EntryDate >= fromDate.Value);
            if (toDate.HasValue) query = query.Where(j => j.EntryDate <= toDate.Value);

            var entries = await query.OrderByDescending(j => j.EntryDate).ToListAsync();

            var list = entries.Select(j => new JournalEntryDto
            {
                Id = j.Id,
                VoucherNo = j.VoucherNo,
                EntryDate = j.EntryDate,
                Narration = j.Narration,
                Reference = j.Reference,
                Source = j.Source,
                IsPosted = j.IsPosted,
                Lines = j.Lines.Select(l => new JournalEntryLineDto
                {
                    Id = l.Id,
                    AccountId = l.AccountId,
                    AccountName = l.Account.Name,
                    AccountCode = l.Account.Code,
                    DebitAmount = l.DebitAmount,
                    CreditAmount = l.CreditAmount,
                    Description = l.Description
                }).ToList()
            }).ToList();

            return new APIResponse<List<JournalEntryDto>>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = list
            };
        }

        public async Task<APIResponse<JournalEntryDto>> PostJournalEntryAsync(int schoolId, CreateJournalEntryDto dto, string user)
        {
            // Validation: Check double-entry integrity
            decimal totalDebit = dto.Lines.Sum(l => l.DebitAmount);
            decimal totalCredit = dto.Lines.Sum(l => l.CreditAmount);

            if (Math.Round(totalDebit, 2) != Math.Round(totalCredit, 2))
            {
                return new APIResponse<JournalEntryDto>
                {
                    Success = false,
                    Message = $"Double entry validation failed. Total Debits (₹{totalDebit}) must equal Total Credits (₹{totalCredit}).",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            // Generate Voucher Number
            var count = await _context.JournalEntries.IgnoreQueryFilters().CountAsync(j => j.SchoolRegistrationId == schoolId);
            string voucherNo = $"JV-{DateTime.UtcNow.Year}-{(count + 1).ToString().PadLeft(5, '0')}";

            var entry = new JournalEntry
            {
                VoucherNo = voucherNo,
                EntryDate = dto.EntryDate,
                Narration = dto.Narration,
                Reference = dto.Reference,
                Source = dto.Source,
                IsPosted = true,
                SchoolRegistrationId = schoolId,
                CreatedBy = user,
                CreatedDate = DateTime.UtcNow
            };

            foreach (var line in dto.Lines)
            {
                var account = await _context.CoaAccounts.FindAsync(line.AccountId);
                if (account == null || account.SchoolRegistrationId != schoolId || !account.IsActive)
                {
                    return new APIResponse<JournalEntryDto>
                    {
                        Success = false,
                        Message = $"Account with ID {line.AccountId} is invalid, inactive, or not found.",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                // Balance Update Logic:
                // Debit increases Asset & Expense, decreases Liability, Revenue, Equity
                // Credit increases Liability, Revenue, Equity, decreases Asset & Expense
                if (account.AccountType.Equals("Asset", StringComparison.OrdinalIgnoreCase) ||
                    account.AccountType.Equals("Expense", StringComparison.OrdinalIgnoreCase))
                {
                    account.CurrentBalance += (line.DebitAmount - line.CreditAmount);
                }
                else
                {
                    account.CurrentBalance += (line.CreditAmount - line.DebitAmount);
                }

                entry.Lines.Add(new JournalEntryLine
                {
                    AccountId = line.AccountId,
                    DebitAmount = line.DebitAmount,
                    CreditAmount = line.CreditAmount,
                    Description = line.Description,
                    SchoolRegistrationId = schoolId,
                    CreatedBy = user,
                    CreatedDate = DateTime.UtcNow
                });
            }

            _context.JournalEntries.Add(entry);
            await _context.SaveChangesAsync();

            return new APIResponse<JournalEntryDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.Created,
                Message = $"Journal Voucher {voucherNo} posted successfully",
                Data = new JournalEntryDto
                {
                    Id = entry.Id,
                    VoucherNo = entry.VoucherNo,
                    EntryDate = entry.EntryDate,
                    Narration = entry.Narration,
                    IsPosted = entry.IsPosted
                }
            };
        }

        public async Task<APIResponse<bool>> DeleteJournalEntryAsync(int schoolId, int entryId, string user)
        {
            var entry = await _context.JournalEntries
                .Include(j => j.Lines)
                .FirstOrDefaultAsync(j => j.SchoolRegistrationId == schoolId && j.Id == entryId && !j.IsDeleted);

            if (entry == null)
            {
                return new APIResponse<bool> { Success = false, Message = "Journal entry not found", StatusCode = HttpStatusCode.NotFound };
            }

            // Rollback account balances
            foreach (var line in entry.Lines)
            {
                var account = await _context.CoaAccounts.FindAsync(line.AccountId);
                if (account != null)
                {
                    if (account.AccountType.Equals("Asset", StringComparison.OrdinalIgnoreCase) ||
                        account.AccountType.Equals("Expense", StringComparison.OrdinalIgnoreCase))
                    {
                        account.CurrentBalance -= (line.DebitAmount - line.CreditAmount);
                    }
                    else
                    {
                        account.CurrentBalance -= (line.CreditAmount - line.DebitAmount);
                    }
                    account.UpdatedBy = user;
                    account.UpdatedDate = DateTime.UtcNow;
                }

                line.IsDeleted = true;
                line.UpdatedBy = user;
                line.UpdatedDate = DateTime.UtcNow;
            }

            entry.IsDeleted = true;
            entry.UpdatedBy = user;
            entry.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new APIResponse<bool>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Message = "Journal Entry deleted and balances rolled back",
                Data = true
            };
        }

        // --- Cash Bank Transactions & Reconciliation ---
        public async Task<APIResponse<List<CashBankTransactionDto>>> GetBankTransactionsAsync(int schoolId, int accountId, bool? reconciled)
        {
            var query = _context.CashBankTransactions
                .Include(t => t.Account)
                .Where(t => t.SchoolRegistrationId == schoolId && t.AccountId == accountId && !t.IsDeleted);

            if (reconciled.HasValue)
            {
                query = query.Where(t => t.IsReconciled == reconciled.Value);
            }

            var txns = await query.OrderByDescending(t => t.TransactionDate).ToListAsync();

            var list = txns.Select(t => new CashBankTransactionDto
            {
                Id = t.Id,
                JournalEntryId = t.JournalEntryId,
                AccountId = t.AccountId,
                AccountName = t.Account.Name,
                TransactionType = t.TransactionType,
                PaymentMode = t.PaymentMode,
                ReferenceNo = t.ReferenceNo,
                TransactionDate = t.TransactionDate,
                Amount = t.Amount,
                IsReconciled = t.IsReconciled,
                ReconciledDate = t.ReconciledDate
            }).ToList();

            return new APIResponse<List<CashBankTransactionDto>>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = list
            };
        }

        public async Task<APIResponse<bool>> ReconcileTransactionAsync(int schoolId, int txnId, DateTime reconciledDate, string user)
        {
            var txn = await _context.CashBankTransactions
                .FirstOrDefaultAsync(t => t.SchoolRegistrationId == schoolId && t.Id == txnId && !t.IsDeleted);

            if (txn == null)
            {
                return new APIResponse<bool> { Success = false, Message = "Transaction not found", StatusCode = HttpStatusCode.NotFound };
            }

            txn.IsReconciled = true;
            txn.ReconciledDate = reconciledDate;
            txn.UpdatedBy = user;
            txn.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new APIResponse<bool>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Message = "Transaction reconciled successfully",
                Data = true
            };
        }

        // --- Budgets ---
        public async Task<APIResponse<List<BudgetPlanDto>>> GetBudgetPlansAsync(int schoolId, string financialYear)
        {
            var plans = await _context.BudgetPlans
                .Include(b => b.Department)
                .Include(b => b.Account)
                .Where(b => b.SchoolRegistrationId == schoolId && b.FinancialYear == financialYear && !b.IsDeleted)
                .ToListAsync();

            var list = plans.Select(b => new BudgetPlanDto
            {
                Id = b.Id,
                FinancialYear = b.FinancialYear,
                DepartmentId = b.DepartmentId,
                DepartmentName = b.Department.Name,
                AccountId = b.AccountId,
                AccountName = b.Account.Name,
                AllocatedAmount = b.AllocatedAmount,
                UtilizedAmount = b.UtilizedAmount
            }).ToList();

            return new APIResponse<List<BudgetPlanDto>>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = list
            };
        }

        public async Task<APIResponse<BudgetPlanDto>> CreateBudgetPlanAsync(int schoolId, CreateBudgetPlanDto dto, string user)
        {
            var plan = new BudgetPlan
            {
                FinancialYear = dto.FinancialYear,
                DepartmentId = dto.DepartmentId,
                AccountId = dto.AccountId,
                AllocatedAmount = dto.AllocatedAmount,
                UtilizedAmount = 0,
                SchoolRegistrationId = schoolId,
                CreatedBy = user,
                CreatedDate = DateTime.UtcNow
            };

            _context.BudgetPlans.Add(plan);
            await _context.SaveChangesAsync();

            return new APIResponse<BudgetPlanDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.Created,
                Message = "Budget Allocation saved",
                Data = new BudgetPlanDto
                {
                    Id = plan.Id,
                    FinancialYear = plan.FinancialYear,
                    DepartmentId = plan.DepartmentId,
                    AccountId = plan.AccountId,
                    AllocatedAmount = plan.AllocatedAmount
                }
            };
        }

        // --- Financial Reports ---
        public async Task<APIResponse<List<TrialBalanceRowDto>>> GetTrialBalanceAsync(int schoolId, DateTime date)
        {
            var accounts = await _context.CoaAccounts
                .Where(a => a.SchoolRegistrationId == schoolId && !a.IsDeleted && a.IsActive)
                .ToListAsync();

            var list = new List<TrialBalanceRowDto>();

            foreach (var acc in accounts)
            {
                decimal debits = acc.AccountType.Equals("Asset", StringComparison.OrdinalIgnoreCase) ||
                                 acc.AccountType.Equals("Expense", StringComparison.OrdinalIgnoreCase)
                    ? acc.CurrentBalance : 0;

                decimal credits = acc.AccountType.Equals("Liability", StringComparison.OrdinalIgnoreCase) ||
                                  acc.AccountType.Equals("Equity", StringComparison.OrdinalIgnoreCase) ||
                                  acc.AccountType.Equals("Revenue", StringComparison.OrdinalIgnoreCase)
                    ? acc.CurrentBalance : 0;

                // Only include accounts with non-zero balances
                if (acc.CurrentBalance != 0)
                {
                    list.Add(new TrialBalanceRowDto
                    {
                        AccountCode = acc.Code,
                        AccountName = acc.Name,
                        AccountType = acc.AccountType,
                        DebitBalance = debits >= 0 ? debits : 0,
                        CreditBalance = credits >= 0 ? credits : 0
                    });
                }
            }

            return new APIResponse<List<TrialBalanceRowDto>>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = list
            };
        }

        public async Task<APIResponse<LedgerStatementDto>> GetLedgerStatementAsync(int schoolId, int accountId, DateTime fromDate, DateTime toDate)
        {
            var account = await _context.CoaAccounts
                .FirstOrDefaultAsync(a => a.SchoolRegistrationId == schoolId && a.Id == accountId && !a.IsDeleted);

            if (account == null)
            {
                return new APIResponse<LedgerStatementDto> { Success = false, Message = "Account not found", StatusCode = HttpStatusCode.NotFound };
            }

            // Query lines posted to this account prior to fromDate for Opening Balance
            var prevLines = await _context.JournalEntryLines
                .Include(l => l.JournalEntry)
                .Where(l => l.SchoolRegistrationId == schoolId && l.AccountId == accountId && l.JournalEntry.EntryDate < fromDate && !l.IsDeleted && l.JournalEntry.IsPosted)
                .ToListAsync();

            decimal openingBalance = 0;
            if (account.AccountType.Equals("Asset", StringComparison.OrdinalIgnoreCase) ||
                account.AccountType.Equals("Expense", StringComparison.OrdinalIgnoreCase))
            {
                openingBalance = prevLines.Sum(l => l.DebitAmount - l.CreditAmount);
            }
            else
            {
                openingBalance = prevLines.Sum(l => l.CreditAmount - l.DebitAmount);
            }

            // Query lines in date range
            var currentLines = await _context.JournalEntryLines
                .Include(l => l.JournalEntry)
                .Where(l => l.SchoolRegistrationId == schoolId && l.AccountId == accountId && l.JournalEntry.EntryDate >= fromDate && l.JournalEntry.EntryDate <= toDate && !l.IsDeleted && l.JournalEntry.IsPosted)
                .OrderBy(l => l.JournalEntry.EntryDate)
                .ToListAsync();

            var statementLines = new List<LedgerStatementLineDto>();
            decimal running = openingBalance;

            foreach (var line in currentLines)
            {
                if (account.AccountType.Equals("Asset", StringComparison.OrdinalIgnoreCase) ||
                    account.AccountType.Equals("Expense", StringComparison.OrdinalIgnoreCase))
                {
                    running += (line.DebitAmount - line.CreditAmount);
                }
                else
                {
                    running += (line.CreditAmount - line.DebitAmount);
                }

                statementLines.Add(new LedgerStatementLineDto
                {
                    Date = line.JournalEntry.EntryDate,
                    VoucherNo = line.JournalEntry.VoucherNo,
                    Narration = line.JournalEntry.Narration ?? "",
                    Reference = line.JournalEntry.Reference ?? "",
                    DebitAmount = line.DebitAmount,
                    CreditAmount = line.CreditAmount,
                    RunningBalance = running
                });
            }

            return new APIResponse<LedgerStatementDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = new LedgerStatementDto
                {
                    AccountName = account.Name,
                    AccountCode = account.Code,
                    OpeningBalance = openingBalance,
                    ClosingBalance = running,
                    Statements = statementLines
                }
            };
        }
    }
}
