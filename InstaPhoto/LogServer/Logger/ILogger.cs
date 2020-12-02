using System.Collections.Generic;

namespace LogServer.Logger
{
    public interface ILogger
    {
        public void SaveLog(string log);
        public List<string> ReadLogs();
    }
}