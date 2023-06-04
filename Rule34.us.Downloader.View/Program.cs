using Rule34.us.Downloader.Logic.Commands;
using Rule34.us.Downloader.Logic.Tags;
using Rule34.us.Downloader.Logic.Utility;

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
                Logger.CrashLog(Path.Combine(PathManager.AppData, "creashReport.log"), "\n" + String.Join("\n", e.Message, e.TargetSite, e.StackTrace));
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
            commandManager.Add(new Command("download", "Download all elements with the given Tags. This is the default Command.",
                                          (tags) => _ = new DownloadCommand(tags, configManager)));
            commandManager.Add(new Command("help", "Display all Commands.", (tags) => commandManager.Help()));

            // Commands
            commandManager.Add(new Command("update", "Check all/specified Folders and download the newest Content.",
                                          (tags) => _ = new UpdateCommand(tags, configManager)));
            commandManager.Add(new Command("complete", "Check all/specified Folder Elements and download missing Content.",
                                          (tags) => _ = new CompleteCommand(tags, configManager)));
            commandManager.Add(new Command("clean", "Check all/specified Folder Elements and delete Content that does not match the given Tags.",
                                          (tags) => _ = new CleanCommand(tags, configManager)));
            commandManager.Add(new Command("show", "Show the Amount of Content available with the given Tags.",
                                          (tags) => _ = new ShowCommand(tags)));
        }
    }
}