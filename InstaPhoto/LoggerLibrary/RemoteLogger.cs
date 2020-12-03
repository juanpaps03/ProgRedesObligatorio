using System.Collections.Generic;
using LoggerLibrary.Rabbit;

namespace LoggerLibrary
{
    public class RemoteLogger: ILogger
    {
        private readonly RabbitQueueHelper _rabbitQueueHelper;

        public RemoteLogger(RabbitQueueHelper rabbitQueueHelper)
        {
            _rabbitQueueHelper = rabbitQueueHelper;
        }

        public void SaveLog(string log)
        {
            _rabbitQueueHelper.SendMessage(log);
        }

        public List<string> ReadLogs()
        {
            throw new System.NotImplementedException();
        }
    }
}