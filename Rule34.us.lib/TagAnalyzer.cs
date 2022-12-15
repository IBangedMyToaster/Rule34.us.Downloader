using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rule34.us.Downloader
{
    public class TagAnalyzer
    {
        private string? tags;

        public TagAnalyzer(string? tags)
        {
            this.tags = tags;
        }

        public List<string> Analyze()
        {
            if(tags == null)
                throw new ArgumentNullException("tags");

            return tags.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(tag => tag.Trim()).ToList();
        }
    }
}
