using System.Collections.Generic;
using System.Threading.Tasks;
using School_DTOs;
using School_DTOs.Administration;

namespace School.Services.Interfaces
{
    public interface ICertificateService
    {
        Task<APIResponse<List<CertificateIssuanceLogDto>>> GetCertificatesAsync(CertificateFilterDto filter, int schoolId);
        Task<APIResponse<CertificateIssuanceLogDto>> GetCertificateByIdAsync(int id, int schoolId);
        Task<APIResponse<CertificateIssuanceLogDto>> IssueCertificateAsync(CreateCertificateIssuanceDto dto, string userId, string userName, int schoolId);
        Task<APIResponse<bool>> RevokeCertificateAsync(int id, string reason, int schoolId);
        Task<APIResponse<bool>> IncrementPrintCountAsync(int id, int schoolId);
        Task<APIResponse<byte[]>> GeneratePdfAsync(int id, int schoolId);
    }
}
