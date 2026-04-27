using Rule34.us.Downloader.Logic.Rule34;
using Rule34.us.Downloader.Logic.Utility;
using Rule34.us.Downloader.Logic.Tagging;

namespace Rule34.us.Downloader.Logic.Commands
{
    // DEBUG = DEBUG = DEBUG = DEBUG = DEBUG = DEBUG = DEBUG = DEBUG = DEBUG = DEBUG = DEBUG = DEBUG = DEBUG = DEBUG = DEBUG = DEBUG = DEBUG = DEBUG = DEBUG = DEBUG = DEBUG = DEBUG = DEBUG = DEBUG = DEBUG = DEBUG = DEBUG
    public class DebugCommand : CommandBase
    {
        public DebugCommand(Tags tags, ConfigManager configManager) : base(tags, configManager)
        {
            // Fake Tags
            tags = new Tags("test");

            configManager.MockConfig();
            TagDirectory? tagDirectory = Execute(out int idCount);
            LogUpdateProgress(idCount, tagDirectory).Invoke();  // Log Result
        }

        private TagDirectory? Execute(out int contentCount)
        {
            Rule34Logistic logistic = new();
            TagDirectory? tagDirectory = null;

            // Get all ids by tags
            Logger.LogSimple($"Downloading {string.Join(" ", this.Tags.Raw)}...\n", ConsoleColor.Yellow); // Log Checking

            List<Content> contentList = new List<Content>()
            {
                new Content("1670251"), // PNG
                new Content("1904447"), // JPEG
                new Content("4974263"), // JPG
                new Content("2193125"), // GIF
                new Content("12588436"), // UNKNOWN
                new Content("4621985")  // WEBM or MP4
            };

            contentCount = contentList.Count();

            if (!contentList.Any())
                return tagDirectory;

            // Get all links by ids
            logistic.GetLinks(contentList);

            // Download all files by links and save in folder
            tagDirectory = TagDirectory.GetTagDirectoryByTags(ConfigManager.Configuration, this.Tags);
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
