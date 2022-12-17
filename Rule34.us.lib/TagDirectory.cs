using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rule34.us.Downloader
{
    public class TagDirectory
    {
        public string Name { get; set; }
        public string OriginalPath { get; set; }
        public List<string> Tags { get; set; }

        public TagDirectory(string path)
        {
            this.Name = new DirectoryInfo(path).Name;
            this.OriginalPath = path;

            Tags = Name.Split('&').ToList();
        }
    }
}
