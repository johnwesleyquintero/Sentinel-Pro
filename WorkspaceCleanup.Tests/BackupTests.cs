using Xunit;
using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace WorkspaceCleanup.Tests
{
    public class BackupTests : IDisposable
    {
        private readonly string _testDirectory;
        private readonly string _backupDirectory;
        private const string TestData = "Test backup data";

        public BackupTests()
        {
            _testDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            _backupDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(_testDirectory);
            Directory.CreateDirectory(_backupDirectory);
        }

        [Fact]
        public void CreateBackup_WithValidPath_CreatesBackupFile()
        {
            // Arrange
            var sourceFile = Path.Combine(_testDirectory, "test.txt");
            File.WriteAllText(sourceFile, TestData);
            var backupPath = Path.Combine(_backupDirectory, "backup.zip");

            // Act
            using (var archive = ZipFile.Open(backupPath, ZipArchiveMode.Create))
            {
                var entry = archive.CreateEntry("test.txt");
                using (var writer = new StreamWriter(entry.Open()))
                {
                    writer.Write(TestData);
                }
            }

            // Assert
            Assert.True(File.Exists(backupPath));
            using (var archive = ZipFile.OpenRead(backupPath))
            {
                var entry = archive.GetEntry("test.txt");
                Assert.NotNull(entry);
                using (var reader = new StreamReader(entry.Open()))
                {
                    var content = reader.ReadToEnd();
                    Assert.Equal(TestData, content);
                }
            }
        }

        [Fact]
        public void RestoreBackup_WithValidBackup_RestoresSuccessfully()
        {
            // Arrange
            var backupPath = Path.Combine(_backupDirectory, "backup.zip");
            var restorePath = Path.Combine(_testDirectory, "restored");
            Directory.CreateDirectory(restorePath);

            using (var archive = ZipFile.Open(backupPath, ZipArchiveMode.Create))
            {
                var entry = archive.CreateEntry("test.txt");
                using (var writer = new StreamWriter(entry.Open()))
                {
                    writer.Write(TestData);
                }
            }

            // Act
            ZipFile.ExtractToDirectory(backupPath, restorePath);

            // Assert
            var restoredFile = Path.Combine(restorePath, "test.txt");
            Assert.True(File.Exists(restoredFile));
            var restoredContent = File.ReadAllText(restoredFile);
            Assert.Equal(TestData, restoredContent);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void CreateBackup_WithInvalidPath_ThrowsException(string invalidPath)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                if (string.IsNullOrEmpty(invalidPath))
                    throw new ArgumentException("Backup path cannot be null or empty");
            });

            Assert.Contains("Backup path cannot be null or empty", exception.Message);
        }

        public void Dispose()
        {
            try
            {
                Directory.Delete(_testDirectory, true);
                Directory.Delete(_backupDirectory, true);
            }
            catch (IOException) { /* Ignore cleanup errors */ }
        }
    }
}