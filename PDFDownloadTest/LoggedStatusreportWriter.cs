using PDFDownload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFDownloadTest
{
    public class LoggedStatusreportWriter : IStatusReportWriter
    {
        private StatusReportWriter statusReportWriter = new StatusReportWriter();
        public List<string> messages = new List<string>();
        public void Write(string message, string statusReportPath)
        {
            messages.Add(message);
            statusReportWriter.Write(message, statusReportPath);
        }
    }
}
