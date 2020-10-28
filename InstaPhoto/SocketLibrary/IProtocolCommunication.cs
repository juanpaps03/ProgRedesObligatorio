using System;

namespace SocketLibrary
{
    public interface IProtocolCommunication
    {
        public void SendRequest(Request request);
        public Response HandleRequest(Func<Request,Response> handler);
    }
}