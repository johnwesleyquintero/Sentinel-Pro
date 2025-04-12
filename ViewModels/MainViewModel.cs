using System.Windows.Input;
using SentinelPro.Models;
using SentinelPro.Services;
using SentinelPro.Services.Interfaces;

namespace SentinelPro.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private object? _currentView;
        private readonly IAIService _aiService;
        private readonly ConfigurationModel _configuration;
        private readonly BackupModel _backupModel;
        private readonly INotificationService _notificationService;
        private readonly ILoggingService _logService;

        public object? CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateBackupsCommand { get; }
        public ICommand NavigateSettingsCommand { get; }

        public MainViewModel(IAIService aiService, ConfigurationModel configuration, BackupModel backupModel, INotificationService notificationService, ILoggingService logService)
        {
            _aiService = aiService;
            _configuration = configuration;
            _backupModel = backupModel;
            _notificationService = notificationService;
            _logService = logService;

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
            CurrentView = new BackupsViewModel(_configuration, _backupModel, _notificationService, _logService); // Pass dependencies
        }

        private void NavigateToSettings()
        {
            CurrentView = new SettingsViewModel(_configuration);
        }
    }
}
