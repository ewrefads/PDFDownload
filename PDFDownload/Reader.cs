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

namespace PDFDownload
{
    //Defining a class for reading Excel files
    public class Reader
    {
        private UserInfoHandler userInfoHandler = new ConsoleOut();

        public UserInfoHandler UserInfoHandler { get => userInfoHandler; set => userInfoHandler = value; }

        //Defining the method that will read an Excel file from a path and return a DataTable containing the data
        public DataTable ReadFile(string filePath)
        {
            //Defining a DataTable to hold the data
            DataTable dataTable = new DataTable();

            //Try catch block to handle potential exceptions when reading the file
            try
            {
                //Reading the excel file using FileStream and ExcelDataReader
                //The 'using' statement ensures that the FileStream is properly disposed of after use
                //The dataTable is used to store the data read from the Excel file 
                using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    dataTable = excelReader.AsDataSet().Tables[0];
                    excelReader.Close();
                }
            }
            catch (Exception ex)
            {
                userInfoHandler.WriteLine($"An error occurred while reading the Excel file: {ex.Message}");
            }
            
            return dataTable;
        }
    }
}
