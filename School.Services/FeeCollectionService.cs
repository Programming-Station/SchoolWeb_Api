using Microsoft.EntityFrameworkCore;
using School.Domain.FeeManagnment;
using School.Infrastructure;
using School.Infrastructure.Repositories.IRepositories;
using School.Services.Interfaces;

namespace School.Services
{
    public class FeeCollectionService : IFeeCollectionService
    {
        private readonly IFeeInstallmentRepository _installmentRepo;
        private readonly IFeePaymentRepository _paymentRepo;
        private readonly IFeeStructureRepository _structureRepo;
        private readonly SchoolDbContext _dbContext;

        public FeeCollectionService(
            IFeeInstallmentRepository installmentRepo,
            IFeePaymentRepository paymentRepo,
            IFeeStructureRepository structureRepo,
            SchoolDbContext dbContext)
        {
            _installmentRepo = installmentRepo;
            _paymentRepo = paymentRepo;
            _structureRepo = structureRepo;
            _dbContext = dbContext;
        }

        public async Task<(bool Success, string Message, List<FeeInstallmentDto> Installments)> GenerateInstallmentsAsync(
            GenerateInstallmentsRequest request, string createdBy, int schoolRegistrationId)
        {
            var structure = await _structureRepo.GetByIdAsync(request.FeeStructureId);
            if (structure == null) return (false, "Fee structure not found.", new());

            // Check if installments already generated for this student+structure
            var existing = await _installmentRepo.GetByStudentAsync(request.StudentId, schoolRegistrationId);
            if (existing.Any(x => x.FeeStructureId == request.FeeStructureId))
                return (false, "Installments already generated for this student.", new());

            var totalAmount = structure.FeeStructureItems.Sum(x => x.Amount);
            var perInstallment = Math.Round(totalAmount / request.NumberOfInstallments, 2);

            var installments = new List<FeeInstallment>();
            for (int i = 1; i <= request.NumberOfInstallments; i++)
            {
                installments.Add(new FeeInstallment
                {
                    FeeStructureId = request.FeeStructureId,
                    StudentId = request.StudentId,
                    InstallmentNo = i,
                    InstallmentName = $"Installment {i}",
                    Amount = i == request.NumberOfInstallments
                        ? totalAmount - (perInstallment * (request.NumberOfInstallments - 1)) // last = remainder
                        : perInstallment,
                    DueDate = request.FirstDueDate.AddMonths(i - 1),
                    Status = "Pending",
                    SchoolRegistrationId = schoolRegistrationId,
                    CreatedBy = createdBy
                });
            }

            await _installmentRepo.AddRangeAsync(installments);
            return (true, $"{request.NumberOfInstallments} installments generated.", installments.Select(MapInstallmentDto).ToList());
        }

        public async Task<(bool Success, string Message, FeePaymentDto Payment)> CollectFeeAsync(
            CollectFeeRequest request, string collectedBy, int schoolRegistrationId)
        {
            var installment = await _installmentRepo.GetByIdAsync(request.FeeInstallmentId);
            if (installment == null || installment.SchoolRegistrationId != schoolRegistrationId)
                return (false, "Installment not found.", null!);

            if (installment.Status == "Paid")
                return (false, "This installment is already paid.", null!);

            var netPayable = installment.Amount + installment.FineAmount - installment.DiscountAmount;
            if (request.AmountPaid <= 0 || request.AmountPaid > netPayable)
                return (false, $"Invalid amount. Net payable is {netPayable:C}.", null!);

            var receiptNo = await _paymentRepo.GenerateReceiptNoAsync(schoolRegistrationId);

            var payment = new FeePayment
            {
                FeeInstallmentId = request.FeeInstallmentId,
                StudentId = request.StudentId,
                AmountPaid = request.AmountPaid,
                PaymentDate = DateTime.Now,
                PaymentMode = request.PaymentMode,
                TransactionRef = request.TransactionRef,
                ReceiptNo = receiptNo,
                CollectedBy = collectedBy,
                Remarks = request.Remarks,
                Status = "Completed",
                SchoolRegistrationId = schoolRegistrationId,
                CreatedBy = collectedBy
            };

            await _paymentRepo.AddAsync(payment);

            // Update installment status
            var newStatus = request.AmountPaid >= netPayable ? "Paid" : "PartiallyPaid";
            await _installmentRepo.UpdateStatusAsync(installment.Id, newStatus, DateTime.Now);

            return (true, $"Payment recorded. Receipt: {receiptNo}", MapPaymentDto(payment, installment));
        }

        public async Task<IEnumerable<FeeInstallmentDto>> GetInstallmentsByStudentAsync(int studentId, int schoolRegistrationId)
        {
            var data = await _installmentRepo.GetByStudentAsync(studentId, schoolRegistrationId);
            return data.Select(MapInstallmentDto);
        }

