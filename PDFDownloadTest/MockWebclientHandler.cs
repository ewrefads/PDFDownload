using PDFDownload;
using System.Net;
using System.Net.Http.Headers;

namespace PDFDownloadTest
{
    /// <summary>
    /// Implementation of IWebClientHandler which gives predefined return values to different inputs 
    /// </summary>
    public class MockWebclientHandler : IWebClientHandler
    {
        /// <summary>
        /// Takes the url and gives a predefined answer based on its value
        /// </summary>
        /// <param name="url">The location of the which the method pretends to download or one of the predefined values</param>
        /// <param name="httpCompletionOption"></param>
        /// <returns>A httpresponse message based on the url</returns>
        public async Task<HttpResponseMessage> GetAsync(string url, HttpCompletionOption httpCompletionOption)
        {
            //Handles actual files
            if (url.Length > 0 && (url.Contains(".pdf") || url.Contains(".txt")))
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
            //Sets up a valid response with a 404 status code
            else if(url.Contains("noFile"))
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            //Pretends to lose connection by setting up the header as if it contains a pdf but does not provide it
            else if(url.Contains("nonexistingFile"))
            {
                string fileName = "noFile.pdf";
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);


                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = fileName.ToString()
                };
                return result;
            }
            //Returns an empty httpResponse message if no value matching one of the previous ones was provided
            else
            {
                return new HttpResponseMessage();
            }
        }
    }
}
