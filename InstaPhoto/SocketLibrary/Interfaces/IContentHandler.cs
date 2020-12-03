using System.Threading.Tasks;
using SocketLibrary.Messages;

namespace SocketLibrary.Interfaces
{
    public interface IContentHandler<T> where T : Message
    {
        public Task SendMessageAsync(T msg);

        public Task<T> ReceiveMessageAsync();
    }
}