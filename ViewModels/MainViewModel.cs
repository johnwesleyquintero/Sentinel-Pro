using System.Windows.Input;
using WorkspaceCleanup.Models;

namespace WorkspaceCleanup.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private object? _currentView;
        private readonly IAIService _aiService;
        private readonly ConfigurationModel _configuration;

        public object? CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateBackupsCommand { get; }
        public ICommand NavigateSettingsCommand { get; }

        public MainViewModel(IAIService aiService, ConfigurationModel configuration)
        {
            _aiService = aiService;
            _configuration = configuration;

            NavigateHomeCommand = new RelayCommand(_ => NavigateToHome());
            NavigateBackupsCommand = new RelayCommand(_ => NavigateToBackups());
            NavigateSettingsCommand = new RelayCommand(_ => NavigateToSettings());

            // Initialize with home view
            NavigateToHome();
        }

        private void NavigateToHome()
        {
            CurrentView = new HomeViewModel(_aiService);
        }

        private void NavigateToBackups()
        {
            CurrentView = new BackupsViewModel(_configuration);
        }

        private void NavigateToSettings()
        {
            CurrentView = new SettingsViewModel(_configuration);
        }
    }
}