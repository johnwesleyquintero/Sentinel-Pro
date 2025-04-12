using System;
using System.Windows;

namespace SentinelPro.Services
{
    /// <summary>
    /// Provides a centralized way to handle and display error messages.
    /// </summary>
    public class ErrorHandlingService
    {
        /// <summary>
        /// Displays an error message to the user.
        /// </summary>
        /// <param name="message">The error message to display.</param>
        /// <param name="title">The title of the error message box.</param>
        public void ShowError(string message, string title = "Error")
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Displays an warning message to the user.
        /// </summary>
        /// <param name="message">The warning message to display.</param>
        /// <param name="title">The title of the warning message box.</param>
        public void ShowWarning(string message, string title = "Warning")
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        /// <summary>
        /// Displays an information message to the user.
        /// </summary>
        /// <param name="message">The information message to display.</param>
        /// <param name="title">The title of the information message box.</param>
        public void ShowInfo(string message, string title = "Information")
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}