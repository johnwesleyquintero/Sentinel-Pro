using System.Windows;

namespace WorkspaceCleanup.Services
{
    public static class NotificationService
    {
        public static void ShowError(string message, string title = "Error")
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

            Log.Error(message);
        }

        public static void ShowWarning(string message, string title = "Warning")
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

            Log.Warning(message);
        }

        public static void ShowInfo(string message, string title = "Information")
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

            Log.Information(message);
        }

        public static bool ShowConfirmation(string message, string title = "Confirmation")
        {
            var result = false;

            Application.Current.Dispatcher.Invoke(() =>
            {
                result = MessageBox.Show(
                    message,
                    title,
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                ) == MessageBoxResult.Yes;
            });

            return result;
        }
    }
}