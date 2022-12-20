using System.Text.Json;
using System.Text.Json.Serialization;

namespace Rule34.us.Downloader
{
    public class Config
    {
        [JsonInclude]
        public string savePath;

        [JsonIgnore]
        string defaultSavePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "rule34.us");
        [JsonIgnore]
        string configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "rule34.json");

        [JsonConstructor]
        public Config(string savePath)
        {
            this.savePath = savePath;
        }

        public Config()
        {
            Config? conf = GetExistingConfig(configPath) ?? Serialize(configPath);

            var som = conf.savePath;

            this.savePath = String.IsNullOrEmpty(conf.savePath) ? defaultSavePath : conf.savePath;
        }

        private Config? GetExistingConfig(string path)
        {
            if (!File.Exists(path))
            {
                File.Create(path).Close();
                savePath = defaultSavePath;
                return null;
            }

            using (StreamReader reader = new StreamReader(path))
            {
                string rawJson = reader.ReadToEnd();
                return JsonSerializer.Deserialize<Config>(rawJson);
            }
        }

        private Config Serialize(string path)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;
            options.IncludeFields = true;
            options.WriteIndented = true;

            File.WriteAllText(path, JsonSerializer.Serialize(this, options));

            return this;
        }
    }
}
