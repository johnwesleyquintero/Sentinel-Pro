using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace SentinelPro
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Set default navigation (e.g., to Home page)
            // Ensure the corresponding RadioButton IsChecked=True in XAML or set it here
            NavigateToPage("Home"); // Or read initial page from ViewModel/Config
        }

        /// <summary>
        /// Handles the Checked event of navigation RadioButtons to switch pages.
        /// </summary>
        private void NavigationButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton { Tag: string pageName } radioButton && radioButton.IsChecked == true)
            {
                NavigateToPage(pageName);
            }
        }

        /// <summary>
        /// Navigates the main content frame to the specified page.
        /// </summary>
        /// <param name="pageName">The name (or key) of the page to navigate to.</param>
        private void NavigateToPage(string pageName)
        {
            // Consider using a more robust mapping (e.g., Dictionary<string, Type>)
            // Navigation functionality will be implemented later
            // Currently just showing the welcome message in main content area
        }

        // Note: The SettingsWindow is likely opened from a Menu or Button defined in MainWindow.xaml
        // Example handler (assuming a button named SettingsButton exists in MainWindow.xaml):
        // private void SettingsButton_Click(object sender, RoutedEventArgs e)
        // {
        //     var settingsWindow = _serviceProvider.GetRequiredService<SettingsWindow>();
        //     settingsWindow.Owner = this; // Optional: Set owner
        //     settingsWindow.ShowDialog();
        // }

        // Removed methods:
        // - CreateBackupsPage()
        // - CreateSettingsPage()
        // - FindVisualChildren<T>() (Unused)
        // - SetupEventHandlers() (Logic moved to BackupsPage/ViewModel)
        // - LoadBackups() (Logic moved to BackupsViewModel)
        // - RestoreSelectedBackup() (Logic moved to BackupsViewModel)
        // - OpenBackupDirectory() (Logic moved to BackupsViewModel)

        // Removed fields:
        // - _errorHandlingService (Should be used within specific ViewModels)
        // - _backupDir (Managed by Settings/Configuration and BackupsViewModel)
        // - _rollbackFile (Managed by Settings/Configuration and BackupsViewModel)
        // - _backups (Managed by BackupsViewModel)

        // Removed helper classes defined within this file:
        // - BackupItem (Moved to Models/BackupItem.cs)
        // - BackupInfo (Moved to Models/BackupInfo.cs)
        // - RelayCommand (Moved to Commands/RelayCommand.cs or similar)
    }
}
