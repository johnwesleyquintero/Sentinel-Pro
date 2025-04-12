using System.Windows;
using Microsoft.Win32;
using WorkspaceCleanup.Models;

namespace WorkspaceCleanup
{
    public partial class RuleEditDialog : Window
    {
        private readonly WorkspaceRule _rule;

        public RuleEditDialog(WorkspaceRule rule)
        {
            InitializeComponent();
            _rule = rule;
            LoadRuleToUI();
        }

        private void LoadRuleToUI()
        {
            PathTextBox.Text = _rule.Path;
            PatternTextBox.Text = _rule.Pattern;
            IncludeSubdirectoriesCheckBox.IsChecked = _rule.IncludeSubdirectories;
            ActionComboBox.SelectedIndex = (int)_rule.Action;
        }

        private void BrowsePath_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog
            {
                Title = "Select Path",
                InitialDirectory = PathTextBox.Text
            };

            if (dialog.ShowDialog() == true)
            {
                PathTextBox.Text = dialog.FolderName;
            }
        }

        private void SaveRule_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PathTextBox.Text))
            {
                MessageBox.Show("Path is required", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _rule.Path = PathTextBox.Text;
            _rule.Pattern = PatternTextBox.Text;
            _rule.IncludeSubdirectories = IncludeSubdirectoriesCheckBox.IsChecked ?? true;
            _rule.Action = (CleanupAction)ActionComboBox.SelectedIndex;

            DialogResult = true;
            Close();
        }

        private void CancelRule_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}