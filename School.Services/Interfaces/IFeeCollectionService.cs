using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace School.Services.Interfaces
{
    public class FeeInstallmentDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = null!;
        public string? EnrollmentNumber { get; set; }
        public int FeeStructureId { get; set; }
        public string FeeStructureName { get; set; } = null!;
        public int InstallmentNo { get; set; }
        public string InstallmentName { get; set; } = null!;
        public decimal Amount { get; set; }
        public decimal FineAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal NetPayable => Amount + FineAmount - DiscountAmount;
        public DateTime DueDate { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime? PaidDate { get; set; }
        public string? Remarks { get; set; }
    }

    public class FeePaymentDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = null!;
        public int FeeInstallmentId { get; set; }
        public string InstallmentName { get; set; } = null!;
        public decimal AmountPaid { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMode { get; set; } = "Cash";
        public string? TransactionRef { get; set; }
        public string ReceiptNo { get; set; } = null!;
        public string? CollectedBy { get; set; }
        public string? Remarks { get; set; }
        public string Status { get; set; } = "Completed";
    }

    public class CollectFeeRequest
    {
        public int StudentId { get; set; }
        public int FeeInstallmentId { get; set; }
        public decimal AmountPaid { get; set; }
        public string PaymentMode { get; set; } = "Cash";
        public string? TransactionRef { get; set; }
        public string? Remarks { get; set; }
    }

    public class GenerateInstallmentsRequest
    {
        public int StudentId { get; set; }
        public int FeeStructureId { get; set; }
        public int NumberOfInstallments { get; set; } = 1;
        public DateTime FirstDueDate { get; set; }
    }

    public class FeeCollectionSummaryDto
    {
        public decimal TotalCollected { get; set; }
        public decimal TotalPending { get; set; }
        public int TotalStudents { get; set; }
        public int PaidStudents { get; set; }
        public int PendingStudents { get; set; }
        public int OverdueStudents { get; set; }
    }

    public interface IFeeCollectionService
    {
        Task<(bool Success, string Message, List<FeeInstallmentDto> Installments)> GenerateInstallmentsAsync(GenerateInstallmentsRequest request, string createdBy, int schoolRegistrationId);
        Task<(bool Success, string Message, FeePaymentDto Payment)> CollectFeeAsync(CollectFeeRequest request, string collectedBy, int schoolRegistrationId);
        Task<IEnumerable<FeeInstallmentDto>> GetInstallmentsByStudentAsync(int studentId, int schoolRegistrationId);
        Task<IEnumerable<FeeInstallmentDto>> GetPendingByStudentAsync(int studentId, int schoolRegistrationId);
        Task<IEnumerable<FeeInstallmentDto>> GetOverdueAsync(int schoolRegistrationId);
        Task<IEnumerable<FeePaymentDto>> GetPaymentsByStudentAsync(int studentId, int schoolRegistrationId);
        Task<IEnumerable<FeePaymentDto>> GetPaymentsByDateRangeAsync(DateTime from, DateTime to, int schoolRegistrationId);
        Task<FeeCollectionSummaryDto> GetCollectionSummaryAsync(DateTime from, DateTime to, int schoolRegistrationId);
        Task<FeePaymentDto?> GetPaymentByReceiptAsync(string receiptNo, int schoolRegistrationId);
        Task<IEnumerable<FeeInstallmentDto>> GetPendingByClassAsync(int classId, int schoolRegistrationId);
    }
}
