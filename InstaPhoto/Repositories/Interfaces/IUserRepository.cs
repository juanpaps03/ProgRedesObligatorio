using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Dtos;

namespace Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserDto>> GetUsersAsync();
        Task<UserDto> GetUsersByUserNameAsync(string userName);
        Task<UserDto> SaveUserAsync(UserDto userDto);
        Task<UserDto> UpdateUserAsync(UserDto userDto);
        Task DeleteUserAsync(UserDto userDto);

    }
}