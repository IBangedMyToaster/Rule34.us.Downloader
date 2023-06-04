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
                if (!TagDirectory.Exists(ConfigManager.Configuration, Tags))
                {
                    Logger.LogSimple($"The Folder \"{(string.Join(" ", Tags.TrimmedInput()))}\" does not Exist!\n", ConsoleColor.Red);
                    return;
                }

                CleanSpecific(TagDirectory.GetTagDirectoryByTags(ConfigManager.Configuration, Tags));
                return;
            }

            CleanAll();
        }

        private void CleanSpecific(TagDirectory tagDirectory)
        {
            // Get all ids by tags
            Logger.LogSimple($"Cleaning {string.Join(" ", tagDirectory.Tags.TrimmedInput())}...\n", ConsoleColor.Yellow); // Log Checking
            string[] ids = logistic.GetAllIdsByTags(tagDirectory.Tags).Select(content => content.Id).ToArray();

            // Compare existing Folder with Links and Filter doubles
            string[] filenames = tagDirectory.GetFullFilenames();
            filenames = filenames.ExceptBy(ids, x => Path.GetFileNameWithoutExtension(x)).ToArray();

            foreach (string file in filenames)
            {
                tagDirectory.DeleteFile(file);
                Logger.LogSimple($"[DELETED] {Path.GetFileName(file)}\n"); // Log
            }

            LogUpdateProgress(filenames.Length, tagDirectory).Invoke();  // Log Result
        }

        private void CleanAll()
        {
            TagDirectory[] tagDirectories = TagDirectory.GetAllTagDirectories(ConfigManager.Configuration);

            foreach (TagDirectory tagDirectory in tagDirectories)
            {
                CleanSpecific(tagDirectory);
            }
        }

        private static Action LogUpdateProgress(int idCount, TagDirectory directory)
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
