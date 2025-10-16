using System;
using System.IO;
using System.Net.Http;
using System.Net;
using System.ComponentModel;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Newtonsoft.Json;
using ExcelDataReader;
using System.Data;
using OfficeOpenXml;
using ClosedXML.Excel;
using System.Runtime.CompilerServices;

namespace PDFDownload
{
    //Defining a class for downloading PDF files
    public class Downloader
    {
        public IWebClientHandler webClientHandler = new HttpClientHandler();
        public static object statusRapportLock = new object();
        public IStatusReportWriter statusReportWriter = new StatusReportWriter();
        public IConsoleOutputHandler consoleOutputHandler = new ConsoleOut();
        //Defining the method that will download a PDF file from a given URL
        public async Task DownloadFile(string url, string downloadPath, string rapportPath, string pdfName, string secondUrl = "")
        {
            //Defining a string to hold the status report
            string rapport = "";

            //Using HttpClient to send a GET request to the specified URL
            //The using statement ensures that the HttpClient is properly disposed of after use
            //Setting a try-catch-finally block to handle exceptions and ensure the report is written
            try
            {
                //Sending a GET request to the URL and awaiting the response
                HttpResponseMessage response = await webClientHandler.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

                //Ensuring the response indicates success
                response.EnsureSuccessStatusCode();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to download PDF. Status code: {response.StatusCode}");
                }
                // Checking if the content type of the responde is PDF
                if (response.Content.Headers.ContentType?.MediaType != "application/pdf" || response.Content.Headers.ContentLength <= 0)
                {
                    throw new Exception("The URL does not point to a valid PDF file.");
                }

                //Reading the content of the response as a byte array
                byte[] pdfBytes = await response.Content.ReadAsByteArrayAsync();

                //Combining the download path and PDF name to get the full file path
                string filePath = Path.Combine(downloadPath, pdfName);

                //Writing the byte array to a file asynchronously
                await File.WriteAllBytesAsync(filePath, pdfBytes);

                //Updating the status report to indicate success
                rapport = pdfName + ": Successfully downloaded." + Environment.NewLine;
            }
            catch (Exception ex)
            {
                //If the first download fails and a second URL is provided, attempt to download from the second URL
                if (secondUrl != "")
                {
                    //Recursively call the DownloadFile method with the second URL
                    await DownloadFile(secondUrl, downloadPath, rapportPath, pdfName);
                }
                else
                {
                    //Updating the status report to indicate failure
                    consoleOutputHandler.WriteLine($"An error occurred: {ex.Message}");
                    rapport = pdfName + ": Failed to download." + Environment.NewLine;
                }
            }
            finally
            {
                //Another try-catch block to handle exceptions when writing the report
                try
                {
                    if(rapportPath.Length > 0 && Path.GetExtension(rapportPath).ToLower() != ".txt")
                    {
                        throw new Exception("rapport is not a txt file");
                    }
                    //Appending the status report to the specified report file asynchronously
                    statusReportWriter.Write(rapport, rapportPath);
                }
                catch (Exception ex)
                {
                    consoleOutputHandler.WriteLine($"An error occurred while writing the report: {ex.Message}");
                }
            }
        }
    }
}
