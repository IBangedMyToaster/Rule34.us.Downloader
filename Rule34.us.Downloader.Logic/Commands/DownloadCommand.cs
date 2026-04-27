using Rule34.us.Downloader.Logic.Rule34;

using Rule34.us.Downloader.Logic.Utility;
using Rule34.us.Downloader.Logic.Tagging;

namespace Rule34.us.Downloader.Logic.Commands
{
    public class DownloadCommand : CommandBase
    {
        public DownloadCommand(Tags tags, ConfigManager configManager) : base(tags, configManager)
        {
            TagDirectory? tagDirectory = Execute(out int idCount);
            LogUpdateProgress(idCount, tagDirectory).Invoke();  // Log Result
        }

        private TagDirectory? Execute(out int contentCount)
        {
            Rule34Logistic logistic = new();
            TagDirectory? tagDirectory = null;

            // Get all ids by tags
            Logger.LogSimple($"Downloading {string.Join(" ", this.Tags.Raw)}...\n", ConsoleColor.Yellow); // Log Checking
            List<Content> contentList = logistic.GetAllIdsByTags(this.Tags);
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
