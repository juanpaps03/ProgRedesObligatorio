using System;
using System.Net.Sockets;
using SocketLibrary.Messages;

namespace SocketLibrary
{
    public class ProtocolCommunication: IProtocolCommunication
    {
        public readonly IMessageCommunication MsgCommunication;

        public ProtocolCommunication(NetworkStream stream)
        {
            
            MsgCommunication = new MessageCommunication(stream);
        }

        public Response SendRequest(Request request)
        {
            MsgCommunication.SendMessage(request);
            // TODO: Add try catch
            return (Response) MsgCommunication.ReceiveMessage();
        }

        public void HandleRequest(Func<Request, Response> handler)
        {
            Message msg = MsgCommunication.ReceiveMessage();
            // TODO: add try catch to cast
            Request request = (Request) msg;
            MsgCommunication.SendMessage(handler(request));
        }
    }
}