using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Extensions.DependencyInjection;
using SentinelPro.Models;
using SentinelPro.Services;
using SentinelPro.ViewModels;
using SentinelPro.Views;

namespace SentinelPro
{
    /// <summary>
    /// Main window of the Workspace Cleanup application, providing backup management and restoration functionality.
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;
        private readonly IErrorHandlingService _errorHandlingService;
        private string _backupDir;
        private string _rollbackFile;
        private readonly ObservableCollection<BackupItem> _backups = new ObservableCollection<BackupItem>();

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// Sets up the backup directory, loads existing backups, and initializes event handlers.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // Get ViewModel from DI container
            _viewModel = ServiceLocator.GetService<MainViewModel>();
            DataContext = _viewModel;

            // Navigate to default page
            NavigateToPage("Home");
        }

        void NavigationButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton)
            {
                string pageName = radioButton.Tag?.ToString();
                if (!string.IsNullOrEmpty(pageName))
                {
                    NavigateToPage(pageName);
                }
            }
        }

        void NavigateToPage(string pageName)
        {
            Page page = pageName switch
            {
                "Home" => new HomePage(),
                "Backups" => new BackupsPage(),
                "Settings" => new SettingsWindow(),
                _ => new HomePage()
            };

            ContentFrame.Navigate(page);
        }

        private Frame CreateBackupsPage()
        {
            var frame = new Frame();
            frame.Content = new Views.BackupsPage();
            return frame;
        }

        private Frame CreateSettingsPage()
        {
            var frame = new Frame();
            var panel = new StackPanel();

            panel.Children.Add(new CheckBox
            {
                Content = "Show detailed information",
                IsChecked = false,
                Margin = new Thickness(0, 0, 0, 8)
            });

            var openDirButton = new Button
            {
                Content = "Open Backup Directory",
                Command = new RelayCommand(() => Process.Start("explorer.exe", _backupDir))
            };

            panel.Children.Add(openDirButton);
            frame.Content = panel;

            return frame;
        }

        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) yield break;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);
                if (child is T t) yield return t;

                foreach (T childOfChild in FindVisualChildren<T>(child))
                    yield return childOfChild;
            }
        }

        private void SetupEventHandlers()
        {
            if (RestoreButton != null)
            {
                RestoreButton.Click += async (s, e) => await RestoreSelectedBackup();
            }
            if (RefreshButton != null)
            {
                RefreshButton.Click += async (s, e) => await LoadBackups();
            }
            if (OpenBackupDirButton != null)
            {
                OpenBackupDirButton.Click += (s, e) => OpenBackupDirectory();
            }
        }

        private async Task LoadBackups()
        {
            try
            {
                if (StatusText != null)
                {
                    StatusText.Text = "Loading backups...";
                }
                _backups.Clear();

                if (!File.Exists(_rollbackFile))
                {
                    if (StatusText != null)
                    {
                        StatusText.Text = "No backup history found";
                    }
                    return;
                }

                var jsonContent = await File.ReadAllTextAsync(_rollbackFile);
                var backupItems = JsonSerializer.Deserialize<List<BackupItem>>(jsonContent);

                if (backupItems != null)
                {
                    foreach (var item in backupItems)
                    {
                        _backups.Add(item);
                    }
                }

                if (StatusText != null)
                {
                    StatusText.Text = $"Loaded {_backups.Count} backups";
                }
            }
            catch (Exception ex)
            {
                _errorHandlingService.ShowError($"Error loading backups: {ex.Message}");
                if (StatusText != null)
                {
                    StatusText.Text = "Error loading backups";
                }
            }
        }

        private async Task RestoreSelectedBackup()
        {
            if (BackupsList == null)
            {
                return;
            }
            var selectedBackup = BackupsList.SelectedItem as BackupItem;
            if (selectedBackup == null)
            {
                _errorHandlingService.ShowWarning("Please select a backup to restore");
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
                if (StatusText != null)
                {
                    StatusText.Text = "Restoring backup...";
                }

                using (var ps = PowerShell.Create())
                {
                    ps.AddScript($".\\workspace_cleanup_rollback.ps1 -RollbackId '{selectedBackup.BackupInfo.Timestamp}'");
                    var results = await ps.InvokeAsync();

                    if (ps.HadErrors)
                    {
                        throw new Exception(string.Join("\n", ps.Streams.Error.Select(e => e.ToString())));
                    }

                    if (StatusText != null)
                    {
                        StatusText.Text = "Backup restored successfully";
                    }
                    _errorHandlingService.ShowInfo("Backup restored successfully");
                }
            }
            catch (Exception ex)
            {
                _errorHandlingService.ShowError($"Error restoring backup: {ex.Message}");
                if (StatusText != null)
                {
                    StatusText.Text = "Error restoring backup";
                }
            }
        }

        private void OpenBackupDirectory()
        {
            if (Directory.Exists(_backupDir))
            {
                Process.Start("explorer.exe", _backupDir);
            }
            else
            {
                _errorHandlingService.ShowWarning("Backup directory does not exist");
            }
        }
    }

    /// <summary>
    /// Represents a backup item in the workspace cleanup system.
    /// </summary>
    public class BackupItem
    {
        /// <summary>
        /// Gets or sets the original path of the backed up item.
        /// </summary>
        public string OriginalPath { get; set; }

        /// <summary>
        /// Gets or sets the backup information associated with this item.
        /// </summary>
        public BackupInfo BackupInfo { get; set; }

        /// <summary>
        /// Gets or sets the action performed during the backup operation.
        /// </summary>
        public string Action { get; set; }
    }

    /// <summary>
    /// Contains detailed information about a backup operation.
    /// </summary>
    public class BackupInfo
    {
        /// <summary>
        /// Gets or sets the timestamp when the backup was created.
        /// </summary>
        public string Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the path where the backup is stored.
        /// </summary>
        public string BackupPath { get; set; }

        /// <summary>
        /// Gets or sets the checksum of the backed up content for integrity verification.
        /// </summary>
        public string Checksum { get; set; }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke() ?? true;
        public void Execute(object parameter) => _execute();
    }
}
