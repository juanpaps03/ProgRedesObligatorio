using System.Collections.Generic;

namespace LoggerLibrary
{
    public interface ILogger
    {
        public void SaveLog(string log);
        public List<string> ReadLogs();
    }
}