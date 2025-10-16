using Microsoft.VisualStudio.TestPlatform.TestHost;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Program = PDFDownloader.Program;
namespace PDFDownloadTest
{
    public class ProgramTests
    {

        public class DefaultPathTests
        {
            [Fact]
            public void DefaultListPathExists()
            {
                Assert.True(Path.Exists(Program.defaultListPath));
            }

            [Fact]
            public void DefaultOutputPathExists()
            {
                Assert.True(Path.Exists(Program.defaultOutputPath));
            }

            [Fact]
            public void DefaultStatusPathExists()
            {
                Assert.True(Path.Exists(Program.defaultStatusPath));
            }


            [Fact]
            public void DefaultDwnPathExists()
            {
                Assert.True(Path.Exists(Program.defaultDwnPath));
            }
        }
    }

    public abstract class ProgramTestBase
    {
        protected ProgramTestBase()
        {
            Program.listPath = Path.GetFullPath("TestFiles/test.xlsx");
            Program.outputPath = Path.GetFullPath("Output");
            Program.statusPath = Path.GetFullPath("Output/StatusRapport.txt");
            if(!Directory.Exists("Output/dwn"))
            {
                Directory.CreateDirectory("Output/dwn");
            }
            Program.dwnPath = Path.GetFullPath("Output/dwn");
        }
    }

    public class ProgramRunsTest: ProgramTestBase
    {
        [Fact]
        public void ProgramRuns()
        {
            try
            {
                Program.Main(Array.Empty<string>());
                Assert.True(true);
            }
            catch (Exception ex)
            {
                Assert.Equal("", ex.Message);
            }
        }
    }

    public class ProgramRunsWithDataTest : ProgramTestBase
    {
        [Fact]
        public void ProgramRunsWithMockData()
        {
            Program.listPath = "TestFiles/GRI_2017_2020 (OfflineTestVersion).xlsx";
            Program.downloader.webClientHandler = new MockWebclientHandler();
            try
            {
                Program.Main(Array.Empty<string>());
                int fileCount = Directory.GetFiles(Program.dwnPath).Count();
                DirectoryInfo di = new DirectoryInfo(Program.dwnPath);
                foreach (FileInfo fi in di.GetFiles())
                {
                    fi.Delete();
                }
                Assert.True(fileCount == 10);
            }
            catch (Exception ex)
            {
                DirectoryInfo di = new DirectoryInfo(Program.dwnPath);
                foreach (FileInfo fi in di.GetFiles())
                {
                    fi.Delete();
                }
                Assert.Equal("an exception was received", ex.Message);
            }
        }

        [Fact]
        public void ProgramRunsWithActualData()
        {
            Program.listPath = "TestFiles/GRI_2017_2020 (Reduced).xlsx";
            try
            {
                Program.Main(Array.Empty<string>());
                int fileCount = Directory.GetFiles(Program.dwnPath).Count();
                DirectoryInfo di = new DirectoryInfo(Program.dwnPath);
                foreach (FileInfo fi in di.GetFiles())
                {
                    fi.Delete();
                }
                Assert.True(fileCount > 0 || Program.downloadAttempts == 51);
            }
            catch (Exception ex)
            {
                DirectoryInfo di = new DirectoryInfo(Program.dwnPath);
                foreach (FileInfo fi in di.GetFiles())
                {
                    fi.Delete();
                }
                Assert.Equal("an exception was received", ex.Message);
            }
        }
    }
}
