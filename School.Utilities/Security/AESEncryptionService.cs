using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;

namespace School.Utilities.Security
{
    public class AESEncryptionService : IEncryptionService
    {
        private readonly EncryptionConfig _settings;
        private const int KeySize = 256;
        private const int BlockSize = 128;
        private const int Iterations = 10000;

        public AESEncryptionService(IOptions<EncryptionConfig> settings)
        {
            _settings = settings.Value;
        }

        public string Encrypt(string plainText, string? salt = null)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException(nameof(plainText));

            salt ??= GenerateSalt();

            using var aes = Aes.Create();
            aes.KeySize = KeySize;
            aes.BlockSize = BlockSize;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            var key = DeriveKey(_settings.EncryptionKey, salt);
            var iv = DeriveIV(_settings.EncryptionKey, salt);

            aes.Key = key;
            aes.IV = iv;

            using var encryptor = aes.CreateEncryptor();
            using var msEncrypt = new MemoryStream();
            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(plainText);
            }

            var encrypted = msEncrypt.ToArray();

            var saltBytes = Encoding.UTF8.GetBytes(salt);
            var saltLengthBytes = BitConverter.GetBytes(saltBytes.Length);
            var combined = new byte[saltLengthBytes.Length + saltBytes.Length + encrypted.Length];
            Buffer.BlockCopy(saltLengthBytes, 0, combined, 0, saltLengthBytes.Length);
            Buffer.BlockCopy(saltBytes, 0, combined, saltLengthBytes.Length, saltBytes.Length);
            Buffer.BlockCopy(encrypted, 0, combined, saltLengthBytes.Length + saltBytes.Length, encrypted.Length);

            var encryptedBase64 = Convert.ToBase64String(combined);

            var signature = GenerateSignature(encryptedBase64);

            return $"{signature}:{encryptedBase64}";
        }

        public string Decrypt(string cipherText, string? salt = null)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException(nameof(cipherText));

            var parts = cipherText.Split(':');
            if (parts.Length != 2)
                throw new ArgumentException("Invalid encrypted format. Expected 'signature:encryptedData'");

            var receivedSignature = parts[0];
            var encryptedBase64 = parts[1];

            if (!VerifySignature(encryptedBase64, receivedSignature))
            {
                throw new CryptographicException("Signature verification failed. Data may have been tampered with.");
            }

            var combined = Convert.FromBase64String(encryptedBase64);

            if (combined.Length < 4)
                throw new ArgumentException("Invalid encrypted data format");

            var saltLength = BitConverter.ToInt32(combined, 0);
            if (combined.Length < 4 + saltLength)
                throw new ArgumentException("Invalid encrypted data format");

            var saltBytes = new byte[saltLength];
            Buffer.BlockCopy(combined, 4, saltBytes, 0, saltLength);
            var extractedSalt = Encoding.UTF8.GetString(saltBytes);

            var encrypted = new byte[combined.Length - 4 - saltLength];
            Buffer.BlockCopy(combined, 4 + saltLength, encrypted, 0, encrypted.Length);

            var saltToUse = salt ?? extractedSalt;

            using var aes = Aes.Create();
            aes.KeySize = KeySize;
            aes.BlockSize = BlockSize;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            var key = DeriveKey(_settings.EncryptionKey, saltToUse);
            var iv = DeriveIV(_settings.EncryptionKey, saltToUse);

            aes.Key = key;
            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor();
            using var msDecrypt = new MemoryStream(encrypted);
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);

            return srDecrypt.ReadToEnd();
        }

        public string GenerateSignature(string data)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_settings.SignatureKey));
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return Convert.ToBase64String(hashBytes);
        }

        public bool VerifySignature(string data, string signature)
        {
            var computedSignature = GenerateSignature(data);
            return computedSignature == signature;
        }

        public string GenerateSalt()
        {
            var saltBytes = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        private byte[] DeriveKey(string password, string salt)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                Encoding.UTF8.GetBytes(salt),
                Iterations,
                HashAlgorithmName.SHA256);
            return pbkdf2.GetBytes(KeySize / 8); // 32 bytes for AES-256
        }

        private byte[] DeriveIV(string password, string salt)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(
                password + "IV", // Different salt for IV
                Encoding.UTF8.GetBytes(salt),
                Iterations,
                HashAlgorithmName.SHA256);
            return pbkdf2.GetBytes(BlockSize / 8); // 16 bytes for AES block size
        }
    }
}

