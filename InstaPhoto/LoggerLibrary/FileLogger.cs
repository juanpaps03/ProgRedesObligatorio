using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using LoggerLibrary.Rabbit;

namespace LoggerLibrary
{
    public class FileLogger: ILogger
    {
        public void SendLog(string log)
        {
            RabbitClient.SendMessage(log);
        }

        public void SaveLog(string logMessage)
        {
            string path = @"./log.txt";
            string formatedLog = $"{DateTime.Now.ToString("s")}: {logMessage}";
            if (!File.Exists(path))
            {
                using StreamWriter sw = File.CreateText(path);
                sw.WriteLine(formatedLog);
            }
            else
            {
                using StreamWriter sw = File.AppendText(path);
                sw.WriteLine(formatedLog);
            }
        }

        public List<string> ReadLogs()
        {
            throw new System.NotImplementedException();
        }
    }
}