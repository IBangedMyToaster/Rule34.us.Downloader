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
        private Logger logger;

        public TagAnalyzer(string? tags)
        {
            this.tags = tags;
        }

        public TagAnalyzer(string? tags, Logger logger) : this(tags)
        {
            this.logger = logger;
        }

        public List<string> Analyze()
        {
            if(tags == null)
                throw new ArgumentNullException("tags");

            logger.LogSimple($"searching Tags: [{HighlightTags(tags)}]\n");

            return tags.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(tag => tag.Trim()).ToList();
        }

        private string HighlightTags(string? tags)
        {
            return String.Join(" ", tags.Split(' ').Select(tag => $"'{tag}'"));
        }
    }
}
