using System;
using System.Collections.Generic;
using System.Net.Sockets;
using SocketLibrary.Messages;
using SocketLibrary.Messages.Login;

namespace SocketLibrary
{
    public class MessageCommunication: IMessageCommunication
    {
        private NetworkCommunication _networkCommunication;

        public MessageCommunication(NetworkStream networkStream)
        {
            _networkCommunication = new NetworkCommunication(networkStream);
        }

        public void SendMessage(Message msg)
        {
            throw new System.NotImplementedException();
        }

        public Message ReceiveMessage()
        {
            byte[] msg = _networkCommunication.ReceiveBytes(1);
            byte messageType = msg[0];
            short messageId = _networkCommunication.ReceiveShort();
            switch ((messageId, messageType))
            {
                case (101, 0):
                    return new LoginRequestHandler(_networkCommunication).ReceiveMessage();
            }
            // TODO Create a custom exception
            throw new Exception();
        }
    }
}