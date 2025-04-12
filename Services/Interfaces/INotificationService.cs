using System;

namespace SentinelPro.Services.Interfaces
{
    public interface INotificationService
    {
        void ShowError(string message, string title = "Error");
        void ShowWarning(string message, string title = "Warning");
        void ShowInfo(string message, string title = "Information");
        void ShowSuccess(string message, string title = "Success");
    }
}