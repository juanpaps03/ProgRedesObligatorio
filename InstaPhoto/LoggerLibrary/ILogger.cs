using System.Collections.Generic;

namespace LoggerLibrary
{
    public interface ILogger
    {
        void SaveLog(string log);
        List<string> ReadLogs();
    }
}