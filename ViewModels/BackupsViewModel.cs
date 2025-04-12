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
                var backupPath = await _backupModel.CreateBackupAsync(_configuration.WorkspacePath);
                BackupHistory.Insert(0, backupPath);
                _notificationService.ShowInfo($"Backup created successfully at {backupPath}");
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
                await _backupModel.RestoreBackupAsync(backupPath, _configuration.WorkspacePath);
                _notificationService.ShowInfo($"Backup restored successfully from {backupPath}");
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
                        .ToList();

                    BackupHistory = new ObservableCollection<string>(backups);
                    _logService.Information($"Loaded {backups.Count} backups from history");
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