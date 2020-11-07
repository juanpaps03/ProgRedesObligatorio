using System.Threading.Tasks;
using SocketLibrary.Messages;

namespace SocketLibrary.Interfaces
{
    public interface IMessageCommunication
    {
        public Task SendMessageAsync(Message msg);

        public Task<Message> ReceiveMessageAsync();
    }
}