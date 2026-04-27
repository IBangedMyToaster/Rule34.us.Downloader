using Rule34.us.Downloader.Logic.Tagging;
using Rule34.us.Downloader.Logic.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rule34.us.Downloader.Logic.Commands
{
    public class CommandBase
    {
        public Tags Tags { get; init; }
        public ConfigManager ConfigManager { get; init; }

        public CommandBase(Tags tags, ConfigManager configManager)
        {
            Tags = tags ?? throw new ArgumentNullException(nameof(tags));
            ConfigManager = configManager ?? throw new ArgumentNullException(nameof(configManager));
        }
    }
}
