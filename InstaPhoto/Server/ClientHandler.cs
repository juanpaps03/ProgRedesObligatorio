using System;
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
using SocketLibrary.Messages.CreatePhoto;
using SocketLibrary.Messages.Error;
using SocketLibrary.Messages.Login;

namespace Server
{
    public class ClientHandler
    {
        private string _clientUsername;
        
        private readonly IProtocolCommunication _protocolCommunication;

        private readonly IPhotoService _photoService;

        public ClientHandler(NetworkStream stream, IDbConnection dbConnection)
        {
            var photoRepository = new PhotoRepository(dbConnection);
            _photoService = new PhotoService(photoRepository);
            
            _protocolCommunication = new ProtocolCommunication(stream);
        }

        public async Task ExecuteAsync()
        {
            
            while (_clientUsername == null)
                await _protocolCommunication.HandleRequestAsync(HandleLoginAsync);

            // >>>>>>> Para testear la subida de fotos, comentar el login y descomentar la linea de abajo
            // _clientUsername = "admin";

            while (true)  // TODO: CHANGE TO LOGOUT CONDITION
                await _protocolCommunication.HandleRequestAsync(HandleRequestAsync);
        }

        private async Task<Response> HandleRequestAsync(Request request)
        {
            return request switch
            {
                CreatePhotoRequest createPhotoRequest => await HandleCreatePhotoAsync(createPhotoRequest),
                _ => new ErrorResponse(ErrorId.UnrecognizedCommand, $"Unrecognized command Id={request.Id}")
            };
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
    }
}