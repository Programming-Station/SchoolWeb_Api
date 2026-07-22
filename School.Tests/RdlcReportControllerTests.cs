using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using School.Services.Interfaces;
using School_API.Controllers;

namespace School.Tests
{
    public class RdlcReportControllerTests
    {
        private readonly Mock<IRdlcCertificateService> _mockRdlcService;
        private readonly Mock<IAdmissionService> _mockAdmissionService;
        private readonly Mock<IRdlcReportManager> _mockReportManager;
        private readonly Mock<IMessageService> _mockMessageService;
        private readonly Mock<IEmailService> _mockEmailService;

        // We can pass null or mock dbContext if not hit in tested endpoints
        private readonly RdlcReportController _controller;

        public RdlcReportControllerTests()
        {
            _mockRdlcService = new Mock<IRdlcCertificateService>();
            _mockAdmissionService = new Mock<IAdmissionService>();
            _mockReportManager = new Mock<IRdlcReportManager>();
            _mockMessageService = new Mock<IMessageService>();
            _mockEmailService = new Mock<IEmailService>();

            _controller = new RdlcReportController(
                _mockRdlcService.Object,
                _mockAdmissionService.Object,
                _mockReportManager.Object,
                null, // DbContext is null as we don't query it for direct receipt/cert endpoints
                _mockMessageService.Object,
                _mockEmailService.Object
            );

            // Mock ControllerContext Request
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "http";
            httpContext.Request.Host = new HostString("localhost", 5000);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public async Task DownloadFeeReceipt_ReturnsPdfFile_WhenReceiptExists()
        {
            // Arrange
            int paymentId = 123;
            var expectedPdfBytes = new byte[] { 1, 2, 3, 4 };
            _mockRdlcService
                .Setup(s => s.GenerateFeeReceiptPdfAsync(paymentId, It.IsAny<string>()))
                .ReturnsAsync(expectedPdfBytes);

            // Act
            var result = await _controller.DownloadFeeReceipt(paymentId);

            // Assert
            var fileResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal("application/pdf", fileResult.ContentType);
            Assert.Equal($"Fee_Receipt_{paymentId}.pdf", fileResult.FileDownloadName);
            Assert.Equal(expectedPdfBytes, fileResult.FileContents);
        }

        [Fact]
        public async Task DownloadFeeReceipt_ReturnsNotFound_WhenPdfGenerationFails()
        {
            // Arrange
            int paymentId = 999;
            _mockRdlcService
                .Setup(s => s.GenerateFeeReceiptPdfAsync(paymentId, It.IsAny<string>()))
                .ReturnsAsync(Array.Empty<byte>());

            // Act
            var result = await _controller.DownloadFeeReceipt(paymentId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
