using PDFDownload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFDownloadTest
{
    /// <summary>
    /// An implementation of IStatusReportWriter which saves the messages to a list
    /// </summary>
    public class LoggedStatusreportWriter : IStatusReportWriter
    {
        private StatusReportWriter statusReportWriter = new StatusReportWriter();
        public List<string> messages = new List<string>();
        /// <summary>
        /// Saves a message to a list and then sends it on to a StatusReportWriter
        /// </summary>
        /// <param name="message">The message to be added to the status report</param>
        /// <param name="statusReportPath">The location of the status report</param>
        public void Write(string message, string statusReportPath)
        {
            messages.Add(message);
            statusReportWriter.Write(message, statusReportPath);
        }
    }
}
