using Rule34.us.Downloader.Logic.Extensions;
using Rule34.us.Downloader.Logic.Utility;

namespace Rule34.us.Downloader.Logic.Commands
{
    public class CommandManager
    {
        private readonly List<Command> commands = new();

        public bool? IsACommand(string value)
        {
            bool command = value.StartsWith(Command.prefix);
            bool valid = IsValid(value.Remove(Command.prefix));

            if (command && valid)
                return true;
            else if(!command)
                return false;

            return null;
        }

        public bool IsValid(string value)
        {
            return commands.Select(command => command.Name).Matches(value);
        }

        public bool Contains(string value)
        {
            return commands.Select(com => com.Name).Matches(value);
        }

        public void Add(Command command)
        {
            commands.Add(command);
        }

        public void Execute(string command, Tags.Tags tags)
        {
            commands.First(com => com.Name == command.Remove(Command.prefix)).Action.Invoke(tags);
        }

        public void ExecuteDefault(Tags.Tags tags)
        {
            commands.First(com => com.Name == "download").Action.Invoke(tags);
        }

        public void Help()
        {
            Console.WriteLine("Usage: [<Command>] [<Tags>]\n");

            Console.WriteLine("Available Commands:");
            foreach (Command command in commands)
            {
                Console.Write($"  --{command.Name,-15}");
                Logger.LogSimple(command.Description + "\n", ConsoleColor.Gray);
            }
        }

    }

    public class Command
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Action<Tags.Tags> Action { get; set; }
        public static readonly string prefix = "--";

        public Command(string name, string description, Action<Tags.Tags> action)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"\"{nameof(name)}\" kann nicht NULL oder leer sein.", nameof(name));
            }

            if (string.IsNullOrEmpty(description))
            {
                throw new ArgumentException($"\"{nameof(description)}\" kann nicht NULL oder leer sein.", nameof(description));
            }

            Name = name;
            Description = description;
            Action = action;
        }
    }
}
