namespace SentinelPro.Models;

public class WorkspaceRule
{
    public required string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public string TargetDirectory { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public List<string> FilePatterns { get; set; } = new();
    public RetentionPolicy RetentionSettings { get; set; } = new();
    public bool EnableCompression { get; set; }
    public bool EnableEncryption { get; set; }
    public bool IsEnabled { get; set; } = true;
    public int RetentionDays { get; set; } = 30;
    public WorkspaceRule Clone()
    {
        return new WorkspaceRule
        {
            Name = this.Name,
            Description = this.Description,
            TargetDirectory = this.TargetDirectory,
            Path = this.Path,
            FilePatterns = new List<string>(this.FilePatterns),
            RetentionSettings = new RetentionPolicy
            {
                KeepLastNDays = this.RetentionSettings.KeepLastNDays,
                MaxTotalSizeGB = this.RetentionSettings.MaxTotalSizeGB,
                CleanupSchedule = this.RetentionSettings.CleanupSchedule
            },
            EnableCompression = this.EnableCompression,
            EnableEncryption = this.EnableEncryption,
            IsEnabled = this.IsEnabled,
            RetentionDays = this.RetentionDays
        };
    }
}

public class RetentionPolicy
{
    public int KeepLastNDays { get; set; } = 30;
    public int MaxTotalSizeGB { get; set; } = 100;
    public string CleanupSchedule { get; set; } = "0 0 * * *";
}