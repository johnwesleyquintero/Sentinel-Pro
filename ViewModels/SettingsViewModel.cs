using System.Windows.Input;
using SentinelPro.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Win32; // Added for OpenFileDialog

namespace SentinelPro.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly ConfigurationModel _configuration;
        private string _backupLocation = string.Empty;
        private string _backupPath = string.Empty;

        public string BackupLocation
        {
            get => _backupLocation;
            set
            {
                if (SetProperty(ref _backupLocation, value))
                {
                    // Assuming BackupLocation is the property to set
                    // _configuration.WorkspacePath = value; // Removed - WorkspacePath does not exist
                    // _configuration.SaveConfiguration(); // Removed - SaveConfiguration method does not exist
                }
            }
        }

        public string BackupPath
        {
            get => _backupPath;
            set
            {
                if (SetProperty(ref _backupPath, value))
                {
                    // _configuration.BackupPath = value; // Removed - BackupPath is not a property of ConfigurationModel
                    // _configuration.SaveConfiguration(); // Removed - SaveConfiguration method does not exist
                }
            }
        }

        public ICommand BrowseWorkspaceCommand { get; }
        public ICommand BrowseBackupCommand { get; }

        public SettingsViewModel(ConfigurationModel configuration)
        {
            _configuration = configuration;
            _backupLocation = configuration.BackupLocation;
            // _backupPath = configuration.BackupPath; // Removed - BackupPath is not a property of ConfigurationModel

            BrowseWorkspaceCommand = new RelayCommand(_ => BrowseBackupLocation());
            BrowseBackupCommand = new RelayCommand(_ => BrowseBackup());
        }

        private void BrowseBackupLocation()
        {
            var dialog = new OpenFileDialog
            {
                ValidateNames = false,
                CheckFileExists = false,
                CheckPathExists = true,
                FileName = "Folder Selection."
            };

            if (dialog.ShowDialog() == true)
            {
                BackupLocation = System.IO.Path.GetDirectoryName(dialog.FileName);
            }
        }

        private void BrowseBackup()
        {
            var dialog = new OpenFileDialog
            {
                ValidateNames = false,
                CheckFileExists = false,
                CheckPathExists = true,
                FileName = "Folder Selection."
            };

            if (dialog.ShowDialog() == true)
            {
                BackupPath = System.IO.Path.GetDirectoryName(dialog.FileName);
            }
        }

        public async Task AddRuleAsync(WorkspaceRule rule)
        {
            // Implement logic to add a rule
            await Task.CompletedTask;
        }

        public async Task UpdateRuleAsync(WorkspaceRule selectedRule, WorkspaceRule rule)
        {
            // Implement logic to update a rule
            await Task.CompletedTask;
        }

        public async Task RemoveRuleAsync(WorkspaceRule selectedRule)
        {
            // Implement logic to remove a rule
            await Task.CompletedTask;
        }

        public async Task SaveSettingsAsync()
        {
            // Implement logic to save settings
            await Task.CompletedTask;
        }
    }
}
