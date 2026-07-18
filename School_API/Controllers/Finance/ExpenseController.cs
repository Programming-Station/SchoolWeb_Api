using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Services.Interfaces;
using School.Infrastructure.Interfaces;
using School.Infrastructure;
using School_API.Common.Interface;
using School_DTOs;
using School_DTOs.Finance;

namespace School_API.Controllers.Finance
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExpenseController : BaseController
    {
        private readonly IAccountingService _svc;
        private readonly SchoolDbContext _context;
        private readonly ITenantService _tenant;

        public ExpenseController(IAccountingService svc, SchoolDbContext context, ITenantService tenant, ICurrentUserService cur) : base(cur)
        {
            _svc = svc;
            _context = context;
            _tenant = tenant;
        }

        private int SchoolId => _tenant.GetTenantId() ?? 1;

        public class CreateExpenseRequest
        {
            public int ExpenseAccountId { get; set; }
            public int CashBankAccountId { get; set; }
            public decimal Amount { get; set; }
            public DateTime Date { get; set; } = DateTime.UtcNow;
            public string? Reference { get; set; }
            public string? Narration { get; set; }
            public int? CostCenterId { get; set; }
            public string? AttachmentUrl { get; set; }
        }

        [HttpGet]
        public async Task<IActionResult> GetExpenses([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            // Fetch journal entries of VoucherType "Payment" (which represents expense)
            var res = await _svc.GetJournalEntriesAsync(SchoolId, fromDate, toDate, null, null);
            if (!res.Success || res.Data == null) return StatusCode((int)res.StatusCode, res);

            var payments = res.Data.Where(j => j.VoucherType == "Payment").ToList();
            return Ok(new APIResponse<List<JournalEntryDto>>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = payments
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseRequest req)
        {
            if (req.Amount <= 0) return BadRequest(new { message = "Amount must be greater than zero." });

            var journalDto = new CreateJournalEntryDto
            {
                EntryDate = req.Date,
                Narration = req.Narration,
                Reference = req.Reference,
                Source = "ExpenseModule",
                VoucherType = "Payment",
                CostCenterId = req.CostCenterId,
                AttachmentUrl = req.AttachmentUrl,
                Lines = new List<CreateJournalEntryLineDto>
                {
                    // Debit Expense Account (Expense)
                    new CreateJournalEntryLineDto
                    {
                        AccountId = req.ExpenseAccountId,
                        DebitAmount = req.Amount,
                        CreditAmount = 0,
                        Description = req.Narration
                    },
                    // Credit Cash/Bank Account (Asset)
                    new CreateJournalEntryLineDto
                    {
                        AccountId = req.CashBankAccountId,
                        DebitAmount = 0,
                        CreditAmount = req.Amount,
                        Description = req.Narration
                    }
                }
            };

            var res = await _svc.PostJournalEntryAsync(SchoolId, journalDto, UserName);
            return StatusCode((int)res.StatusCode, res);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var res = await _svc.DeleteJournalEntryAsync(SchoolId, id, UserName);
            return res.Success ? Ok(res) : StatusCode((int)res.StatusCode, res);
        }
    }
}
