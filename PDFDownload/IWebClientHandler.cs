using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFDownload
{
    /// <summary>
    /// A wrapper to handle calls requiring internet connection
    /// </summary>
    public interface IWebClientHandler
    {
        /// <summary>
        /// Handles Get calls
        /// </summary>
        /// <param name="url">The url to connect to</param>
        /// <param name="httpCompletionOption"></param>
        /// <returns>The result of the call</returns>
        public Task<HttpResponseMessage> GetAsync(string url, HttpCompletionOption httpCompletionOption);
    }
}
