using System.Windows;
using WorkspaceCleanup.Services;

namespace SentinelPro
{
    /// <summary>
    /// Application entry point for the Workspace Cleanup tool.
    /// Handles application-level events and initialization.
    /// </summary>
    public partial class App : Application
    {
        private MonitoringService? _monitoringService;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Initialize services
            LoggingService.Initialize();
            ErrorHandlingService.Initialize();
            ServiceLocator.Initialize();

            // Start monitoring
            _monitoringService = new MonitoringService(ServiceLocator.GetService<IConfiguration>());

            // Handle application exit
            Current.Exit += (s, args) =>
            {
                _monitoringService?.Dispose();
                LoggingService.Shutdown();
                ServiceLocator.Shutdown();
            };
        }
    }
}