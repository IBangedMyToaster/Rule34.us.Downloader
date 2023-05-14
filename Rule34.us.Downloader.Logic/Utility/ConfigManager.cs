using System.Text.Json;

namespace Rule34.us.Downloader.Logic.Utility
{
    public class ConfigManager
    {
        public Config Configuration { get; private set; }
        public string GetSavePath => Configuration.SavePath;

        public ConfigManager()
        {
            Configuration = GetConfig();
        }

        internal Config GetConfig()
        {
            return CheckForExistingConfig();
        }

        internal Config CheckForExistingConfig()
        {
            if (!File.Exists(Config.configPath))
            {
                File.Create(Config.configPath).Close();

                Config conf = new();
                Save(conf);

                return conf;
            }

            using StreamReader reader = new(Config.configPath);
            string rawJson = reader.ReadToEnd();
            return JsonSerializer.Deserialize<Config>(rawJson) ?? throw new ArgumentNullException("Existing config was empty!");
        }

        public void Save(Config config)
        {
            _ = Serialize(Config.configPath, config);
        }

        private T Serialize<T>(string path, T val)
        {
            JsonSerializerOptions options = new()
            {
                PropertyNameCaseInsensitive = true,
                IncludeFields = false,
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(val, options);
            File.WriteAllText(path, json);

            return val;
        }
    }
}
