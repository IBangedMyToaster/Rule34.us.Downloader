using System.Text.Json.Serialization;

namespace Rule34.us.Downloader.Logic.Utility
{
    public class Config
    {
        public static string configPath = PathManager.PathInAppData("rule34.json");

        [JsonPropertyOrder(1)]
        public string[] ShadowTags { get; set; }
        public string SavePath { get; set; }

        public Config()
        {
            ShadowTags = Array.Empty<string>();
            SavePath = PathManager.PathInPictures("rule34.us");
        }
    }
}
