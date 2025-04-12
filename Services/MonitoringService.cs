using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;

namespace WorkspaceCleanup.Services
{
    /// <summary>
    /// Service responsible for monitoring application performance metrics including CPU and RAM usage.
    /// Provides real-time monitoring and notifications when resource usage exceeds configured thresholds.
    /// </summary>
    public class MonitoringService
    {
        private readonly PerformanceCounter _cpuCounter;
        private readonly PerformanceCounter _ramCounter;
        private readonly string _processName;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the MonitoringService with the specified configuration.
        /// Sets up performance counters for CPU and RAM monitoring.
        /// </summary>
        /// <param name="configuration">The application configuration containing monitoring settings.</param>
        public MonitoringService(IConfiguration configuration)
        {
            _configuration = configuration;
            _processName = Process.GetCurrentProcess().ProcessName;

            _cpuCounter = new PerformanceCounter("Process", "% Processor Time", _processName);
            _ramCounter = new PerformanceCounter("Process", "Working Set", _processName);

            InitializeMonitoring();
        }

        /// <summary>
        /// Initializes the performance monitoring system with configured thresholds.
        /// Reads the performance threshold from configuration and starts the monitoring process.
        /// </summary>
        private void InitializeMonitoring()
        {
            try
            {
                var performanceThreshold = _configuration.GetValue<double>("PerformanceThreshold", 80.0);
                var memoryThreshold = _configuration.GetValue<double>("MemoryThreshold", 1024);
                
                StartPerformanceMonitoring(performanceThreshold);
                StartMemoryMonitoring(memoryThreshold);
                TrackOperationLatency();
                Log.Information("Performance monitoring initialized with thresholds: CPU {CpuThreshold}%, Memory {MemThreshold}MB", 
                    performanceThreshold, memoryThreshold);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to initialize performance monitoring");
            }
        }

        private void TrackOperationLatency()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            
            // Track latency for critical operations
            AppDomain.CurrentDomain.FirstChanceException += (sender, e) => 
                Log.Debug("Operation latency: {Latency}ms", stopwatch.ElapsedMilliseconds);
        }

        public void TrackCpuUsage()
        {
            var usage = _cpuCounter.NextValue();
            Log.Verbose("CPU Usage: {CpuUsage}%", usage);
            if (usage > _configuration.GetValue<double>("PerformanceThreshold", 80.0))
                Log.Warning("CPU threshold exceeded: {CpuUsage}%", usage);
        }

        public void TrackMemoryUsage()
        {
            var usage = _ramCounter.NextValue() / 1024 / 1024;
            Log.Verbose("Memory Usage: {MemoryUsage}MB", usage);
            if (usage > _configuration.GetValue<double>("MemoryThreshold", 1024))
                Log.Warning("Memory threshold exceeded: {MemoryUsage}MB", usage);
        }

        /// <summary>
        /// Starts continuous monitoring of CPU and RAM usage in a background task.
        /// Triggers notifications when usage exceeds specified thresholds.
        /// </summary>
        /// <param name="threshold">The CPU usage threshold percentage that triggers warnings.</param>
        private void StartPerformanceMonitoring(double threshold)
        {
            Task.Run(async () =>
            {
                while (!_disposed)
                {
                    try
                    {
                        var cpuUsage = _cpuCounter.NextValue();
                        var ramUsageMB = _ramCounter.NextValue() / 1024 / 1024;

                        if (cpuUsage > threshold)
                        {
                            Log.Warning($"High CPU usage detected: {cpuUsage:F1}%");
                            NotificationService.ShowWarning($"High CPU usage: {cpuUsage:F1}%");
                            throw new PerformanceThresholdExceededException($"CPU usage {cpuUsage:F1}% exceeds threshold {threshold}%");
                        }

                        if (ramUsageMB > _configuration.GetValue<double>("MemoryThreshold", 1024))
                        {
                            Log.Warning($"High memory usage detected: {ramUsageMB:F1} MB");
                            NotificationService.ShowWarning($"High memory usage: {ramUsageMB:F1} MB");
                            throw new PerformanceThresholdExceededException($"Memory usage {ramUsageMB:F1}MB exceeds threshold");
                        }

                        await Task.Delay(TimeSpan.FromSeconds(30));
                    }
                    catch (PerformanceThresholdExceededException ex)
                    {
                        Log.Error(ex, "Performance threshold exceeded");
                        await Task.Delay(TimeSpan.FromMinutes(1));
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Error in performance monitoring");
                        await Task.Delay(TimeSpan.FromMinutes(5));
                    }
                }
            });
        }

        private bool _disposed;

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            
            _cpuCounter?.Dispose();
            _ramCounter?.Dispose();
            
            GC.SuppressFinalize(this);
        }
    }
}