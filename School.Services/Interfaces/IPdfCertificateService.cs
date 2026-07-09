using School_DTOs.Student;

namespace School.Services.Interfaces
{
    public interface IPdfCertificateService
    {
        Task<byte[]> GenerateRegistrationCertificateAsync(AdmissionApplicationDto registration, string baseUrl);
        Task<byte[]> GenerateFeeReceiptPdfAsync(int paymentId, string baseUrl);
    }
}
