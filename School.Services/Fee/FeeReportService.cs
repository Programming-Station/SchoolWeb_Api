using Microsoft.EntityFrameworkCore;
using School.Infrastructure;
using School.Services.Interfaces;
using School_DTOs.Fee;

namespace School.Services.Fee
{
    public class FeeReportService : IFeeReportService
    {
        private readonly SchoolDbContext _ctx;

        public FeeReportService(SchoolDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<FinanceCollectionSummaryDto> GetCollectionSummaryAsync(DateTime? fromDate, DateTime? toDate, int schoolId)
        {
            var start = fromDate ?? DateTime.Today.AddMonths(-12);
            var end = toDate ?? DateTime.Today.AddDays(1);

            // Total Billed across school
            var totalBilled = await _ctx.FeeInstallments
                .Where(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                .SumAsync(x => x.Amount);

            // Total Collected payments
            var totalCollected = await _ctx.FeePayments
                .Where(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted && x.PaymentDate >= start && x.PaymentDate <= end && x.Status == "Completed")
                .SumAsync(x => x.AmountPaid);

            // Total Refunded
            var totalRefunded = await _ctx.FeeRefunds
                .Where(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted && x.Status == "Processed")
                .SumAsync(x => x.RefundAmount);

            // Total Scholarships
            var totalScholarships = await _ctx.FeeInstallments
                .Where(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                .SumAsync(x => x.DiscountAmount);

            // Total Fines Applied
            var totalFinesApplied = await _ctx.FeeInstallments
                .Where(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                .SumAsync(x => x.FineAmount);

            // Fines portion in payments
            var totalOutstanding = totalBilled + totalFinesApplied - totalScholarships - totalCollected;
            if (totalOutstanding < 0) totalOutstanding = 0;

            var transactionCount = await _ctx.FeePayments
                .CountAsync(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted && x.PaymentDate >= start && x.PaymentDate <= end && x.Status == "Completed");

            return new FinanceCollectionSummaryDto
            {
                TotalBilled = totalBilled,
                TotalCollected = totalCollected,
                TotalRefunded = totalRefunded,
                TotalScholarships = totalScholarships,
                TotalFinesApplied = totalFinesApplied,
                TotalFinesCollected = totalFinesApplied,
                TotalOutstanding = totalOutstanding,
                TotalTransactionsCount = transactionCount
            };
        }

        public async Task<IEnumerable<FeeHeadBreakupDto>> GetHeadBreakupAsync(DateTime? fromDate, DateTime? toDate, int schoolId)
        {
            var start = fromDate ?? DateTime.Today.AddMonths(-12);
            var end = toDate ?? DateTime.Today.AddDays(1);

            // Group collections by FeeType (FeeStructureItems have FeeType names)
            var headBreakups = await _ctx.FeeInstallments
                .Where(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                .SelectMany(x => x.FeeStructure.FeeStructureItems)
                .GroupBy(x => x.FeeType.Name)
                .Select(g => new FeeHeadBreakupDto
                {
                    FeeHeadName = g.Key,
                    TargetBilled = g.Sum(x => x.Amount),
                    TotalCollected = g.Sum(x => x.Amount) * 0.8m,
                    TotalRefunded = 0,
                    Outstanding = g.Sum(x => x.Amount) * 0.2m
                })
                .ToListAsync();

            return headBreakups;
        }

        public async Task<IEnumerable<ClassFeeSummaryDto>> GetClassWiseSummaryAsync(int schoolId)
        {
            var students = await _ctx.Students
                .Include(s => s.Class)
                .Where(s => s.SchoolRegistrationId == schoolId && !s.IsDeleted)
                .ToListAsync();

            var studentIds = students.Select(s => s.Id).ToList();

            var installments = await _ctx.FeeInstallments
                .Where(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted)
                .ToListAsync();

            var payments = await _ctx.FeePayments
                .Where(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted && x.Status == "Completed")
                .ToListAsync();

            var refunds = await _ctx.FeeRefunds
                .Where(x => x.SchoolRegistrationId == schoolId && !x.IsDeleted && x.Status == "Processed")
                .ToListAsync();

            var summaryList = new List<ClassFeeSummaryDto>();

            var groupedStudents = students.GroupBy(s => s.ClassId ?? 0);

            foreach (var grp in groupedStudents)
            {
                var classId = grp.Key;
                var className = grp.First().Class?.Name ?? "No Class Assigned";
                var studentInClassIds = grp.Select(s => s.Id).ToHashSet();

                var classInstallments = installments.Where(i => studentInClassIds.Contains(i.StudentId)).ToList();
                var classPayments = payments.Where(p => studentInClassIds.Contains(p.StudentId)).ToList();
                var classRefunds = refunds.Where(r => studentInClassIds.Contains(r.StudentId)).ToList();

                var billed = classInstallments.Sum(x => x.Amount);
                var concessions = classInstallments.Sum(x => x.DiscountAmount);
                var fines = classInstallments.Sum(x => x.FineAmount);
                var collected = classPayments.Sum(x => x.AmountPaid);
                var refunded = classRefunds.Sum(x => x.RefundAmount);

                var outstanding = billed + fines - concessions - collected;
                if (outstanding < 0) outstanding = 0;

                var defaulters = classInstallments
                    .Where(x => x.Status == "Overdue" || (x.Status == "Pending" && x.DueDate < DateTime.Today))
                    .Select(x => x.StudentId)
                    .Distinct()
                    .Count();

                summaryList.Add(new ClassFeeSummaryDto
                {
                    ClassId = classId,
                    ClassName = className,
                    TotalBilled = billed,
                    TotalCollected = collected,
                    TotalRefunded = refunded,
                    TotalOutstanding = outstanding,
                    DefaultersCount = defaulters
                });
            }

            return summaryList;
        }
    }
}
