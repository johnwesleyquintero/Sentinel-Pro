namespace WorkspaceCleanup.Services.Exceptions;

public class PerformanceThresholdExceededException : Exception
{
    public PerformanceThresholdExceededException(double threshold, double actualValue) 
        : base($"Performance threshold {threshold}% exceeded with {actualValue}% utilization")
    {
        Threshold = threshold;
        ActualValue = actualValue;
    }

    public double Threshold { get; }
    public double ActualValue { get; }
}