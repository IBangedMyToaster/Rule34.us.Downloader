using Rule34.us.Downloader.Logic.Tags;
using Rule34.us.Downloader.Logic.Utility;

namespace Rule34.us.Downloader.Logic
{
    public class MainViewViewModel
    {
        private string command;
        private List<string> tags;
        private FileManager fileManager;

        private ConfigManager configManager = new ConfigManager();
        private Config configuration;

        public MainViewViewModel(List<string> tags)
        {
            this.tags = tags;

            configuration = configManager.GetConfig();
            fileManager = new FileManager(configuration);

            DownloadTags(tags);
        }

        public MainViewViewModel(string command, List<string> tags)
        {
            this.command = command;
            this.tags = tags;

            configuration = configManager.GetConfig();
            fileManager = new FileManager(configuration);

            GetActionByCommand(command, tags).Invoke();
        }

        private Action GetActionByCommand(string command, List<string> tags) => command switch
        {
            //"--help" => () => PrintHelp(),

            "--global" => () => GlobalAdd(tags),
            "--rm" => () => GlobalRemove(tags),
            "--folder" => () => AlterSaveFolder(tags),
            "--complete" => () => CompleteFiles(),
            "--update" => () => UpdateAllFiles(tags),
            _ => throw new NotImplementedException(command)
        };

        private void GlobalRemove(List<string> tags)
        {
            if (!tags.Any())
            {
                configuration.GlobalTags.PrintTags();
                return;
            }

            configuration.GlobalTags.Remove(tags.ToArray());
            configManager.Save(configuration);
        }

        private void GlobalAdd(List<string> tags)
        {
            if (!tags.Any())
            {
                configuration.GlobalTags.PrintTags();
                return;
            }

            configuration.GlobalTags.AddRange(tags.ToArray());
            configManager.Save(configuration);
        }

        private void AlterSaveFolder(List<string> tags)
        {
            if (!tags.Any())
            {
                Logger.LogSimple(configuration.SavePath + "\n", ConsoleColor.White);
                return;
            }

            string path = string.Join(" ", tags.Select(tag => tag.Trim()));

            if (!Directory.Exists(path))
            {
                Logger.LogSimple($"Folder {path} does not exist or cant be accessed!", ConsoleColor.Red);
                return;
            }

            configuration.SavePath = path;
            configManager.Save(configuration);
        }

        #region CompleteFiles

        private void CompleteFiles()
        {
            List<TagDirectory> directories = fileManager.GetFoldersWithTags(tags).Select(path => new TagDirectory(path)).ToList();

            foreach (var directory in directories)
            {
                var ids = GetIds(directory.Tags);
                string[] missingIds = ids.Except(directory.GetFiles()).ToArray();
                string tagFolder = fileManager.CheckTagFolder(missingIds, directory.Tags);
                fileManager.DownloadMultiple(missingIds, tagFolder);
            }
        }

        #endregion

        #region Update Files

        public void UpdateAllFiles(List<string> tags)
        {
            string id;
            List<TagDirectory> directories = fileManager.GetFoldersWithTags(tags).Select(path => new TagDirectory(path)).ToList();

            foreach (var directory in directories)
            {
                id = fileManager.GetLastIdFromFolder(directory.OriginalPath);
                var ids = GetIds(directory.Tags, id);

                if (!ids.Any())
                {
                    Logger.LogSimple($"Directory {directory.Name} is UpToDate!\n", ConsoleColor.Green);
                    continue;
                }

                string tagFolder = fileManager.CheckTagFolder(ids, tags, directory.OriginalPath);
                fileManager.DownloadMultiple(ids, tagFolder);
                Logger.LogSimple($"Updated {directory.Name}\n", ConsoleColor.Green);
            }
        }

        #endregion

        public void DownloadTags(List<string> tags)
        {
            var ids = GetIds(tags);
            string tagFolder = fileManager.CheckTagFolder(ids, tags);

            fileManager.DownloadMultiple(ids.ToArray(), tagFolder);
        }

        public string[] GetIds(List<string> tags, string id = null)
        {
            if (configuration.GlobalTags != null)
                tags = tags.Select(tag => tag.ToLower().Trim()).Concat(configuration.GlobalTags.GetTags()).ToList();

            return fileManager.RetrieveIdsByTags(tags, id);
        }
    }
}
