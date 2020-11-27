using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using Repositories.Dtos;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services
{
    public class PhotoServiceLocal : IPhotoService
    {
        private readonly IPhotoRepository _photoRepository;
        private readonly IMapper _mapper;

        public PhotoServiceLocal(IPhotoRepository photoRepository)
        {
            _photoRepository = photoRepository;
            
            var config = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<Photo, PhotoDto>();
                    cfg.CreateMap<PhotoDto, Photo>();
                });
            _mapper = config.CreateMapper();
        }

        public async Task<Photo> SavePhotoAsync(Photo photo)
        {
            var photoDto = MapModelToDto(photo);
            var resultDto = await _photoRepository.SavePhotoDtoAsync(photoDto);
            return MapDtoToModel(resultDto);
        }

        public async Task<IEnumerable<Photo>> GetPhotosFromUserAsync(string username)
        {
            var photoDtoList = await _photoRepository.GetPhotosFromUserAsync(username);
            return photoDtoList.Select(MapDtoToModel);
        }

        private Photo MapDtoToModel(PhotoDto resultDto)
        {
            return _mapper.Map<Photo>(resultDto);
        }

        private PhotoDto MapModelToDto(Photo photo)
        {
            return _mapper.Map<PhotoDto>(photo);
        }
    }
}