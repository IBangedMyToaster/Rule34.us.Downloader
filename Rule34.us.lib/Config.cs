using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Text.Json.Serialization;
using Rule34.us.Downloader.Utility;

namespace Rule34.us.Downloader
{
    public class Config
    {
        internal static string configPath = PathManager.PathInDocuments("rule34.json");

        [JsonPropertyOrder(1)]
        public GlobalTags GlobalTags { get; set; }

        private string _savePath = PathManager.PathInPictures("rule34.us");
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
    }
}
