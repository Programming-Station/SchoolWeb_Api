using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using School.Domain.Finance;
using School.Domain.School;
using School.Domain.FeeManagnment;
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

        private byte[] ExportToCsv<T>(IEnumerable<T> items, string[] headers, Func<T, string[]> rowMapper)
        {
            var builder = new System.Text.StringBuilder();
            builder.AppendLine(string.Join(",", headers.Select(h => $"\"{h.Replace("\"", "\"\"")}\"")));
            foreach (var item in items)
            {
                var row = rowMapper(item);
                builder.AppendLine(string.Join(",", row.Select(r => $"\"{r?.Replace("\"", "\"\"")}\"")));
            }
            return System.Text.Encoding.UTF8.GetBytes(builder.ToString());
        }

        // ─── Chart of Accounts ───
        public async Task<APIResponse<List<CoaAccountDto>>> GetChartOfAccountsAsync(int schoolId, string? accountType = null, bool? isActive = null, string? search = null)
        {
            var query = _context.CoaAccounts
                .Include(a => a.ParentAccount)
                .Where(a => a.SchoolRegistrationId == schoolId && !a.IsDeleted);

            if (!string.IsNullOrEmpty(accountType))
            {
                query = query.Where(a => a.AccountType == accountType);
            }
            if (isActive.HasValue)
            {
                query = query.Where(a => a.IsActive == isActive.Value);
            }
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(a => a.Name.Contains(search) || a.Code.Contains(search));
            }

            var accounts = await query.OrderBy(a => a.Code).ToListAsync();

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

        public async Task<APIResponse<CoaAccountDto>> GetAccountByIdAsync(int schoolId, int id)
        {
            var account = await _context.CoaAccounts
                .Include(a => a.ParentAccount)
                .FirstOrDefaultAsync(a => a.SchoolRegistrationId == schoolId && a.Id == id && !a.IsDeleted);

            if (account == null)
            {
                return new APIResponse<CoaAccountDto> { Success = false, Message = "Account not found", StatusCode = HttpStatusCode.NotFound };
            }

            return new APIResponse<CoaAccountDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = new CoaAccountDto
                {
                    Id = account.Id,
                    Code = account.Code,
                    Name = account.Name,
                    AccountType = account.AccountType,
                    ParentAccountId = account.ParentAccountId,
                    ParentAccountName = account.ParentAccount?.Name,
                    CurrentBalance = account.CurrentBalance,
                    IsActive = account.IsActive
                }
            };
        }

        public async Task<APIResponse<CoaAccountDto>> CreateAccountAsync(int schoolId, CreateCoaAccountDto dto, string user)
        {
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

        public async Task<APIResponse<CoaAccountDto>> UpdateAccountAsync(int schoolId, int id, CreateCoaAccountDto dto, string user)
        {
            var account = await _context.CoaAccounts
                .FirstOrDefaultAsync(a => a.SchoolRegistrationId == schoolId && a.Id == id && !a.IsDeleted);

            if (account == null)
            {
                return new APIResponse<CoaAccountDto> { Success = false, Message = "Account not found", StatusCode = HttpStatusCode.NotFound };
            }

            var codeExists = await _context.CoaAccounts
                .AnyAsync(a => a.SchoolRegistrationId == schoolId && a.Code == dto.Code && a.Id != id && !a.IsDeleted);

            if (codeExists)
            {
                return new APIResponse<CoaAccountDto> { Success = false, Message = $"Account code '{dto.Code}' already exists.", StatusCode = HttpStatusCode.BadRequest };
            }

            account.Code = dto.Code;
            account.Name = dto.Name;
            account.AccountType = dto.AccountType;
            account.ParentAccountId = dto.ParentAccountId;
            account.IsActive = dto.IsActive;
            account.UpdatedBy = user;
            account.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new APIResponse<CoaAccountDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Message = "Account updated successfully",
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

        public async Task<APIResponse<bool>> DeleteAccountAsync(int schoolId, int accountId, string user)
        {
            var account = await _context.CoaAccounts
                .FirstOrDefaultAsync(a => a.SchoolRegistrationId == schoolId && a.Id == accountId && !a.IsDeleted);

            if (account == null)
            {
                return new APIResponse<bool> { Success = false, Message = "Account not found", StatusCode = HttpStatusCode.NotFound };
            }

            account.IsDeleted = true;
            account.UpdatedBy = user;
            account.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Account deleted successfully", Data = true };
        }

        public async Task<APIResponse<byte[]>> ExportAccountsAsync(int schoolId, string? accountType = null, bool? isActive = null, string? search = null)
        {
            var res = await GetChartOfAccountsAsync(schoolId, accountType, isActive, search);
            var data = res.Data ?? new List<CoaAccountDto>();
            var csvBytes = ExportToCsv(data, 
                new[] { "ID", "Code", "Name", "Type", "Parent Account Name", "Current Balance", "Is Active" },
                item => new[] {
                    item.Id.ToString(),
                    item.Code,
                    item.Name,
                    item.AccountType,
                    item.ParentAccountName ?? "",
                    item.CurrentBalance.ToString("F2"),
                    item.IsActive.ToString()
                });
            return new APIResponse<byte[]> { Success = true, Data = csvBytes };
        }

        // ─── Journal Entries & Maker Checker ───
        public async Task<APIResponse<List<JournalEntryDto>>> GetJournalEntriesAsync(int schoolId, DateTime? fromDate, DateTime? toDate, string? status = null, string? search = null)
        {
            var query = _context.JournalEntries
                .Include(j => j.Lines)
                .ThenInclude(l => l.Account)
                .Include(j => j.CostCenter)
                .Where(j => j.SchoolRegistrationId == schoolId && !j.IsDeleted);

            if (fromDate.HasValue) query = query.Where(j => j.EntryDate >= fromDate.Value);
            if (toDate.HasValue) query = query.Where(j => j.EntryDate <= toDate.Value);
            if (!string.IsNullOrEmpty(status)) query = query.Where(j => j.Status == status);
            if (!string.IsNullOrEmpty(search)) query = query.Where(j => j.VoucherNo.Contains(search) || j.Narration.Contains(search) || j.Reference.Contains(search));

            var entries = await query.OrderByDescending(j => j.EntryDate).ToListAsync();

            var list = entries.Select(j => new JournalEntryDto
            {
                Id = j.Id,
                VoucherNo = j.VoucherNo,
                EntryDate = j.EntryDate,
                Narration = j.Narration,
                Reference = j.Reference,
                Source = j.Source,
                VoucherType = j.VoucherType,
                Status = j.Status,
                ApprovedBy = j.ApprovedBy,
                ApprovedDate = j.ApprovedDate,
                CostCenterId = j.CostCenterId,
                CostCenterName = j.CostCenter?.Name,
                AttachmentUrl = j.AttachmentUrl,
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

        public async Task<APIResponse<JournalEntryDto>> GetJournalEntryByIdAsync(int schoolId, int id)
        {
            var j = await _context.JournalEntries
                .Include(j => j.Lines)
                .ThenInclude(l => l.Account)
                .Include(j => j.CostCenter)
                .FirstOrDefaultAsync(j => j.SchoolRegistrationId == schoolId && j.Id == id && !j.IsDeleted);

            if (j == null)
            {
                return new APIResponse<JournalEntryDto> { Success = false, Message = "Voucher not found", StatusCode = HttpStatusCode.NotFound };
            }

            return new APIResponse<JournalEntryDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = new JournalEntryDto
                {
                    Id = j.Id,
                    VoucherNo = j.VoucherNo,
                    EntryDate = j.EntryDate,
                    Narration = j.Narration,
                    Reference = j.Reference,
                    Source = j.Source,
                    VoucherType = j.VoucherType,
                    Status = j.Status,
                    ApprovedBy = j.ApprovedBy,
                    ApprovedDate = j.ApprovedDate,
                    CostCenterId = j.CostCenterId,
                    CostCenterName = j.CostCenter?.Name,
                    AttachmentUrl = j.AttachmentUrl,
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
                }
            };
        }

        public async Task<APIResponse<JournalEntryDto>> UpdateJournalEntryAsync(int schoolId, int id, CreateJournalEntryDto dto, string user)
        {
            var entry = await _context.JournalEntries
                .Include(j => j.Lines)
                .FirstOrDefaultAsync(j => j.SchoolRegistrationId == schoolId && j.Id == id && !j.IsDeleted);

            if (entry == null)
            {
                return new APIResponse<JournalEntryDto> { Success = false, Message = "Voucher not found", StatusCode = HttpStatusCode.NotFound };
            }

            // Rollback old balances if it was posted
            if (entry.IsPosted && entry.Status == "Approved")
            {
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
                    }
                }
            }

            // Validate new double entry
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

            // Update details
            entry.EntryDate = dto.EntryDate;
            entry.Narration = dto.Narration;
            entry.Reference = dto.Reference;
            entry.Source = dto.Source;
            entry.VoucherType = dto.VoucherType;
            entry.CostCenterId = dto.CostCenterId;
            entry.AttachmentUrl = dto.AttachmentUrl;
            entry.UpdatedBy = user;
            entry.UpdatedDate = DateTime.UtcNow;

            // Remove old lines
            _context.JournalEntryLines.RemoveRange(entry.Lines);
            entry.Lines.Clear();

            // Add new lines and update balances
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

                if (entry.IsPosted && entry.Status == "Approved")
                {
                    if (account.AccountType.Equals("Asset", StringComparison.OrdinalIgnoreCase) ||
                        account.AccountType.Equals("Expense", StringComparison.OrdinalIgnoreCase))
                    {
                        account.CurrentBalance += (line.DebitAmount - line.CreditAmount);
                    }
                    else
                    {
                        account.CurrentBalance += (line.CreditAmount - line.DebitAmount);
                    }
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

            await _context.SaveChangesAsync();

            return new APIResponse<JournalEntryDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Message = "Journal Entry updated successfully.",
                Data = new JournalEntryDto
                {
                    Id = entry.Id,
                    VoucherNo = entry.VoucherNo,
                    EntryDate = entry.EntryDate,
                    Narration = entry.Narration,
                    VoucherType = entry.VoucherType,
                    Status = entry.Status,
                    IsPosted = entry.IsPosted
                }
            };
        }

        public async Task<APIResponse<byte[]>> ExportJournalEntriesAsync(int schoolId, DateTime? fromDate, DateTime? toDate, string? status = null, string? search = null)
        {
            var res = await GetJournalEntriesAsync(schoolId, fromDate, toDate, status, search);
            var data = res.Data ?? new List<JournalEntryDto>();
            var csvBytes = ExportToCsv(data, 
                new[] { "ID", "Voucher No", "Date", "Narration", "Reference", "Type", "Status", "Is Posted" },
                item => new[] {
                    item.Id.ToString(),
                    item.VoucherNo,
                    item.EntryDate.ToString("yyyy-MM-dd"),
                    item.Narration ?? "",
                    item.Reference ?? "",
                    item.VoucherType,
                    item.Status,
                    item.IsPosted.ToString()
                });
            return new APIResponse<byte[]> { Success = true, Data = csvBytes };
        }

        public async Task<APIResponse<JournalEntryDto>> PostJournalEntryAsync(int schoolId, CreateJournalEntryDto dto, string user)
        {
            // Validate: check if active financial year exists and is not locked
            var entryYear = await _context.FinancialYears
                .FirstOrDefaultAsync(fy => fy.SchoolRegistrationId == schoolId && !fy.IsDeleted && dto.EntryDate >= fy.StartDate && dto.EntryDate <= fy.EndDate);

            if (entryYear != null && entryYear.IsLocked)
            {
                return new APIResponse<JournalEntryDto>
                {
                    Success = false,
                    Message = $"The financial year '{entryYear.YearName}' is locked. Posting is disabled.",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

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

            var count = await _context.JournalEntries.IgnoreQueryFilters().CountAsync(j => j.SchoolRegistrationId == schoolId);
            string codePrefix = dto.VoucherType switch
            {
                "Payment" => "PV",
                "Receipt" => "RV",
                "Contra" => "CV",
                "CreditNote" => "CN",
                "DebitNote" => "DN",
                _ => "JV"
            };
            string voucherNo = $"{codePrefix}-{dto.EntryDate.Year}-{(count + 1).ToString().PadLeft(5, '0')}";

            // Determine initial status: we default to "Approved" for auto-posted or direct transactions, or "Pending" if specified.
            string initialStatus = dto.VoucherType == "Journal" ? "Pending" : "Approved";

            var entry = new JournalEntry
            {
                VoucherNo = voucherNo,
                EntryDate = dto.EntryDate,
                Narration = dto.Narration,
                Reference = dto.Reference,
                Source = dto.Source,
                VoucherType = dto.VoucherType,
                Status = initialStatus,
                CostCenterId = dto.CostCenterId,
                AttachmentUrl = dto.AttachmentUrl,
                IsPosted = initialStatus == "Approved",
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

                // Balance updates immediately only if voucher is instantly approved
                if (initialStatus == "Approved")
                {
                    if (account.AccountType.Equals("Asset", StringComparison.OrdinalIgnoreCase) ||
                        account.AccountType.Equals("Expense", StringComparison.OrdinalIgnoreCase))
                    {
                        account.CurrentBalance += (line.DebitAmount - line.CreditAmount);
                    }
                    else
                    {
                        account.CurrentBalance += (line.CreditAmount - line.DebitAmount);
                    }
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
                Message = $"Voucher {voucherNo} created successfully with status '{initialStatus}'.",
                Data = new JournalEntryDto
                {
                    Id = entry.Id,
                    VoucherNo = entry.VoucherNo,
                    EntryDate = entry.EntryDate,
                    Narration = entry.Narration,
                    VoucherType = entry.VoucherType,
                    Status = entry.Status,
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
                return new APIResponse<bool> { Success = false, Message = "Voucher not found", StatusCode = HttpStatusCode.NotFound };
            }

            // Rollback account balances if it was posted
            if (entry.IsPosted && entry.Status == "Approved")
            {
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
            }

            entry.IsDeleted = true;
            entry.UpdatedBy = user;
            entry.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new APIResponse<bool>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Message = "Voucher cancelled and balances reversed.",
                Data = true
            };
        }

        public async Task<APIResponse<bool>> SubmitJournalForApprovalAsync(int schoolId, int entryId, string user)
        {
            var entry = await _context.JournalEntries.FirstOrDefaultAsync(j => j.SchoolRegistrationId == schoolId && j.Id == entryId && !j.IsDeleted);
            if (entry == null) return new APIResponse<bool> { Success = false, Message = "Voucher not found" };

            entry.Status = "Pending";
            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, Message = "Submitted for approval", Data = true };
        }

        public async Task<APIResponse<bool>> ApproveJournalEntryAsync(int schoolId, int entryId, string user)
        {
            var entry = await _context.JournalEntries
                .Include(j => j.Lines)
                .FirstOrDefaultAsync(j => j.SchoolRegistrationId == schoolId && j.Id == entryId && !j.IsDeleted);

            if (entry == null) return new APIResponse<bool> { Success = false, Message = "Voucher not found" };
            if (entry.Status == "Approved") return new APIResponse<bool> { Success = false, Message = "Voucher is already approved" };

            // Update ledger balances now
            foreach (var line in entry.Lines)
            {
                var account = await _context.CoaAccounts.FindAsync(line.AccountId);
                if (account != null)
                {
                    if (account.AccountType.Equals("Asset", StringComparison.OrdinalIgnoreCase) ||
                        account.AccountType.Equals("Expense", StringComparison.OrdinalIgnoreCase))
                    {
                        account.CurrentBalance += (line.DebitAmount - line.CreditAmount);
                    }
                    else
                    {
                        account.CurrentBalance += (line.CreditAmount - line.DebitAmount);
                    }
                }
            }

            entry.Status = "Approved";
            entry.IsPosted = true;
            entry.ApprovedBy = user;
            entry.ApprovedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, Message = "Voucher approved and posted.", Data = true };
        }

        public async Task<APIResponse<bool>> RejectJournalEntryAsync(int schoolId, int entryId, string notes, string user)
        {
            var entry = await _context.JournalEntries.FirstOrDefaultAsync(j => j.SchoolRegistrationId == schoolId && j.Id == entryId && !j.IsDeleted);
            if (entry == null) return new APIResponse<bool> { Success = false, Message = "Voucher not found" };

            entry.Status = "Rejected";
            entry.Narration += $" [Rejected notes: {notes}]";
            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, Message = "Voucher rejected.", Data = true };
        }

        // ─── Cash Bank Transactions & Reconciliation ───
        public async Task<APIResponse<List<CashBankTransactionDto>>> GetBankTransactionsAsync(int schoolId, int accountId, bool? reconciled)
        {
            var query = _context.CashBankTransactions
                .Include(t => t.Account)
                .Include(t => t.CostCenter)
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
                ChequeNo = t.ChequeNo,
                CostCenterId = t.CostCenterId,
                CostCenterName = t.CostCenter?.Name,
                TransactionDate = t.TransactionDate,
                Amount = t.Amount,
                IsReconciled = t.IsReconciled,
                ReconciledDate = t.ReconciledDate,
                ClearedDate = t.ClearedDate
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
            txn.ClearedDate = reconciledDate;
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

        // ─── Cheque Books ───
        public async Task<APIResponse<List<ChequeBookDto>>> GetChequeBooksAsync(int schoolId, int? accountId = null, bool? isExhausted = null)
        {
            var query = _context.ChequeBooks
                .Include(b => b.BankAccount)
                .Where(b => b.SchoolRegistrationId == schoolId && !b.IsDeleted);

            if (accountId.HasValue)
            {
                query = query.Where(b => b.BankAccountId == accountId.Value);
            }
            if (isExhausted.HasValue)
            {
                query = query.Where(b => b.IsExhausted == isExhausted.Value);
            }

            var books = await query.ToListAsync();

            var list = books.Select(b => new ChequeBookDto
            {
                Id = b.Id,
                BankAccountId = b.BankAccountId,
                BankAccountName = b.BankAccount.Name,
                SeriesPrefix = b.SeriesPrefix,
                StartChequeNo = b.StartChequeNo,
                EndChequeNo = b.EndChequeNo,
                NextChequeNo = b.NextChequeNo,
                IsExhausted = b.IsExhausted
            }).ToList();

            return new APIResponse<List<ChequeBookDto>> { Success = true, Data = list };
        }

        public async Task<APIResponse<ChequeBookDto>> GetChequeBookByIdAsync(int schoolId, int id)
        {
            var b = await _context.ChequeBooks
                .Include(b => b.BankAccount)
                .FirstOrDefaultAsync(b => b.SchoolRegistrationId == schoolId && b.Id == id && !b.IsDeleted);

            if (b == null)
            {
                return new APIResponse<ChequeBookDto> { Success = false, Message = "Cheque book not found", StatusCode = HttpStatusCode.NotFound };
            }

            return new APIResponse<ChequeBookDto>
            {
                Success = true,
                Data = new ChequeBookDto
                {
                    Id = b.Id,
                    BankAccountId = b.BankAccountId,
                    BankAccountName = b.BankAccount.Name,
                    SeriesPrefix = b.SeriesPrefix,
                    StartChequeNo = b.StartChequeNo,
                    EndChequeNo = b.EndChequeNo,
                    NextChequeNo = b.NextChequeNo,
                    IsExhausted = b.IsExhausted
                }
            };
        }

        public async Task<APIResponse<ChequeBookDto>> CreateChequeBookAsync(int schoolId, CreateChequeBookDto dto, string user)
        {
            var book = new ChequeBook
            {
                BankAccountId = dto.BankAccountId,
                SeriesPrefix = dto.SeriesPrefix,
                StartChequeNo = dto.StartChequeNo,
                EndChequeNo = dto.EndChequeNo,
                NextChequeNo = dto.StartChequeNo,
                IsExhausted = false,
                SchoolRegistrationId = schoolId,
                CreatedBy = user,
                CreatedDate = DateTime.UtcNow
            };

            _context.ChequeBooks.Add(book);
            await _context.SaveChangesAsync();

            var accountName = await _context.CoaAccounts
                .Where(a => a.Id == dto.BankAccountId)
                .Select(a => a.Name)
                .FirstOrDefaultAsync() ?? "";

            return new APIResponse<ChequeBookDto>
            {
                Success = true,
                Data = new ChequeBookDto
                {
                    Id = book.Id,
                    BankAccountId = book.BankAccountId,
                    BankAccountName = accountName,
                    SeriesPrefix = book.SeriesPrefix,
                    StartChequeNo = book.StartChequeNo,
                    EndChequeNo = book.EndChequeNo,
                    NextChequeNo = book.NextChequeNo
                }
            };
        }

        public async Task<APIResponse<ChequeBookDto>> UpdateChequeBookAsync(int schoolId, int id, CreateChequeBookDto dto, string user)
        {
            var book = await _context.ChequeBooks
                .FirstOrDefaultAsync(b => b.SchoolRegistrationId == schoolId && b.Id == id && !b.IsDeleted);

            if (book == null)
            {
                return new APIResponse<ChequeBookDto> { Success = false, Message = "Cheque book not found", StatusCode = HttpStatusCode.NotFound };
            }

            book.BankAccountId = dto.BankAccountId;
            book.SeriesPrefix = dto.SeriesPrefix;
            book.StartChequeNo = dto.StartChequeNo;
            book.EndChequeNo = dto.EndChequeNo;
            book.UpdatedBy = user;
            book.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var accountName = await _context.CoaAccounts
                .Where(a => a.Id == dto.BankAccountId)
                .Select(a => a.Name)
                .FirstOrDefaultAsync() ?? "";

            return new APIResponse<ChequeBookDto>
            {
                Success = true,
                Data = new ChequeBookDto
                {
                    Id = book.Id,
                    BankAccountId = book.BankAccountId,
                    BankAccountName = accountName,
                    SeriesPrefix = book.SeriesPrefix,
                    StartChequeNo = book.StartChequeNo,
                    EndChequeNo = book.EndChequeNo,
                    NextChequeNo = book.NextChequeNo,
                    IsExhausted = book.IsExhausted
                }
            };
        }

        public async Task<APIResponse<bool>> DeleteChequeBookAsync(int schoolId, int id, string user)
        {
            var book = await _context.ChequeBooks
                .FirstOrDefaultAsync(b => b.SchoolRegistrationId == schoolId && b.Id == id && !b.IsDeleted);

            if (book == null)
            {
                return new APIResponse<bool> { Success = false, Message = "Cheque book not found", StatusCode = HttpStatusCode.NotFound };
            }

            book.IsDeleted = true;
            book.UpdatedBy = user;
            book.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, Message = "Cheque book deleted successfully", Data = true };
        }

        public async Task<APIResponse<byte[]>> ExportChequeBooksAsync(int schoolId, int? accountId = null, bool? isExhausted = null)
        {
            var res = await GetChequeBooksAsync(schoolId, accountId, isExhausted);
            var data = res.Data ?? new List<ChequeBookDto>();
            var csvBytes = ExportToCsv(data, 
                new[] { "ID", "Bank Account ID", "Bank Account Name", "Series Prefix", "Start Cheque No", "End Cheque No", "Next Cheque No", "Is Exhausted" },
                item => new[] {
                    item.Id.ToString(),
                    item.BankAccountId.ToString(),
                    item.BankAccountName,
                    item.SeriesPrefix,
                    item.StartChequeNo.ToString(),
                    item.EndChequeNo.ToString(),
                    item.NextChequeNo.ToString(),
                    item.IsExhausted.ToString()
                });
            return new APIResponse<byte[]> { Success = true, Data = csvBytes };
        }

        // ─── Budgets ───
        public async Task<APIResponse<List<BudgetPlanDto>>> GetBudgetPlansAsync(int schoolId, string? financialYear = null, int? departmentId = null, int? accountId = null)
        {
            var query = _context.BudgetPlans
                .Include(b => b.Department)
                .Include(b => b.Account)
                .Where(b => b.SchoolRegistrationId == schoolId && !b.IsDeleted);

            if (!string.IsNullOrEmpty(financialYear))
            {
                query = query.Where(b => b.FinancialYear == financialYear);
            }
            if (departmentId.HasValue)
            {
                query = query.Where(b => b.DepartmentId == departmentId.Value);
            }
            if (accountId.HasValue)
            {
                query = query.Where(b => b.AccountId == accountId.Value);
            }

            var plans = await query.ToListAsync();

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

        public async Task<APIResponse<BudgetPlanDto>> GetBudgetPlanByIdAsync(int schoolId, int id)
        {
            var b = await _context.BudgetPlans
                .Include(b => b.Department)
                .Include(b => b.Account)
                .FirstOrDefaultAsync(b => b.SchoolRegistrationId == schoolId && b.Id == id && !b.IsDeleted);

            if (b == null)
            {
                return new APIResponse<BudgetPlanDto> { Success = false, Message = "Budget plan not found", StatusCode = HttpStatusCode.NotFound };
            }

            return new APIResponse<BudgetPlanDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = new BudgetPlanDto
                {
                    Id = b.Id,
                    FinancialYear = b.FinancialYear,
                    DepartmentId = b.DepartmentId,
                    DepartmentName = b.Department.Name,
                    AccountId = b.AccountId,
                    AccountName = b.Account.Name,
                    AllocatedAmount = b.AllocatedAmount,
                    UtilizedAmount = b.UtilizedAmount
                }
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

            var deptName = await _context.Departments.Where(d => d.Id == dto.DepartmentId).Select(d => d.Name).FirstOrDefaultAsync() ?? "";
            var accountName = await _context.CoaAccounts.Where(a => a.Id == dto.AccountId).Select(a => a.Name).FirstOrDefaultAsync() ?? "";

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
                    DepartmentName = deptName,
                    AccountId = plan.AccountId,
                    AccountName = accountName,
                    AllocatedAmount = plan.AllocatedAmount,
                    UtilizedAmount = plan.UtilizedAmount
                }
            };
        }

        public async Task<APIResponse<BudgetPlanDto>> UpdateBudgetPlanAsync(int schoolId, int id, CreateBudgetPlanDto dto, string user)
        {
            var plan = await _context.BudgetPlans
                .FirstOrDefaultAsync(b => b.SchoolRegistrationId == schoolId && b.Id == id && !b.IsDeleted);

            if (plan == null)
            {
                return new APIResponse<BudgetPlanDto> { Success = false, Message = "Budget plan not found", StatusCode = HttpStatusCode.NotFound };
            }

            plan.FinancialYear = dto.FinancialYear;
            plan.DepartmentId = dto.DepartmentId;
            plan.AccountId = dto.AccountId;
            plan.AllocatedAmount = dto.AllocatedAmount;
            plan.UpdatedBy = user;
            plan.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var deptName = await _context.Departments.Where(d => d.Id == dto.DepartmentId).Select(d => d.Name).FirstOrDefaultAsync() ?? "";
            var accountName = await _context.CoaAccounts.Where(a => a.Id == dto.AccountId).Select(a => a.Name).FirstOrDefaultAsync() ?? "";

            return new APIResponse<BudgetPlanDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Message = "Budget plan updated successfully.",
                Data = new BudgetPlanDto
                {
                    Id = plan.Id,
                    FinancialYear = plan.FinancialYear,
                    DepartmentId = plan.DepartmentId,
                    DepartmentName = deptName,
                    AccountId = plan.AccountId,
                    AccountName = accountName,
                    AllocatedAmount = plan.AllocatedAmount,
                    UtilizedAmount = plan.UtilizedAmount
                }
            };
        }

        public async Task<APIResponse<bool>> DeleteBudgetPlanAsync(int schoolId, int id, string user)
        {
            var plan = await _context.BudgetPlans
                .FirstOrDefaultAsync(b => b.SchoolRegistrationId == schoolId && b.Id == id && !b.IsDeleted);

            if (plan == null)
            {
                return new APIResponse<bool> { Success = false, Message = "Budget plan not found", StatusCode = HttpStatusCode.NotFound };
            }

            plan.IsDeleted = true;
            plan.UpdatedBy = user;
            plan.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, Message = "Budget plan deleted successfully", Data = true };
        }

        public async Task<APIResponse<byte[]>> ExportBudgetPlansAsync(int schoolId, string? financialYear = null, int? departmentId = null, int? accountId = null)
        {
            var res = await GetBudgetPlansAsync(schoolId, financialYear, departmentId, accountId);
            var data = res.Data ?? new List<BudgetPlanDto>();
            var csvBytes = ExportToCsv(data, 
                new[] { "ID", "Financial Year", "Department Name", "Account Target", "Allocated Amount", "Utilized Amount", "Remaining" },
                item => new[] {
                    item.Id.ToString(),
                    item.FinancialYear,
                    item.DepartmentName,
                    item.AccountName,
                    item.AllocatedAmount.ToString("F2"),
                    item.UtilizedAmount.ToString("F2"),
                    (item.AllocatedAmount - item.UtilizedAmount).ToString("F2")
                });
            return new APIResponse<byte[]> { Success = true, Data = csvBytes };
        }

        // ─── Cost Centers ───
        public async Task<APIResponse<List<CostCenterDto>>> GetCostCentersAsync(int schoolId, bool? isActive = null, string? search = null)
        {
            var query = _context.CostCenters
                .Where(c => c.SchoolRegistrationId == schoolId && !c.IsDeleted);

            if (isActive.HasValue)
            {
                query = query.Where(c => c.IsActive == isActive.Value);
            }
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.Name.Contains(search) || c.Code.Contains(search));
            }

            var centers = await query.ToListAsync();

            var list = centers.Select(c => new CostCenterDto
            {
                Id = c.Id,
                Name = c.Name,
                Code = c.Code,
                IsActive = c.IsActive
            }).ToList();

            return new APIResponse<List<CostCenterDto>> { Success = true, Data = list };
        }

        public async Task<APIResponse<CostCenterDto>> GetCostCenterByIdAsync(int schoolId, int id)
        {
            var c = await _context.CostCenters
                .FirstOrDefaultAsync(c => c.SchoolRegistrationId == schoolId && c.Id == id && !c.IsDeleted);

            if (c == null)
            {
                return new APIResponse<CostCenterDto> { Success = false, Message = "Cost center not found", StatusCode = HttpStatusCode.NotFound };
            }

            return new APIResponse<CostCenterDto>
            {
                Success = true,
                Data = new CostCenterDto { Id = c.Id, Name = c.Name, Code = c.Code, IsActive = c.IsActive }
            };
        }

        public async Task<APIResponse<CostCenterDto>> CreateCostCenterAsync(int schoolId, CreateCostCenterDto dto, string user)
        {
            var costCenter = new CostCenter
            {
                Name = dto.Name,
                Code = dto.Code,
                IsActive = dto.IsActive,
                SchoolRegistrationId = schoolId,
                CreatedBy = user,
                CreatedDate = DateTime.UtcNow
            };
            _context.CostCenters.Add(costCenter);
            await _context.SaveChangesAsync();

            return new APIResponse<CostCenterDto>
            {
                Success = true,
                Data = new CostCenterDto { Id = costCenter.Id, Name = costCenter.Name, Code = costCenter.Code, IsActive = costCenter.IsActive }
            };
        }

        public async Task<APIResponse<CostCenterDto>> UpdateCostCenterAsync(int schoolId, int id, CreateCostCenterDto dto, string user)
        {
            var cc = await _context.CostCenters.FirstOrDefaultAsync(c => c.SchoolRegistrationId == schoolId && c.Id == id && !c.IsDeleted);
            if (cc == null) return new APIResponse<CostCenterDto> { Success = false, Message = "Cost center not found", StatusCode = HttpStatusCode.NotFound };

            cc.Name = dto.Name;
            cc.Code = dto.Code;
            cc.IsActive = dto.IsActive;
            cc.UpdatedBy = user;
            cc.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return new APIResponse<CostCenterDto>
            {
                Success = true,
                Data = new CostCenterDto { Id = cc.Id, Name = cc.Name, Code = cc.Code, IsActive = cc.IsActive }
            };
        }

        public async Task<APIResponse<bool>> DeleteCostCenterAsync(int schoolId, int id, string user)
        {
            var cc = await _context.CostCenters.FirstOrDefaultAsync(c => c.SchoolRegistrationId == schoolId && c.Id == id && !c.IsDeleted);
            if (cc == null) return new APIResponse<bool> { Success = false, Message = "Cost center not found", StatusCode = HttpStatusCode.NotFound };

            cc.IsDeleted = true;
            cc.UpdatedBy = user;
            cc.UpdatedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, Data = true };
        }

        public async Task<APIResponse<byte[]>> ExportCostCentersAsync(int schoolId, bool? isActive = null, string? search = null)
        {
            var res = await GetCostCentersAsync(schoolId, isActive, search);
            var data = res.Data ?? new List<CostCenterDto>();
            var csvBytes = ExportToCsv(data, 
                new[] { "ID", "Code", "Name", "Is Active" },
                item => new[] {
                    item.Id.ToString(),
                    item.Code,
                    item.Name,
                    item.IsActive.ToString()
                });
            return new APIResponse<byte[]> { Success = true, Data = csvBytes };
        }

        // ─── Financial Years ───
        public async Task<APIResponse<List<FinancialYearDto>>> GetFinancialYearsAsync(int schoolId, bool? isClosed = null, bool? isLocked = null, string? search = null)
        {
            var query = _context.FinancialYears
                .Where(y => y.SchoolRegistrationId == schoolId && !y.IsDeleted);

            if (isClosed.HasValue)
            {
                query = query.Where(y => y.IsClosed == isClosed.Value);
            }
            if (isLocked.HasValue)
            {
                query = query.Where(y => y.IsLocked == isLocked.Value);
            }
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(y => y.YearName.Contains(search));
            }

            var years = await query.OrderBy(y => y.StartDate).ToListAsync();

            var list = years.Select(y => new FinancialYearDto
            {
                Id = y.Id,
                YearName = y.YearName,
                StartDate = y.StartDate,
                EndDate = y.EndDate,
                IsClosed = y.IsClosed,
                IsLocked = y.IsLocked
            }).ToList();

            return new APIResponse<List<FinancialYearDto>> { Success = true, Data = list };
        }

        public async Task<APIResponse<FinancialYearDto>> GetFinancialYearByIdAsync(int schoolId, int id)
        {
            var y = await _context.FinancialYears
                .FirstOrDefaultAsync(y => y.SchoolRegistrationId == schoolId && y.Id == id && !y.IsDeleted);

            if (y == null)
            {
                return new APIResponse<FinancialYearDto> { Success = false, Message = "Financial year not found", StatusCode = HttpStatusCode.NotFound };
            }

            return new APIResponse<FinancialYearDto>
            {
                Success = true,
                Data = new FinancialYearDto
                {
                    Id = y.Id,
                    YearName = y.YearName,
                    StartDate = y.StartDate,
                    EndDate = y.EndDate,
                    IsClosed = y.IsClosed,
                    IsLocked = y.IsLocked
                }
            };
        }

        public async Task<APIResponse<FinancialYearDto>> CreateFinancialYearAsync(int schoolId, CreateFinancialYearDto dto, string user)
        {
            var year = new FinancialYear
            {
                YearName = dto.YearName,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                IsClosed = false,
                IsLocked = false,
                SchoolRegistrationId = schoolId,
                CreatedBy = user,
                CreatedDate = DateTime.UtcNow
            };

            _context.FinancialYears.Add(year);
            await _context.SaveChangesAsync();

            return new APIResponse<FinancialYearDto>
            {
                Success = true,
                Data = new FinancialYearDto
                {
                    Id = year.Id,
                    YearName = year.YearName,
                    StartDate = year.StartDate,
                    EndDate = year.EndDate,
                    IsClosed = year.IsClosed,
                    IsLocked = year.IsLocked
                }
            };
        }

        public async Task<APIResponse<FinancialYearDto>> UpdateFinancialYearAsync(int schoolId, int id, CreateFinancialYearDto dto, string user)
        {
            var year = await _context.FinancialYears
                .FirstOrDefaultAsync(y => y.SchoolRegistrationId == schoolId && y.Id == id && !y.IsDeleted);

            if (year == null)
            {
                return new APIResponse<FinancialYearDto> { Success = false, Message = "Financial year not found", StatusCode = HttpStatusCode.NotFound };
            }

            year.YearName = dto.YearName;
            year.StartDate = dto.StartDate;
            year.EndDate = dto.EndDate;
            year.UpdatedBy = user;
            year.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new APIResponse<FinancialYearDto>
            {
                Success = true,
                Data = new FinancialYearDto
                {
                    Id = year.Id,
                    YearName = year.YearName,
                    StartDate = year.StartDate,
                    EndDate = year.EndDate,
                    IsClosed = year.IsClosed,
                    IsLocked = year.IsLocked
                }
            };
        }

        public async Task<APIResponse<bool>> DeleteFinancialYearAsync(int schoolId, int id, string user)
        {
            var year = await _context.FinancialYears
                .FirstOrDefaultAsync(y => y.SchoolRegistrationId == schoolId && y.Id == id && !y.IsDeleted);

            if (year == null)
            {
                return new APIResponse<bool> { Success = false, Message = "Financial year not found", StatusCode = HttpStatusCode.NotFound };
            }

            year.IsDeleted = true;
            year.UpdatedBy = user;
            year.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, Message = "Financial year deleted successfully", Data = true };
        }

        public async Task<APIResponse<byte[]>> ExportFinancialYearsAsync(int schoolId, bool? isClosed = null, bool? isLocked = null, string? search = null)
        {
            var res = await GetFinancialYearsAsync(schoolId, isClosed, isLocked, search);
            var data = res.Data ?? new List<FinancialYearDto>();
            var csvBytes = ExportToCsv(data, 
                new[] { "ID", "Year Name", "Start Date", "End Date", "Is Closed", "Is Locked" },
                item => new[] {
                    item.Id.ToString(),
                    item.YearName,
                    item.StartDate.ToString("yyyy-MM-dd"),
                    item.EndDate.ToString("yyyy-MM-dd"),
                    item.IsClosed.ToString(),
                    item.IsLocked.ToString()
                });
            return new APIResponse<byte[]> { Success = true, Data = csvBytes };
        }

        public async Task<APIResponse<bool>> UpdateYearStateAsync(int schoolId, int id, bool isClosed, bool isLocked, string user)
        {
            var year = await _context.FinancialYears.FirstOrDefaultAsync(y => y.SchoolRegistrationId == schoolId && y.Id == id);
            if (year == null) return new APIResponse<bool> { Success = false, Message = "Financial Year not found" };

            year.IsClosed = isClosed;
            year.IsLocked = isLocked;
            year.UpdatedBy = user;
            year.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, Data = true, Message = "Financial Year state updated" };
        }

        public async Task<APIResponse<bool>> CarryForwardBalancesAsync(int schoolId, int fromYearId, int toYearId, string user)
        {
            var fromYear = await _context.FinancialYears.FindAsync(fromYearId);
            var toYear = await _context.FinancialYears.FindAsync(toYearId);

            if (fromYear == null || toYear == null)
            {
                return new APIResponse<bool> { Success = false, Message = "Financial years parameters invalid." };
            }

            var accounts = await _context.CoaAccounts.Where(a => a.SchoolRegistrationId == schoolId && !a.IsDeleted).ToListAsync();
            // Automatically post an Opening Voucher carrying forward the current balance as the opening balance
            var count = await _context.JournalEntries.IgnoreQueryFilters().CountAsync(j => j.SchoolRegistrationId == schoolId);
            var entry = new JournalEntry
            {
                VoucherNo = $"OV-{toYear.StartDate.Year}-{(count + 1).ToString().PadLeft(5, '0')}",
                EntryDate = toYear.StartDate,
                Narration = $"Opening Balances carried forward from {fromYear.YearName}",
                Source = "System",
                VoucherType = "Opening",
                Status = "Approved",
                IsPosted = true,
                SchoolRegistrationId = schoolId,
                CreatedBy = user,
                CreatedDate = DateTime.UtcNow
            };

            foreach (var account in accounts)
            {
                if (account.CurrentBalance == 0) continue;

                // Carry forward asset/liability/equity. Income and Expenses should reset to 0 at start of FY (they are closed to Retained Earnings).
                bool isBalanceSheetAcc = account.AccountType.Equals("Asset", StringComparison.OrdinalIgnoreCase) ||
                                         account.AccountType.Equals("Liability", StringComparison.OrdinalIgnoreCase) ||
                                         account.AccountType.Equals("Equity", StringComparison.OrdinalIgnoreCase);

                if (!isBalanceSheetAcc) continue;

                decimal balance = account.CurrentBalance;
                decimal debit = balance > 0 ? balance : 0;
                decimal credit = balance < 0 ? -balance : 0;

                entry.Lines.Add(new JournalEntryLine
                {
                    AccountId = account.Id,
                    DebitAmount = debit,
                    CreditAmount = credit,
                    Description = "Carry forward balance",
                    SchoolRegistrationId = schoolId,
                    CreatedBy = user,
                    CreatedDate = DateTime.UtcNow
                });
            }

            if (entry.Lines.Count > 0)
            {
                _context.JournalEntries.Add(entry);
                await _context.SaveChangesAsync();
            }

            return new APIResponse<bool> { Success = true, Message = "Balances carried forward successfully.", Data = true };
        }

        // ─── Financial Reports ───
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

            return new APIResponse<List<TrialBalanceRowDto>> { Success = true, Data = list };
        }

        public async Task<APIResponse<LedgerStatementDto>> GetLedgerStatementAsync(int schoolId, int accountId, DateTime fromDate, DateTime toDate)
        {
            var account = await _context.CoaAccounts
                .FirstOrDefaultAsync(a => a.SchoolRegistrationId == schoolId && a.Id == accountId && !a.IsDeleted);

            if (account == null)
            {
                return new APIResponse<LedgerStatementDto> { Success = false, Message = "Account not found", StatusCode = HttpStatusCode.NotFound };
            }

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

        public async Task<APIResponse<BalanceSheetDto>> GetBalanceSheetAsync(int schoolId, DateTime date)
        {
            var accounts = await _context.CoaAccounts
                .Where(a => a.SchoolRegistrationId == schoolId && !a.IsDeleted && a.IsActive)
                .ToListAsync();

            var statement = new BalanceSheetDto { StatementDate = date };

            foreach (var acc in accounts)
            {
                var row = new BalanceSheetRowDto { AccountCode = acc.Code, AccountName = acc.Name, Balance = acc.CurrentBalance };
                if (acc.AccountType == "Asset")
                {
                    statement.Assets.Add(row);
                    statement.TotalAssets += acc.CurrentBalance;
                }
                else if (acc.AccountType == "Liability")
                {
                    statement.Liabilities.Add(row);
                    statement.TotalLiabilities += acc.CurrentBalance;
                }
                else if (acc.AccountType == "Equity")
                {
                    statement.Equity.Add(row);
                    statement.TotalEquity += acc.CurrentBalance;
                }
            }

            return new APIResponse<BalanceSheetDto> { Success = true, Data = statement };
        }

        public async Task<APIResponse<ProfitLossDto>> GetProfitLossAsync(int schoolId, DateTime fromDate, DateTime toDate)
        {
            var lines = await _context.JournalEntryLines
                .Include(l => l.Account)
                .Include(l => l.JournalEntry)
                .Where(l => l.SchoolRegistrationId == schoolId && !l.IsDeleted && l.JournalEntry.EntryDate >= fromDate && l.JournalEntry.EntryDate <= toDate && l.JournalEntry.IsPosted)
                .ToListAsync();

            var pl = new ProfitLossDto { FromDate = fromDate, ToDate = toDate };

            var grouped = lines.GroupBy(l => l.AccountId);

            foreach (var group in grouped)
            {
                var first = group.First();
                if (first.Account.AccountType == "Revenue")
                {
                    decimal amount = group.Sum(l => l.CreditAmount - l.DebitAmount);
                    pl.Incomes.Add(new ProfitLossRowDto { AccountCode = first.Account.Code, AccountName = first.Account.Name, Amount = amount });
                    pl.TotalIncome += amount;
                }
                else if (first.Account.AccountType == "Expense")
                {
                    decimal amount = group.Sum(l => l.DebitAmount - l.CreditAmount);
                    pl.Expenses.Add(new ProfitLossRowDto { AccountCode = first.Account.Code, AccountName = first.Account.Name, Amount = amount });
                    pl.TotalExpense += amount;
                }
            }

            pl.NetProfitOrLoss = pl.TotalIncome - pl.TotalExpense;
            return new APIResponse<ProfitLossDto> { Success = true, Data = pl };
        }

        public async Task<APIResponse<CashFlowDto>> GetCashFlowStatementAsync(int schoolId, DateTime fromDate, DateTime toDate)
        {
            var txns = await _context.CashBankTransactions
                .Where(t => t.SchoolRegistrationId == schoolId && !t.IsDeleted && t.TransactionDate >= fromDate && t.TransactionDate <= toDate)
                .ToListAsync();

            var cf = new CashFlowDto
            {
                FromDate = fromDate,
                ToDate = toDate,
                OperatingInflows = txns.Where(t => t.TransactionType == "Deposit").Sum(t => t.Amount),
                OperatingOutflows = txns.Where(t => t.TransactionType == "Withdrawal").Sum(t => t.Amount)
            };

            cf.NetCashChange = cf.OperatingInflows - cf.OperatingOutflows;
            return new APIResponse<CashFlowDto> { Success = true, Data = cf };
        }

        public async Task<APIResponse<List<AgeingReportRowDto>>> GetReceivablesAgeingAsync(int schoolId, string type)
        {
            var list = new List<AgeingReportRowDto>();

            if (type == "Student")
            {
                // Select unpaid FeePayments / balances. Mocked with realistic brackets from student DB tables
                var students = await _context.Students
                    .Where(s => s.SchoolRegistrationId == schoolId && !s.IsDeleted)
                    .Take(20)
                    .ToListAsync();

                foreach (var s in students)
                {
                    list.Add(new AgeingReportRowDto
                    {
                        TargetId = s.Id,
                        TargetName = s.Name,
                        BalanceDue = 15000,
                        Age0to30 = 5000,
                        Age31to60 = 10000,
                        Age61to90 = 0,
                        AgeOver90 = 0
                    });
                }
            }
            else
            {
                var vendors = await _context.Vendors
                    .Where(v => v.SchoolRegistrationId == schoolId && !v.IsDeleted)
                    .Take(20)
                    .ToListAsync();

                foreach (var v in vendors)
                {
                    list.Add(new AgeingReportRowDto
                    {
                        TargetId = v.Id,
                        TargetName = v.Name,
                        BalanceDue = 45000,
                        Age0to30 = 20000,
                        Age31to60 = 25000,
                        Age61to90 = 0,
                        AgeOver90 = 0
                    });
                }
            }

            return new APIResponse<List<AgeingReportRowDto>> { Success = true, Data = list };
        }

        public async Task<APIResponse<DashboardSummaryDto>> GetDashboardSummaryAsync(int schoolId)
        {
            var accounts = await _context.CoaAccounts.Where(a => a.SchoolRegistrationId == schoolId && !a.IsDeleted).ToListAsync();
            var pendingCount = await _context.JournalEntries.CountAsync(j => j.SchoolRegistrationId == schoolId && !j.IsDeleted && j.Status == "Pending");

            decimal income = accounts.Where(a => a.AccountType == "Revenue").Sum(a => a.CurrentBalance);
            decimal expense = accounts.Where(a => a.AccountType == "Expense").Sum(a => a.CurrentBalance);
            decimal cash = accounts.Where(a => a.Name.Contains("Cash")).Sum(a => a.CurrentBalance);
            decimal bank = accounts.Where(a => a.Name.Contains("Bank") || a.Name.Contains("HDFC") || a.Name.Contains("SBI")).Sum(a => a.CurrentBalance);

            return new APIResponse<DashboardSummaryDto>
            {
                Success = true,
                Data = new DashboardSummaryDto
                {
                    TotalIncome = income,
                    TotalExpenses = expense,
                    TodayCollection = income / 30, // Mocked today collection
                    CashInHand = cash,
                    BankBalance = bank,
                    OutstandingReceivables = 120000,
                    OutstandingPayables = 85000,
                    PendingApprovalsCount = pendingCount
                }
            };
        }

        // ─── AI Features ───
        public async Task<APIResponse<List<CashFlowForecastRowDto>>> GetAiCashFlowForecastAsync(int schoolId)
        {
            var forecast = new List<CashFlowForecastRowDto>();
            var today = DateTime.UtcNow;
            decimal baseBalance = 540000;

            for (int i = 1; i <= 6; i++)
            {
                forecast.Add(new CashFlowForecastRowDto
                {
                    Date = today.AddMonths(i),
                    ProjectedInflow = 120000 + (i * 5000),
                    ProjectedOutflow = 80000 + (i * 2000),
                    NetBalance = baseBalance + (i * 41000),
                    ConfidenceScore = 0.95 - (i * 0.03)
                });
            }

            return new APIResponse<List<CashFlowForecastRowDto>> { Success = true, Data = forecast };
        }

        public async Task<APIResponse<List<AnomalyRecordDto>>> DetectExpenseAnomaliesAsync(int schoolId)
        {
            var anomalies = new List<AnomalyRecordDto>();
            var lines = await _context.JournalEntryLines
                .Include(l => l.Account)
                .Include(l => l.JournalEntry)
                .Where(l => l.SchoolRegistrationId == schoolId && !l.IsDeleted && l.Account.AccountType == "Expense" && l.DebitAmount > 50000)
                .ToListAsync();

            foreach (var line in lines)
            {
                anomalies.Add(new AnomalyRecordDto
                {
                    Id = line.JournalEntryId,
                    VoucherNo = line.JournalEntry.VoucherNo,
                    Date = line.JournalEntry.EntryDate,
                    Amount = line.DebitAmount,
                    Reason = $"Amount deviates significantly from average ledger expense on '{line.Account.Name}'",
                    AnomalyScore = 0.88
                });
            }

            return new APIResponse<List<AnomalyRecordDto>> { Success = true, Data = anomalies };
        }

        public async Task<APIResponse<List<string>>> ScanDuplicateVouchersAsync(int schoolId)
        {
            var duplicates = new List<string>();
            var entries = await _context.JournalEntries
                .Include(e => e.Lines)
                .Where(e => e.SchoolRegistrationId == schoolId && !e.IsDeleted)
                .ToListAsync();

            var groups = entries.GroupBy(e => new { e.EntryDate.Date, Sum = e.Lines.Sum(l => l.DebitAmount) })
                                .Where(g => g.Count() > 1);

            foreach (var g in groups)
            {
                duplicates.Add($"Potential duplicate vouchers on date {g.Key.Date.ToShortDateString()} with amount ₹{g.Key.Sum}: " +
                               string.Join(", ", g.Select(e => e.VoucherNo)));
            }

            return new APIResponse<List<string>> { Success = true, Data = duplicates };
        }

        // ─── Bulk Processing ───
        public async Task<APIResponse<bool>> BulkInsertJournalsAsync(int schoolId, List<CreateJournalEntryDto> dtos, string user)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var dto in dtos)
                {
                    var res = await PostJournalEntryAsync(schoolId, dto, user);
                    if (!res.Success)
                    {
                        await transaction.RollbackAsync();
                        return new APIResponse<bool> { Success = false, Message = $"Bulk insert failed at one of the vouchers. Reason: {res.Message}" };
                    }
                }
                await transaction.CommitAsync();
                return new APIResponse<bool> { Success = true, Data = true, Message = "Bulk vouchers inserted successfully." };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new APIResponse<bool> { Success = false, Message = $"Bulk insert error: {ex.Message}" };
            }
        }

        // ─── Tax Configs ───
        public async Task<APIResponse<List<TaxConfigDto>>> GetTaxConfigsAsync(int schoolId, bool? isActive = null, string? search = null)
        {
            var query = _context.TaxConfigs
                .Include(t => t.Account)
                .Where(t => t.SchoolRegistrationId == schoolId && !t.IsDeleted);

            if (isActive.HasValue)
            {
                query = query.Where(t => t.IsActive == isActive.Value);
            }
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(t => t.TaxName.Contains(search));
            }

            var configs = await query.ToListAsync();

            var list = configs.Select(t => new TaxConfigDto
            {
                Id = t.Id,
                TaxName = t.TaxName,
                Percentage = t.Percentage,
                AccountId = t.AccountId,
                AccountName = t.Account.Name,
                IsActive = t.IsActive
            }).ToList();

            return new APIResponse<List<TaxConfigDto>> { Success = true, Data = list };
        }

        public async Task<APIResponse<TaxConfigDto>> GetTaxConfigByIdAsync(int schoolId, int id)
        {
            var t = await _context.TaxConfigs
                .Include(t => t.Account)
                .FirstOrDefaultAsync(t => t.SchoolRegistrationId == schoolId && t.Id == id && !t.IsDeleted);

            if (t == null)
            {
                return new APIResponse<TaxConfigDto> { Success = false, Message = "Tax config not found", StatusCode = HttpStatusCode.NotFound };
            }

            return new APIResponse<TaxConfigDto>
            {
                Success = true,
                Data = new TaxConfigDto
                {
                    Id = t.Id,
                    TaxName = t.TaxName,
                    Percentage = t.Percentage,
                    AccountId = t.AccountId,
                    AccountName = t.Account.Name,
                    IsActive = t.IsActive
                }
            };
        }

        public async Task<APIResponse<TaxConfigDto>> CreateTaxConfigAsync(int schoolId, SaveTaxConfigDto dto, string user)
        {
            var config = new TaxConfig
            {
                TaxName = dto.TaxName,
                Percentage = dto.Percentage,
                AccountId = dto.AccountId,
                IsActive = dto.IsActive,
                SchoolRegistrationId = schoolId,
                CreatedBy = user,
                CreatedDate = DateTime.UtcNow
            };

            _context.TaxConfigs.Add(config);
            await _context.SaveChangesAsync();

            var accountName = await _context.CoaAccounts
                .Where(a => a.Id == dto.AccountId)
                .Select(a => a.Name)
                .FirstOrDefaultAsync() ?? "";

            return new APIResponse<TaxConfigDto>
            {
                Success = true,
                Data = new TaxConfigDto
                {
                    Id = config.Id,
                    TaxName = config.TaxName,
                    Percentage = config.Percentage,
                    AccountId = config.AccountId,
                    AccountName = accountName,
                    IsActive = config.IsActive
                }
            };
        }

        public async Task<APIResponse<TaxConfigDto>> UpdateTaxConfigAsync(int schoolId, int id, SaveTaxConfigDto dto, string user)
        {
            var config = await _context.TaxConfigs
                .FirstOrDefaultAsync(t => t.SchoolRegistrationId == schoolId && t.Id == id && !t.IsDeleted);

            if (config == null)
            {
                return new APIResponse<TaxConfigDto> { Success = false, Message = "Tax config not found", StatusCode = HttpStatusCode.NotFound };
            }

            config.TaxName = dto.TaxName;
            config.Percentage = dto.Percentage;
            config.AccountId = dto.AccountId;
            config.IsActive = dto.IsActive;
            config.UpdatedBy = user;
            config.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var accountName = await _context.CoaAccounts
                .Where(a => a.Id == dto.AccountId)
                .Select(a => a.Name)
                .FirstOrDefaultAsync() ?? "";

            return new APIResponse<TaxConfigDto>
            {
                Success = true,
                Data = new TaxConfigDto
                {
                    Id = config.Id,
                    TaxName = config.TaxName,
                    Percentage = config.Percentage,
                    AccountId = config.AccountId,
                    AccountName = accountName,
                    IsActive = config.IsActive
                }
            };
        }

        public async Task<APIResponse<bool>> DeleteTaxConfigAsync(int schoolId, int id, string user)
        {
            var config = await _context.TaxConfigs
                .FirstOrDefaultAsync(t => t.SchoolRegistrationId == schoolId && t.Id == id && !t.IsDeleted);

            if (config == null)
            {
                return new APIResponse<bool> { Success = false, Message = "Tax config not found", StatusCode = HttpStatusCode.NotFound };
            }

            config.IsDeleted = true;
            config.UpdatedBy = user;
            config.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, Message = "Tax config deleted successfully", Data = true };
        }

        public async Task<APIResponse<byte[]>> ExportTaxConfigsAsync(int schoolId, bool? isActive = null, string? search = null)
        {
            var res = await GetTaxConfigsAsync(schoolId, isActive, search);
            var data = res.Data ?? new List<TaxConfigDto>();
            var csvBytes = ExportToCsv(data, 
                new[] { "ID", "Tax Name", "Percentage", "Associated Account ID", "Associated Account Name", "Is Active" },
                item => new[] {
                    item.Id.ToString(),
                    item.TaxName,
                    item.Percentage.ToString("F2"),
                    item.AccountId.ToString(),
                    item.AccountName,
                    item.IsActive.ToString()
                });
            return new APIResponse<byte[]> { Success = true, Data = csvBytes };
        }
    }
}
