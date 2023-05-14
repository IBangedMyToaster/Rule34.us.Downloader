using Rule34.us.Downloader.Logic.Commands;
using Rule34.us.Downloader.Logic.Tags;
using Rule34.us.Downloader.Logic.Utility;
using System.Text;

namespace Rule34.us.Downloader.View
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            ConfigManager configManager = new();
            CommandManager commandManager = new();
            InitialiozeCommands(commandManager, configManager);

            try
            {
                do
                {
                    ConsoleManager.PrintTitle();
                    Tags tags = new(Console.ReadLine(), configManager.Configuration.ShadowTags);
                    Console.WriteLine();

                    if (!tags.UserInput.Any())
                    {
                        continue;
                    }

                    if (commandManager.IsACommand(tags.UserInput.First()))
                    {
                        commandManager.Execute(tags.UserInput.First(), tags);
                    }
                    else
                    {
                        commandManager.ExecuteDefault(tags);
                    }
                } while (ConsoleManager.UserWantsToContinue());
            }
            catch (Exception e)
            {
                Logger.CrashLog("creashReport.txt", e.Message);
            }
        }

        private static void InitialiozeCommands(CommandManager commandManager, ConfigManager configManager)
        {
            // Default
            commandManager.Add(new Command("download", "Download alls elements with the given tags. This is the default command", (tags) => new DownloadCommand(tags, configManager)));
            commandManager.Add(new Command("help", "Shows all commands.", (tags) => commandManager.Help()));

            // Commands
            commandManager.Add(new Command("update", "Updates all/specified folder. Usage: --update [tag1 tag2]", (tags) => new UpdateCommand(tags, configManager)));
            commandManager.Add(new Command("complete", "Checks all Folder elements and downloads missing elements.", (tags) => new CompleteCommand(tags, configManager)));
            commandManager.Add(new Command("clean", "Checks all Folder elements and removes unnecessary Files.", (tags) => new CleanCommand(tags, configManager)));
            commandManager.Add(new Command("show", "Shows the amount of elements with available with the given tags.", (tags) => new ShowCommand(tags)));
        }
    }
}