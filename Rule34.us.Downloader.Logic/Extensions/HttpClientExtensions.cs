using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rule34.us.Downloader.Logic.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task DownloadFileTaskAsync(this HttpClient client, Uri uri, string FileName)
        {
            using Stream s = await client.GetStreamAsync(uri);
            using FileStream fs = new(FileName, FileMode.Create);
            await s.CopyToAsync(fs);
        }
    }
}