        public async Task<IEnumerable<FeeInstallmentDto>> GetPendingByStudentAsync(int studentId, int schoolRegistrationId)
        {
            var data = await _installmentRepo.GetPendingByStudentAsync(studentId, schoolRegistrationId);
            return data.Select(MapInstallmentDto);
        }

        public async Task<IEnumerable<FeeInstallmentDto>> GetOverdueAsync(int schoolRegistrationId)
        {
            var data = await _installmentRepo.GetOverdueAsync(schoolRegistrationId);
            return data.Select(MapInstallmentDto);
        }

        public async Task<IEnumerable<FeePaymentDto>> GetPaymentsByStudentAsync(int studentId, int schoolRegistrationId)
        {
            var data = await _paymentRepo.GetByStudentAsync(studentId, schoolRegistrationId);
            return data.Select(p => MapPaymentDto(p, p.FeeInstallment));
        }

        public async Task<IEnumerable<FeePaymentDto>> GetPaymentsByDateRangeAsync(DateTime from, DateTime to, int schoolRegistrationId)
        {
            var data = await _paymentRepo.GetByDateRangeAsync(from, to, schoolRegistrationId);
            return data.Select(p => MapPaymentDto(p, p.FeeInstallment));
        }

        public async Task<FeeCollectionSummaryDto> GetCollectionSummaryAsync(DateTime from, DateTime to, int schoolRegistrationId)
        {
            var collected = await _paymentRepo.GetTotalCollectedAsync(from, to, schoolRegistrationId);
            var overdues = await _installmentRepo.GetOverdueAsync(schoolRegistrationId);
            var pending = overdues.Sum(x => x.Amount + x.FineAmount - x.DiscountAmount);

            return new FeeCollectionSummaryDto
            {
                TotalCollected = collected,
                TotalPending = pending,
                OverdueStudents = overdues.Select(x => x.StudentId).Distinct().Count()
            };
        }

        public async Task<FeePaymentDto?> GetPaymentByReceiptAsync(string receiptNo, int schoolRegistrationId)
        {
            var payment = await _paymentRepo.GetByReceiptNoAsync(receiptNo, schoolRegistrationId);
            if (payment == null) return null;
            return MapPaymentDto(payment, payment.FeeInstallment);
        }

        // ── Mappers ──────────────────────────────────────────────────────────
        private static FeeInstallmentDto MapInstallmentDto(FeeInstallment x) => new()
        {
            Id = x.Id,
            StudentId = x.StudentId,
            StudentName = x.Student?.Name ?? string.Empty,
            EnrollmentNumber = x.Student?.EnrollmentNumber,
            FeeStructureId = x.FeeStructureId,
            FeeStructureName = x.FeeStructure?.Name ?? string.Empty,
            InstallmentNo = x.InstallmentNo,
            InstallmentName = x.InstallmentName,
            Amount = x.Amount,
            FineAmount = x.FineAmount,
            DiscountAmount = x.DiscountAmount,
            DueDate = x.DueDate,
            Status = x.Status,
            PaidDate = x.PaidDate,
            Remarks = x.Remarks
        };

        private static FeePaymentDto MapPaymentDto(FeePayment p, FeeInstallment? inst) => new()
        {
            Id = p.Id,
            StudentId = p.StudentId,
            StudentName = p.Student?.Name ?? string.Empty,
            FeeInstallmentId = p.FeeInstallmentId,
            InstallmentName = inst?.InstallmentName ?? string.Empty,
            AmountPaid = p.AmountPaid,
            PaymentDate = p.PaymentDate,
            PaymentMode = p.PaymentMode,
            TransactionRef = p.TransactionRef,
            ReceiptNo = p.ReceiptNo,
            CollectedBy = p.CollectedBy,
            Remarks = p.Remarks,
            Status = p.Status
        };

        public async Task<IEnumerable<FeeInstallmentDto>> GetPendingByClassAsync(int classId, int schoolRegistrationId)
        {
            // Get student IDs for the given class
            var studentIds = await _dbContext.Students
                .Where(s => s.ClassId == classId && s.SchoolRegistrationId == schoolRegistrationId && !s.IsDeleted)
                .Select(s => s.Id)
                .ToListAsync();

            if (!studentIds.Any()) return Enumerable.Empty<FeeInstallmentDto>();

            var pendingInstallments = await _dbContext.FeeInstallments
                .Include(x => x.Student)
                .Include(x => x.FeeStructure)
                .Where(x => studentIds.Contains(x.StudentId)
                    && x.SchoolRegistrationId == schoolRegistrationId
                    && (x.Status == "Pending" || x.Status == "Overdue" || x.Status == "PartiallyPaid")
                    && !x.IsDeleted)
                .OrderBy(x => x.DueDate)
                .ToListAsync();

            return pendingInstallments.Select(MapInstallmentDto);
        }
    }
}
