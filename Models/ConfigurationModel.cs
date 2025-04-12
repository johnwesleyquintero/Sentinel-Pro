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
    public string ProjectId { get; set; } = Environment.GetEnvironmentVariable("GOOGLE_CLOUD_PROJECT_ID");
    public string LocationId { get; set; } = "us-central1"; // Default remains valid
    public string ModelName { get; set; } = "gemini-pro"; // Default remains valid
}