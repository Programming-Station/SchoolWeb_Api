using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using School.Services.Interfaces;
using School.Infrastructure.Interfaces;
using School_API.Common.Interface;
using School_API.Controllers.Administration;
using School_DTOs;
using Xunit;

namespace School.Tests
{
    public class AdministrationControllerTests
    {
        private readonly Mock<IAdministrationService> _mockAdminService;
        private readonly Mock<ITenantService> _mockTenantService;
        private readonly Mock<ICurrentUserService> _mockCurrentUserService;
        private readonly AdministrationController _controller;

        public AdministrationControllerTests()
        {
            _mockAdminService = new Mock<IAdministrationService>();
            _mockTenantService = new Mock<ITenantService>();
            _mockCurrentUserService = new Mock<ICurrentUserService>();

            _mockTenantService.Setup(s => s.GetTenantId()).Returns(1);

            _controller = new AdministrationController(
                _mockAdminService.Object,
                _mockTenantService.Object,
                _mockCurrentUserService.Object
            );
        }

        [Fact]
        public async Task ExportData_ReturnsCsvFile_WhenSuccessful()
        {
            // Arrange
            string entityName = "Student";
            var csvBytes = Encoding.UTF8.GetBytes("Id,Name\n1,Jane Doe");
            var serviceResponse = new APIResponse<byte[]>
            {
                Success = true,
                Data = csvBytes
            };

            _mockAdminService
                .Setup(s => s.ExportDataAsync(entityName, 1))
                .ReturnsAsync(serviceResponse);

            // Act
            var result = await _controller.ExportData(entityName);

            // Assert
            var fileResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal("text/csv", fileResult.ContentType);
            Assert.Equal($"{entityName}_export.csv", fileResult.FileDownloadName);
            Assert.Equal(csvBytes, fileResult.FileContents);
        }

        [Fact]
        public async Task ImportData_ReturnsOk_WhenUploadIsSuccessful()
        {
            // Arrange
            string entityName = "Student";
            var csvContent = "Id,Name\n1,Jane Doe";
            var fileMock = new Mock<IFormFile>();
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));
            fileMock.Setup(f => f.OpenReadStream()).Returns(ms);
            fileMock.Setup(f => f.FileName).Returns("students.csv");
            fileMock.Setup(f => f.Length).Returns(ms.Length);

            fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<System.Threading.CancellationToken>()))
                .Callback<Stream, System.Threading.CancellationToken>((stream, token) =>
                {
                    ms.Position = 0;
                    ms.CopyTo(stream);
                })
                .Returns(Task.CompletedTask);

            var serviceResponse = new APIResponse<bool>
            {
                Success = true,
                Message = "Import completed successfully."
            };

            _mockAdminService
                .Setup(s => s.ImportDataAsync(entityName, It.IsAny<byte[]>(), 1))
                .ReturnsAsync(serviceResponse);

            // Act
            var result = await _controller.ImportData(entityName, fileMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<APIResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal("Import completed successfully.", response.Message);
        }
    }
}
