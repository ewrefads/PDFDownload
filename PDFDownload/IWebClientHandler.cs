using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFDownload
{
    public interface IWebClientHandler
    {
        public Task<HttpResponseMessage> GetAsync(string url, HttpCompletionOption httpCompletionOption);
    }
}
