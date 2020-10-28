using System;

namespace SocketLibrary
{
    public class ProtocolCommunication: IProtocolCommunication
    {
        public void SendRequest(Request request)
        {
            throw new NotImplementedException();
        }

        public Response HandleRequest(Func<Request, Response> handler)
        {
            throw new NotImplementedException();
        }
    }
}