using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rule34.us.Downloader.Extensions
{
    public static class StringExtensions
    {
        public static bool Contains(this string value, IEnumerable<string> list)
        {
            return list.All(item => value.Contains(item));
        }
    }
}
