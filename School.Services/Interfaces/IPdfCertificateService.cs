using School_DTOs.Student;

namespace School.Services.Interfaces
{
    public interface IPdfCertificateService
    {
        Task<byte[]> GenerateRegistrationCertificateAsync(StudentRegistrationDto registration, string baseUrl);
        Task<byte[]> GenerateFeeReceiptPdfAsync(int paymentId, string baseUrl);
    }
}

