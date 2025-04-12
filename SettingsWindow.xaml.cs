using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using SentinelPro.Models;
using SentinelPro.ViewModels;

namespace SentinelPro
{
    public partial class SettingsWindow : Page
    {
        private readonly SettingsViewModel _viewModel;

        public SettingsWindow()
        {
            InitializeComponent();
            _viewModel = App.Current.Services.GetService<SettingsViewModel>();
            DataContext = _viewModel;
        }

        private void BrowseDefaultWorkspace_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.BrowseWorkspaceCommand.Execute(null);
        }

        private void BrowseBackupDirectory_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.BrowseBackupCommand.Execute(null);
        }

        private async void AddRule_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new RuleEditDialog();
            var window = new Window
            {
                Content = dialog,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            if (window.ShowDialog() == true)
            {
                await _viewModel.AddRuleAsync(dialog.Rule);
            }
        }

        private async void EditRule_Click(object sender, RoutedEventArgs e)
        {
            if (RulesListView.SelectedItem is WorkspaceRule selectedRule)
            {
                var dialog = new RuleEditDialog(selectedRule);
                var window = new Window
                {
                    Content = dialog,
                    SizeToContent = SizeToContent.WidthAndHeight,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };

                if (window.ShowDialog() == true)
                {
                    await _viewModel.UpdateRuleAsync(selectedRule, dialog.Rule);
                }
            }
        }

        private async void RemoveRule_Click(object sender, RoutedEventArgs e)
        {
            if (RulesListView.SelectedItem is WorkspaceRule selectedRule)
            {
                await _viewModel.RemoveRuleAsync(selectedRule);
            }
        }

        private async void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.SaveSettingsAsync();
        }
    }
}