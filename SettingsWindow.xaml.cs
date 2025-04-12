using System.Windows;
using System.Windows.Controls;
using SentinelPro.Models;

namespace WorkspaceCleanup
{
    public partial class SettingsWindow : Window
    {
        private ConfigurationModel _configuration;

        public SettingsWindow()
        {
            InitializeComponent();
            _configuration = ConfigurationModel.LoadConfiguration();
            LoadConfigurationToUI();
        }

        private void LoadConfigurationToUI()
        {
            DefaultWorkspacePathTextBox.Text = _configuration.DefaultWorkspacePath;
            BackupDirectoryTextBox.Text = _configuration.BackupDirectory;
            RulesListView.ItemsSource = _configuration.WorkspaceRules;
        }

        private void BrowseDefaultWorkspace_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog
            {
                Title = "Select Default Workspace Path",
                InitialDirectory = DefaultWorkspacePathTextBox.Text
            };

            if (dialog.ShowDialog() == true)
            {
                DefaultWorkspacePathTextBox.Text = dialog.FolderName;
            }
        }

        private void BrowseBackupDirectory_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog
            {
                Title = "Select Backup Directory",
                InitialDirectory = BackupDirectoryTextBox.Text
            };

            if (dialog.ShowDialog() == true)
            {
                BackupDirectoryTextBox.Text = dialog.FolderName;
            }
        }

        private void AddRule_Click(object sender, RoutedEventArgs e)
        {
            var rule = new WorkspaceRule();
            var dialog = new RuleEditDialog(rule);

            if (dialog.ShowDialog() == true)
            {
                _configuration.WorkspaceRules.Add(rule);
                RulesListView.Items.Refresh();
            }
        }

        private void EditRule_Click(object sender, RoutedEventArgs e)
        {
            if (RulesListView.SelectedItem is WorkspaceRule rule)
            {
                var dialog = new RuleEditDialog(rule);
                if (dialog.ShowDialog() == true)
                {
                    RulesListView.Items.Refresh();
                }
            }
            else
            {
                MessageBox.Show("Please select a rule to edit", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void RemoveRule_Click(object sender, RoutedEventArgs e)
        {
            if (RulesListView.SelectedItem is WorkspaceRule rule)
            {
                var result = MessageBox.Show(
                    "Are you sure you want to remove this rule?",
                    "Confirm Remove",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _configuration.WorkspaceRules.Remove(rule);
                    RulesListView.Items.Refresh();
                }
            }
            else
            {
                MessageBox.Show("Please select a rule to remove", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            _configuration.DefaultWorkspacePath = DefaultWorkspacePathTextBox.Text;
            _configuration.BackupDirectory = BackupDirectoryTextBox.Text;
            _configuration.SaveConfiguration();
            DialogResult = true;
            Close();
        }

        private void CancelSettings_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}