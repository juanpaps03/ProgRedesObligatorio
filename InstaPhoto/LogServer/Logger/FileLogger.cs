using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;

namespace LogServer.Logger
{
    public class FileLogger: ILogger
    {
        public void SaveLog(string log)
        {
            string path = @"\home\juanpaps03\log.txt";
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(log);   
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(log);
                }
            }
        }

        public List<string> ReadLogs()
        {
            throw new System.NotImplementedException();
        }
    }
}