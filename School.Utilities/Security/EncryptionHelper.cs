using System.Text;
using System.Text.Json;

namespace School.Utilities.Security
{
    /// <summary>
    /// Helper class for UI/client-side encryption (reference implementation)
    /// This shows how UI should encrypt data before sending to API
    /// </summary>
    public static class EncryptionHelper
    {
        /// <summary>
        /// Example method showing how to encrypt data on client side
        /// This is a reference - actual implementation should be in JavaScript/TypeScript
        /// </summary>
        public static string EncryptRequest<T>(T requestObject, IEncryptionService encryptionService)
        {
            // Serialize object to JSON
            var json = JsonSerializer.Serialize(requestObject, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            });

            // Encrypt the JSON string
            return encryptionService.Encrypt(json);
        }

        /// <summary>
        /// Example method showing how to decrypt response on client side
        /// </summary>
        public static T? DecryptResponse<T>(string encryptedResponse, IEncryptionService encryptionService)
        {
            // Decrypt the response
            var decryptedJson = encryptionService.Decrypt(encryptedResponse);

            // Deserialize to object
            return JsonSerializer.Deserialize<T>(decryptedJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
    }
}

