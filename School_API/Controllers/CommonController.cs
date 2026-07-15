using School.Services.Interfaces; 
using School_API.Common.Interface;
using School_DTOs;
using Microsoft.AspNetCore.Mvc;

namespace School_API.Controllers
{
    public class CommonController : BaseController
    {
        private readonly IImageService _imageService;
        private readonly IDocumentService _documentService;
        public CommonController(IImageService imageService, IDocumentService documentService, ICurrentUserService currentUser) : base(currentUser)
        {
            _imageService = imageService;
            _documentService = documentService;
        }

        /// <summary>
        /// Upload an image file
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file, string? folderName = null)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new APIResponse<string>
                {
                    Message = "File is required",
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                });
            }

            if (string.IsNullOrWhiteSpace(folderName) && Request.Form.ContainsKey("folderName"))
            {
                folderName = Request.Form["folderName"].ToString();
            }

            var imagePath = await _imageService.UploadImageAsync(file, folderName);
            return Ok(new APIResponse<string>
            {
                Data = imagePath,
                Success = true,
                Message = "Image uploaded successfully"
            });

        }

        /// <summary>
        /// Get/Serve an image file from storage path
        /// </summary>
        [HttpGet]
        public IActionResult GetImage([FromQuery] string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return BadRequest(new APIResponse<string>
                {
                    Message = "Image path is required",
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                });
            }

            var decodedPath = Uri.UnescapeDataString(path);
            
            var filePath = _imageService.GetImageFilePath(decodedPath);
            
            if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
            {
                return NotFound(new APIResponse<string>
                {
                    Message = "Image not found",
                    StatusCode = System.Net.HttpStatusCode.NotFound
                });
            }

            var fileInfo = new System.IO.FileInfo(filePath);
            var contentType = GetContentType(fileInfo.Extension);
            var fileStream = System.IO.File.OpenRead(filePath);
            
            return File(fileStream, contentType, fileInfo.Name);
        }

        [HttpPost]
        public async Task<IActionResult> UploadDocument(IFormFile file, [FromQuery] string? folderName = null, [FromQuery] bool compress = false)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new APIResponse<string> { Message = "File is required", Success = false });
            }
            var path = await _documentService.UploadAsync(file, folderName, compress);
            return Ok(new APIResponse<string> { Data = path, Success = true, Message = "Document uploaded successfully" });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteDocument([FromQuery] string path)
        {
            var result = await _documentService.DeleteAsync(path);
            return Ok(new APIResponse<bool> { Data = result, Success = result, Message = result ? "Document deleted" : "Failed to delete document" });
        }

        [HttpGet]
        public async Task<IActionResult> DownloadDocument([FromQuery] string path)
        {
            try
            {
                var bytes = await _documentService.DownloadAsync(path);
                var fileName = Path.GetFileName(path);
                var extension = Path.GetExtension(path);
                return File(bytes, GetContentType(extension), fileName);
            }
            catch (Exception ex)
            {
                return NotFound(new APIResponse<string> { Message = ex.Message, Success = false });
            }
        }

        private string GetContentType(string extension)
        {
            return extension.ToLowerInvariant() switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                ".bmp" => "image/bmp",
                ".svg" => "image/svg+xml",
                ".pdf" => "application/pdf",
                ".doc" or ".docx" => "application/msword",
                ".xls" or ".xlsx" => "application/vnd.ms-excel",
                ".zip" => "application/zip",
                _ => "application/octet-stream"
            };
        }
    }

}
