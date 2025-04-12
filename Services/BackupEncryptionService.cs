using System.Security.Cryptography;
using System.Text;
using System.IO; // Added for MemoryStream
// using Windows.Security.Credentials; // Removed - Requires Windows SDK, consider ProtectedData instead

namespace SentinelPro.Services;

public sealed class BackupEncryptionService
{
    private const string VAULT_RESOURCE = "SentinelProBackupEncryption";

    public byte[] EncryptBackup(byte[] data)
    {
        using var aes = Aes.Create();
        aes.KeySize = 256;
        aes.GenerateKey(); // Generate IV and Key

        // Key storage/retrieval needs proper implementation (e.g., using ProtectedData or user prompt)
        // byte[] encryptionKey = RandomNumberGenerator.GetBytes(32); // Example: Generate random key
        // StoreKeyInVault(aes.Key); // Example: Store the generated key securely

        // For now, let's assume the key is somehow provided or retrieved
        // This part needs a real implementation based on the chosen key management strategy
        if (aes.Key == null || aes.Key.Length == 0)
        {
             throw new InvalidOperationException("Encryption key is not set.");
        }


        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV); // Use Key and IV
        // Prepend IV to the ciphertext for decryption
        byte[] encryptedData = PerformCryptography(data, encryptor);
        byte[] result = new byte[aes.IV.Length + encryptedData.Length];
        Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);
        Buffer.BlockCopy(encryptedData, 0, result, aes.IV.Length, encryptedData.Length);
        return result;
    }

    public byte[] DecryptBackup(byte[] cipherTextWithIv)
    {
        // Key retrieval needs proper implementation
        // var key = RetrieveKeyFromVault(); // Example: Retrieve the key securely
        byte[] key = null; // Placeholder - MUST BE REPLACED with actual key retrieval

        if (key == null || key.Length == 0)
        {
            throw new InvalidOperationException("Decryption key is not available.");
        }

        using var aes = Aes.Create();
        aes.Key = key;

        // Extract IV from the beginning of the cipherTextWithIv
        byte[] iv = new byte[aes.BlockSize / 8];
        if (cipherTextWithIv.Length < iv.Length)
        {
            throw new ArgumentException("Ciphertext is too short to contain IV.", nameof(cipherTextWithIv));
        }
        Buffer.BlockCopy(cipherTextWithIv, 0, iv, 0, iv.Length);
        aes.IV = iv;

        byte[] cipherText = new byte[cipherTextWithIv.Length - iv.Length];
        Buffer.BlockCopy(cipherTextWithIv, iv.Length, cipherText, 0, cipherText.Length);


        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV); // Use Key and IV
        return PerformCryptography(cipherText, decryptor);
    }

    private static byte[] PerformCryptography(byte[] data, ICryptoTransform cryptoTransform)
    {
        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write);
        cs.Write(data, 0, data.Length);
        cs.FlushFinalBlock();
        return ms.ToArray();
    }

    // Removed StoreKeyInVault and RetrieveKeyFromVault as PasswordVault is not suitable here.
    // Key management needs to be implemented using a different approach (e.g., ProtectedData).
    // private void StoreKeySecurely(byte[] key) { /* ... use ProtectedData ... */ }
    // private byte[] RetrieveKeySecurely() { /* ... use ProtectedData ... */ }
}
