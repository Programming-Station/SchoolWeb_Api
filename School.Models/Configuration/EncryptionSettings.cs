namespace School.Models.Configuration
{
    public class EncryptionSettings
    {
        public string EncryptionKey { get; set; } = string.Empty;
        public string SignatureKey { get; set; } = string.Empty;
        public bool EnableEncryption { get; set; } = true;
        public bool LogEncryptedRequests { get; set; } = true;
        public List<string> EncryptedEndpoints { get; set; } = new List<string>();
    }
}

