using NPOI.HPSF;
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
    }
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
}
