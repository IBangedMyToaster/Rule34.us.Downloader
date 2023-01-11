using Rule34.us.Downloader.Utility;
using System.Text.Json.Serialization;

namespace Rule34.us.Downloader
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
            this.SavePath = savePath;
            this.GlobalTags = globalTags;
        }

        public Config()
        {
            this.GlobalTags = new GlobalTags();
            this.SavePath = PathManager.PathInPictures("rule34.us");
        }
    }
}
