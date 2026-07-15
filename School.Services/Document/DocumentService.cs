using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using School.Services.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;

namespace School.Services.DocumentManagement
{
    public class DocumentMetadata
    {
        public string OriginalFileName { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string MimeType { get; set; } = string.Empty;
        public string Hash { get; set; } = string.Empty;
        public string UploadedBy { get; set; } = string.Empty;
        public DateTime UploadedOn { get; set; }
        public int? TenantId { get; set; }
        public int? BranchId { get; set; }
    }

    public class DocumentService : IDocumentService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<DocumentService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _storagePath;
        private const string UploadsFolder = "uploads";
        private const int MaxFileSizeBytes = 10 * 1024 * 1024; // 10MB default
        private const int DefaultQuality = 80;

        public DocumentService(
            IWebHostEnvironment environment, 
            ILogger<DocumentService> logger, 
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _environment = environment;
            _logger = logger;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;

            var configuredPath = _configuration.GetSection("AppSettings:ImageStoragePath").Value;

            if (string.IsNullOrWhiteSpace(configuredPath))
            {
                _storagePath = Path.Combine(_environment.ContentRootPath, "wwwroot", UploadsFolder);
            }
            else
            {
                _storagePath = Path.GetFullPath(configuredPath.Trim().Replace('/', Path.DirectorySeparatorChar));
            }

            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }
        }

        private (int? tenantId, int? branchId, string userId) GetSessionDetails()
        {
            int? tenantId = null;
            int? branchId = null;
            string userId = "System";

            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                var user = httpContext.User;
                if (user != null)
                {
                    var tenantClaim = user.Claims.FirstOrDefault(c => c.Type == "TenantId" || c.Type == "SchoolRegistrationId");
                    if (tenantClaim != null && int.TryParse(tenantClaim.Value, out int tId))
                    {
                        tenantId = tId;
                    }

                    var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                    if (userIdClaim != null)
                    {
                        userId = userIdClaim.Value;
                    }
                }

                if (httpContext.Request.Headers.TryGetValue("X-Branch-Id", out var branchHeader) && 
                    int.TryParse(branchHeader.ToString(), out int bId))
                {
                    branchId = bId;
                }
                else if (httpContext.Request.Query.TryGetValue("branchId", out var branchQuery) && 
                         int.TryParse(branchQuery.ToString(), out int bIdQuery))
                {
                    branchId = bIdQuery;
                }
            }

