using School_DTOs.Student;

namespace School.Services.Interfaces
{
    public interface IRdlcCertificateService
    {
        Task<byte[]> GenerateRegistrationCertificateAsync(AdmissionApplicationDto registration, string baseUrl);
        Task<byte[]> GenerateFeeReceiptPdfAsync(int paymentId, string baseUrl);

        // ── New Student Domain Reports ──────────────────────────────────────────
        Task<byte[]> GenerateAdmissionApplicationSummaryAsync(int applicationId);
        Task<byte[]> GenerateAdmissionsPipelineReportAsync(string? status, string? courseName, DateTime? fromDate, DateTime? toDate);
        Task<byte[]> GenerateStudentClassDirectoryAsync(int classId, int? sectionId);
        Task<byte[]> GenerateAdmissionAuditTrailReportAsync(DateTime? fromDate, DateTime? toDate);
        Task<byte[]> GenerateAcademicQualificationAnalysisAsync(string? boardName, string? passingYear);
        Task<byte[]> GenerateParentStudentEmergencyDirectoryAsync(int classId);
        Task<byte[]> GenerateAdmissionDeviationReportAsync();
    }
}
