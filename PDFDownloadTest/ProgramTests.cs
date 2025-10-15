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
            public void DefaultListPathDoesNotExists()
            {
                Assert.True(!Path.Exists(Program.defaultListPath));
            }

            [Fact]
            public void DefaultOutputPathDoesNotExists()
            {
                Assert.True(!Path.Exists(Program.defaultOutputPath));
            }

            [Fact]
            public void DefaultStatusPathDoesNotExists()
            {
                Assert.True(!Path.Exists(Program.defaultStatusPath));
            }


            [Fact]
            public void DefaultDwnPathDoesNotExists()
            {
                Assert.True(!Path.Exists(Program.defaultDwnPath));
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
                Program.Main(new string[0]);
                Assert.True(true);
            }
            catch (Exception ex)
            {
                Assert.Equal("", ex.Message);
            }
        }
    }
}
