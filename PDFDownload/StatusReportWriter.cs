using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFDownload
{
    public class StatusReportWriter : IStatusReportWriter
    {
        public void Write(string message, string statusReportPath)
        {
            lock(Downloader.statusRapportLock)
            {
                File.AppendAllText(statusReportPath, message);
            }
            
        }
    }
}
