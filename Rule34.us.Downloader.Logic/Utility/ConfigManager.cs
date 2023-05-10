using Rule34.us.Downloader.Logic;
using System.Text.Json;

namespace Rule34.us.Downloader.Logic.Utility
{
    internal class ConfigManager
    {
        internal Config GetConfig()
        {
            return CheckForExistingConfig();
        }

        internal Config CheckForExistingConfig()
        {
            if (!File.Exists(Config.configPath))
            {
                File.Create(Config.configPath).Close();

                var conf = new Config();
                Save(conf);

                return conf;
            }

            using (StreamReader reader = new StreamReader(Config.configPath))
            {
                string rawJson = reader.ReadToEnd();
                return JsonSerializer.Deserialize<Config>(rawJson) ?? throw new ArgumentNullException("Existing config was empty!");
            }
        }

        public void Save(Config config) => Serialize(Config.configPath, config);

        private T Serialize<T>(string path, T val)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;
            options.IncludeFields = false;
            options.WriteIndented = true;

            string json = JsonSerializer.Serialize(val, options);
            File.WriteAllText(path, json);

            return val;
        }
    }
}
