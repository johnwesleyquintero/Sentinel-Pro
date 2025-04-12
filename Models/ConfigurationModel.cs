using System.Text.Json;

namespace SentinelPro.Models
{
    public class ConfigurationModel
    {
        public string DefaultWorkspacePath { get; set; } = string.Empty;
        public List<WorkspaceRule> WorkspaceRules { get; set; } = new();
        public string BackupDirectory { get; set; } = string.Empty;

        public static ConfigurationModel LoadConfiguration()
        {
            var configPath = GetConfigurationPath();
            if (!File.Exists(configPath))
            {
                return CreateDefaultConfiguration();
            }

            try
            {
                var jsonContent = File.ReadAllText(configPath);
                return JsonSerializer.Deserialize<ConfigurationModel>(jsonContent) ?? CreateDefaultConfiguration();
            }
            catch
            {
                return CreateDefaultConfiguration();
            }
        }

        public void SaveConfiguration()
        {
            var configPath = GetConfigurationPath();
            var configDir = Path.GetDirectoryName(configPath);
            
            if (!string.IsNullOrEmpty(configDir) && !Directory.Exists(configDir))
            {
                Directory.CreateDirectory(configDir);
            }

            var jsonContent = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(configPath, jsonContent);
        }

        private static ConfigurationModel CreateDefaultConfiguration()
        {
            return new ConfigurationModel
            {
                DefaultWorkspacePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                BackupDirectory = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    "__cleanup_backups__"
                )
            };
        }

        private static string GetConfigurationPath()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(appDataPath, "Sentinel-Pro", "config.json");
        }
    }

    public class WorkspaceRule
    {
        public string Path { get; set; } = string.Empty;
        public string Pattern { get; set; } = string.Empty;
        public bool IncludeSubdirectories { get; set; } = true;
        public CleanupAction Action { get; set; } = CleanupAction.Backup;
    }

    public enum CleanupAction
    {
        Backup,
        Delete,
        Ignore
    }
}