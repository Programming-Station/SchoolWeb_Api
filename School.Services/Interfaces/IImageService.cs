using Microsoft.AspNetCore.Http;

namespace School.Services.Interfaces
{
    public interface IImageService
    {
        /// <summary>
        /// Uploads and saves an image file to the server
        /// Automatically resizes if image is larger than 1MB
        /// </summary>
        /// <param name="file">The image file to upload</param>
        /// <param name="folderName">Subfolder name (e.g., "slider", "gallery", "hero", "certificates")</param>
        /// <returns>Full path to the saved image (e.g., "D:\\Images\\slider\\image.jpg")</returns>
        Task<string> UploadImageAsync(IFormFile file, string? folderName = null);

        /// <summary>
        /// Deletes an image file from the server
        /// </summary>
        /// <param name="imagePath">Full path or relative path to the image</param>
        /// <returns>True if deleted successfully, false otherwise</returns>
        Task<bool> DeleteImageAsync(string imagePath);

        /// <summary>
        /// Resizes and compresses an image if it's larger than 1MB
        /// </summary>
        /// <param name="imageStream">The image stream</param>
        /// <param name="maxSizeKB">Maximum size in KB (default: 1024KB = 1MB)</param>
        /// <param name="quality">JPEG quality (1-100, default: 85)</param>
        /// <returns>Resized image stream</returns>
        Task<MemoryStream> ResizeImageIfNeededAsync(Stream imageStream, int maxSizeKB = 1024, int quality = 85);

        /// <summary>
        /// Gets the full file path for an image, converting relative paths to full paths
        /// </summary>
        /// <param name="imagePath">Full path or relative path to the image</param>
        /// <returns>Full file system path if image exists, null otherwise</returns>
        string? GetImageFilePath(string imagePath);
    }
}
