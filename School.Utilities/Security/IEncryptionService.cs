namespace School.Utilities.Security
{
    public interface IEncryptionService
    {
        string Encrypt(string plainText, string? salt = null);
        string Decrypt(string cipherText, string? salt = null);
        string GenerateSignature(string data);
        bool VerifySignature(string data, string signature);
        string GenerateSalt();
    }
}

