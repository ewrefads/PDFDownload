using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFDownloadTest
{
    /// <summary>
    /// Class containing various tests to check if loose test files exists
    /// </summary>
    [Collection("Filehandling")]
    public class PDFDownloadTestTests
    {
        /// <summary>
        /// Ensures the folder containing the test files exists
        /// </summary>
        [Fact]
        public void TestFilesFolderExists()
        {
            Assert.True(Directory.Exists("TestFiles"));
        }

        /// <summary>
        /// Checks if the corrupted xlsx file exists
        /// </summary>
        [Fact]
        public void TestCorruptedXlsxExists()
        {
            Assert.True(Path.Exists("TestFiles/testCorrupted.xlsx"));
        }

        /// <summary>
        /// Checks if the test.txt file exists
        /// </summary>
        [Fact]
        public void TestTxtExists()
        {
            Assert.True(Path.Exists("TestFiles/test.txt"));
        }

        /// <summary>
        /// Checks if the file containing the reduced version of the actual dataset exists
        /// </summary>
        [Fact]
        public void ActualDataExists()
        {
            Assert.True(Path.Exists("TestFiles/GRI_2017_2020 (Reduced).xlsx"));
        }

        /// <summary>
        /// Checks if the offline data exists
        /// </summary>
        [Fact]
        public void OfflineDataExists()
        {
            Assert.True(Path.Exists("TestFiles/GRI_2017_2020 (OfflineTestVersion).xlsx"));
        }

        /// <summary>
        /// Checks if the docx status report exists
        /// </summary>
        [Fact]
        public void StatusReportDocxExists()
        {
            Assert.True(Path.Exists("TestFiles/StatusReport.docx"));
        }

        /// <summary>
        /// Checks if the test pdf exists
        /// </summary>
        [Fact]
        public void TestPDFExists()
        {
            Assert.True(Path.Exists("TestFiles/test.pdf"));
        }
    }
}
