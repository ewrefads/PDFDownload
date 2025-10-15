using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFDownloadTest
{
    public class PDFDownloadTestTests
    {
        [Fact]
        public void TestFilesFolderExists()
        {
            Assert.True(Directory.Exists("TestFiles"));
        }

        [Fact]
        public void testCorruptedXlsxExists()
        {
            Assert.True(Path.Exists("TestFiles/testCorrupted.xlsx"));
        }

        /*[Fact]
        public void testXlsxExists()
        {
            lock (ReaderTestBase.fileLock)
            {
                Assert.True(Path.Exists("TestFiles/test.xlsx"));
            }
            
        }*/

        [Fact]
        public void testTxtExists()
        {
            Assert.True(Path.Exists("TestFiles/test.txt"));
        }
        [Fact]
        public void actualDataExists()
        {
            Assert.True(Path.Exists("TestFiles/GRI_2017_2020 (1).xlsx"));
        }
    }
}
