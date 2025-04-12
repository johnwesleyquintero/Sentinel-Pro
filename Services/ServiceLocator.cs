using Microsoft.Extensions.DependencyInjection;
using SentinelPro.Services.Interfaces;
using SentinelPro.ViewModels;
using SentinelPro.Models;
using SentinelPro.Services;

namespace SentinelPro.Services
{
    public static class ServiceLocator
    {
        private static IServiceProvider? _serviceProvider;

        public static void Initialize()
        {
            var services = new ServiceCollection();

            // Add encryption service
            services.AddSingleton<BackupEncryptionService>();

            // Register services
            services.AddSingleton<ConfigurationModel>();
            services.AddHttpClient();

            // Register ViewModels
            services.AddSingleton<MainViewModel>();
            services.AddTransient<SettingsViewModel>();
            services.AddSingleton<IErrorHandlingService, ErrorHandlingService>();
            _serviceProvider = services.BuildServiceProvider();
        }

        public static T GetService<T>() where T : class
        {
            if (_serviceProvider == null)
            {
                throw new InvalidOperationException("ServiceLocator is not initialized");
            }

            var service = _serviceProvider.GetService<T>();
            if (service == null)
            {
                throw new InvalidOperationException($"Service of type {typeof(T)} is not registered");
            }

            return service;
        }

        public static void Shutdown()
        {
            if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}