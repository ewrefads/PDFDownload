using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFDownload
{
    /// <summary>
    /// Handles get calls to the http client
    /// </summary>
    public class HttpClientHandler : IWebClientHandler
    {
        /// <summary>
        /// Makes an async get call and returns the result
        /// </summary>
        /// <param name="url">Which url to connect to</param>
        /// <param name="httpCompletionOption"></param>
        /// <returns>The resulting HttpResponseMessage from the call</returns>
        public async Task<HttpResponseMessage> GetAsync(string url, HttpCompletionOption httpCompletionOption)
        {
            using (HttpClient client = new HttpClient())
            {
                return await client.GetAsync(url, httpCompletionOption);
            }
        }
    }
}
