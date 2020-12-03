using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using Domain;
using Exceptions;
using Grpc.Core;
using LoggerLibrary;
using Services;
using Services.Interfaces;
using SocketLibrary;
using SocketLibrary.Constants;
using SocketLibrary.Interfaces;
using SocketLibrary.Messages;
using SocketLibrary.Messages.CommentList;
using SocketLibrary.Messages.CommentPhoto;
using SocketLibrary.Messages.CreatePhoto;
using SocketLibrary.Messages.CreateUser;
using SocketLibrary.Messages.Error;
using SocketLibrary.Messages.Login;
using SocketLibrary.Messages.Logout;
using SocketLibrary.Messages.PhotoList;
using SocketLibrary.Messages.UserList;

namespace Server
{
    public class ClientHandler
    {
        public Guid Id;

        private readonly NetworkStream _networkStream;
        private string _clientUsername;
        private bool _exit;

        private readonly IProtocolCommunication _protocolCommunication;

        private readonly IPhotoService _photoService;
        private readonly IUserService _userService;
        private readonly ICommentService _commentService;
        private readonly ILogger _logger;

        public ClientHandler(NetworkStream stream, ChannelBase channel, ILogger logger)
        {
            _photoService = new PhotoServiceRemote(channel);
            _userService = new UserServiceRemote(channel);
            _commentService = new CommentServiceRemote(channel);

            _protocolCommunication = new ProtocolCommunication(stream);
            _networkStream = stream;
            _logger = logger;
        }

        ~ClientHandler()
        {
            _networkStream.Dispose();
        }

        public string GetClientName()
        {
            return _clientUsername ?? "<Anonymous>";
        }

        public async Task ExecuteAsync()
        {
            while (!_exit)
            {
                try
                {
                    await _protocolCommunication.HandleRequestAsync(HandleRequestAsync);
                }
                catch (ConnectionLost)
                {
                    _exit = true;
                }
            }
        }

        private async Task<Response> HandleRequestAsync(Request request)
        {
            return request switch
            {
                LoginRequest loginRequest => await HandleLoginAsync(loginRequest),
                CreatePhotoRequest createPhotoRequest => await HandleCreatePhotoAsync(createPhotoRequest),
                PhotoListRequest photoListRequest => await HandlePhotoListAsync(photoListRequest),
                CreateUserRequest createUserRequest => await HandleCreateUserAsync(createUserRequest),
                UserListRequest _ => await HandleUserListAsync(),
                CommentPhotoRequest commentPhotoRequest => await HandleCommentPhotoAsync(commentPhotoRequest),
                CommentListRequest commentListRequest => await HandleCommentListAsync(commentListRequest),
                LogoutRequest _ => await HandleLogoutAsync(),
                _ => new ErrorResponse(ErrorId.UnrecognizedCommand, $"Unrecognized command Id={request.Id}")
            };
        }

        private async Task<Response> HandleLogoutAsync()
        {
            _logger.SaveLog($"[id={Id.ToString()}][username={_clientUsername}] New user logout request");

            _exit = true;
            return new LogoutResponse();
        }

        private async Task<Response> HandleUserListAsync()
        {
            _logger.SaveLog($"[id={Id.ToString()}][username={_clientUsername}] New list users request");

            var users = new List<User>(await _userService.GetUsersAsync());

            return new UserListResponse(users);
        }

        private async Task<Response> HandlePhotoListAsync(PhotoListRequest photoListRequest)
        {
            _logger.SaveLog(
                $"[id={Id.ToString()}]" +
                $"[username={_clientUsername}] New list photo request of user {photoListRequest.Username}"
            );

            var user = await _userService.GetUserByUserNameAsync(photoListRequest.Username);
            if (user == null)
                return new ErrorResponse(
                    ErrorId.UserNotFound,
                    $"The user {photoListRequest.Username} doesn't exist"
                );

            var photos = new List<Photo>(await _photoService.GetPhotosFromUserAsync(photoListRequest.Username));

            return new PhotoListResponse(photoListRequest.Username, photos);
        }

