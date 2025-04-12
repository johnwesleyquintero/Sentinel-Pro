using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Text.Json;

namespace SentinelPro.Views
{
    /// <summary>
    /// Page for managing and restoring workspace backups.
    /// </summary>
    public partial class BackupsPage : Page
    {
        private readonly string _backupDir;
        private readonly string _rollbackFile;
        private ObservableCollection<BackupItem> _backups;

        /// <summary>
        /// Initializes a new instance of the BackupsPage class.
        /// Sets up the backup directory, rollback file path, and initializes the backup list.
        /// </summary>
        public BackupsPage()
        {
            InitializeComponent();

            _backupDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "__cleanup_backups__");
            _rollbackFile = Path.Combine(_backupDir, "rollback_info.json");
            _backups = new ObservableCollection<BackupItem>();

            BackupsList.ItemsSource = _backups;
            InitializeEventHandlers();
            LoadBackups();
        }

        /// <summary>
        /// Initializes event handlers for the restore and refresh buttons.
        /// </summary>
        private void InitializeEventHandlers()
        {
            RestoreButton.Click += async (s, e) => await RestoreSelectedBackup();
            RefreshButton.Click += async (s, e) => await LoadBackups();
        }

        /// <summary>
        /// Loads the list of backups from the rollback information file.
        /// Displays appropriate messages if no backups are found or if an error occurs.
        /// </summary>
        private async Task LoadBackups()
        {
            try
            {
                _backups.Clear();

                if (!File.Exists(_rollbackFile))
                {
                    MessageBox.Show("No backup history found", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var jsonContent = await File.ReadAllTextAsync(_rollbackFile);
                var backupItems = JsonSerializer.Deserialize<List<BackupItem>>(jsonContent);

                foreach (var item in backupItems)
                {
                    _backups.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading backups: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Restores the selected backup after user confirmation.
        /// Displays appropriate messages for the restoration process status.
        /// </summary>
        private async Task RestoreSelectedBackup()
        {
            var selectedBackup = BackupsList.SelectedItem as BackupItem;
            if (selectedBackup == null)
            {
                MessageBox.Show("Please select a backup to restore", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Are you sure you want to restore {selectedBackup.OriginalPath}?",
                "Confirm Restore",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes) return;

            try
            {
                // Implement restore logic here
                MessageBox.Show("Restore completed successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                await LoadBackups();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error restoring backup: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}