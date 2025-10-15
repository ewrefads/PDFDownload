using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFDownload
{
    public class HttpClientHandler : IWebClientHandler
    {
        public async Task<HttpResponseMessage> GetAsync(string url, HttpCompletionOption httpCompletionOption)
        {
            using (HttpClient client = new HttpClient())
            {
                return await client.GetAsync(url, httpCompletionOption);
            }
        }
    }
}
