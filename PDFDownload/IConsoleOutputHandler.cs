using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFDownload
{
    public interface IConsoleOutputHandler
    {
        public void Write(string message);
        public void WriteLine(string message);

        public List<string> Read();
    }

    public class ConsoleOut : IConsoleOutputHandler
    {
        List<string> messages = new List<string>();
        public List<string> Read()
        {
            return messages;
        }

        public void Write(string message)
        {
            Console.Write(message);
            messages.Add(message);
        }

        public void WriteLine(string message)
        {
            Console.WriteLine(message);
            messages.Add(message);
        }
    }
}
