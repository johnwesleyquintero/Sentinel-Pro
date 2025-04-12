using Serilog;
using System.IO;

namespace SentinelPro.Services
{
    /// <summary>
    /// Provides centralized logging functionality for the application using Serilog.
    /// Handles log initialization, file management, and application-wide logging operations.
    /// </summary>
    public static class LoggingService
    {
        /// <summary>
        /// Gets the path where log files are stored. Located in the LocalApplicationData folder
        /// under WorkspaceCleanup/Logs directory.
        /// </summary>
        private static readonly string LogPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "WorkspaceCleanup",
            "Logs");

        /// <summary>
        /// Initializes the logging service by setting up Serilog configuration.
        /// Creates the log directory if it doesn't exist and configures the logger with
        /// daily rolling file output, 31-day retention policy, and structured logging format.
        /// </summary>
        public static void Initialize()
        {
            Directory.CreateDirectory(LogPath);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(
                    Path.Combine(LogPath, "log-.txt"),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 31,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            Log.Information("Application started");
        }

        /// <summary>
        /// Performs a clean shutdown of the logging service.
        /// Ensures all pending log entries are written and resources are properly released.
        /// </summary>
        public static void Shutdown()
        {
            Log.Information("Application shutting down");
            Log.CloseAndFlush();
        }
    }
}