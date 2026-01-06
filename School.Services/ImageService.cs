using School.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace School.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<ImageService> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _imageStoragePath;
        private const string UploadsFolder = "uploads";
        private const int MaxFileSizeMB = 5;
        private const int MaxFileSizeBytes = 5 * 1024 * 1024; // 5MB
        private const int DefaultMaxSizeKB = 1024; // 1MB
        private const int DefaultQuality = 85;

        public ImageService(IWebHostEnvironment environment, ILogger<ImageService> logger, IConfiguration configuration)
        {
            _environment = environment;
            _logger = logger;
            _configuration = configuration;

            // Get image storage path from appsettings
            var configuredPath = _configuration.GetSection("AppSettings:ImageStoragePath").Value;

            if (string.IsNullOrWhiteSpace(configuredPath))
            {
                // Fallback to default path if not configured
                _imageStoragePath = Path.Combine(_environment.ContentRootPath, "wwwroot", UploadsFolder);
            }
            else
            {
                // Normalize the path (convert forward slashes to backslashes on Windows, ensure trailing separator is removed)
                _imageStoragePath = Path.GetFullPath(configuredPath.Trim().Replace('/', Path.DirectorySeparatorChar));
            }

            _logger.LogInformation($"Image storage path configured: {_imageStoragePath}");

            // Ensure the base storage directory exists
            if (!Directory.Exists(_imageStoragePath))
            {
                Directory.CreateDirectory(_imageStoragePath);
                _logger.LogInformation($"Created image storage directory: {_imageStoragePath}");
            }
        }

        public async Task<string> UploadImageAsync(IFormFile file, string? folderName = null)
        {
            try
            {
                // Use provided folderName or default to "Images"
                folderName = string.IsNullOrWhiteSpace(folderName) ? "Images" : folderName;

                // Validate file
                if (file == null || file.Length == 0)
                {
                    throw new ArgumentException("File is empty or null");
                }

                if (file.Length > MaxFileSizeBytes)
                {
                    throw new ArgumentException($"File size exceeds maximum allowed size of {MaxFileSizeMB}MB");
                }

                // Validate file type
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    throw new ArgumentException($"File type {fileExtension} is not allowed. Allowed types: {string.Join(", ", allowedExtensions)}");
                }

                // Create folder structure: {ImageStoragePath}/{folderName}
                var uploadsPath = Path.Combine(_imageStoragePath, UploadsFolder, folderName);
                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                    _logger.LogInformation($"Created folder: {uploadsPath}");
                }

                // Generate unique filename
                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(uploadsPath, fileName);

                // Read file stream
                using var fileStream = file.OpenReadStream();
                MemoryStream processedStream;

                // Check if file needs resizing (> 1MB)
                if (file.Length > DefaultMaxSizeKB * 1024)
                {
                    _logger.LogInformation($"Image size ({file.Length / 1024}KB) exceeds 1MB, resizing...");
                    processedStream = await ResizeImageIfNeededAsync(fileStream, DefaultMaxSizeKB, DefaultQuality);
                }
                else
                {
                    processedStream = new MemoryStream();
                    await fileStream.CopyToAsync(processedStream);
                    processedStream.Position = 0;
                }

                // Save file
                using (var outputStream = new FileStream(filePath, FileMode.Create))
                {
                    await processedStream.CopyToAsync(outputStream);
                }

                // Return full path
                _logger.LogInformation($"Image saved successfully: {filePath}");
                return filePath;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error uploading image: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteImageAsync(string imagePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(imagePath))
                {
                    return false;
                }

                string fullPath;

                // Check if imagePath is already a full path (starts with drive letter or root)
                if (Path.IsPathRooted(imagePath) && (imagePath.Length >= 2 && imagePath[1] == ':' || imagePath.StartsWith("\\\\")))
                {
                    // It's already a full path, use it directly
                    fullPath = imagePath;

                    // Security check: ensure path is within the configured image storage path
                    var normalizedImagePath = Path.GetFullPath(imagePath);
                    var normalizedStoragePath = Path.GetFullPath(_imageStoragePath);

                    if (!normalizedImagePath.StartsWith(normalizedStoragePath, StringComparison.OrdinalIgnoreCase))
                    {
                        _logger.LogWarning($"Attempted to delete file outside image storage path: {imagePath}");
                        return false;
                    }
                }
                else
                {
                    // It's a relative path, construct full path from storage directory
                    var cleanPath = imagePath.TrimStart('/', '\\');
                    fullPath = Path.Combine(_imageStoragePath, cleanPath);
                }

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    _logger.LogInformation($"Image deleted successfully: {fullPath}");
                    return true;
                }

                _logger.LogWarning($"Image not found: {fullPath}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting image: {ex.Message}");
                return false;
            }
        }

        public async Task<MemoryStream> ResizeImageIfNeededAsync(Stream imageStream, int maxSizeKB = DefaultMaxSizeKB, int quality = DefaultQuality)
        {
            try
            {
                imageStream.Position = 0;
                using var image = await Image.LoadAsync(imageStream);

                var originalSize = imageStream.Length;
                var targetSizeBytes = maxSizeKB * 1024;

                // If already smaller than target, return as is
                if (originalSize <= targetSizeBytes)
                {
                    var result = new MemoryStream();
                    imageStream.Position = 0;
                    await imageStream.CopyToAsync(result);
                    result.Position = 0;
                    return result;
                }

                // Calculate target dimensions (maintain aspect ratio)
                var currentWidth = image.Width;
                var currentHeight = image.Height;
                var aspectRatio = (double)currentWidth / currentHeight;

                // Start with 80% of original size and iterate if needed
                int targetWidth = (int)(currentWidth * 0.8);
                int targetHeight = (int)(currentHeight * 0.8);

                // Ensure minimum dimensions
                if (targetWidth < 100) targetWidth = 100;
                if (targetHeight < 100) targetHeight = 100;

                // Resize image
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(targetWidth, targetHeight),
                    Mode = ResizeMode.Max
                }));

                // Save to memory stream with compression
                var outputStream = new MemoryStream();
                var encoder = new JpegEncoder
                {
                    Quality = quality
                };

                await image.SaveAsync(outputStream, encoder);
                outputStream.Position = 0;

                // If still too large, reduce quality and try again
                int attempts = 0;
                while (outputStream.Length > targetSizeBytes && attempts < 5)
                {
                    quality = Math.Max(50, quality - 10); // Reduce quality by 10, minimum 50
                    outputStream.Dispose();
                    outputStream = new MemoryStream();

                    imageStream.Position = 0;
                    using var reloadedImage = await Image.LoadAsync(imageStream);
                    reloadedImage.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(targetWidth, targetHeight),
                        Mode = ResizeMode.Max
                    }));

                    encoder = new JpegEncoder { Quality = quality };
                    await reloadedImage.SaveAsync(outputStream, encoder);
                    outputStream.Position = 0;
                    attempts++;
                }

                // If still too large, reduce dimensions
                if (outputStream.Length > targetSizeBytes)
                {
                    targetWidth = (int)(targetWidth * 0.8);
                    targetHeight = (int)(targetHeight * 0.8);

                    imageStream.Position = 0;
                    using var finalImage = await Image.LoadAsync(imageStream);
                    finalImage.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(targetWidth, targetHeight),
                        Mode = ResizeMode.Max
                    }));

                    outputStream.Dispose();
                    outputStream = new MemoryStream();
                    encoder = new JpegEncoder { Quality = 75 };
                    await finalImage.SaveAsync(outputStream, encoder);
                    outputStream.Position = 0;
                }

                _logger.LogInformation($"Image resized from {originalSize / 1024}KB to {outputStream.Length / 1024}KB");
                return outputStream;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error resizing image: {ex.Message}");
                throw;
            }
        }

        public string? GetImageFilePath(string imagePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(imagePath))
                {
                    return null;
                }

                string fullPath;

                // Check if imagePath is already a full path (starts with drive letter or root)
                if (Path.IsPathRooted(imagePath) && (imagePath.Length >= 2 && imagePath[1] == ':' || imagePath.StartsWith("\\\\")))
                {
                    // It's already a full path, use it directly
                    fullPath = imagePath;
                    
                    // Security check: ensure path is within the configured image storage path
                    var normalizedImagePath = Path.GetFullPath(imagePath);
                    var normalizedStoragePath = Path.GetFullPath(_imageStoragePath);
                    
                    if (!normalizedImagePath.StartsWith(normalizedStoragePath, StringComparison.OrdinalIgnoreCase))
                    {
                        _logger.LogWarning($"Attempted to access file outside image storage path: {imagePath}");
                        return null;
                    }
                }
                else
                {
                    // It's a relative path, construct full path from storage directory
                    var cleanPath = imagePath.TrimStart('/', '\\');
                    fullPath = Path.Combine(_imageStoragePath, cleanPath);
                }

                // Verify file exists
                if (File.Exists(fullPath))
                {
                    return fullPath;
                }

                _logger.LogWarning($"Image file not found: {fullPath}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting image file path: {ex.Message}");
                return null;
            }
        }
    }
}
