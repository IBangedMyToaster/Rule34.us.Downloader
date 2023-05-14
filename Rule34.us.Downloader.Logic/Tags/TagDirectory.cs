using Rule34.us.Downloader.Logic.Utility;

namespace Rule34.us.Downloader.Logic.Tags
{
    public class TagDirectory
    {
        public string OriginalPath { get; private set; }
        public Tags Tags { get; private set; }
        public string Name => TagDirectory.ConvertTagsToDirName(Tags);

        public TagDirectory(string path)
        {
            OriginalPath = path ?? throw new ArgumentNullException(nameof(path));
            Tags = TagDirectory.GetTagsByPath(OriginalPath);
        }

        public List<string> GetFilenames()
        {
            return Directory.GetFiles(OriginalPath).Select(file => Path.GetFileNameWithoutExtension(file)).ToList();
        }

        public string[] GetFullFilenames()
        {
            return Directory.GetFiles(OriginalPath);
        }

        public void DeleteFile(string file)
        {
            string path = Path.Combine(OriginalPath, file);
            File.Delete(path);
        }

        public string? GetLastId()
        {
            return GetFilenames().Select(id => int.Parse(id)).Max().ToString();
        }

        // Static
        public static string ConvertTagsToDirName(Tags tags)
        {
            return string.Join(" & ", tags.TrimmedInput());
        }

        public static TagDirectory GetDirectoryByTags(Config config, Tags tags)
        {
            string savePath = config.SavePath;
            string directoryName = IsDirName(Path.Combine(savePath, tags.ToString())) ? tags.ToString() : ConvertTagsToDirName(tags);
            _ = Directory.CreateDirectory(savePath);

            TagDirectory tagDirectory = new(Directory.GetDirectories(savePath).FirstOrDefault(dir => new DirectoryInfo(dir).Name == directoryName)
                                        ?? Directory.CreateDirectory(Path.Combine(savePath, directoryName)).FullName);
            tagDirectory.Tags.SetShadowTags(config.ShadowTags);

            return tagDirectory;
        }

        private static bool IsDirName(string v)
        {
            return Directory.Exists(v);
        }

        internal static Tags GetTagsByPath(string path)
        {
            return new Tags(new DirectoryInfo(path).Name.Split(" & ").Select(tag => tag.Trim()).ToArray());
        }
    }
}
