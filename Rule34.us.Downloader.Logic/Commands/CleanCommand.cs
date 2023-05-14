using Rule34.us.Downloader.Logic.Rule34;
using Rule34.us.Downloader.Logic.Tags;
using Rule34.us.Downloader.Logic.Utility;

namespace Rule34.us.Downloader.Logic.Commands
{
    public class CleanCommand
    {
        public Tags.Tags Tags { get; private set; }
        public ConfigManager ConfigManager { get; }

        private readonly Rule34Logistic logistic = new();

        public CleanCommand(Tags.Tags tags, ConfigManager configManager)
        {
            Tags = tags ?? throw new ArgumentNullException(nameof(tags));
            ConfigManager = configManager;

            if (Tags.TrimmedInput().Any())
            {
                CleanSpecific(TagDirectory.GetDirectoryByTags(ConfigManager.Configuration, Tags));
                return;
            }

            CleanAll();
        }

        private void CleanSpecific(TagDirectory tagDirectory)
        {
            // Get all ids by tags
            Logger.LogSimple($"Cleaning {string.Join(" ", tagDirectory.Tags.TrimmedInput())}...\n", ConsoleColor.Yellow); // Log Checking
            string[] ids = logistic.GetAllIdsByTags(tagDirectory.Tags, out int pages);

            // Compare existing Folder with Links and Filter doubles
            string[] filenames = tagDirectory.GetFullFilenames();
            filenames = filenames.ExceptBy(ids, x => Path.GetFileNameWithoutExtension(x)).ToArray();

            foreach (string file in filenames)
            {
                tagDirectory.DeleteFile(file);
                Logger.LogSimple($"[DELETED] {file}\n"); // Log
            }

            LogUpdateProgress(filenames.Length, tagDirectory).Invoke();  // Log Result
        }

        private void CleanAll()
        {
            TagDirectory[] tagDirectories = Directory.GetDirectories(ConfigManager.GetSavePath).Select(dir => TagDirectory.GetDirectoryByTags(ConfigManager.Configuration, TagDirectory.GetTagsByPath(dir))).ToArray();

            foreach (TagDirectory tagDirectory in tagDirectories)
            {
                CleanSpecific(tagDirectory);
            }
        }

        private Action LogUpdateProgress(int idCount, TagDirectory directory)
        {
            return idCount switch
            {
                0 => () => Logger.LogSimple($"\"{directory.Name}\" was Clean on Execution.\n\n", ConsoleColor.Green),
                1 => () => Logger.LogSimple($"Cleaned \"{directory.Name}\" and removed {idCount} element\n\n", ConsoleColor.Yellow),
                _ => () => Logger.LogSimple($"Cleaned \"{directory.Name}\" and removed {idCount} elements\n\n", ConsoleColor.Yellow),
            };
        }
    }
}
