using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using QRCoder;
using School.Infrastructure;
using School.Services.Interfaces;
using ZXing;
using ZXing.Common;
using ZXing.Rendering;

namespace School.Services.Reporting
{
    /// <summary>
    /// Enterprise QR Code and Barcode generation service.
    /// Supports QRCoder for QR and ZXing.Net for Code128, EAN13, Code39, DataMatrix barcodes.
    /// All entity-specific generators embed verification URLs from tenant branding.
    /// </summary>
    public class QrBarcodeService : IQrBarcodeService
    {
        private readonly SchoolDbContext _db;
        private readonly IReportingBrandingService _brandingService;
        private readonly IMemoryCache _cache;
        private readonly ILogger<QrBarcodeService> _logger;

        public QrBarcodeService(
            SchoolDbContext db,
            IReportingBrandingService brandingService,
            IMemoryCache cache,
            ILogger<QrBarcodeService> logger)
        {
            _db = db;
            _brandingService = brandingService;
            _cache = cache;
            _logger = logger;
        }

        // ─── QR Code ──────────────────────────────────────────────────────────

        public async Task<byte[]> GenerateQrAsync(string data, int size = 250)
        {
            return await Task.Run(() =>
            {
                using var qrGenerator = new QRCodeGenerator();
                var qrData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
                using var qrCode = new PngByteQRCode(qrData);
                return qrCode.GetGraphic(size / 25); // pixels per module
            });
        }

        public async Task<string> GenerateQrBase64Async(string data, int size = 250)
        {
            var bytes = await GenerateQrAsync(data, size);
            return Convert.ToBase64String(bytes);
        }

        // ─── Barcode ─────────────────────────────────────────────────────────

        public async Task<byte[]> GenerateBarcodeAsync(
            string data, string format = "Code128", int width = 300, int height = 100)
        {
            return await Task.Run(() =>
            {
                var barcodeFormat = format.ToUpperInvariant() switch
                {
                    "EAN13" => BarcodeFormat.EAN_13,
                    "CODE39" => BarcodeFormat.CODE_39,
                    "QR" => BarcodeFormat.QR_CODE,
                    "DATAMATRIX" => BarcodeFormat.DATA_MATRIX,
                    _ => BarcodeFormat.CODE_128
                };

                var writer = new BarcodeWriterPixelData
                {
                    Format = barcodeFormat,
                    Options = new EncodingOptions
                    {
                        Width = width,
                        Height = height,
                        Margin = 5,
                        PureBarcode = format.ToUpperInvariant() != "QR"
                    }
                };

                var pixelData = writer.Write(data);

                // Convert pixel data to PNG using BitmapData approach
                using var bitmap = new System.Drawing.Bitmap(
                    pixelData.Width, pixelData.Height,
                    System.Drawing.Imaging.PixelFormat.Format32bppRgb);

                var bitmapData = bitmap.LockBits(
                    new System.Drawing.Rectangle(0, 0, pixelData.Width, pixelData.Height),
                    System.Drawing.Imaging.ImageLockMode.WriteOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppRgb);

                try
                {
                    System.Runtime.InteropServices.Marshal.Copy(
                        pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }

                using var ms = new MemoryStream();
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            });
        }

        public async Task<string> GenerateBarcodeBase64Async(
            string data, string format = "Code128", int width = 300, int height = 100)
        {
            var bytes = await GenerateBarcodeAsync(data, format, width, height);
            return Convert.ToBase64String(bytes);
        }

        // ─── Entity-specific generators ───────────────────────────────────────

        public async Task<byte[]> GenerateStudentQrAsync(int studentId, int? tenantId = null)
        {
            var branding = await _brandingService.GetBrandingAsync(tenantId);
            var baseUrl = branding.QrVerificationBaseUrl ?? "https://schoolsaas.com/verify";
            var data = $"{baseUrl}/student/{studentId}";
            return await GenerateQrAsync(data, 200);
        }

        public async Task<byte[]> GenerateEmployeeQrAsync(int employeeId, int? tenantId = null)
        {
            var branding = await _brandingService.GetBrandingAsync(tenantId);
            var baseUrl = branding.QrVerificationBaseUrl ?? "https://schoolsaas.com/verify";
            var data = $"{baseUrl}/employee/{employeeId}";
            return await GenerateQrAsync(data, 200);
        }

        public async Task<byte[]> GenerateFeeReceiptQrAsync(int paymentId, int? tenantId = null)
        {
            var branding = await _brandingService.GetBrandingAsync(tenantId);
            var baseUrl = branding.QrVerificationBaseUrl ?? "https://schoolsaas.com/verify";
            var data = $"{baseUrl}/fee-receipt/{paymentId}";
            return await GenerateQrAsync(data, 200);
        }

        public async Task<byte[]> GenerateCertificateQrAsync(string certificateNumber, int? tenantId = null)
        {
            var branding = await _brandingService.GetBrandingAsync(tenantId);
            var baseUrl = branding.QrVerificationBaseUrl ?? "https://schoolsaas.com/verify";
            var data = $"{baseUrl}/certificate/{Uri.EscapeDataString(certificateNumber)}";
            return await GenerateQrAsync(data, 200);
        }

        public async Task<byte[]> GenerateBookBarcodeAsync(string isbn, int? tenantId = null)
        {
            var branding = await _brandingService.GetBrandingAsync(tenantId);
            var prefix = branding.BarcodePrefix ?? "LIB";
            return await GenerateBarcodeAsync($"{prefix}-{isbn}", "Code128", 280, 80);
        }

        public async Task<byte[]> GenerateInventoryBarcodeAsync(string itemCode, int? tenantId = null)
        {
            var branding = await _brandingService.GetBrandingAsync(tenantId);
            var prefix = branding.BarcodePrefix ?? "INV";
            return await GenerateBarcodeAsync($"{prefix}-{itemCode}", "Code128", 280, 80);
        }

        public async Task<byte[]> GenerateStudentIdBarcodeAsync(string studentId, int? tenantId = null)
        {
            var branding = await _brandingService.GetBrandingAsync(tenantId);
            var prefix = branding.BarcodePrefix ?? "STU";
            return await GenerateBarcodeAsync($"{prefix}{studentId}", "Code128", 250, 70);
        }
    }
}
