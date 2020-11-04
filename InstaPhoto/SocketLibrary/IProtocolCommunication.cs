using System;

namespace SocketLibrary
{
    public interface IProtocolCommunication
    {
        public Response SendRequest(Request request);
        public void HandleRequest(Func<Request,Response> handler);
    }
}