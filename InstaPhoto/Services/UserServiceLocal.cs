using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Repositories.Dtos;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            IEnumerable<UserDto> usersDto = await _userRepository.GetUsersAsync();
            return usersDto.Select(userDto => MapUserDtoToDomain(userDto)).ToList();
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            UserDto userDto = await _userRepository.GetUsersByUserNameAsync(userName);
            if (userDto != null)
            {
                return MapUserDtoToDomain(userDto);
            }

            return null;
        }

        public async Task<User> SaveUserAsync(User user)
        {
            UserDto userDto = MapUserDomainToDto(user);
            var responseUserDto = await _userRepository.SaveUserAsync(userDto);
            return MapUserDtoToDomain(responseUserDto);
        }

        private UserDto MapUserDomainToDto(User user)
        {
            return new UserDto
            {
                Username = user.Username,
                Password = user.Password,
                Admin = user.Admin
            };
        }

        private User MapUserDtoToDomain(UserDto userDto)
        {
            return new User
            {
                Username = userDto.Username,
                Password = userDto.Password,
                Admin = userDto.Admin
            };
        }
    }
}