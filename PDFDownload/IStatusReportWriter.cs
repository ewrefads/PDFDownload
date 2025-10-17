using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFDownload
{
    /// <summary>
    /// A wrappter interface to handle writing to a status report
    /// </summary>
    public interface IStatusReportWriter
    {
        /// <summary>
        /// Writes the given message to the end of the status report
        /// </summary>
        /// <param name="message">The message to be written</param>
        /// <param name="statusReportPath">The location of the status report</param>
        public void Write(string message, string statusReportPath);
    }
}
