using System.Threading.Tasks;

namespace School.Services.Interfaces
{
    public interface IReceiptService
    {
        Task<byte[]> GenerateReceiptPdfAsync(int paymentId, string baseUrl);
    }
}
