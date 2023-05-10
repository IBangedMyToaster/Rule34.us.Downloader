using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rule34.us.Downloader.Logic.Extensions
{
    public static class IEnumerableExtensions
    {
        public static bool Matches(this IEnumerable<string> collection, string value)
        {
            return collection.Any(x => x == value);
        }
    }
}
