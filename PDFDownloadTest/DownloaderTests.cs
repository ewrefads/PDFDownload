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
    public class DownloaderTestsBase
    {
        protected Downloader downloader = new Downloader();
        public static string workingLink = Path.GetFullPath("TestFiles/test.pdf");
        public static string fakeLink = Path.GetFullPath("TestFiles/nonexistingFile");
        public static string nonWorkingLink = Path.GetFullPath("TestFiles/noFile");
        public static string NonPdfLink = Path.GetFullPath("TestFiles/test.txt");
        public static string testNum = "test";
        public static string dwnPath = Path.GetFullPath("Output/dwn");
        public static string rapportPath = Path.GetFullPath("Output/StatusRapport.txt");
        protected static object pdfFileLock = new object();
        protected static object statusRepportLock = Downloader.statusRapportLock;
        protected LoggedStatusreportWriter statusreportWriter = new LoggedStatusreportWriter();
        
        public DownloaderTestsBase()
        {
            downloader.webClientHandler = new MockWebclientHandler();
            downloader.statusReportWriter = statusreportWriter;
        }
    }
    [Collection("Filehandling")]
    public class CanDownloaderDownloadFromWorkingLinkTest: DownloaderTestsBase
    {
        
        [Fact]
        public async void CanDownloaderDownloadFromWorkingPrimaryLink()
        {
            testNum = Guid.NewGuid().ToString();
            string fileName = testNum + ".pdf";
            await downloader.DownloadFile(workingLink, dwnPath, rapportPath, fileName, "");
            bool canRead = false;
            using (var stream = File.Open(Path.Combine(dwnPath, fileName), FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                canRead = true;
            }
            File.Delete(Path.Combine(dwnPath, fileName));
            Assert.True(canRead);
        }

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
    [Collection("Filehandling")]
    public class DownloaderSkipsNonworkingLinksTest: DownloaderTestsBase
    {
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
    [Collection("Filehandling")]
    public class DownloaderSkipsNonPDFLinks:DownloaderTestsBase
    {
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

    [Collection("filehandling")]
    public class DownloaderHandlesLostConnectionTests: DownloaderTestsBase
    {
        [Fact]
        public async void DownloaderHandlesCompleteConnectionLoss()
        {
            testNum = Guid.NewGuid().ToString();
            string fileName = testNum + ".pdf";
            await downloader.DownloadFile(fakeLink, dwnPath, rapportPath, testNum + ".pdf", fakeLink);
            Assert.False(File.Exists(Path.Combine(dwnPath, fileName)));
        }
    }

    [Collection("filehandling")]
    public class DownloaderHandlesStatusReportErrorsTest: DownloaderTestsBase
    {
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

        [Fact]
        public async void DownloaderCreatesStatusReportIfItDoesNotExist()
        {
            string noStatusRapport = "testStatusRaport.txt";
            testNum = Guid.NewGuid().ToString();
            string fileName = testNum + ".pdf";
            await downloader.DownloadFile(workingLink, dwnPath, noStatusRapport, testNum + ".pdf", workingLink);
            bool statusReportCreated = File.Exists(noStatusRapport);
            bool fileDownloaded = File.Exists(Path.Combine(dwnPath, fileName));
            File.Delete(noStatusRapport);
            File.Delete(Path.Combine(dwnPath, fileName));
            Assert.True(statusReportCreated);
        }

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
