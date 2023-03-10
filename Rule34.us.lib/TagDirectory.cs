using Rule34.us.Downloader.Extensions;

namespace Rule34.us.Downloader
{
    public class TagDirectory
    {
        public string Name { get; set; }
        public string OriginalPath { get; set; }
        public List<string> Tags { get; set; }

        public TagDirectory(string path)
        {
            this.Name = new DirectoryInfo(path).Name;
            this.OriginalPath = path;

            Tags = Name.Split('&').Trim().ToList();
        }

        public List<string> GetFiles()
        {
            return Directory.GetFiles(this.OriginalPath).Select(file => Path.GetFileNameWithoutExtension(file)).ToList();
        }
    }
}
