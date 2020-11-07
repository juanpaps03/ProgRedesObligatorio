using System;
using System.Threading.Tasks;
using SocketLibrary.Messages;

namespace SocketLibrary.Interfaces
{
    public interface IProtocolCommunication
    {
        public Task<Response> SendRequestAsync(Request request);
        public Task HandleRequestAsync(Func<Request,Response> handler);
    }
}