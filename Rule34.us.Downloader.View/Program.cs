using Rule34.us.Downloader.Logic.Commands;
using Rule34.us.Downloader.Logic.Tags;
using Rule34.us.Downloader.Logic.Utility;
using System.Runtime.CompilerServices;
using System.Text;

namespace Rule34.us.Downloader.View
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ConfigManager configManager = new();
            Directory.CreateDirectory(configManager.GetSavePath);
            CommandManager commandManager = new();
            bool startedWithArgs = args.Any();
            InitializeCommands(commandManager, configManager);

            try
            {
                do
                {
                    ConsoleManager.PrintTitle();
                    Tags? tags = ReadUserInput(args, configManager);

                    if (tags == null)
                    {
                        Logger.LogSimple("Input was invalid!\n", ConsoleColor.Red);
                        continue;
                    }

                    bool? isValidCommand = commandManager.IsACommand(tags.UserInput.First());

                    if (!tags.UserInput.Any())
                    {
                        continue;
                    }

                    AcionOnCommandResult(isValidCommand, commandManager, tags).Invoke();

                    if (startedWithArgs)
                        break;

                } while (ConsoleManager.UserWantsToContinue());
            }
            catch (Exception e)
            {
                Logger.CrashLog(Path.Combine(PathManager.AppData, "creashReport.log"), e.Message);
            }
        }

        private static Tags? ReadUserInput(string[] args, ConfigManager configManager)
        {
            string? input = args.Any() ? String.Join(" ", args) : Console.ReadLine();

            if (args.Any())
                Console.Write(String.Join(" ", args));

            Console.WriteLine();

            if (String.IsNullOrEmpty(input))
                return null;

            return new Tags(input, configManager.Configuration.ShadowTags);
        }

        private static Action AcionOnCommandResult(bool? isValidCommnad, CommandManager commandManager, Tags tags) => isValidCommnad switch
        {
            true => () => commandManager.Execute(tags.UserInput.First(), tags),
            false => () => commandManager.ExecuteDefault(tags),
            null => () => Logger.LogSimple($"Command \"{tags.UserInput.First()}\" is invalid!\n", ConsoleColor.Red)
        };


        private static void InitializeCommands(CommandManager commandManager, ConfigManager configManager)
        {
            // Default
            commandManager.Add(new Command("download", "Download all elements with the given tags. This is the default command.",
                                          (tags) => _ = new DownloadCommand(tags, configManager)));
            commandManager.Add(new Command("help", "Display all commands.", (tags) => commandManager.Help()));

            // Commands
            commandManager.Add(new Command("update", "Checks all/specified Folders and downloads newest Content.",
                                          (tags) => _ = new UpdateCommand(tags, configManager)));
            commandManager.Add(new Command("complete", "Checks all/specified Folder Elements and downloads missing Elements.",
                                          (tags) => _ = new CompleteCommand(tags, configManager)));
            commandManager.Add(new Command("clean", "Checks all/specified Folder elements and removes unnecessary Files.",
                                          (tags) => _ = new CleanCommand(tags, configManager)));
            commandManager.Add(new Command("show", "Shows the amount of elements with available with the given tags.",
                                          (tags) => _ = new ShowCommand(tags)));
        }
    }
}