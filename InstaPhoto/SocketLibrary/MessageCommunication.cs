using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using SocketLibrary.Constants;
using SocketLibrary.Interfaces;
using SocketLibrary.Messages;
using SocketLibrary.Messages.CreatePhoto;
using SocketLibrary.Messages.Error;
using SocketLibrary.Messages.Login;
using SocketLibrary.Messages.PhotoList;
using SocketLibrary.Messages.UserList;

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
                
                // Photo list
                case PhotoListRequest photoListRequest:
                    var photoListRequestHandler = new PhotoListRequestHandler(_networkCommunication);
                    await photoListRequestHandler.SendMessageAsync(photoListRequest);
                    break;
                case PhotoListResponse photoListResponse:
                    var photoListResponseHandler = new PhotoListResponseHandler(
                        _networkCommunication, 
                        _fileCommunication
                    );
                    await photoListResponseHandler.SendMessageAsync(photoListResponse);
                    break;
                
                // User list
                case UserListRequest userListRequest:
                    var userListRequestHandler = new UserListRequestHandler(_networkCommunication);
                    await userListRequestHandler.SendMessageAsync(userListRequest);
                    break;
                case UserListResponse userListResponse:
                    var userListResponseHandler = new UserListResponseHandler(
                        _networkCommunication
                    );
                    await userListResponseHandler.SendMessageAsync(userListResponse);
                    break;
                
                default:
                    // TODO Create a custom exception
                    throw new Exception($"Message not recognized ID={msg.Id}, type={msg.Type}");
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
                
                // Photo list
                case (MessageId.PhotoList, MessageType.Request):
                    var photoListRequestHandler = new PhotoListRequestHandler(_networkCommunication);
                    return await photoListRequestHandler.ReceiveMessageAsync();
                case (MessageId.PhotoList, MessageType.Response):
                    var photoListResponseHandler = new PhotoListResponseHandler(
                        _networkCommunication, 
                        _fileCommunication
                    );
                    return await photoListResponseHandler.ReceiveMessageAsync();
                
                //User list
                case (MessageId.UserList, MessageType.Request):
                    var userListRequestHandler = new UserListRequestHandler(_networkCommunication);
                    return await userListRequestHandler.ReceiveMessageAsync();
                case (MessageId.UserList, MessageType.Response):
                    var userListResponseHandler = new UserListResponseHandler(
                        _networkCommunication
                    );
                    return await userListResponseHandler.ReceiveMessageAsync();
            }

            // TODO Create a custom exception
            throw new Exception($"Message not recognized ID={messageId}, type={messageType}");
        }
    }
}