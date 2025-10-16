using Microsoft.VisualStudio.TestPlatform.TestHost;
using PDFDownload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFDownloadTest
{
    public abstract class UserInfoHandlerTestsBase
    {
        protected IConsoleOutputHandler infoHandler;

        protected UserInfoHandlerTestsBase()
        {
            infoHandler = new ConsoleOut();
        }
    }

    public class WriteLineOutputsToListTest : UserInfoHandlerTestsBase
    {
        [Fact]
        public void WriteLineOutPutsToList()
        {
            
            infoHandler.WriteLine("test");
            Assert.Single(infoHandler.Read());
        }
    }

    public class WriteOutputsToListTest : UserInfoHandlerTestsBase 
    {
        [Fact]
        public void WriteOutputsToList()
        {
            
            infoHandler.Write("test");
            Assert.Single(infoHandler.Read());
        }
    }
    public class ReadReturnsListTest : UserInfoHandlerTestsBase
    {
        [Fact]
        public void ReadReturnsList()
        {
            Assert.Equal(typeof(List<string>), infoHandler.Read().GetType());
        }
    }
}
