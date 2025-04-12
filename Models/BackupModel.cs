using System;
using System.IO;
using System.IO.Compression;
// using System.Security.Cryptography; // Not used in the provided snippet
// using Org.BouncyCastle.Crypto; // Not used in the provided snippet
using System.Threading.Tasks;
// using System.Collections.Generic; // Not used in the provided snippet
// using System.ComponentModel; // Not used in the provided snippet
using Microsoft.Extensions.Logging; // <--- This namespace is correct and already present

namespace SentinelPro.Models
{

    /// <summary>
    /// Handles backup creation and restoration operations with compression and encryption capabilities.
    /// </summary>
    public class BackupModel
    {
        // Correct Type: Use the generic ILogger<T> for category naming
        private readonly ILogger<BackupModel> _logger;
        private readonly string _backupDirectory;
        private readonly string _encryptionKey; // Note: Encryption is not implemented in the provided methods

        /// <summary>
        /// Initializes a new instance of the BackupModel class.
        /// </summary>
        /// <param name="logger">The logger for recording operational events.</param>
        /// <param name="backupDirectory">The directory where backups will be stored.</param>
        /// <param name="encryptionKey">The key used for encrypting backup data.</param>
        /// <exception cref="ArgumentNullException">Thrown when any parameter is null.</exception>
        // Correct Parameter Type: Use ILogger<BackupModel>
        public BackupModel(ILogger<BackupModel> logger, string backupDirectory, string encryptionKey)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _backupDirectory = backupDirectory ?? throw new ArgumentNullException(nameof(backupDirectory));
            _encryptionKey = encryptionKey ?? throw new ArgumentNullException(nameof(encryptionKey)); // Store the key, even if not used yet

            // Ensure backup directory exists
            if (!Directory.Exists(_backupDirectory))
            {
                 Directory.CreateDirectory(_backupDirectory);
                _logger.LogInformation("Created backup directory: {BackupDirectory}", _backupDirectory);
            }
        }

