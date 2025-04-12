using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SentinelPro.Services;
using Windows.Security.Credentials;

namespace SentinelPro.Tests;

[TestClass]
public class BackupEncryptionServiceTests
{
    [TestMethod]
    public void EncryptDecrypt_RoundTrip_Success()
    {
        // Arrange
        var service = new BackupEncryptionService();
        var originalData = System.Text.Encoding.UTF8.GetBytes("Test Data");

        // Act
        var encrypted = service.EncryptBackup(originalData);
        var decrypted = service.DecryptBackup(encrypted);

        // Assert
        CollectionAssert.AreEqual(originalData, decrypted);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Decrypt_WithMissingKey_ThrowsException()
    {
        // Arrange
        var service = new BackupEncryptionService();
        var vault = new PasswordVault();
        vault.Remove(vault.Retrieve(BackupEncryptionService.VAULT_RESOURCE, "EncryptionKey"));

        // Act & Assert
        service.DecryptBackup(new byte[32]);
    }

    [TestMethod]
    public void KeyStorage_ValidatesWindowsCredentialManagerIntegration()
    {
        // Arrange
        var service = new BackupEncryptionService();
        var testKey = new byte[32];
        new Random().NextBytes(testKey);

        // Act
        service.StoreKeyInVault(testKey);
        var retrievedKey = service.RetrieveKeyFromVault();

        // Assert
        CollectionAssert.AreEqual(testKey, retrievedKey);
    }
}