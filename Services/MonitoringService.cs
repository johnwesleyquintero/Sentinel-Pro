using System; // Added for ArgumentNullException, IDisposable, GC
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks; // Added for Task
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging; // Added for ILogger
using SentinelPro.Services.Interfaces; // Added for INotificationService
using SentinelPro.Services.Exceptions; // Added for PerformanceThresholdExceededException

namespace SentinelPro.Services
{
    /// <summary>
    /// Service responsible for monitoring application performance metrics including CPU and RAM usage.
    /// Provides real-time monitoring and notifications when resource usage exceeds configured thresholds.
    /// </summary>
    public class MonitoringService : IMonitoringService, IDisposable // Added IDisposable
    {
        private readonly ILogger<MonitoringService> _logger; // Added logger field
        private readonly INotificationService _notificationService; // Added notification service field
        private readonly PerformanceCounter _cpuCounter;
        private readonly PerformanceCounter _ramCounter;
        private readonly string _processName;
        private readonly IConfiguration _configuration;
        private bool _disposed; // Added _disposed field

        /// <summary>
        /// Initializes a new instance of the MonitoringService with the specified configuration.
        /// Sets up performance counters for CPU and RAM monitoring.
        /// </summary>
        /// <param name="configuration">The application configuration containing monitoring settings.</param>
        /// <param name="logger">Logger instance.</param>
        /// <param name="notificationService">Notification service instance.</param>
        public MonitoringService(IConfiguration configuration, ILogger<MonitoringService> logger, INotificationService notificationService) // Added logger and notificationService parameters
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); // Assign logger
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService)); // Assign notification service
            _processName = Process.GetCurrentProcess().ProcessName;

            // Platform check for PerformanceCounter
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                try
                {
                    // Use readOnly=true for counters if possible, might require admin privileges otherwise
                    _cpuCounter = new PerformanceCounter("Process", "% Processor Time", _processName, true);
                    _ramCounter = new PerformanceCounter("Process", "Working Set", _processName, true);
                    InitializeMonitoring();
                }
                catch (Exception ex)
                {
                     _logger.LogError(ex, "Failed to initialize Windows performance counters for process {ProcessName}. Monitoring will be limited.", _processName);
                     _cpuCounter = null;
                     _ramCounter = null;
                }
            }
            else
            {
                _logger.LogWarning("Performance counters are only available on Windows. Monitoring is disabled.");
                _cpuCounter = null;
                _ramCounter = null;
            }
        }

        public void StartMonitoring()
        {
            InitializeMonitoring();
        }

        public void StopMonitoring()
        {
            Dispose();
        }

        /// <summary>
        /// Initializes the performance monitoring system with configured thresholds.
        /// Reads the performance threshold from configuration and starts the monitoring process.
        /// </summary>
        private void InitializeMonitoring()
        {
             // Only initialize if counters are available
            if (_cpuCounter == null || _ramCounter == null)
            {
                _logger.LogInformation("Skipping monitoring initialization as performance counters are not available.");
                return;
            }

            try
            {
                var performanceThreshold = _configuration.GetValue<double>("PerformanceThreshold", 80.0);
                var memoryThresholdMB = _configuration.GetValue<double>("MemoryThresholdMB", 1024.0); // Use MB consistently

                StartPerformanceMonitoring(performanceThreshold, memoryThresholdMB); // Pass both thresholds
                // TrackOperationLatency(); // Commented out - needs review
                _logger.LogInformation("Performance monitoring initialized with thresholds: CPU {CpuThreshold}%, Memory {MemThreshold}MB",
                    performanceThreshold, memoryThresholdMB);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize performance monitoring"); // Use injected logger
            }
        }

        // Removed TrackOperationLatency, TrackCpuUsage, TrackMemoryUsage methods

        /// <summary>
        /// Starts continuous monitoring of CPU and RAM usage in a background task.
        /// Triggers notifications when usage exceeds specified thresholds.
        /// </summary>
        /// <param name="cpuThreshold">The CPU usage threshold percentage that triggers warnings.</param>
        /// <param name="memoryThresholdMB">The Memory usage threshold in MB that triggers warnings.</param>
        private void StartPerformanceMonitoring(double cpuThreshold, double memoryThresholdMB) // Added memory threshold parameter
        {
             // Check again if counters are valid before starting task
            if (_cpuCounter == null || _ramCounter == null)
            {
                _logger.LogWarning("Cannot start performance monitoring task as counters are not initialized.");
                return;
            }

            Task.Run(async () =>
            {
                _logger.LogInformation("Starting background performance monitoring task.");
                // Initial reads to prime the counters
                try
                {
                    _cpuCounter.NextValue();
                    _ramCounter.NextValue();
                    await Task.Delay(TimeSpan.FromSeconds(1)); // Wait a bit before the loop
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during initial read of performance counters. Monitoring task cannot start.");
                    return; // Exit task if initial read fails
                }


                while (!_disposed)
                {
                    try
                    {
                        // Ensure counters are still valid before reading
                        if (_cpuCounter == null || _ramCounter == null) break;

                        float cpuUsage = 0;
                        float ramUsageMB = 0;

                        try
                        {
                             cpuUsage = _cpuCounter.NextValue();
                             ramUsageMB = _ramCounter.NextValue() / 1024 / 1024; // MB
                             _logger.LogDebug("Current Usage - CPU: {CpuUsage:F1}%, RAM: {RamUsageMB:F1}MB", cpuUsage, ramUsageMB);
                        }
                        catch (InvalidOperationException ioex)
                        {
                            _logger.LogError(ioex, "Error reading performance counters. Monitoring might be stopped.");
                            // Consider stopping the loop or re-initializing counters if possible
                            break; // Exit loop on counter error
                        }


                        bool thresholdExceeded = false;
                        // string exceptionMessage = ""; // Removed as throw is removed

                        if (cpuUsage > cpuThreshold)
                        {
                            thresholdExceeded = true;
                            var cpuMessage = $"CPU usage {cpuUsage:F1}% exceeds threshold {cpuThreshold}%";
                            _logger.LogWarning(cpuMessage); // Use injected logger
                            _notificationService.ShowWarning($"High CPU usage: {cpuUsage:F1}%"); // Use injected service
                            // Optionally throw: throw new PerformanceThresholdExceededException(cpuThreshold, cpuUsage);
                        }

                        if (ramUsageMB > memoryThresholdMB)
                        {
                             thresholdExceeded = true;
                             var memMessage = $"Memory usage {ramUsageMB:F1}MB exceeds threshold {memoryThresholdMB}MB";
                            _logger.LogWarning(memMessage); // Use injected logger
                            _notificationService.ShowWarning($"High memory usage: {ramUsageMB:F1} MB"); // Use injected service
                             // Optionally throw: throw new PerformanceThresholdExceededException(memoryThresholdMB, ramUsageMB);
                        }

                        // Removed throw new PerformanceThresholdExceededException(...)

                        await Task.Delay(TimeSpan.FromSeconds(30)); // Monitoring interval
                    }
                    // Removed catch for PerformanceThresholdExceededException as it's no longer thrown here
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Unhandled error in performance monitoring loop."); // Use injected logger
                        await Task.Delay(TimeSpan.FromMinutes(5)); // Longer delay on generic error
                    }
                }
                 _logger.LogInformation("Background performance monitoring task stopped.");
            });
        }

        // Removed duplicate _disposed field

        // Correct Dispose pattern
        public void Dispose()
        {
            Dispose(true); // Call disposing version
            GC.SuppressFinalize(this);
        }

        // Protected virtual Dispose method
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                // Dispose managed state (managed objects).
                 _logger.LogDebug("Disposing MonitoringService resources.");
                _cpuCounter?.Dispose();
                _ramCounter?.Dispose();
            }

            // Free unmanaged resources (unmanaged objects) and override finalizer
            // Set large fields to null

            _disposed = true;
        }

        // Finalizer (optional, only if you have unmanaged resources directly in this class)
         ~MonitoringService()
         {
             // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
             Dispose(disposing: false);
         }
    }
}
