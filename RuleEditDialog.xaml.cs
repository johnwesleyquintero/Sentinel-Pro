using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using SentinelPro.Models;
using SentinelPro.ViewModels;

namespace SentinelPro
{
    public partial class RuleEditDialog : Page
    {
        private readonly WorkspaceRule _originalRule;
        public WorkspaceRule Rule { get; private set; }

        public RuleEditDialog(WorkspaceRule rule = null)
        {
            InitializeComponent();
            _originalRule = rule;
            Rule = rule?.Clone() ?? new WorkspaceRule();
            DataContext = Rule;

            // Set default values for new rules
            if (rule == null)
            {
                Rule.IsEnabled = true;
                Rule.RetentionDays = 30;
            }
        }



        private void BrowsePath_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Rule.Path = dialog.SelectedPath;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateRule())
            {
                DialogResult = true;
                Close();
            }
        }

        private bool ValidateRule()
        {
            if (string.IsNullOrWhiteSpace(Rule.Path))
            {
                MessageBox.Show("Please select a valid path.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (Rule.RetentionDays <= 0)
            {
                MessageBox.Show("Retention days must be greater than 0.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}