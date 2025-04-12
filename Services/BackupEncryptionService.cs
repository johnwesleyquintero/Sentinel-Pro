using System.Security.Cryptography;
using Windows.Security.Credentials;

namespace SentinelPro.Services;

public sealed class BackupEncryptionService
{
    private const string VAULT_RESOURCE = "SentinelProBackupEncryption";
    
    public byte[] EncryptBackup(byte[] data)
    {
        using var aes = Aes.Create();
        aes.KeySize = 256;
        aes.GenerateKey();
        
        StoreKeyInVault(aes.Key);
        
        using var encryptor = aes.CreateEncryptor();
        return PerformCryptography(data, encryptor);
    }

    public byte[] DecryptBackup(byte[] cipherText)
    {
        var key = RetrieveKeyFromVault();
        using var aes = Aes.Create();
        aes.Key = key;
        
        using var decryptor = aes.CreateDecryptor();
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

    private void StoreKeyInVault(byte[] key)
    {
        var vault = new PasswordVault();
        vault.Add(new PasswordCredential(VAULT_RESOURCE, "EncryptionKey", Convert.ToBase64String(key)));
    }

    private byte[] RetrieveKeyFromVault()
    {
        var vault = new PasswordVault();
        var credential = vault.Retrieve(VAULT_RESOURCE, "EncryptionKey");
        credential.RetrievePassword();
        return Convert.FromBase64String(credential.Password);
    }
}