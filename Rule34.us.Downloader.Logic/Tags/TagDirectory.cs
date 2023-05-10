using Rule34.us.Downloader.Logic.Extensions;

namespace Rule34.us.Downloader.Logic.Tags
{
    public class TagDirectory
    {
        public string Name { get; set; }
        public string OriginalPath { get; set; }
        public List<string> Tags { get; set; }

        public TagDirectory(string path)
        {
            Name = new DirectoryInfo(path).Name;
            OriginalPath = path;

            Tags = Name.Split('&').Trim().ToList();
        }

        public List<string> GetFiles()
        {
            return Directory.GetFiles(OriginalPath).Select(file => Path.GetFileNameWithoutExtension(file)).ToList();
        }
    }
}