            return (tenantId, branchId, userId);
        }

        private string GetIsolatedDirectory(string folderName)
        {
            var (tenantId, branchId, _) = GetSessionDetails();
            var path = _storagePath;

            if (tenantId.HasValue)
            {
                path = Path.Combine(path, $"tenant_{tenantId.Value}");
            }
            if (branchId.HasValue)
            {
                path = Path.Combine(path, $"branch_{branchId.Value}");
            }

            path = Path.Combine(path, folderName);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        public async Task<string> UploadAsync(IFormFile file, string? folderName = null, bool compress = false, int? targetWidth = null, int? targetHeight = null)
        {
            try
            {
                await ValidateFileAsync(file);

                folderName = string.IsNullOrWhiteSpace(folderName) ? "Documents" : folderName;
                var destinationDir = GetIsolatedDirectory(folderName);

                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(destinationDir, uniqueFileName);

                using var fileStream = file.OpenReadStream();
                var isImage = new[] { ".jpg", ".jpeg", ".png", ".webp" }.Contains(fileExtension);

                if (isImage && (compress || targetWidth.HasValue || targetHeight.HasValue))
                {
                    using var image = await Image.LoadAsync(fileStream);
                    
                    if (targetWidth.HasValue || targetHeight.HasValue)
                    {
                        int w = targetWidth ?? image.Width;
                        int h = targetHeight ?? image.Height;
                        image.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Size = new Size(w, h),
                            Mode = ResizeMode.Max
                        }));
                    }

                    var encoder = new JpegEncoder { Quality = DefaultQuality };
                    await image.SaveAsync(filePath, encoder);
                }
                else
                {
                    using var outputStream = new FileStream(filePath, FileMode.Create);
                    await fileStream.CopyToAsync(outputStream);
                }

                // Write metadata sidecar file
                fileStream.Position = 0;
                var hash = ComputeHash(fileStream);
                var (tenantId, branchId, userId) = GetSessionDetails();

                var metadata = new DocumentMetadata
                {
                    OriginalFileName = file.FileName,
                    FileSize = file.Length,
                    MimeType = file.ContentType,
                    Hash = hash,
                    UploadedBy = userId,
                    UploadedOn = DateTime.UtcNow,
                    TenantId = tenantId,
                    BranchId = branchId
                };

                var metadataPath = $"{filePath}.metadata.json";
                var json = JsonSerializer.Serialize(metadata, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(metadataPath, json);

                _logger.LogInformation($"File and metadata uploaded successfully: {filePath}");
                
                // Return web relative path starting with /uploads/
                var relativePath = Path.GetRelativePath(_storagePath, filePath).Replace('\\', '/');
                return $"/uploads/{relativePath}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error uploading document: {ex.Message}");
                throw;
            }
        }

        public async Task<string> UploadAsync(byte[] fileBytes, string fileName, string? folderName = null)
        {
            try
            {
                if (fileBytes == null || fileBytes.Length == 0)
                {
                    throw new ArgumentException("File bytes are empty or null");
                }

                folderName = string.IsNullOrWhiteSpace(folderName) ? "Documents" : folderName;
                var destinationDir = GetIsolatedDirectory(folderName);

                var fileExtension = Path.GetExtension(fileName).ToLowerInvariant();
                var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(destinationDir, uniqueFileName);

                await File.WriteAllBytesAsync(filePath, fileBytes);

                using var ms = new MemoryStream(fileBytes);
                var hash = ComputeHash(ms);
                var (tenantId, branchId, userId) = GetSessionDetails();

                var metadata = new DocumentMetadata
                {
                    OriginalFileName = fileName,
                    FileSize = fileBytes.Length,
                    MimeType = GetContentType(fileExtension),
                    Hash = hash,
                    UploadedBy = userId,
                    UploadedOn = DateTime.UtcNow,
                    TenantId = tenantId,
                    BranchId = branchId
                };

                var metadataPath = $"{filePath}.metadata.json";
                var json = JsonSerializer.Serialize(metadata, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(metadataPath, json);

                _logger.LogInformation($"File (bytes) and metadata uploaded successfully: {filePath}");

                var relativePath = Path.GetRelativePath(_storagePath, filePath).Replace('\\', '/');
                return $"/uploads/{relativePath}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error uploading document bytes: {ex.Message}");
                throw;
            }
        }

        public async Task<string> ReplaceAsync(string existingFilePath, IFormFile newFile, bool compress = false)
        {
            await DeleteAsync(existingFilePath);
            
            string folderName = "Documents";
            if (!string.IsNullOrEmpty(existingFilePath))
            {
                var parts = existingFilePath.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length > 1)
                {
                    folderName = parts[parts.Length - 2];
                }
            }

            return await UploadAsync(newFile, folderName, compress);
        }

        public async Task<bool> DeleteAsync(string filePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    return false;
                }

                string fullPath = ResolveFullPath(filePath);

                if (File.Exists(fullPath))
                {
                    var trashDir = Path.Combine(_storagePath, ".trash");
                    if (!Directory.Exists(trashDir))
                    {
                        Directory.CreateDirectory(trashDir);
                    }

                    var fileName = Path.GetFileName(fullPath);
                    var trashPath = Path.Combine(trashDir, fileName);
                    
                    if (File.Exists(trashPath))
                    {
                        File.Delete(trashPath);
                    }

                    File.Move(fullPath, trashPath);

                    // Move metadata too if exists
                    var metadataPath = $"{fullPath}.metadata.json";
                    if (File.Exists(metadataPath))
                    {
                        var trashMetadataPath = $"{trashPath}.metadata.json";
                        if (File.Exists(trashMetadataPath))
                        {
                            File.Delete(trashMetadataPath);
                        }
                        File.Move(metadataPath, trashMetadataPath);
                    }

                    _logger.LogInformation($"File moved to trash: {trashPath}");
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting file: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RestoreAsync(string trashedFilePath)
        {
            try
            {
                var fileName = Path.GetFileName(trashedFilePath);
                var trashPath = Path.Combine(_storagePath, ".trash", fileName);

                if (File.Exists(trashPath))
                {
                    var restoreDir = GetIsolatedDirectory("Restored");
                    var restorePath = Path.Combine(restoreDir, fileName);
                    File.Move(trashPath, restorePath);

                    // Restore metadata too if exists
                    var trashMetadataPath = $"{trashPath}.metadata.json";
                    if (File.Exists(trashMetadataPath))
                    {
                        var restoreMetadataPath = $"{restorePath}.metadata.json";
                        File.Move(trashMetadataPath, restoreMetadataPath);
                    }

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error restoring file: {ex.Message}");
                return false;
            }
        }

        public async Task<byte[]> DownloadAsync(string filePath)
        {
            string fullPath = ResolveFullPath(filePath);
            if (File.Exists(fullPath))
            {
                return await File.ReadAllBytesAsync(fullPath);
            }
            throw new FileNotFoundException("File not found", fullPath);
        }

        public async Task<string?> GetThumbnailPathAsync(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            if (new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" }.Contains(extension))
            {
                return filePath;
            }
            return "/assets/images/document-icon.png";
        }

        public async Task<string?> GetPreviewPathAsync(string filePath)
        {
            return filePath;
        }

        public Task<bool> ValidateFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is empty or null");
            }

            if (file.Length > MaxFileSizeBytes)
            {
                throw new ArgumentException($"File size exceeds maximum allowed limit of {MaxFileSizeBytes / 1024 / 1024}MB");
            }

            var allowedExtensions = new[] 
            { 
                ".jpg", ".jpeg", ".png", ".gif", ".webp",
                ".pdf", ".doc", ".docx", ".xls", ".xlsx",
                ".ppt", ".pptx", ".zip", ".rar", ".txt",
                ".csv", ".json", ".xml", ".apk"
            };

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
            {
                throw new ArgumentException($"File type '{extension}' is not allowed.");
            }

            return Task.FromResult(true);
        }

        public async Task<string> RenameAsync(string filePath, string newFileName)
        {
            string fullPath = ResolveFullPath(filePath);
            if (File.Exists(fullPath))
            {
                var dir = Path.GetDirectoryName(fullPath) ?? _storagePath;
                var extension = Path.GetExtension(fullPath);
                var newPath = Path.Combine(dir, $"{newFileName}{extension}");
                File.Move(fullPath, newPath);

                // Rename metadata file too
                var metadataPath = $"{fullPath}.metadata.json";
                if (File.Exists(metadataPath))
                {
                    var newMetadataPath = $"{newPath}.metadata.json";
                    File.Move(metadataPath, newMetadataPath);
                }
                
                var relativePath = Path.GetRelativePath(_storagePath, newPath).Replace('\\', '/');
                return $"/uploads/{relativePath}";
            }
            throw new FileNotFoundException("File not found", fullPath);
        }

        public string ComputeHash(Stream stream)
        {
            using var sha = SHA256.Create();
            var hashBytes = sha.ComputeHash(stream);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }

        public async Task<bool> IsDuplicateAsync(IFormFile file, string folderName)
        {
            var destinationDir = GetIsolatedDirectory(folderName);
            if (!Directory.Exists(destinationDir))
            {
                return false;
            }

            using var currentStream = file.OpenReadStream();
            var currentHash = ComputeHash(currentStream);

            var existingMetadataFiles = Directory.GetFiles(destinationDir, "*.metadata.json");
            foreach (var metaPath in existingMetadataFiles)
            {
                try
                {
                    var json = await File.ReadAllTextAsync(metaPath);
                    var metadata = JsonSerializer.Deserialize<DocumentMetadata>(json);
                    if (metadata != null && metadata.Hash == currentHash)
                    {
                        return true;
                    }
                }
                catch
                {
                    // Ignore parsing errors for individual file
                }
            }

            return false;
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

        private string ResolveFullPath(string filePath)
        {
            if (Path.IsPathRooted(filePath))
            {
                return filePath;
            }

            var cleanPath = filePath.TrimStart('/', '\\');
            if (cleanPath.StartsWith("uploads/", StringComparison.OrdinalIgnoreCase))
            {
                cleanPath = cleanPath.Substring(8);
            }
            else if (cleanPath.StartsWith("wwwroot/uploads/", StringComparison.OrdinalIgnoreCase))
            {
                cleanPath = cleanPath.Substring(16);
            }

            return Path.Combine(_storagePath, cleanPath);
        }
    }
}
