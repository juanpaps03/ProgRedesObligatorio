using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using SocketLibrary.Interfaces;
using SocketLibrary.Messages;

namespace SocketLibrary
{
    public class ProtocolCommunication: IProtocolCommunication
    {
        private readonly IMessageCommunication _msgCommunication;

        public ProtocolCommunication(NetworkStream stream)
        {
            
            _msgCommunication = new MessageCommunication(stream);
        }

        public async Task<Response> SendRequestAsync(Request request)
        {
            await _msgCommunication.SendMessageAsync(request);
            // TODO: Add try catch
            return (Response) await _msgCommunication.ReceiveMessageAsync();
        }

        public async Task HandleRequestAsync(Func<Request, Response> handler)
        {
            // TODO: add try catch to cast
            Request request = (Request) await _msgCommunication.ReceiveMessageAsync();
            await _msgCommunication.SendMessageAsync(handler(request));
        }
    }
}