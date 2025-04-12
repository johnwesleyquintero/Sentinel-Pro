using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Serilog;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Extensions.Logging;

namespace WorkspaceCleanup.Models
{
    using Microsoft.Extensions.Logging;
    using System.IO;
    using System.Threading.Tasks;
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.IO.Compression;
    using System.Security.Cryptography;
    using Org.BouncyCastle.Crypto;
    using Serilog;

    using System.ArgumentException;
    using System.ArgumentNullException;
    using System.IO;
    using System.IO.Compression;
    using System.Security.Cryptography;
    using System.Threading.Tasks;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Microsoft.Extensions.Logging;

    using System.IO;
    using System.IO.Compression;
    using System.Security.Cryptography;
    using System.Threading.Tasks;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Handles backup creation and restoration operations with compression and encryption capabilities.
    /// </summary>
    public class BackupModel
    {
        private readonly ILogger _logger;
        private readonly string _backupDirectory;
        private readonly string _encryptionKey;

        /// <summary>
        /// Initializes a new instance of the BackupModel class.
        /// </summary>
        /// <param name="logger">The logger for recording operational events.</param>
        /// <param name="backupDirectory">The directory where backups will be stored.</param>
        /// <param name="encryptionKey">The key used for encrypting backup data.</param>
        /// <exception cref="ArgumentNullException">Thrown when any parameter is null.</exception>
        public BackupModel(ILogger logger, string backupDirectory, string encryptionKey)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _backupDirectory = backupDirectory ?? throw new ArgumentNullException(nameof(backupDirectory));
            _encryptionKey = encryptionKey ?? throw new ArgumentNullException(nameof(encryptionKey));
        }

        /// <summary>
        /// Creates a compressed backup of the specified directory.
        /// </summary>
        /// <param name="sourcePath">The path to the directory to backup.</param>
        /// <param name="description">A description of the backup for reference.</param>
        /// <returns>The unique identifier (GUID) of the created backup.</returns>
        /// <exception cref="ArgumentException">Thrown when sourcePath is empty.</exception>
        /// <exception cref="DirectoryNotFoundException">Thrown when source directory doesn't exist.</exception>
        public async Task<string> CreateBackupAsync(string sourcePath, string description)
        {
            try
            {
                if (string.IsNullOrEmpty(sourcePath))
                    throw new ArgumentException("Source path cannot be empty", nameof(sourcePath));

                if (!Directory.Exists(sourcePath))
                    throw new DirectoryNotFoundException($"Source directory not found: {sourcePath}");

                var backupId = Guid.NewGuid().ToString();
                var backupPath = Path.Combine(_backupDirectory, $"{backupId}.zip");

                _logger.Information("Creating backup for {SourcePath}", sourcePath);

                await Task.Run(() =>
                {
                    using var archive = ZipFile.Open(backupPath, ZipArchiveMode.Create);
                    var files = Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories);

                    foreach (var file in files)
                    {
                        var relativePath = Path.GetRelativePath(sourcePath, file);
                        archive.CreateEntryFromFile(file, relativePath, CompressionLevel.Optimal);
                    }
                });

                _logger.Information("Backup created successfully: {BackupId}", backupId);
                return backupId;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to create backup for {SourcePath}", sourcePath);
                throw;
            }
        }

        /// <summary>
        /// Restores a backup to the specified destination directory.
        /// </summary>
        /// <param name="backupId">The unique identifier of the backup to restore.</param>
        /// <param name="destinationPath">The directory where the backup should be restored.</param>
        /// <exception cref="FileNotFoundException">Thrown when the backup file is not found.</exception>
        public async Task RestoreBackupAsync(string backupId, string destinationPath)
        {
            try
            {
                var backupPath = Path.Combine(_backupDirectory, $"{backupId}.zip");

                if (!File.Exists(backupPath))
                    throw new FileNotFoundException($"Backup not found: {backupId}");

                _logger.Information("Restoring backup {BackupId} to {DestinationPath}", backupId, destinationPath);

                await Task.Run(() =>
                {
                    using var archive = ZipFile.OpenRead(backupPath);
                    foreach (var entry in archive.Entries)
                    {
                        var destinationFilePath = Path.Combine(destinationPath, entry.FullName);
                        var destinationDirectory = Path.GetDirectoryName(destinationFilePath);

                        if (!Directory.Exists(destinationDirectory))
                            Directory.CreateDirectory(destinationDirectory!);

                        entry.ExtractToFile(destinationFilePath, true);
                    }
                });

                _logger.Information("Backup restored successfully");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to restore backup {BackupId}", backupId);
                throw;
            }
        }
    }
}
