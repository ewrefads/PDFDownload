using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFDownload
{
    /// <summary>
    /// A wrapper interface for write console calls
    /// </summary>
    public interface IConsoleOutputHandler
    {
        /// <summary>
        /// Handles Write calls
        /// </summary>
        /// <param name="message">The message to be displayed</param>
        public void Write(string message);

        /// <summary>
        /// Handles WriteLine calls
        /// </summary>
        /// <param name="message">The message to be displayed</param>
        public void WriteLine(string message);

        /// <summary>
        /// If the implementation logs the calls this method returns the list of calls
        /// </summary>
        /// <returns>a list containing all previous calls</returns>
        public List<string> Read();
    }

    /// <summary>
    /// An implementation of IConsoleOutputHandler
    /// </summary>
    public class ConsoleOut : IConsoleOutputHandler
    {
        List<string> messages = new List<string>();
        public List<string> Read()
        {
            return messages;
        }

        /// <summary>
        /// Prints the message and saves it to its own log
        /// </summary>
        /// <param name="message">The message to be displayed</param>
        public void Write(string message)
        {
            Console.Write(message);
            messages.Add(message);
        }

        /// <summary>
        /// Prints the message and saves it to its own log
        /// </summary>
        /// <param name="message">The message to be displayed</param>
        public void WriteLine(string message)
        {
            Console.WriteLine(message);
            messages.Add(message);
        }
    }
}
