using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using Grpc.Core;
using GrpcServer;
using Services.Interfaces;

namespace Services
{
    public class PhotoServiceRemote : IPhotoService
    {
        private readonly Photos.PhotosClient _client;
        private readonly IMapper _mapper;

        public PhotoServiceRemote(ChannelBase channel)
        {
            _client = new Photos.PhotosClient(channel);

            var config = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<PhotoMessage, Photo>();
                    cfg.CreateMap<Photo, PhotoMessage>();
                });
            _mapper = config.CreateMapper();
        }

        public async Task<Photo> SavePhotoAsync(Photo photo)
        {
            var request = new SavePhotoRequest
            {
                Photo = _mapper.Map<PhotoMessage>(photo)
            };
            var reply = await _client.SavePhotoAsync(request);
            return _mapper.Map<Photo>(reply.Photo);
        }

        public async Task<IEnumerable<Photo>> GetPhotosFromUserAsync(string username)
        {
            var request = new GetPhotosFromUserRequest {Username = username};
            var reply = await _client.GetPhotosFromUserAsync(request);
            return _mapper.Map<IEnumerable<Photo>>(reply.PhotoList);
        }
    }
}