using System;
using SocketLibrary.Messages;

namespace SocketLibrary
{
    public interface IProtocolCommunication
    {
        public Response SendRequest(Request request);
        public void HandleRequest(Func<Request,Response> handler);
    }
}