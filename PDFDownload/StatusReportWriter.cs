using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFDownload
{
    /// <summary>
    /// An implementation of IStatusReportWriter
    /// </summary>
    public class StatusReportWriter : IStatusReportWriter
    {
        /// <summary>
        /// Writes the message to the end of the given status report
        /// </summary>
        /// <param name="message">The message to be written</param>
        /// <param name="statusReportPath">The location of the status report</param>
        public void Write(string message, string statusReportPath)
        {
            lock(Downloader.statusRapportLock)
            {
                File.AppendAllText(statusReportPath, message);
            }
            
        }
    }
}
