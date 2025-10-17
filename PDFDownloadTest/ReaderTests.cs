using Microsoft.VisualStudio.TestPlatform.TestHost;
using PDFDownload;
using PDFDownloader;
using System.Data;
using System.Reflection.PortableExecutable;

namespace PDFDownloadTest
{
    /// <summary>
    /// Base class for the tests of the Reader class
    /// </summary>
    [Collection("Filehandling")]
    public abstract class ReaderTestBase : IDisposable
    {
        public static object fileLock = new object();
        protected Reader reader;
        protected IConsoleOutputHandler userInfoHandler;
        protected ReaderTestBase()
        {
            reader = new Reader();
            userInfoHandler = new ConsoleOut();
            reader.ConsoleOutPutHandler = userInfoHandler;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }
        public void Dispose()
        {
            userInfoHandler.Read().Clear();
        }
    }

    /// <summary>
    /// class containing a test to check what happens when no file is provided
    /// </summary>
    [Collection("Filehandling")]
    public class FailsToReadNoFileTest: ReaderTestBase
    {
        /// <summary>
        /// Checks to see if the program fails if no file is provided
        /// </summary>
        [Fact]
        public void FailsToReadNoFile()
        {
            reader.ReadFile("");
            var outputString = userInfoHandler.Read()[userInfoHandler.Read().Count - 1];
            Assert.Contains($"An error occurred while reading the Excel file: ", outputString);
        }
    }

    /// <summary>
    /// class containing a test to check what happens when a non xlsx file is provided
    /// </summary>
    [Collection("Filehandling")]
    public class FailsToReadNonXlsxFileTest: ReaderTestBase
    {
        /// <summary>
        /// Checks to see if the program fails if no xlsx file is provided
        /// </summary>
        [Fact]
        public void FailsToReadNonXlsxFile()
        {
            reader.ReadFile(Path.GetFullPath("TestFiles/test.txt"));
            var outputString = userInfoHandler.Read()[userInfoHandler.Read().Count - 1];
            Assert.Contains($"An error occurred while reading the Excel file: ", outputString);
        }
    }
    /// <summary>
    /// class containing a test to check what happens when a corrupted xlsx file is provided
    /// </summary>
    [Collection("Filehandling")]
    public class FailsToReadCorruptedFileTest: ReaderTestBase
    {
        /// <summary>
        /// Checks to see if the program fails if a corrupted xlsx file is provided
        /// </summary>
        [Fact]
        public void FailsToReadCorruptedFile()
        {
            reader.ConsoleOutPutHandler = userInfoHandler;
            reader.ReadFile(Path.GetFullPath("TestFiles/testCorrupted.xlsx"));
            var outputString = userInfoHandler.Read()[userInfoHandler.Read().Count - 1];
            Assert.Contains($"An error occurred while reading the Excel file: ", outputString);
        }
    }

    /// <summary>
    /// class containing a test to see what happens when a working xlsx file is provided
    /// </summary>
    [Collection("Filehandling")]
    public class ReadsFileFromAbsolutePathTest: ReaderTestBase
    {
        /// <summary>
        /// Check to see if a working xlsx file is read correctly
        /// </summary>
        [Fact]
        public void ReadsFileFromAbsolutePath()
        {
            
            string path = Path.GetFullPath("TestFiles/test.xlsx");
            lock(fileLock)
            {
                Assert.Equal(2, reader.ReadFile(path).Rows.Count);
            }
            
        }
    }

    /// <summary>
    /// class containing a test to see what happens when the reduced data set gets loaded
    /// </summary>
    [Collection("Filehandling")]
    public class ActualReducedDataLoadsTest: ReaderTestBase
    {
        /// <summary>
        /// Checks to see if a larger dataset can be loaded
        /// </summary>
        [Fact]
        public void ActualReducedDataLoads()
        {
            DataTable table = reader.ReadFile(Path.GetFullPath("TestFiles/GRI_2017_2020 (Reduced).xlsx"));
            Assert.Equal(20, table.Rows.Count);
            Assert.Equal(41, table.Columns.Count);
        }
    }
}