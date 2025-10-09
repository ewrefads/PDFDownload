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

namespace PDFDownloader
{
    class Program
    {
        //private static readonly HttpClient _httpClient = new HttpClient();

        static async Task Main(string[] args)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            //Path for the list of URLs
            string listPath = @"C:\Visual Studio Projecter\PDFDownload\PDFDownload\List Folder\GRI_2017_2020 (1).xlsx";

            //Path for output folder
            string outputPath = @"C:\Visual Studio Projecter\PDFDownload\PDFDownload\Output\";

            //Path for status rapport
            string statusPath = @"C:\Visual Studio Projecter\PDFDownload\PDFDownload\Output\StatusRapport.txt";

            //Path for existing downloads
            string dwnPath = @"C:\Visual Studio Projecter\PDFDownload\PDFDownload\Output\dwn\";

            //using(var workbook = new XLWorkbook(listPath))
            //{
            //    var worksheet = workbook.Worksheet(1);
            //    var dataTable = new DataTable();

            //    foreach (var cell in worksheet.Row(1).Cells())
            //    {
            //        dataTable.Columns.Add(cell.Value.ToString());
            //    }

            //    foreach (var row in worksheet.RowsUsed().Skip(1)) // Skip header row
            //    {
            //        var dataRow = dataTable.NewRow();
            //        for (int i = 0; i < dataTable.Columns.Count; i++)
            //        {
            //            dataRow[i] = row.Cell(i + 1).Value.ToString();
            //        }
            //        dataTable.Rows.Add(dataRow);
            //    }

            //    foreach (DataRow row in dataTable.Rows)
            //    {
            //        if (row[URL].ToString() == "")
            //            continue;
            //        string? id = row[ID]?.ToString();
            //        string? url = row[URL]?.ToString();
            //        Console.WriteLine($"ID: {id}, URL: {url}");
            //    }
            //}

            using (FileStream stream = File.Open(listPath, FileMode.Open, FileAccess.Read))
            {
                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                DataTable dataTable = excelReader.AsDataSet().Tables[0];
                excelReader.Close();

                foreach (DataRow row in dataTable.Rows)
                {
                    if(row[37].ToString() == "")
                        continue;

                    string ?id = row[0]?.ToString();
                    string pdfName = id + ".pdf";
                    string ?url = row[37]?.ToString();
                    string ?secondaryUrl = row[38]?.ToString();
                    bool secondaryUrlProper = secondaryUrl != "" && secondaryUrl != null;
                    //Console.WriteLine($"ID: {id}, URL: {url}");

                    if (id.Equals("BR50071"))
                    {
                        Console.WriteLine("Debug stop");
                    }

                    using (HttpClient client = new HttpClient())
                    {
                        try
                        {
                            HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                            response.EnsureSuccessStatusCode();

                            if (!response.IsSuccessStatusCode)
                            {
                                throw new Exception($"Failed to download PDF. Status code: {response.StatusCode}");
                            }

                            if (response.Content.Headers.ContentType?.MediaType != "application/pdf")
                            {
                                throw new Exception("The URL does not point to a valid PDF file.");
                            }

                            byte[] pdfBytes = await response.Content.ReadAsByteArrayAsync();

                            string filePath = Path.Combine(dwnPath, pdfName);

                            await File.WriteAllBytesAsync(filePath, pdfBytes);

                            using (StreamWriter writer = File.AppendText(statusPath))
                            {
                                writer.WriteLine(pdfName + ": Successfully downloaded.");
                                writer.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                                response.EnsureSuccessStatusCode();

                                if (!response.IsSuccessStatusCode)
                                {
                                    throw new Exception($"Failed to download PDF. Status code: {response.StatusCode}");
                                }

                                if (response.Content.Headers.ContentType?.MediaType != "application/pdf")
                                {
                                    throw new Exception("The URL does not point to a valid PDF file.");
                                }

                                byte[] pdfBytes = await response.Content.ReadAsByteArrayAsync();

                                string filePath = Path.Combine(dwnPath, pdfName);

                                await File.WriteAllBytesAsync(filePath, pdfBytes);

                                using (StreamWriter writer = File.AppendText(statusPath))
                                {
                                    writer.WriteLine(pdfName + ": Successfully downloaded.");
                                    writer.Close();
                                }
                            }
                            catch (Exception innerEx)
                            {
                                Console.WriteLine($"An error occurred: {innerEx.Message}");
                                using (StreamWriter writer = File.AppendText(statusPath))
                                {
                                    writer.WriteLine(pdfName + ": Failed to download.");
                                    writer.Close();
                                }
                            }

                            //Console.WriteLine($"An error occurred: {ex.Message}");
                            //using (StreamWriter writer = File.AppendText(statusPath))
                            //{
                            //    writer.WriteLine(pdfName + ": Failed to download.");
                            //    writer.Close();
                            //}
                        }
                    }
                }
            }

            //using (_httpClient)
            //{
            //    try
            //    {
            //        HttpResponseMessage response = await _httpClient.GetAsync("http://cdn12.a1.net/m/resources/media/pdf/A1-Umwelterkl-rung-2016-2017.pdf", HttpCompletionOption.ResponseHeadersRead);
            //        response.EnsureSuccessStatusCode();

            //        if (!response.IsSuccessStatusCode)
            //        {
            //            throw new Exception($"Failed to download PDF. Status code: {response.StatusCode}");
            //        }

            //        if (response.Content.Headers.ContentType?.MediaType != "application/pdf")
            //        {
            //            throw new Exception("The URL does not point to a valid PDF file.");
            //        }

            //        byte[] pdfBytes = await response.Content.ReadAsByteArrayAsync();

            //        string filePath = Path.Combine(dwnPath, "test.pdf");

            //        await File.WriteAllBytesAsync(filePath, pdfBytes);

            //        using (StreamWriter writer = File.AppendText(statusPath))
            //        {
            //            writer.WriteLine("test.pdf: Successfully downloaded.");
            //            writer.Close();
            //        }

            //        Console.WriteLine("PDF downloaded successfully.");
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"An error occurred: {ex.Message}");
            //        using (StreamWriter writer = File.AppendText(statusPath))
            //        {
            //            writer.WriteLine("test.pdf: Failed to download.");
            //            writer.Close();
            //        }
            //        Console.WriteLine("PDF failed to download.");
            //    }
            //}
        }
    }

    public class ExcelRow
    {
        public string ID { get; set; }
        public string Url { get; set; }
        public string ATStatus { get; set; }

        public ExcelRow(string id, string url, string atStatus)
        {
            ID = id;
            Url = url;
            ATStatus = atStatus;
        }
    }
}