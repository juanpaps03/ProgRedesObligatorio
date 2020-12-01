using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserByUserNameAsync(string userName);
        Task<User> SaveUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
    }
}