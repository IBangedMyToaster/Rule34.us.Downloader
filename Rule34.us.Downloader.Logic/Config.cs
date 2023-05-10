using Rule34.us.Downloader.Logic.Tags;
using Rule34.us.Downloader.Logic.Utility;
using System.Text.Json.Serialization;

namespace Rule34.us.Downloader.Logic
{
    public class Config
    {
        public static string configPath = PathManager.PathInDocuments("rule34.json");

        [JsonPropertyOrder(1)]
        public GlobalTags GlobalTags { get; set; }

        private string _savePath;
        public string SavePath
        {
            get { return _savePath; }
            set { _savePath = value; }
        }

        public Config(string savePath, GlobalTags globalTags)
        {
            SavePath = savePath;
            GlobalTags = globalTags;
        }

        public Config()
        {
            GlobalTags = new GlobalTags();
            SavePath = PathManager.PathInPictures("rule34.us");
        }
    }
}
