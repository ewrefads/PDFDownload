using Microsoft.VisualStudio.TestPlatform.TestHost;
using PDFDownload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFDownloadTest
{
    /// <summary>
    /// Base class for tests of the ConsoleOutputHandler class 
    /// </summary>
    public abstract class ConsoleOutputHandlerTestsBase
    {
        protected IConsoleOutputHandler infoHandler;

        protected ConsoleOutputHandlerTestsBase()
        {
            infoHandler = new ConsoleOut();
        }
    }

    /// <summary>
    /// Class containing a test to see if writeLine messages gets added to the list
    /// </summary>
    public class WriteLineOutputsToListTest : ConsoleOutputHandlerTestsBase
    {
        /// <summary>
        /// Checks if writeLine calls adds the message to the list
        /// </summary>
        [Fact]
        public void WriteLineOutPutsToList()
        {
            
            infoHandler.WriteLine("test");
            Assert.Single(infoHandler.Read());
        }
    }

    /// <summary>
    /// Class containing a test to see if write messages gets added to the list 
    /// </summary>
    public class WriteOutputsToListTest : ConsoleOutputHandlerTestsBase 
    {
        /// <summary>
        /// Checks if write calls adds the message to the list
        /// </summary>
        [Fact]
        public void WriteOutputsToList()
        {
            
            infoHandler.Write("test");
            Assert.Single(infoHandler.Read());
        }
    }

    /// <summary>
    /// Class containing a method to see what the read method returns
    /// </summary>
    public class ReadReturnsListTest : ConsoleOutputHandlerTestsBase
    {
        /// <summary>
        /// Checks to see if the read method returns a list
        /// </summary>
        [Fact]
        public void ReadReturnsList()
        {
            Assert.Equal(typeof(List<string>), infoHandler.Read().GetType());
        }
    }
}
