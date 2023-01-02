using HtmlAgilityPack;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;

namespace Rule34.us.Downloader
{
    public class MainViewViewModel
    {
        private string command;
        private List<string> tags;
        private FileManager fileManager;
        private List<string> globalTags
        {
            get
            {
                return FileManager.configManager.Configuration.GlobalTags.Tags;
            }
        }

        public MainViewViewModel(List<string> tags)
        {
            this.tags = tags;

            this.fileManager = new FileManager();

            DownloadTags(tags);
        }

        public MainViewViewModel(string command, List<string> tags)
        {
            this.command = command;
            this.tags = tags;

            this.fileManager = new FileManager();

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
                FileManager.configManager.Configuration.GlobalTags.PrintTags();
                return;
            }

            FileManager.configManager.Configuration.GlobalTags.Remove(tags.ToArray());
            FileManager.configManager.Save();
        }

        private void GlobalAdd(List<string> tags)
        {
            if(!tags.Any())
            {
                FileManager.configManager.Configuration.GlobalTags.PrintTags();
                return;
            }

            FileManager.configManager.Configuration.GlobalTags.AddRange(tags.ToArray());
            FileManager.configManager.Save();
        }

        private void AlterSaveFolder(List<string> tags)
        {
            if (!tags.Any())
            {
                Logger.LogSimple(FileManager.configManager.Configuration.SavePath+"\n", ConsoleColor.White);
                return;
            }

            string path = String.Join(" ", tags.Select(tag => tag.Trim()));

            if (!Directory.Exists(path))
            {
                Logger.LogSimple($"Folder {path} does not exist or cant be accessed!", ConsoleColor.Red);
                return;
            }

            FileManager.configManager.Configuration.SavePath = path;
            FileManager.configManager.Save();
        }

        private void CompleteFiles()
        {
            List<TagDirectory> directories = fileManager.GetFoldersWithTags(tags).Select(path => new TagDirectory(path)).ToList();

            foreach(var directory in directories)
            {
                var ids = GetIds(directory.Tags);
                string[] missingIds = ids.Except(directory.GetFiles()).ToArray();
                string tagFolder = fileManager.CheckTagFolder(missingIds, directory.Tags);
                fileManager.DownloadMultiple(missingIds, tagFolder);
            }
        }

        public void UpdateAllFiles(List<string> tags)
        {
            string id;
            List<TagDirectory> directories = fileManager.GetFoldersWithTags(tags).Select(path => new TagDirectory(path)).ToList();

            foreach (var directory in directories)
            {
                id = fileManager.GetLastIdFromFolder(directory.OriginalPath);
                var ids = GetIds(directory.Tags, id);

                if(!ids.Any())
                {
                    Logger.LogSimple($"Directory {directory.Name} is UpToDate!\n", ConsoleColor.Green);
                    continue;
                }

                string tagFolder = fileManager.CheckTagFolder(ids, tags, directory.OriginalPath);
                fileManager.DownloadMultiple(ids, tagFolder);
                Logger.LogSimple($"Updated {directory.Name}\n", ConsoleColor.Green);
            }
        }

        public void DownloadTags(List<string> tags)
        {
            var ids = GetIds(tags);
            string tagFolder = fileManager.CheckTagFolder(ids, tags);

            fileManager.DownloadMultiple(ids.ToArray(), tagFolder);
        }

        public string[] GetIds(List<string> tags, string id = null)
         {
            if(globalTags != null)
                tags = tags.Select(tag => tag.ToLower().Trim()).Union(FileManager.configManager.Configuration.GlobalTags.GetTags()).ToList();

            return fileManager.RetrieveIdsByTags(tags, id);
        }
    }
}
