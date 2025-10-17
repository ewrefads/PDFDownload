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
    /// <summary>
    /// Tests to ensure the default paths of the program works
    /// </summary>
    public class DefaultPathTests
    {
        /// <summary>
        /// Ensures the default path to the data works
        /// </summary>
        [Fact]
        public void DefaultListPathExists()
        {
            Assert.True(Path.Exists(Program.defaultListPath));
        }

        /// <summary>
        /// Ensures the default path to the output folder works
        /// </summary>
        [Fact]
        public void DefaultOutputPathExists()
        {
            Assert.True(Path.Exists(Program.defaultOutputPath));
        }

        /// <summary>
        /// Ensures the default path to the status report works
        /// </summary>
        [Fact]
        public void DefaultStatusPathExists()
        {
            Assert.True(Path.Exists(Program.defaultStatusPath));
        }

        /// <summary>
        /// Ensures the default path to the download folder works
        /// </summary>
        [Fact]
        public void DefaultDwnPathExists()
        {
            Assert.True(Path.Exists(Program.defaultDwnPath));
        }
    }

    /// <summary>
    /// Base class for tests of the main program
    /// </summary>
    public abstract class ProgramTestBase
    {
        /// <summary>
        /// Sets the programs path to various test folders
        /// </summary>
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

    /// <summary>
    /// class containing a basic test to ensure the program can complete its main method
    /// </summary>
    public class ProgramRunsTest: ProgramTestBase
    {
        /// <summary>
        /// Checks to see if the program completes its main method without issues using only a singe actual datapoint
        /// </summary>
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

    /// <summary>
    /// Tests to see if the program runs with data
    /// </summary>
    public class ProgramRunsWithDataTest : ProgramTestBase
    {
        /// <summary>
        /// Checks to see if the program can run with mock data. The data has links provided to local files
        /// </summary>
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

        /// <summary>
        /// Checks to see if the program can handle actual data
        /// </summary>
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
