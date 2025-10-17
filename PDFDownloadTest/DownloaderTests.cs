using NPOI.HPSF;
using PDFDownload;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PDFDownloadTest
{
    /// <summary>
    /// Base class for test classes of the Downloader class
    /// </summary>
    public class DownloaderTestsBase
    {
        protected Downloader downloader = new Downloader();
        public static string workingLink = Path.GetFullPath("TestFiles/test.pdf");
        public static string fakeLink = Path.GetFullPath("TestFiles/nonexistingFile");
        public static string nonWorkingLink = Path.GetFullPath("TestFiles/noFile");
        public static string NonPdfLink = Path.GetFullPath("TestFiles/test.txt");
        public static string testNum = "test";
        public static string dwnPath = "Output/dwn";
        public static string rapportPath = Path.GetFullPath("Output/StatusRapport.txt");
        protected static object pdfFileLock = new object();
        protected static object statusRepportLock = Downloader.statusRapportLock;
        protected LoggedStatusreportWriter statusreportWriter = new LoggedStatusreportWriter();
        
        public DownloaderTestsBase()
        {
            if(!Path.Exists(dwnPath))
            {
                Directory.CreateDirectory(dwnPath);
            }
            dwnPath = Path.GetFullPath(dwnPath);
            downloader.webClientHandler = new MockWebclientHandler();
            downloader.statusReportWriter = statusreportWriter;
        }
    }
    /// <summary>
    /// Tests related to handling of working links
    /// </summary>
    [Collection("Filehandling")]
    public class CanDownloaderDownloadFromWorkingLinkTest: DownloaderTestsBase
    {
        /// <summary>
        /// Checks to ensure a file gets downloaded from a valid link
        /// Test id: DT-01
        /// </summary>
        [Fact]
        public async void CanDownloaderDownloadFromWorkingPrimaryLink()
        {
            testNum = Guid.NewGuid().ToString();
            string fileName = testNum + ".pdf";
            await downloader.DownloadFile(workingLink, dwnPath, rapportPath, fileName, "");
            bool canRead = false;
            //Checks to see if the file has been created
            using (var stream = File.Open(Path.Combine(dwnPath, fileName), FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                canRead = true;
            }
            File.Delete(Path.Combine(dwnPath, fileName));
            Assert.True(canRead);
        }

        /// <summary>
        /// Checks if the status report gets updated correctly on a succesful download
        /// </summary>
        [Fact]
        public async void DoesStatusRapportGetUpdatedOnSuccesfulDownload()
        {
            testNum = Guid.NewGuid().ToString();
            string fileName = testNum + ".pdf";
            await downloader.DownloadFile(workingLink, dwnPath, rapportPath, fileName, "");
            bool entryAdded = false;
            lock(statusRepportLock)
            {
                foreach (string line in statusreportWriter.messages)
                {
                    if (line.Contains(testNum) && line.Contains("Successfully downloaded"))
                    {
                        entryAdded = true;
                        break;
                    }
                }
            }
            
            File.Delete(Path.Combine(dwnPath, fileName));
            Assert.True(entryAdded);
        }

        /// <summary>
        /// Checks if the file gets downloaded if only the secondary link is correct
        /// </summary>
        [Fact]
        public async void CanDownloaderDownloadFromWorkingSecondaryLink()
        {
            testNum = Guid.NewGuid().ToString();
            string fileName = testNum + ".pdf";
            await downloader.DownloadFile("", dwnPath, rapportPath, testNum + ".pdf", workingLink);
            bool canRead = false;
            using (var stream = File.Open(Path.Combine(dwnPath, fileName), FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                canRead = true;
            }
            File.Delete(Path.Combine(dwnPath, fileName));
            Assert.True(canRead);

        }
    }
    /// <summary>
    /// Class containing methods related to links which can be connected to but does not return a downloadable pdf
    /// </summary>
    [Collection("Filehandling")]
    public class DownloaderSkipsNonworkingLinksTest: DownloaderTestsBase
    {
        /// <summary>
        /// Checks if a file gets downloaded if the first link does not work
        /// </summary>
        [Fact]
        public async void DownloaderSkipsNonWorkingPrimaryLink()
        {
            testNum = Guid.NewGuid().ToString();
            string fileName = testNum + ".pdf";
            await downloader.DownloadFile(nonWorkingLink, dwnPath, rapportPath, fileName, workingLink);
            bool canRead = false;
            using (var stream = File.Open(Path.Combine(dwnPath, fileName), FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                canRead = true;
            }
            File.Delete(Path.Combine(dwnPath, fileName));
            Assert.True(canRead);
            
        }

        /// <summary>
        /// Checks to ensure the downloader does not attempt to create a file if the secondary link is not valid
        /// </summary>
        [Fact]
        public async void NothingGetsCreatedIfSecondaryLinkDoesNotWork()
        {
            testNum = Guid.NewGuid().ToString();
            string fileName = testNum + ".pdf";
            await downloader.DownloadFile("", dwnPath, rapportPath, testNum + ".pdf", nonWorkingLink);
            try
            {
                using (var stream = File.Open(Path.Combine(dwnPath, fileName), FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    Assert.True(false);
                }

            }
            catch (Exception ex)
            {
                Assert.True(true);
            }

        }

        /// <summary>
        /// Checks to ensure the status report gets updated correctly on a failed download from a nonworking link
        /// </summary>
        [Fact]
        public async void DoesStatusRapportGetUpdatedOnNonWorkingLinkDownload()
        {
            testNum = Guid.NewGuid().ToString();
            string fileName = testNum + ".pdf";
            await downloader.DownloadFile(nonWorkingLink, dwnPath, rapportPath, fileName, nonWorkingLink);
            bool entryAdded = false;
            lock(statusRepportLock)
            {
                foreach (string line in statusreportWriter.messages)
                {
                    if (line.Contains(testNum) && line.Contains("Failed to download"))
                    {
                        entryAdded = true;
                        break;
                    }
                }
            }
            File.Delete(Path.Combine(dwnPath, fileName));
            if(!entryAdded)
            {
                Assert.Equal("", testNum);
            }
            Assert.True(entryAdded);
        }
    }

    /// <summary>
    /// class containing tests related to working links which does not provide a downloadable pdf
    /// </summary>
    [Collection("Filehandling")]
    public class DownloaderSkipsNonPDFLinks:DownloaderTestsBase
    {
        /// <summary>
        /// Checks to ensure links without pdfs does not get downloaded
        /// </summary>
        [Fact]
        public async void NonPDFFileDoesNotGetDownloaded()
        {
            testNum = Guid.NewGuid().ToString();
            string fileName = testNum + ".pdf";
            await downloader.DownloadFile(NonPdfLink, dwnPath, rapportPath, testNum + ".pdf", "");
            try
            {
                using (var stream = File.Open(Path.Combine(dwnPath, fileName), FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    Assert.True(false);
                }

            }
            catch (Exception ex)
            {
                Assert.True(true);
            }
            
        }
    }

    /// <summary>
    /// class containing tests related to lost connections
    /// </summary>
    [Collection("filehandling")]
    public class DownloaderHandlesLostConnectionTests: DownloaderTestsBase
    {
        /// <summary>
        /// Check to ensure the downloader handles a complete connection loss after getting the initial header data
        /// </summary>
        [Fact]
        public async void DownloaderHandlesCompleteConnectionLoss()
        {
            testNum = Guid.NewGuid().ToString();
            string fileName = testNum + ".pdf";
            await downloader.DownloadFile(fakeLink, dwnPath, rapportPath, testNum + ".pdf", fakeLink);
            Assert.False(File.Exists(Path.Combine(dwnPath, fileName)));
        }
    }

    /// <summary>
    /// Handles various error scenarios related to the status report
    /// </summary>
    [Collection("filehandling")]
    public class DownloaderHandlesStatusReportErrorsTest: DownloaderTestsBase
    {
        /// <summary>
        /// Checks if the downloader throws an error if no path to the status report being provided
        /// </summary>
        [Fact]
        public async void DownloaderThrowsExceptionIfNoReportIsProvided()
        {
            string noStatusRapport = "";
            testNum = Guid.NewGuid().ToString();
            string fileName = testNum + ".pdf";
            await downloader.DownloadFile(workingLink, dwnPath, noStatusRapport, testNum + ".pdf", workingLink);
            bool errorLogged = false;
            bool fileDownloaded = File.Exists(Path.Combine(dwnPath, fileName));
            foreach(string message in downloader.consoleOutputHandler.Read())
            {
                if(message.Contains("An error occurred while writing the report: The value cannot be an empty string. (Parameter 'path')"))
                {
                    errorLogged = true;
                    break;
                }
            }
            Assert.True(errorLogged && fileDownloaded);
        }

        /// <summary>
        /// Checks if the downloader creates the status report if it has not yet been created at the path provided
        /// </summary>
        [Fact]
        public async void DownloaderCreatesStatusReportIfItDoesNotExist()
        {
            string noStatusRapport = "testStatusRaport.txt";
            testNum = Guid.NewGuid().ToString();
            string fileName = testNum + ".pdf";
            bool fileExisted = File.Exists(noStatusRapport);
            await downloader.DownloadFile(workingLink, dwnPath, noStatusRapport, testNum + ".pdf", workingLink);
            bool statusReportCreated = File.Exists(noStatusRapport);
            bool fileDownloaded = File.Exists(Path.Combine(dwnPath, fileName));
            File.Delete(noStatusRapport);
            File.Delete(Path.Combine(dwnPath, fileName));
            Assert.True(statusReportCreated && !fileExisted);
        }

        /// <summary>
        /// Checks to ensure the downloader only writes to a txt file
        /// </summary>
        [Fact]
        public async void DownloaderThrowsExceptionIfReportIsNotTxtFile()
        {
            string noStatusRapport = Path.GetFullPath("TestFiles/StatusReport.docx");
            testNum = Guid.NewGuid().ToString();
            string fileName = testNum + ".pdf";
            await downloader.DownloadFile(workingLink, dwnPath, noStatusRapport, testNum + ".pdf", workingLink);
            bool errorLogged = false;
            bool fileDownloaded = File.Exists(Path.Combine(dwnPath, fileName));
            foreach (string message in downloader.consoleOutputHandler.Read())
            {
                if (message.Contains("rapport is not a txt file"))
                {
                    errorLogged = true;
                    break;
                }
            }
            Assert.True(errorLogged && fileDownloaded);
        }
    }
}
