using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace School.Services.Interfaces
{
    public interface IDocumentService
    {
        Task<string> UploadAsync(IFormFile file, string? folderName = null, bool compress = false, int? targetWidth = null, int? targetHeight = null);
        Task<string> UploadAsync(byte[] fileBytes, string fileName, string? folderName = null);
        Task<string> ReplaceAsync(string existingFilePath, IFormFile newFile, bool compress = false);
        Task<bool> DeleteAsync(string filePath);
        Task<bool> RestoreAsync(string trashedFilePath);
        Task<byte[]> DownloadAsync(string filePath);
        Task<string?> GetThumbnailPathAsync(string filePath);
        Task<string?> GetPreviewPathAsync(string filePath);
        Task<bool> ValidateFileAsync(IFormFile file);
        Task<string> RenameAsync(string filePath, string newFileName);
        string ComputeHash(Stream stream);
        Task<bool> IsDuplicateAsync(IFormFile file, string folderName);
    }
}
