using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

/// <summary>
/// Summary description for HttpTools
/// http://rextester.com/discussion/XPKY90132/async-example-with-HttpClient
/// </summary>
/// 



public static class HttpTools
{
    public static async Task<string> DownloadPage(string url)
    {
        using (var client = new HttpClient())
        {
            using (var r = await client.GetAsync(new Uri(url)))
            {
                string result = await r.Content.ReadAsStringAsync();
                return result;
            }
        }
    }
}