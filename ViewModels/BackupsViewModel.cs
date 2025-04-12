using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using SentinelPro.Models;
using SentinelPro.Services;
using SentinelPro.Services.Interfaces;

namespace SentinelPro.ViewModels
{
    public class BackupsViewModel : ViewModelBase
    {
        private readonly ConfigurationModel _configuration;
        private readonly BackupModel _backupModel;
        private readonly INotificationService _notificationService;
        private readonly ILoggingService _logService;
        private ObservableCollection<string> _backupHistory;
        private bool _isBackupInProgress;

        public ObservableCollection<string> BackupHistory
        {
            get => _backupHistory;
            private set => SetProperty(ref _backupHistory, value);
        }

        public bool IsBackupInProgress
        {
            get => _isBackupInProgress;
            private set => SetProperty(ref _isBackupInProgress, value);
        }

        public ICommand CreateBackupCommand { get; }
        public ICommand RestoreBackupCommand { get; }

        public BackupsViewModel(ConfigurationModel configuration, BackupModel backupModel, INotificationService notificationService, ILoggingService logService)
        {
            _configuration = configuration;
            _backupModel = backupModel;
            _notificationService = notificationService;
            _logService = logService;
            _backupHistory = new ObservableCollection<string>();

            CreateBackupCommand = new RelayCommand(async _ => await CreateBackup(),
                _ => !IsBackupInProgress);
            RestoreBackupCommand = new RelayCommand(async param => await RestoreBackup(param as string),
                param => !IsBackupInProgress && param is string);

            LoadBackupHistory();
        }

        private async Task CreateBackup()
        {
            try
            {
                IsBackupInProgress = true;
                // TODO: CreateBackupAsync needs a source path and description. Using BackupLocation as source for now. Needs clarification.
                var sourcePath = _configuration.BackupLocation; // Assuming BackupLocation is the source to backup
                var description = $"Manual Backup {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                var backupId = await _backupModel.CreateBackupAsync(sourcePath, description);
                var backupFileName = $"{backupId}.zip"; // Assuming BackupModel returns ID, not full path
                BackupHistory.Insert(0, backupFileName); // Add the file name to history
                _notificationService.ShowInfo($"Backup '{backupFileName}' created successfully.");
            }
            catch (Exception ex)
            {
                _notificationService.ShowError($"Failed to create backup: {ex.Message}");
                _logService.Error(ex, "Backup creation failed");
            }
            finally
            {
                IsBackupInProgress = false;
            }
        }

        private async Task RestoreBackup(string? backupPath)
        {
            if (string.IsNullOrWhiteSpace(backupPath)) return;

            try
            {
                IsBackupInProgress = true;
                // Assuming backupPath is just the ID (filename without extension)
                var backupId = Path.GetFileNameWithoutExtension(backupPath);
                var destinationPath = _configuration.BackupLocation; // Assuming BackupLocation is the restore destination
                 if (string.IsNullOrWhiteSpace(backupId))
                 {
                    throw new ArgumentException("Invalid backup file selected.", nameof(backupPath));
                 }
                await _backupModel.RestoreBackupAsync(backupId, destinationPath);
                _notificationService.ShowInfo($"Backup '{backupPath}' restored successfully to {destinationPath}");
            }
            catch (ArgumentException aex) // Catch specific argument exception
            {
                 _notificationService.ShowError($"Failed to restore backup: {aex.Message}");
                 _logService.Error(aex, "Backup restoration failed due to invalid argument");
            }
            catch (Exception ex)
            {
                _notificationService.ShowError($"Failed to restore backup: {ex.Message}");
                _logService.Error(ex, "Backup restoration failed");
            }
            finally
            {
                IsBackupInProgress = false;
            }
        }

        private void LoadBackupHistory()
        {
            try
            {
                var backupDir = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "SentinelPro",
                    "Backups"
                );

                if (Directory.Exists(backupDir))
                {
                    var backups = Directory.GetFiles(backupDir, "*.zip")
                        .OrderByDescending(f => new FileInfo(f).CreationTime)
                        .Select(Path.GetFileName) // Get only filenames
                        .ToList();

                    BackupHistory = new ObservableCollection<string>(backups!); // Add non-null assertion
                    _logService.Info($"Loaded {backups.Count} backups from history"); // Use Info method
                }
                else
                {
                    _logService.Info("Backup directory does not exist. No history loaded.");
                }
            }
            catch (Exception ex)
            {
                _notificationService.ShowError($"Failed to load backup history: {ex.Message}");
                _logService.Error(ex, "Failed to load backup history");
            }
        }
    }
}