        /// <summary>
        /// Creates a compressed backup of the specified directory.
        /// </summary>
        /// <param name="sourcePath">The path to the directory to backup.</param>
        /// <param name="description">A description of the backup for reference.</param>
        /// <returns>The unique identifier (GUID) of the created backup.</returns>
        /// <exception cref="ArgumentException">Thrown when sourcePath is empty.</exception>
        /// <exception cref="DirectoryNotFoundException">Thrown when source directory doesn't exist.</exception>
        public async Task<string> CreateBackupAsync(string sourcePath, string description) // Description parameter is currently unused
        {
            try
            {
                if (string.IsNullOrEmpty(sourcePath))
                    throw new ArgumentException("Source path cannot be empty", nameof(sourcePath));

                if (!Directory.Exists(sourcePath))
                    throw new DirectoryNotFoundException($"Source directory not found: {sourcePath}");

                var backupId = Guid.NewGuid().ToString();
                var backupFileName = $"{backupId}.zip"; // Consider adding timestamp or description info to filename if needed
                var backupPath = Path.Combine(_backupDirectory, backupFileName);

                // Ensure backup directory exists
                Directory.CreateDirectory(_backupDirectory);

                // Correct Method: Use LogInformation with structured logging placeholders {}
                _logger.LogInformation("Creating backup for {SourcePath} with ID {BackupId} to {BackupPath}", sourcePath, backupId, backupPath);

                await Task.Run(() =>
                {
                    // Ensure files are not locked before proceeding (basic check)
                    try
                    {
                        using var archive = ZipFile.Open(backupPath, ZipArchiveMode.Create);
                        var files = Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories);

                        foreach (var file in files)
                        {
                            var relativePath = Path.GetRelativePath(sourcePath, file);
                            // Add entry even if file is empty, handle potential access errors per file if needed
                            archive.CreateEntryFromFile(file, relativePath, CompressionLevel.Optimal);
                            _logger.LogDebug("Added {File} to archive {BackupId}", relativePath, backupId); // Example Debug log
                        }
                    }
                    catch (IOException ioEx)
                    {
                        // Log specific file access issues if needed
                        _logger.LogError(ioEx, "IO error during zip creation for backup {BackupId}", backupId);
                        // Clean up potentially incomplete zip file
                        if (File.Exists(backupPath)) File.Delete(backupPath);
                        throw; // Re-throw to indicate failure
                    }
                });

                // Correct Method: Use LogInformation
                _logger.LogInformation("Backup created successfully: {BackupId} at {BackupPath}", backupId, backupPath);
                return backupId;
            }
            catch (Exception ex) // Catch specific exceptions if possible (ArgumentException, DirectoryNotFoundException already handled)
            {
                // Correct Method: Use LogError, passing the exception as the first argument
                _logger.LogError(ex, "Failed to create backup for {SourcePath}. Description: {Description}", sourcePath, description);
                throw; // Re-throw the original exception to preserve stack trace and signal failure
            }
        }

        /// <summary>
        /// Restores a backup to the specified destination directory.
        /// </summary>
        /// <param name="backupId">The unique identifier of the backup to restore.</param>
        /// <param name="destinationPath">The directory where the backup should be restored.</param>
        /// <exception cref="FileNotFoundException">Thrown when the backup file is not found.</exception>
        /// <exception cref="ArgumentException">Thrown if backupId or destinationPath is invalid.</exception>
        public async Task RestoreBackupAsync(string backupId, string destinationPath)
        {
            if (string.IsNullOrWhiteSpace(backupId))
                throw new ArgumentException("Backup ID cannot be empty.", nameof(backupId));
            if (string.IsNullOrWhiteSpace(destinationPath))
                throw new ArgumentException("Destination path cannot be empty.", nameof(destinationPath));

            var backupFileName = $"{backupId}.zip";
            var backupPath = Path.Combine(_backupDirectory, backupFileName);

            try
            {
                if (!File.Exists(backupPath))
                {
                    _logger.LogWarning("Backup file not found: {BackupPath}", backupPath);
                    throw new FileNotFoundException($"Backup not found: {backupPath}", backupPath);
                }

                // Correct Method: Use LogInformation
                _logger.LogInformation("Restoring backup {BackupId} from {BackupPath} to {DestinationPath}", backupId, backupPath, destinationPath);

                // Ensure destination directory exists
                Directory.CreateDirectory(destinationPath);

                await Task.Run(() =>
                {
                    try
                    {
                        using var archive = ZipFile.OpenRead(backupPath);
                        // Consider safety: Extracting to a temporary location first, then moving?
                        // Consider overwriting behavior carefully. ExtractToFile(..., true) overwrites.
                        archive.ExtractToDirectory(destinationPath, true); // Simpler extraction if overwriting is acceptable
                    }
                    catch (IOException ioEx)
                    {
                        _logger.LogError(ioEx, "IO error during extraction for backup {BackupId} to {DestinationPath}", backupId, destinationPath);
                        throw; // Re-throw to indicate failure
                    }
                    // Catch other potential exceptions like InvalidDataException for corrupt archives
                });

                // Correct Method: Use LogInformation
                _logger.LogInformation("Backup {BackupId} restored successfully to {DestinationPath}", backupId, destinationPath);
            }
            catch (FileNotFoundException fnfEx) // Keep specific catch
            {
                // Already logged warning, just rethrow
                throw;
            }
            catch (Exception ex)
            {
                // Correct Method: Use LogError
                _logger.LogError(ex, "Failed to restore backup {BackupId} to {DestinationPath}", backupId, destinationPath);
                throw; // Re-throw the original exception
            }
        }

        // Note: Encryption/Decryption methods using _encryptionKey would need to be added here.
        // They would likely involve reading the zip stream, encrypting/decrypting it (e.g., using AES),
        // and writing the result to a new stream/file. Libraries like BouncyCastle or standard .NET crypto can be used.
    }
}
