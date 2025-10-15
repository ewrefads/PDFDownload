using PDFDownload;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFDownloadTest
{
    public class DownloaderTestsBase
    {
        protected Downloader downloader = new Downloader();
        protected string workingLink = "TestFiles/test.pdf";
        protected string nonWorkingLink = "TestFiles/noFile";
        protected string NonPdfLink = "TestFiles/test.txt";
        protected string testNum;
        protected string dwnPath = Path.GetFullPath("Output");
        protected string rapportPath = Path.GetFullPath("Output/StatusRapport.txt");
        protected static object pdfFileLock = new object();
        
        public DownloaderTestsBase()
        {
            testNum = "test";
            downloader.webClientHandler = new MockWebclientHandler();
        }
    }

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
        public void CanDownloaderDownloadFromWorkingSecondaryLink()
        {
            testNum = Guid.NewGuid().ToString();
            downloader.DownloadFile("", dwnPath, rapportPath, testNum + ".pdf", workingLink);
            if (File.Exists(Path.Combine(dwnPath, testNum + ".pdf")))
            {
                File.Delete(Path.Combine(dwnPath, testNum + ".pdf"));
                Assert.True(true);
            }
            else
            {
                Assert.True(false);
            }

        }
    }

    public class DownloaderSkipsNonworkingLinksTest: DownloaderTestsBase
    {
        [Fact]
        public void DownloaderSkipsNonWorkingPrimaryLink()
        {
            testNum = Guid.NewGuid().ToString();
            lock (pdfFileLock)
            {
                string fileName = testNum + ".pdf";
                downloader.DownloadFile(nonWorkingLink, dwnPath, rapportPath, fileName, workingLink);
                if (File.Exists(Path.Combine(dwnPath, fileName)))
                {
                    File.Delete(Path.Combine(dwnPath, fileName));
                    Assert.True(true);
                }
                else
                {
                    Assert.True(false);
                }
            }
            
        }
        [Fact]
        public void NothingGetsCreatedIfSecondaryLinkDoesNotWork()
        {
            testNum = Guid.NewGuid().ToString();
            lock (pdfFileLock)
            {
                downloader.DownloadFile("", dwnPath, rapportPath, testNum + ".pdf", nonWorkingLink);
                if (!File.Exists(Path.Combine(dwnPath, testNum + ".pdf")))
                {

                    Assert.True(true);
                }
                else
                {
                    File.Delete(Path.Combine(dwnPath, testNum + ".pdf"));
                    Assert.True(false);
                }
            }
            
        }
    }
    public class DownloaderSkipsNonPDFLinks:DownloaderTestsBase
    {
        [Fact]
        public void NonPDFFileDoesNotGetDownloaded()
        {
            testNum = Guid.NewGuid().ToString();
            downloader.DownloadFile(NonPdfLink, dwnPath, rapportPath, testNum + ".pdf", "");
            if (!File.Exists(Path.Combine(dwnPath, testNum + ".pdf")))
            {

                Assert.True(true);
            }
            else
            {
                File.Delete(Path.Combine(dwnPath, testNum + ".pdf"));
                Assert.True(false);
            }
        }
    }
}
