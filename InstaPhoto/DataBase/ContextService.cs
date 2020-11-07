using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using DataBase.Dtos;
using DataBase.Interfaces;
using Domain;

namespace DataBase
{
    public class ContextService : IContextService
    {
        private readonly IRepository _repository;

        public ContextService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            IEnumerable<UserDto> usersDto = await _repository.GetUsersAsync();
            return usersDto.Select(userDto => MapUserDtoToDomain(userDto)).ToList();
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            UserDto userDto = await _repository.GetUsersByUserNameAsync(userName);
            if (userDto != null)
            {
                return MapUserDtoToDomain(userDto);
            }

            return null;
        }

        public async Task<User> SaveUserAsync(User user)
        {
            UserDto userDto = MapUserDomainToDto(user);
            var responseUserDto = await _repository.SaveUserAsync(userDto);
            return MapUserDtoToDomain(responseUserDto);
        }

        private UserDto MapUserDomainToDto(User user)
        {
            return new UserDto
            {
                UserName = user.UserName,
                Password = user.Password,
                Admin = user.Admin
            };
        }

        private User MapUserDtoToDomain(UserDto userDto)
        {
            return new User
            {
                UserName = userDto.UserName,
                Password = userDto.Password,
                Admin = userDto.Admin
            };
        }
    }
}