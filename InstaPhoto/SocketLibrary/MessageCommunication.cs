using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using SocketLibrary.Interfaces;
using SocketLibrary.Messages;
using SocketLibrary.Messages.Login;

namespace SocketLibrary
{
    public class MessageCommunication: IMessageCommunication
    {
        private readonly NetworkCommunication _networkCommunication;

        public MessageCommunication(NetworkStream networkStream)
        {
            _networkCommunication = new NetworkCommunication(networkStream);
        }

        public async Task SendMessageAsync(Message msg)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Message> ReceiveMessageAsync()
        {
            byte[] msg = await _networkCommunication.ReceiveBytesAsync(1);
            byte messageType = msg[0];
            short messageId = await _networkCommunication.ReceiveShortAsync();
            switch ((messageId, messageType))
            {
                case (101, 0):
                    var loginRequestHandler = new LoginRequestHandler(_networkCommunication);
                    return await loginRequestHandler.ReceiveMessageAsync();
            }
            // TODO Create a custom exception
            throw new Exception();
        }
    }
}