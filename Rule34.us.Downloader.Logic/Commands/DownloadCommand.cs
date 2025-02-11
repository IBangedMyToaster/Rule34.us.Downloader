using Rule34.us.Downloader.Logic.Rule34;
using Rule34.us.Downloader.Logic.Tags;
using Rule34.us.Downloader.Logic.Utility;

namespace Rule34.us.Downloader.Logic.Commands
{
    public class DownloadCommand
    {
        public Tags.Tags Tags { get; private set; }
        public ConfigManager ConfigManager { get; }

        public DownloadCommand(Tags.Tags tags, ConfigManager configManager)
        {
            Tags = tags ?? throw new ArgumentNullException(nameof(tags));
            ConfigManager = configManager;
            TagDirectory? tagDirectory = Execute(out int idCount);
            LogUpdateProgress(idCount, tagDirectory).Invoke();  // Log Result
        }

        private TagDirectory? Execute(out int contentCount)
        {
            Rule34Logistic logistic = new();
            TagDirectory? tagDirectory = null;

            // Get all ids by tags
            Logger.LogSimple($"Downloading {string.Join(" ", Tags.Raw)}...\n", ConsoleColor.Yellow); // Log Checking
            List<Content> contentList = logistic.GetAllIdsByTags(Tags);
            contentCount = contentList.Count();

            if (!contentList.Any())
                return tagDirectory;

            // Get all links by ids
            logistic.GetLinks(contentList);

            // Download all files by links and save in folder
            tagDirectory = TagDirectory.GetTagDirectoryByTags(ConfigManager.Configuration, Tags);
            logistic.Download(tagDirectory.OriginalPath, contentList);
            return tagDirectory;
        }

        private static Action LogUpdateProgress(int idCount, TagDirectory? tagDirectory)
        {
            return idCount switch
            {
                0 => () => Logger.LogSimple($"No elements with the given tags were found !\n\n", ConsoleColor.Red),
                _ => () => Logger.LogSimple($"Saved {idCount} elements in \"{tagDirectory?.Name}\".\n\n", ConsoleColor.Yellow),
            };
        }
    }
}
