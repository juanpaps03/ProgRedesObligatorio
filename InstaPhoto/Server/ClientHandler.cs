using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using Domain;
using Exceptions;
using Repositories;
using Services;
using Services.Interfaces;
using SocketLibrary;
using SocketLibrary.Constants;
using SocketLibrary.Interfaces;
using SocketLibrary.Messages;
using SocketLibrary.Messages.CommentList;
using SocketLibrary.Messages.CreatePhoto;
using SocketLibrary.Messages.Error;
using SocketLibrary.Messages.Login;
using SocketLibrary.Messages.PhotoList;
using SocketLibrary.Messages.UserList;
using SocketLibrary.Messages.CommentPhoto;
using SocketLibrary.Messages.CommentList;

namespace Server
{
    public class ClientHandler
    {
        private string _clientUsername;

        private readonly IProtocolCommunication _protocolCommunication;

        private readonly IPhotoService _photoService;
        private readonly IUserService _userService;
        private readonly ICommentService _commentService;

        public ClientHandler(NetworkStream stream, IDbConnection dbConnection)
        {
            var photoRepository = new PhotoRepository(dbConnection);
            _photoService = new PhotoService(photoRepository);
            var userRepository = new UserRepository(dbConnection);
            _userService = new UserService(userRepository);
            var commentRepository = new CommentRepository(dbConnection);
            _commentService = new CommentService(commentRepository);

            _protocolCommunication = new ProtocolCommunication(stream);
        }

        public async Task ExecuteAsync()
        {
            // while (_clientUsername == null)
            //     await _protocolCommunication.HandleRequestAsync(HandleLoginAsync);

            // >>>>>>> Para testear la subida de fotos, comentar el login y descomentar la linea de abajo
            _clientUsername = "admin";

            while (true) // TODO: CHANGE TO LOGOUT CONDITION
                await _protocolCommunication.HandleRequestAsync(HandleRequestAsync);
        }

        private async Task<Response> HandleRequestAsync(Request request)
        {
            return request switch
            {
                CreatePhotoRequest createPhotoRequest => await HandleCreatePhotoAsync(createPhotoRequest),
                PhotoListRequest photoListRequest => await HandlePhotoListAsync(photoListRequest),
                UserListRequest userListRequest => await HandleUserListAsync(),
                CommentPhotoRequest commentPhotoRequest => await HandleCommentPhotoAsync(commentPhotoRequest),
                CommentListRequest commentListRequest => await HandleCommentListAsync(commentListRequest),
                _ => new ErrorResponse(ErrorId.UnrecognizedCommand, $"Unrecognized command Id={request.Id}")
            };
        }

        private async Task<Response> HandleUserListAsync()
        {
            var users = new List<User>(await _userService.GetUsersAsync());

            return new UserListResponse(users);
        }
        
        private async Task<Response> HandlePhotoListAsync(PhotoListRequest photoListRequest)
        {
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

        private async Task<Response> HandleLoginAsync(Request request)
        {
            // TODO: IMPLEMENT LOGIN
            _clientUsername = "admin";
            return new LoginResponse();
        }
        
        private async Task<Response> HandleCommentPhotoAsync(CommentPhotoRequest commentPhotoRequest)
        {
            try
            {
                var user = await _userService.GetUserByUserNameAsync(commentPhotoRequest.UserName);
                if (user == null)
                    return new ErrorResponse(
                        ErrorId.UserNotFound,
                        $"The user {commentPhotoRequest.UserName} doesn't exist"
                    );
                var photo = await _photoService.GetPhotoByNamePhotoAsync(commentPhotoRequest.NamePhoto);
                if (photo == null)
                    return new ErrorResponse(
                        ErrorId.PhotoNotExist,
                        $"The photo {commentPhotoRequest.NamePhoto} doesn't exist"
                    );
                
                // Guardo comentario
                await _commentService.SaveCommentAsync(new Comment
                {
                    NamePhoto = commentPhotoRequest.NamePhoto,
                    UserName = commentPhotoRequest.UserName,
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
            var photo = await _photoService.GetPhotoByNamePhotoAsync(commentListRequest.Namephoto);
            if (photo == null)
                return new ErrorResponse(
                    ErrorId.PhotoNotExist,
                    $"The photo {commentListRequest.Namephoto} doesn't exist"
                );

            var comments = new List<Comment>(await _commentService.GetCommentsByNamePhotoAsync(commentListRequest.Namephoto));

            return new CommentListResponse(commentListRequest.Namephoto, comments);
        }
    }
}