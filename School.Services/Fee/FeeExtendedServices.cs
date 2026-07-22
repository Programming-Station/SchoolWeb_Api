using School.Domain.FeeManagnment;
using School.Infrastructure.Repositories.IRepositories;
using School.Services.Interfaces;

namespace School.Services.Fee
{
    // ══════════════════════════════════════════════════════════════════════════
    // DTOs
    // ══════════════════════════════════════════════════════════════════════════
    public class FeeFineDto
    {
        public int Id { get; set; }
        public int FeeInstallmentId { get; set; }
        public string? InstallmentName { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = null!;
        public decimal FineAmount { get; set; }
        public string FineType { get; set; } = "Fixed";
        public int? DaysLate { get; set; }
        public string? Reason { get; set; }
        public string Status { get; set; } = "Pending";
        public string? AppliedBy { get; set; }
        public int SchoolRegistrationId { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class ScholarshipDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = null!;
        public string ScholarshipName { get; set; } = null!;
        public string? Description { get; set; }
        public string DiscountType { get; set; } = "Fixed";
        public decimal DiscountValue { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public string Status { get; set; } = "Active";
        public string? ApprovedBy { get; set; }
        public string? Remarks { get; set; }
    }

    public class FeeRefundDto
    {
        public int Id { get; set; }
        public int FeePaymentId { get; set; }
        public string? ReceiptNo { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = null!;
        public decimal RefundAmount { get; set; }
        public string? Reason { get; set; }
        public string RefundMode { get; set; } = "Cash";
        public string? RefundRef { get; set; }
        public DateTime RefundDate { get; set; }
        public string Status { get; set; } = "Pending";
        public string? ApprovedBy { get; set; }
        public string? Remarks { get; set; }
    }

    // Fee Summary DTO for reports
    public class FeeSummaryDto
    {
        public decimal TotalCollected { get; set; }
        public decimal TotalPending { get; set; }
        public decimal TotalOverdue { get; set; }
        public decimal TotalFines { get; set; }
        public decimal TotalRefunds { get; set; }
        public decimal TotalScholarships { get; set; }
        public int TotalStudents { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }

    // ══════════════════════════════════════════════════════════════════════════
    // INTERFACES
    // ══════════════════════════════════════════════════════════════════════════
    public interface IFeeFineService
    {
        Task<(bool Success, string Message, FeeFineDto Fine)> ApplyFineAsync(FeeFineDto dto, string appliedBy, int schoolId);
        Task<IEnumerable<FeeFineDto>> GetByStudentAsync(int studentId, int schoolId);
        Task<IEnumerable<FeeFineDto>> GetPendingAsync(int schoolId);
        Task<(bool Success, string Message)> WaiveFineAsync(int id);
        Task<(bool Success, string Message)> MarkPaidAsync(int id);
        Task<(bool Success, string Message)> DeleteAsync(int id);
    }

    public interface IScholarshipService
    {
        Task<(bool Success, string Message, ScholarshipDto Scholarship)> CreateAsync(ScholarshipDto dto, string createdBy, int schoolId);
        Task<IEnumerable<ScholarshipDto>> GetByStudentAsync(int studentId, int schoolId);
        Task<IEnumerable<ScholarshipDto>> GetActiveAsync(int schoolId);
        Task<(bool Success, string Message)> UpdateAsync(ScholarshipDto dto);
        Task<(bool Success, string Message)> RevokeAsync(int id);
    }

    public interface IFeeRefundService
    {
        Task<(bool Success, string Message, FeeRefundDto Refund)> RequestRefundAsync(FeeRefundDto dto, string requestedBy, int schoolId);
        Task<IEnumerable<FeeRefundDto>> GetByStudentAsync(int studentId, int schoolId);
        Task<IEnumerable<FeeRefundDto>> GetPendingAsync(int schoolId);
        Task<(bool Success, string Message)> ApproveAsync(int id, string approvedBy);
        Task<(bool Success, string Message)> ProcessAsync(int id, string processedBy, string? refundRef);
        Task<(bool Success, string Message)> RejectAsync(int id, string rejectedBy);
        Task<decimal> GetTotalRefundedAsync(DateTime from, DateTime to, int schoolId);
    }

    // ══════════════════════════════════════════════════════════════════════════
    // IMPLEMENTATIONS
    // ══════════════════════════════════════════════════════════════════════════
    public class FeeFineService : IFeeFineService
    {
        private readonly IFeeFineRepository _repo;
        public FeeFineService(IFeeFineRepository repo) => _repo = repo;

        public async Task<(bool, string, FeeFineDto)> ApplyFineAsync(FeeFineDto dto, string appliedBy, int schoolId)
        {
            var f = new FeeFine
            {
                FeeInstallmentId = dto.FeeInstallmentId,
                StudentId = dto.StudentId,
                FineAmount = dto.FineAmount,
                FineType = dto.FineType,
                DaysLate = dto.DaysLate,
                Reason = dto.Reason,
                Status = "Pending",
                AppliedBy = appliedBy,
                SchoolRegistrationId = schoolId,
                CreatedBy = appliedBy
            };
            await _repo.AddAsync(f);
            dto.Id = f.Id;
            return (true, "Fine applied.", dto);
        }

        public async Task<IEnumerable<FeeFineDto>> GetByStudentAsync(int studentId, int schoolId)
            => (await _repo.GetByStudentAsync(studentId, schoolId)).Select(Map);

        public async Task<IEnumerable<FeeFineDto>> GetPendingAsync(int schoolId)
            => (await _repo.GetPendingAsync(schoolId)).Select(Map);

        public async Task<(bool, string)> WaiveFineAsync(int id)
        { var r = await _repo.UpdateStatusAsync(id, "Waived"); return r > 0 ? (true, "Fine waived.") : (false, "Not found."); }

        public async Task<(bool, string)> MarkPaidAsync(int id)
        { var r = await _repo.UpdateStatusAsync(id, "Paid"); return r > 0 ? (true, "Fine marked paid.") : (false, "Not found."); }

        public async Task<(bool, string)> DeleteAsync(int id)
        { var r = await _repo.DeleteAsync(id); return r > 0 ? (true, "Deleted.") : (false, "Not found."); }

        private static FeeFineDto Map(FeeFine f) => new()
        {
            Id = f.Id,
            FeeInstallmentId = f.FeeInstallmentId,
            InstallmentName = f.FeeInstallment?.InstallmentName,
            StudentId = f.StudentId,
            StudentName = f.Student?.Name ?? "",
            FineAmount = f.FineAmount,
            FineType = f.FineType,
            DaysLate = f.DaysLate,
            Reason = f.Reason,
            Status = f.Status,
            AppliedBy = f.AppliedBy
        };
    }

    public class ScholarshipService : IScholarshipService
    {
        private readonly IStudentScholarshipRepository _repo;
        public ScholarshipService(IStudentScholarshipRepository repo) => _repo = repo;

        public async Task<(bool, string, ScholarshipDto)> CreateAsync(ScholarshipDto dto, string createdBy, int schoolId)
        {
            var s = new StudentScholarship
            {
                StudentId = dto.StudentId,
                ScholarshipName = dto.ScholarshipName,
                Description = dto.Description,
                DiscountType = dto.DiscountType,
                DiscountValue = dto.DiscountValue,
                ValidFrom = dto.ValidFrom,
                ValidTo = dto.ValidTo,
                Status = "Active",
                ApprovedBy = dto.ApprovedBy,
                Remarks = dto.Remarks,
                SchoolRegistrationId = schoolId,
                CreatedBy = createdBy
            };
            await _repo.AddAsync(s);
            dto.Id = s.Id;
            return (true, "Scholarship created.", dto);
        }

        public async Task<IEnumerable<ScholarshipDto>> GetByStudentAsync(int studentId, int schoolId)
            => (await _repo.GetByStudentAsync(studentId, schoolId)).Select(Map);

        public async Task<IEnumerable<ScholarshipDto>> GetActiveAsync(int schoolId)
            => (await _repo.GetActiveAsync(schoolId)).Select(Map);

        public async Task<(bool, string)> UpdateAsync(ScholarshipDto dto)
        {
            var s = await _repo.GetByIdAsync(dto.Id);
            if (s == null) return (false, "Not found.");
            s.ScholarshipName = dto.ScholarshipName; s.DiscountType = dto.DiscountType;
            s.DiscountValue = dto.DiscountValue; s.ValidFrom = dto.ValidFrom; s.ValidTo = dto.ValidTo;
            s.Remarks = dto.Remarks;
            await _repo.UpdateAsync(s);
            return (true, "Updated.");
        }

        public async Task<(bool, string)> RevokeAsync(int id)
        {
            var s = await _repo.GetByIdAsync(id);
            if (s == null) return (false, "Not found.");
            s.Status = "Cancelled";
            await _repo.UpdateAsync(s);
            return (true, "Scholarship revoked.");
        }

        private static ScholarshipDto Map(StudentScholarship s) => new()
        {
            Id = s.Id,
            StudentId = s.StudentId,
            StudentName = s.Student?.Name ?? "",
            ScholarshipName = s.ScholarshipName,
            Description = s.Description,
            DiscountType = s.DiscountType,
            DiscountValue = s.DiscountValue,
            ValidFrom = s.ValidFrom,
            ValidTo = s.ValidTo,
            Status = s.Status,
            ApprovedBy = s.ApprovedBy,
            Remarks = s.Remarks
        };
    }

    public class FeeRefundService : IFeeRefundService
    {
        private readonly IFeeRefundRepository _repo;
        private readonly IEmailService _emailService;

        public FeeRefundService(IFeeRefundRepository repo, IEmailService emailService)
        {
            _repo = repo;
            _emailService = emailService;
        }

        public async Task<(bool, string, FeeRefundDto)> RequestRefundAsync(FeeRefundDto dto, string requestedBy, int schoolId)
        {
            var r = new FeeRefund
            {
                FeePaymentId = dto.FeePaymentId,
                StudentId = dto.StudentId,
                RefundAmount = dto.RefundAmount,
                Reason = dto.Reason,
                RefundMode = dto.RefundMode,
                RefundDate = dto.RefundDate == default ? DateTime.Today : dto.RefundDate,
                Status = "Pending",
                Remarks = dto.Remarks,
                SchoolRegistrationId = schoolId,
                CreatedBy = requestedBy
            };
            await _repo.AddAsync(r);
            dto.Id = r.Id;
            return (true, "Refund request submitted.", dto);
        }

        public async Task<IEnumerable<FeeRefundDto>> GetByStudentAsync(int studentId, int schoolId)
            => (await _repo.GetByStudentAsync(studentId, schoolId)).Select(Map);

        public async Task<IEnumerable<FeeRefundDto>> GetPendingAsync(int schoolId)
            => (await _repo.GetPendingAsync(schoolId)).Select(Map);

        public async Task<(bool, string)> ApproveAsync(int id, string approvedBy)
        { var r = await _repo.UpdateStatusAsync(id, "Approved", approvedBy); return r > 0 ? (true, "Approved.") : (false, "Not found."); }

        public async Task<(bool, string)> ProcessAsync(int id, string processedBy, string? refundRef)
        {
            var refund = await _repo.GetByIdAsync(id);
            if (refund == null) return (false, "Not found.");
            refund.Status = "Processed"; refund.RefundRef = refundRef; refund.ApprovedBy = processedBy;
            await _repo.UpdateStatusAsync(id, "Processed", processedBy);

            try
            {
                var email = refund.Student?.ApplicationUser?.Email;
                if (!string.IsNullOrEmpty(email))
                {
                    var placeholders = new Dictionary<string, string>
                    {
                        { "SchoolName", "School SAAS" },
                        { "StudentName", refund.Student?.Name ?? "" },
                        { "RefundRef", refundRef ?? "-" },
                        { "RefundAmount", refund.RefundAmount.ToString("C") },
                        { "RefundDate", refund.RefundDate.ToString("dd-MMM-yyyy") },
                        { "RefundMode", refund.RefundMode },
                        { "Reason", refund.Reason ?? "-" },
                        { "LoginUrl", "http://localhost:4200" }
                    };
                    await _emailService.SendTemplateAsync(email, "Fee Refund Processed", placeholders);
                }
            }
            catch
            {
                // Silently ignore email failures
            }

            return (true, "Refund processed.");
        }

        public async Task<(bool, string)> RejectAsync(int id, string rejectedBy)
        { var r = await _repo.UpdateStatusAsync(id, "Rejected", rejectedBy); return r > 0 ? (true, "Rejected.") : (false, "Not found."); }

        public async Task<decimal> GetTotalRefundedAsync(DateTime from, DateTime to, int schoolId)
            => await _repo.GetTotalRefundedAsync(from, to, schoolId);

        private static FeeRefundDto Map(FeeRefund r) => new()
        {
            Id = r.Id,
            FeePaymentId = r.FeePaymentId,
            ReceiptNo = r.FeePayment?.ReceiptNo,
            StudentId = r.StudentId,
            StudentName = r.Student?.Name ?? "",
            RefundAmount = r.RefundAmount,
            Reason = r.Reason,
            RefundMode = r.RefundMode,
            RefundRef = r.RefundRef,
            RefundDate = r.RefundDate,
            Status = r.Status,
            ApprovedBy = r.ApprovedBy,
            Remarks = r.Remarks
        };
    }
}
