using System.Windows.Input;
using WorkspaceCleanup.Models;

namespace WorkspaceCleanup.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly ConfigurationModel _configuration;
        private string _workspacePath = string.Empty;
        private string _backupPath = string.Empty;

        public string WorkspacePath
        {
            get => _workspacePath;
            set
            {
                if (SetProperty(ref _workspacePath, value))
                {
                    _configuration.WorkspacePath = value;
                    _configuration.SaveConfiguration();
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
                    _configuration.BackupPath = value;
                    _configuration.SaveConfiguration();
                }
            }
        }

        public ICommand BrowseWorkspaceCommand { get; }
        public ICommand BrowseBackupCommand { get; }

        public SettingsViewModel(ConfigurationModel configuration)
        {
            _configuration = configuration;
            _workspacePath = configuration.WorkspacePath;
            _backupPath = configuration.BackupPath;

            BrowseWorkspaceCommand = new RelayCommand(_ => BrowseWorkspace());
            BrowseBackupCommand = new RelayCommand(_ => BrowseBackup());
        }

        private void BrowseWorkspace()
        {
            var dialog = new Microsoft.Win32.FolderBrowserDialog
            {
                Description = "Select Workspace Directory",
                ShowNewFolderButton = true
            };

            if (dialog.ShowDialog() == true)
            {
                WorkspacePath = dialog.SelectedPath;
            }
        }

        private void BrowseBackup()
        {
            var dialog = new Microsoft.Win32.FolderBrowserDialog
            {
                Description = "Select Backup Directory",
                ShowNewFolderButton = true
            };

            if (dialog.ShowDialog() == true)
            {
                BackupPath = dialog.SelectedPath;
            }
        }
    }
}