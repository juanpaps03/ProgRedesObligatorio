using System.Threading.Tasks;
using System.Collections.Generic;
using Domain;

namespace DataBase.Interfaces
{
    public interface IContextService
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserByUserNameAsync(string userName);
        Task<User> SaveUserAsync(User user);
    }
}