using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SentinelPro.Services;
using SentinelPro.ViewModels;
using SentinelPro.Views;
using System;
using System.IO;
using System.Windows;
using SentinelPro.Services.Interfaces;

namespace SentinelPro
{
    /// <summary>
    /// Application entry point for the Sentinel Pro application.
    /// Handles application-level events, configuration, service registration (DI), and startup.
    /// </summary>
    public partial class App : Application, IDisposable
    {
        private IHost _host = null!;
        private ILogger<App> _logger = null!;
        public IServiceProvider Services { get; private set; } = null!;
        public IConfiguration Configuration { get; private set; } = null!;

        public App()
        {
            // Optional: Initialize things before host creation if needed
        }
        /// <summary>
        /// Gets the current instance of the application.
        /// </summary>
        public new static App Current => (App)Application.Current;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ServiceLocator.Initialize();
            Services = ServiceLocator.GetService<IServiceProvider>();
            Configuration = ServiceLocator.GetService<IConfiguration>();
            _logger = Services.GetService<ILogger<App>>();

            var mainWindow = Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            try
            {
                var monitoringService = Services.GetService<IMonitoringService>();
                monitoringService?.StartMonitoring();
                _logger?.LogInformation("Monitoring service started.");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to start monitoring service.");
            }

            Current.Exit += App_Exit;
            Current.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            _logger?.LogCritical(e.Exception, "An unhandled UI exception occurred.");

            var errorHandlingService = Services?.GetService<IErrorHandlingService>();
            if (errorHandlingService != null)
            {
                errorHandlingService.HandleException(e.Exception);
            }
            else
            {
                // Fallback if DI/Error Service fails
                MessageBox.Show($"An unexpected error occurred: {e.Exception.Message}", "Unhandled Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Prevent default WPF crash behavior
            e.Handled = true;

            // Optional: Decide if the application should shut down on unhandled exceptions
            // Shutdown(-1);
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            _logger?.LogInformation("Application shutting down...");

            var monitoringService = _host?.Services.GetService<IMonitoringService>();
            monitoringService?.StopMonitoring();

            if (_host != null)
            {
                //await _host.StopAsync();
                _host.Dispose();
                _host = null;
            }

            _logger?.LogInformation("Application shutdown complete.");
        }

        public void Dispose()
        {
            _host?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
