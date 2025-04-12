using System;

namespace SentinelPro.Services.Interfaces
{
    public interface ILoggingService
    {
        void Debug(string message);
        void Info(string message);
        void Warning(string message);
        void Error(string message);
        void Error(Exception ex, string message);
        void Fatal(string message);
        void Fatal(Exception ex, string message);
    }
}