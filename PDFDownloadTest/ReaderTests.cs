using Microsoft.VisualStudio.TestPlatform.TestHost;
using PDFDownload;
using PDFDownloader;
using System.Data;
using System.Reflection.PortableExecutable;

namespace PDFDownloadTest
{
    public abstract class ReaderTestBase : IDisposable
    {
        public static object fileLock = new object();
        protected Reader reader;
        protected IConsoleOutputHandler userInfoHandler;
        protected ReaderTestBase()
        {
            reader = new Reader();
            userInfoHandler = new ConsoleOut();
            reader.UserInfoHandler = userInfoHandler;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }
        public void Dispose()
        {
            userInfoHandler.Read().Clear();
        }
    }

    public class FailsToReadNoFileTest: ReaderTestBase
    {
        [Fact]
        public void FailsToReadNoFile()
        {
            reader.ReadFile("");
            var outputString = userInfoHandler.Read()[userInfoHandler.Read().Count - 1];
            Assert.Contains($"An error occurred while reading the Excel file: ", outputString);
        }
    }

    public class FailsToReadNonXlsxFileTest: ReaderTestBase
    {
        [Fact]
        public void FailsToReadNonXlsxFile()
        {
            reader.ReadFile(Path.GetFullPath("TestFiles/test.txt"));
            var outputString = userInfoHandler.Read()[userInfoHandler.Read().Count - 1];
            Assert.Contains($"An error occurred while reading the Excel file: ", outputString);
        }
    }

    public class FailsToReadCorruptedFileTest: ReaderTestBase
    {
        [Fact]
        public void FailsToReadCorruptedFile()
        {
            reader.UserInfoHandler = userInfoHandler;
            reader.ReadFile(Path.GetFullPath("TestFiles/testCorrupted.xlsx"));
            var outputString = userInfoHandler.Read()[userInfoHandler.Read().Count - 1];
            Assert.Contains($"An error occurred while reading the Excel file: ", outputString);
        }
    }

    public class ReadsFileFromAbsolutePathTest: ReaderTestBase
    {
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

    public class ActualDataLoadsTest: ReaderTestBase
    {
        [Fact]
        public void ActualDataLoads()
        {
            DataTable table = reader.ReadFile(Path.GetFullPath("TestFiles/GRI_2017_2020 (Reduced).xlsx"));
            Assert.Equal(20, table.Rows.Count);
            Assert.Equal(41, table.Columns.Count);
        }
    }
}