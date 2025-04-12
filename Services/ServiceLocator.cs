using Microsoft.Extensions.DependencyInjection;
using WorkspaceCleanup.Models;

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
            services.AddSingleton<IAIService, OllamaAIService>();
            services.AddHttpClient();

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