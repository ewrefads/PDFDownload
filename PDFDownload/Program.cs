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
using PDFDownload;

namespace PDFDownloader
{
    public class Program
    {
        //Path for the list of URLs
        
        public static readonly string defaultListPath = "ListFolder/GRI_2017_2020 (1).xlsx";
        public static string listPath = defaultListPath;
        //Path for output folder
        public static readonly string defaultOutputPath = "Output";
        public static string outputPath = defaultOutputPath;

        //Path for status rapport
        public static string defaultStatusPath = "Output/StatusRapport.txt";
        public static string statusPath = defaultStatusPath;

        //Path for existing downloads
        public static string defaultDwnPath = "Output/dwn";
        public static string dwnPath = defaultDwnPath;

        public static int downloadAttempts = 0;

        public static Downloader downloader = new Downloader();
        public static void Main(string[] args)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            //Creating instances of the Downloader and Reader classes
            Reader reader = new Reader();

            //Getting a list of existing PDF files in the download directory
            string[] existingFiles = Directory.GetFiles(dwnPath, "*.pdf");

            //Reading the Excel file using the Reader class
            DataTable dataTable = new DataTable();
            dataTable = reader.ReadFile(listPath);

            List<Task> tasks = new List<Task>();
            //Iterating through each row in the DataTable
            foreach (DataRow row in dataTable.Rows)
            {
                //Extracting the ID and URLs from the row
                string? id = row[0].ToString();
                string pdfName = id + ".pdf";
                string? url = row[37]?.ToString();
                string? secondaryUrl = row[38]?.ToString();

                //Skipping the download if the URL is empty or if the file already exists
                if (/*url == "" || */existingFiles.Contains(pdfName))
                    continue;

                //Downloading the PDF file using the Downloader class if it isn't already downloaded
                Task t = Task.Run(() => downloader.DownloadFile(url, dwnPath, statusPath, pdfName, secondaryUrl));
                tasks.Add(t);
                downloadAttempts++;
            }
            Task.WaitAll(tasks.ToArray());
        }
    }
}