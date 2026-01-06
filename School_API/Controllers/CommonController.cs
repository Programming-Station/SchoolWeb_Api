using School.Services.Interfaces; 
using School_API.Common.Interface;
using School_DTOs;
using Microsoft.AspNetCore.Mvc;

namespace School_API.Controllers
{
    public class CommonController : BaseController
    {
        private readonly IImageService _imageService;
        public CommonController(IImageService imageService, ICurrentUserService currentUser) : base(currentUser)
        {
            _imageService = imageService;
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

            // Get folderName from form data if not provided as parameter
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

            // Decode the path if it's URL encoded
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
                _ => "application/octet-stream"
            };
        }
    }

}
