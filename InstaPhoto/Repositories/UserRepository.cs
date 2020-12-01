using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using Repositories.Dtos;
using Repositories.Interfaces;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _dbConnection;
    
        public UserRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
    
        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            _dbConnection.Open();
            IEnumerable<UserDto> users = await _dbConnection.GetAllAsync<UserDto>();
            _dbConnection.Close();
            return users;
        }
    
        public async Task<UserDto> GetUsersByUserNameAsync(string userName)
        {
            _dbConnection.Open();
            UserDto userDto = await _dbConnection.GetAsync<UserDto>(userName);
            _dbConnection.Close();
            return userDto;
        }

        public async Task<UserDto> SaveUserAsync(UserDto userDto)
        {
            _dbConnection.Open();
            await _dbConnection.InsertAsync(userDto);
            var responseUserDto = await _dbConnection.GetAsync<UserDto>(userDto.Username);
            _dbConnection.Close();
            return responseUserDto;
        }

        public async Task<UserDto> UpdateUserAsync(UserDto userDto)
        {
            _dbConnection.Open();
            try
            {
                bool success = await _dbConnection.UpdateAsync(userDto);
                if (success)
                {
                    return await _dbConnection.GetAsync<UserDto>(userDto.Username);
                }
                return null;
            }
            finally
            {
                _dbConnection.Close();
            }
        }

    }
}