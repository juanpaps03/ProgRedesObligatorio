using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Dtos;

namespace Repositories.Interfaces
{
    public interface IPhotoRepository
    {
        Task<PhotoDto> SavePhotoDtoAsync(PhotoDto photoDto);
        Task<IEnumerable<PhotoDto>> GetPhotosFromUserAsync(string username);
        Task<PhotoDto> GetPhotoByNamePhotoAsync(string namePhoto);
    }
}