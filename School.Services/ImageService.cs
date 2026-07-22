using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using School.Services.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

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

            var configuredPath = _configuration.GetSection("AppSettings:ImageStoragePath").Value;

            if (string.IsNullOrWhiteSpace(configuredPath))
            {
                _imageStoragePath = Path.Combine(_environment.ContentRootPath, "wwwroot", UploadsFolder);
            }
            else
            {
                _imageStoragePath = Path.GetFullPath(configuredPath.Trim().Replace('/', Path.DirectorySeparatorChar));
            }

            _logger.LogInformation($"Image storage path configured: {_imageStoragePath}");

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
                folderName = string.IsNullOrWhiteSpace(folderName) ? "Images" : folderName;

                if (file == null || file.Length == 0)
                {
                    throw new ArgumentException("File is empty or null");
                }

                if (file.Length > MaxFileSizeBytes)
                {
                    throw new ArgumentException($"File size exceeds maximum allowed size of {MaxFileSizeMB}MB");
                }

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    throw new ArgumentException($"File type {fileExtension} is not allowed. Allowed types: {string.Join(", ", allowedExtensions)}");
                }

                var uploadsPath = Path.Combine(_imageStoragePath, UploadsFolder, folderName);
                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                    _logger.LogInformation($"Created folder: {uploadsPath}");
                }

                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(uploadsPath, fileName);

                using var fileStream = file.OpenReadStream();
                MemoryStream processedStream;

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

                using (var outputStream = new FileStream(filePath, FileMode.Create))
                {
                    await processedStream.CopyToAsync(outputStream);
                }

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

                if (Path.IsPathRooted(imagePath) && (imagePath.Length >= 2 && imagePath[1] == ':' || imagePath.StartsWith("\\\\")))
                {
                    fullPath = imagePath;

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

                if (originalSize <= targetSizeBytes)
                {
                    var result = new MemoryStream();
                    imageStream.Position = 0;
                    await imageStream.CopyToAsync(result);
                    result.Position = 0;
                    return result;
                }

                var currentWidth = image.Width;
                var currentHeight = image.Height;
                var aspectRatio = (double)currentWidth / currentHeight;

                int targetWidth = (int)(currentWidth * 0.8);
                int targetHeight = (int)(currentHeight * 0.8);

                if (targetWidth < 100) targetWidth = 100;
                if (targetHeight < 100) targetHeight = 100;

                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(targetWidth, targetHeight),
                    Mode = ResizeMode.Max
                }));

                var outputStream = new MemoryStream();
                var encoder = new JpegEncoder
                {
                    Quality = quality
                };

                await image.SaveAsync(outputStream, encoder);
                outputStream.Position = 0;

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

                if (Path.IsPathRooted(imagePath) && (imagePath.Length >= 2 && imagePath[1] == ':' || imagePath.StartsWith("\\\\")))
                {
                    fullPath = imagePath;

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
                    var cleanPath = imagePath.TrimStart('/', '\\');
                    fullPath = Path.Combine(_imageStoragePath, cleanPath);
                }

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
