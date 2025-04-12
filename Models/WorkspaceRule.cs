namespace SentinelPro.Models;

public class WorkspaceRule
{
    public required string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public string TargetDirectory { get; set; } = string.Empty;
    public List<string> FilePatterns { get; set; } = new();
    public RetentionPolicy RetentionSettings { get; set; } = new();
    public bool EnableCompression { get; set; }
    public bool EnableEncryption { get; set; }
}

public class RetentionPolicy
{
    public int KeepLastNDays { get; set; } = 30;
    public int MaxTotalSizeGB { get; set; } = 100;
    public string CleanupSchedule { get; set; } = "0 0 * * *";
}