        private async Task<Response> HandleCreatePhotoAsync(CreatePhotoRequest createPhotoRequest)
        {
            _logger.SaveLog(
                $"[id={Id.ToString()}][username={_clientUsername}] New photo upload: {createPhotoRequest.Name}"
            );

            try
            {
                var photoPath = $"photos/{_clientUsername}/{createPhotoRequest.Name}";

                // Persist photo
                await _photoService.SavePhotoAsync(new Photo
                {
                    Name = createPhotoRequest.Name,
                    File = photoPath,
                    Username = _clientUsername
                });

                // Persist file
                Directory.CreateDirectory(Path.GetDirectoryName(photoPath));
                if (File.Exists(photoPath))
                    File.Delete(photoPath); // Trust only in the DB
                File.Move(createPhotoRequest.FilePath, photoPath);

                return new CreatePhotoResponse();
            }
            catch (PhotoAlreadyExists e)
            {
                return new ErrorResponse(ErrorId.PhotoNameAlreadyExists, e.Message);
            }
        }

        private async Task<Response> HandleCreateUserAsync(CreateUserRequest createUserRequest)
        {
            _logger.SaveLog($"[id={Id.ToString()}] New create user request, username: {createUserRequest.UserName}");

            var user = new User()
            {
                Username = createUserRequest.UserName,
                Password = createUserRequest.Password,
                Admin = false
            };

            try
            {
                var createdUser = await _userService.SaveUserAsync(user);
                _clientUsername = createdUser.Username;
                return new CreateUserResponse();
            }
            catch (DatabaseSaveError)
            {
                return new ErrorResponse(ErrorId.UserAlreadyExists, "User already exists");
            }
        }

        private async Task<Response> HandleLoginAsync(LoginRequest request)
        {
            _logger.SaveLog($"[id={Id.ToString()}] New Login request, username: {request.UserName}");

            User user = await _userService.GetUserByUserNameAsync(request.UserName);
            if (user is null)
                return new ErrorResponse(ErrorId.InvalidCredentials, "Invalid Credentials");
            if (request.Password != user.Password)
                return new ErrorResponse(ErrorId.InvalidCredentials, "Invalid Credentials");

            _clientUsername = user.Username;
            return new LoginResponse();
        }

        private async Task<Response> HandleCommentPhotoAsync(CommentPhotoRequest commentPhotoRequest)
        {
            _logger.SaveLog(
                $"[id={Id.ToString()}]" +
                $"[username={_clientUsername}] New comment photo request " +
                $"for {commentPhotoRequest.NamePhoto} " +
                $"from user {commentPhotoRequest.UserName}, " +
                $"comment: {commentPhotoRequest.Text}"
            );

            try
            {
                var user = await _userService.GetUserByUserNameAsync(commentPhotoRequest.UserName);
                if (user == null)
                    return new ErrorResponse(
                        ErrorId.UserNotFound,
                        $"The user {commentPhotoRequest.UserName} doesn't exist"
                    );
                var photo = await _photoService.GetPhotoByPhotoNameAsync(
                    commentPhotoRequest.UserName,
                    commentPhotoRequest.NamePhoto
                );
                if (photo == null)
                    return new ErrorResponse(
                        ErrorId.PhotoNotExist,
                        $"The photo {commentPhotoRequest.NamePhoto} doesn't exist"
                    );

                // Guardo comentario
                await _commentService.SaveCommentAsync(new Comment
                {
                    PhotoName = commentPhotoRequest.NamePhoto,
                    Username = commentPhotoRequest.UserName,
                    Text = commentPhotoRequest.Text
                });

                return new CommentPhotoResponse();
            }
            catch (UserNotFound e)
            {
                return new ErrorResponse(ErrorId.UserNotFound, e.Message);
            }
            catch (PhotoNotExist e)
            {
                return new ErrorResponse(ErrorId.PhotoNotExist, e.Message);
            }
        }

        private async Task<Response> HandleCommentListAsync(CommentListRequest commentListRequest)
        {
            _logger.SaveLog(
                $"[id={Id.ToString()}]" +
                $"[username={_clientUsername}] New list comment request " +
                $"for {commentListRequest.PhotoName} " +
                $"from user {commentListRequest.Username}"
            );

            var photo = await _photoService.GetPhotoByPhotoNameAsync(
                commentListRequest.Username,
                commentListRequest.PhotoName
            );
            if (photo == null)
                return new ErrorResponse(
                    ErrorId.PhotoNotExist,
                    $"The photo {commentListRequest.PhotoName} doesn't exist"
                );

            var comments = new List<Comment>(
                await _commentService.GetCommentsByNamePhotoAsync(
                    commentListRequest.Username,
                    commentListRequest.PhotoName
                )
            );

            return new CommentListResponse(
                commentListRequest.Username,
                commentListRequest.PhotoName,
                comments
            );
        }
    }
}