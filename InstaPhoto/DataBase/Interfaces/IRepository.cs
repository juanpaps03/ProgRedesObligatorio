using System.Collections.Generic;
using System.Threading.Tasks;
using DataBase.Dtos;

namespace DataBase.Interfaces
{
    public interface IRepository
    {
        Task<IEnumerable<UserDto>> GetUsersAsync();
        Task<UserDto> GetUsersByUserNameAsync(string userName);
        Task<UserDto> SaveUserAsync(UserDto userDto);

    }
}