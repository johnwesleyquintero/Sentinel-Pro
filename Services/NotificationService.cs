using System; // Added for ArgumentNullException
using System.Windows;
using Microsoft.Extensions.Logging; // Added for ILogger
using SentinelPro.Services.Interfaces; // Added for INotificationService

namespace SentinelPro.Services
{
    // Changed from static class to instance class implementing INotificationService
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger; // Added logger field

        // Added constructor for dependency injection
        public NotificationService(ILogger<NotificationService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Changed from static to instance method
        public void ShowError(string message, string title = "Error")
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show(
                    message,
                    title,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            });

            _logger.LogError("Error Shown: {Title} - {Message}", title, message); // Use injected logger
        }

        // Changed from static to instance method
        public void ShowWarning(string message, string title = "Warning")
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show(
                    message,
                    title,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            });

             _logger.LogWarning("Warning Shown: {Title} - {Message}", title, message); // Use injected logger
        }

        // Changed from static to instance method
        public void ShowInfo(string message, string title = "Information")
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show(
                    message,
                    title,
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            });

            _logger.LogInformation("Info Shown: {Title} - {Message}", title, message); // Use injected logger
        }

        // Added missing method from INotificationService interface
        public void ShowSuccess(string message, string title = "Success")
        {
             Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show(
                    message,
                    title,
                    MessageBoxButton.OK,
                    MessageBoxImage.Information // Using Information icon for Success, adjust if needed
                );
            });
             _logger.LogInformation("Success Shown: {Title} - {Message}", title, message); // Use injected logger
        }


        // This method is not part of INotificationService but kept for potential direct usage
        // Changed from static to instance method
        public bool ShowConfirmation(string message, string title = "Confirmation")
        {
            var result = false;
            string logMessage = $"Confirmation Requested: {title} - {message}";

            Application.Current.Dispatcher.Invoke(() =>
            {
                result = MessageBox.Show(
                    message,
                    title,
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                ) == MessageBoxResult.Yes;
            });

            _logger.LogInformation("{LogMessage} - Result: {Result}", logMessage, result ? "Yes" : "No"); // Log confirmation result
            return result;
        }
    }
}
