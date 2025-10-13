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
            public void ListPathExists()
            {
                Assert.True(Path.Exists(Program.defaultListPath));
            }

            [Fact]
            public void OutputPathExists()
            {
                Assert.True(Path.Exists(Program.defaultOutputPath));
            }

            [Fact]
            public void StatusPathExists()
            {
                Assert.True(Path.Exists(Program.statusPath));
            }


            [Fact]
            public void DwnPathExists()
            {
                Assert.True(Path.Exists(Program.dwnPath));
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
