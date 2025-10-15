using PDFDownload;
using System.Net;
using System.Net.Http.Headers;

namespace PDFDownloadTest
{
    public class MockWebclientHandler : IWebClientHandler
    {
        public async Task<HttpResponseMessage> GetAsync(string url, HttpCompletionOption httpCompletionOption)
        {

            if (url.Length > 0 && !url.Contains("noFile"))
            {
                FileStream fileStream = File.OpenRead(url);
                string fileName = Path.GetFileName(url);
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

                result.Content = new StreamContent(fileStream);

                if(url.Contains(".pdf"))
                {
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                }
                
                if (url.Contains(".txt"))
                {
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                }
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = fileName.ToString()
                };
                return result;
            }
            else if(url.Contains("noFile"))
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            else
            {
                return null;
            }
        }
    }
}
