namespace SentinelPro.Models;

public class ConfigurationModel
{
    public string BackupLocation { get; set; } = @"C:\Backups";
    public int RetentionDays { get; set; } = 30;
    public bool EnableEncryption { get; set; } = true;
    public List<WorkspaceRule> ActiveRules { get; set; } = new();
    public GeminiSettings AISettings { get; set; } = new();
}

public class GeminiSettings
{
    public string ProjectId { get; set; } = "your-project-id";
    public string LocationId { get; set; } = "us-central1";
    public string ModelName { get; set; } = "gemini-pro";
}