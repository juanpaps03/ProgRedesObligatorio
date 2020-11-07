using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using SocketLibrary.Constants;
using SocketLibrary.Interfaces;
using SocketLibrary.Messages;
using SocketLibrary.Messages.CreatePhoto;
using SocketLibrary.Messages.Error;
using SocketLibrary.Messages.Login;

namespace SocketLibrary
{
    public class MessageCommunication : IMessageCommunication
    {
        private readonly INetworkCommunication _networkCommunication;
        private readonly IFileCommunication _fileCommunication;

        public MessageCommunication(NetworkStream networkStream)
        {
            _networkCommunication = new NetworkCommunication(networkStream);
            _fileCommunication = new FileCommunication(networkStream);
        }

        public async Task SendMessageAsync(Message msg)
        {
            await _networkCommunication.SendBytesAsync(new[] {(byte) msg.Type});
            await _networkCommunication.SendShortAsync((short) msg.Id);
            switch (msg)
            {
                // Error
                case ErrorResponse errorResponse:
                    var errorResponseHandler = new ErrorResponseHandler(_networkCommunication);
                    await errorResponseHandler.SendMessageAsync(errorResponse);
                    break;
                
                // Login
                case LoginRequest loginRequest:
                    var loginRequestHandler = new LoginRequestHandler(_networkCommunication);
                    await loginRequestHandler.SendMessageAsync(loginRequest);
                    break;
                
                // Create photo
                case CreatePhotoRequest createPhotoRequest:
                    var createPhotoRequestHandler = new CreatePhotoRequestHandler(
                        _networkCommunication,
                        _fileCommunication
                    );
                    await createPhotoRequestHandler.SendMessageAsync(createPhotoRequest);
                    break;
                case CreatePhotoResponse createPhotoResponse:
                    var createPhotoResponseHandler = new CreatePhotoResponseHandler();
                    await createPhotoResponseHandler.SendMessageAsync(createPhotoResponse);
                    break;
                default:
                    // TODO Create a custom exception
                    throw new Exception();
            }
        }

        public async Task<Message> ReceiveMessageAsync()
        {
            var messageType = (MessageType) (await _networkCommunication.ReceiveBytesAsync(1))[0];
            var messageId = (MessageId) await _networkCommunication.ReceiveShortAsync();
            switch (messageId, messageType)
            {
                // Error
                case (MessageId.Error, MessageType.Response):
                    var errorResponseHandler = new ErrorResponseHandler(_networkCommunication);
                    return await errorResponseHandler.ReceiveMessageAsync();
                
                // Login
                case (MessageId.Login, MessageType.Request):
                    var loginRequestHandler = new LoginRequestHandler(_networkCommunication);
                    return await loginRequestHandler.ReceiveMessageAsync();
                
                // Create photo
                case (MessageId.CreatePhoto, MessageType.Request):
                    var createPhotoRequestHandler = new CreatePhotoRequestHandler(
                        _networkCommunication,
                        _fileCommunication
                    );
                    return await createPhotoRequestHandler.ReceiveMessageAsync();
                case (MessageId.CreatePhoto, MessageType.Response):
                    var createPhotoResponseHandler = new CreatePhotoResponseHandler();
                    return await createPhotoResponseHandler.ReceiveMessageAsync();
            }

            // TODO Create a custom exception
            throw new Exception();
        }
    }
}