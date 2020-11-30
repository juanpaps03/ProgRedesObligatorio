using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace Services.Interfaces
{
    public interface IPhotoService
    {
        Task<Photo> SavePhotoAsync(Photo photo);
        Task<IEnumerable<Photo>> GetPhotosFromUserAsync(string username);
        Task<Photo> GetPhotoByPhotoNameAsync(string username, string photoName);
    }
}