using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rule34.us.Downloader.Extensions
{
    public static class StringExtensions
    {
        public static bool Matches(this string value, string matchValue)
        {
            string val = value.ToLower().Trim();
            matchValue = matchValue.ToLower().Trim();

            return val == matchValue;
        }
    }
}
