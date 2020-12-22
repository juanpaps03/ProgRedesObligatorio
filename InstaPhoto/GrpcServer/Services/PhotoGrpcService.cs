using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Services.Interfaces;

namespace GrpcServer.Services
{
    public class PhotoGrpcService : Photos.PhotosBase
    {
        private readonly ILogger<PhotoGrpcService> _logger;

        private readonly IPhotoService _photoService;
        private readonly IMapper _mapper;

        public PhotoGrpcService(ILogger<PhotoGrpcService> logger, IPhotoService photoService)
        {
            _logger = logger;
            _photoService = photoService;

            var config = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<PhotoMessage, Photo>();
                    cfg.CreateMap<Photo, PhotoMessage>();
                });
            _mapper = config.CreateMapper();
        }

        public override async Task<SavePhotoReply> SavePhoto(SavePhotoRequest request, ServerCallContext context)
        {
            var photo = _mapper.Map<Photo>(request.Photo);

            try
            {
                var responsePhoto = await _photoService.SavePhotoAsync(photo);
                return new SavePhotoReply
                {
                    Photo = _mapper.Map<PhotoMessage>(responsePhoto)
                };
            }
            catch (Exception e)
            {
                return new SavePhotoReply
                {
                    Photo = null
                };
            }
        }

        public override async Task<GetPhotosFromUserReply> GetPhotosFromUser(
            GetPhotosFromUserRequest request,
            ServerCallContext context
        )
        {
            var photoList = await _photoService.GetPhotosFromUserAsync(request.Username);
            return new GetPhotosFromUserReply
            {
                PhotoList = {_mapper.Map<IEnumerable<PhotoMessage>>(photoList)}
            };
        }

        public override async Task<GetPhotoByNameReply> GetPhotoByName(
            GetPhotoByNameRequest request,
            ServerCallContext context
        )
        {
            var photo = await _photoService.GetPhotoByPhotoNameAsync(request.Username, request.PhotoName);
            return new GetPhotoByNameReply
            {
                Photo = _mapper.Map<PhotoMessage>(photo)
            };
        }
    }
}