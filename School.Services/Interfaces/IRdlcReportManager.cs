namespace School.Services.Interfaces
{
    public interface IRdlcReportManager
    {
        Task<byte[]> RenderReportAsync(
            string reportName,
            string renderType,
            Dictionary<string, object> dataSources,
            Dictionary<string, string> parameters);

        Task<byte[]> GenerateQrCodeBase64Async(string data);
        Task<byte[]> GenerateBarcodeBase64Async(string data);
    }
}
