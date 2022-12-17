using System.Text.Json;
using System.Text.Json.Serialization;

namespace Rule34.us.Downloader
{
    public class Config
    {
        [JsonInclude]
        public string savePath = "Results";

        [JsonConstructor]
        public Config(string savePath)
        {
            this.savePath = savePath;
        }

        public Config(string savePath, bool serializer)
        {
            if (!File.Exists(savePath))
            {
                File.SetAttributes(savePath, (new FileInfo(savePath)).Attributes | FileAttributes.Normal);
                File.Create(savePath).Close();
                Serialize(savePath);
                return;
            }

            Config? conf = InitializeConfig(savePath) ?? Serialize(savePath);

            this.savePath = conf.savePath;
        }

        private Config? InitializeConfig(string path)
        {
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
