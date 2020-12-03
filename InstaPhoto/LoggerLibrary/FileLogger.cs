using System;
using System.Collections.Generic;
using System.IO;

namespace LoggerLibrary
{
    public class FileLogger: ILogger
    {
        public void SaveLog(string logMessage)
        {
            const string path = "application.log"; // TODO: MAYBE MOVE TO SETTINGS
            
            var formattedLog = $"{DateTime.Now:s}: {logMessage}";
            if (!File.Exists(path))
            {
                using var sw = File.CreateText(path);
                sw.WriteLine(formattedLog);
            }
            else
            {
                using var sw = File.AppendText(path);
                sw.WriteLine(formattedLog);
            }
        }

        public List<string> ReadLogs()
        {
            throw new NotImplementedException();
        }
    }
}