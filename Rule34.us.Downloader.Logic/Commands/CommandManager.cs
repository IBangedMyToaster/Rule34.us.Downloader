using Rule34.us.Downloader.Logic.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rule34.us.Downloader.Logic.Commands
{
    public class CommandManager
    {
        List<Command> commands = new List<Command>();

        public bool IsACommand(string value)
        {
            return value.StartsWith(Command.prefix) && IsValid(value);
        }

        private bool IsValid(string value)
        {
            throw new NotImplementedException();
            return value.Contains(Command.prefix);
        }

        public bool Contains(string value)
        {
            return commands.Select(com => com.Name).Matches(value);
        }

        public void Add(Command command)
        {
            commands.Add(command);
        }
    }

    public class Command
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Action Action { get; set; }
        public static string prefix = "--";

        public Command(string name, string description, Action action)
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
            Action = action ?? throw new ArgumentNullException(nameof(action));
        }
    }
}
