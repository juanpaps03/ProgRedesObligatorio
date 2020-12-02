using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Helpers;
using Repositories.Dtos;
using Repositories.Interfaces;
using Services.Interfaces;
using Domain.Responses;

namespace Services
{
    public class UserServiceLocal : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserServiceLocal(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            IEnumerable<UserDto> usersDto = await _userRepository.GetUsersAsync();
            return usersDto.Select(userDto => MapUserDtoToDomain(userDto)).ToList();
        }

        public async Task<PaginatedResponse<User>> GetUsersAsync(int page, int pageSize)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return null;
            }

            int totalUsers = await _userRepository.GetTotalUsers();
            if (totalUsers > 0)
            {
                IEnumerable<UserDto> repoUsers = await _userRepository.GetUsersAsync(page, pageSize);
                if (repoUsers == null || !repoUsers.Any())
                {
                    return null;
                }

                var users = new List<User>();
                foreach (var userDto in repoUsers)
                {
                    users.Add(MapUserDtoToDomain(userDto));
                }

                return PaginationHelper<User>.GeneratePaginatedResponse(pageSize, totalUsers, users);
            }

            return null;
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
            try
            {
                UserDto userDto = MapUserDomainToDto(user);
                var responseUserDto = await _userRepository.SaveUserAsync(userDto);
                return MapUserDtoToDomain(responseUserDto);
            }
            catch
            {
                throw new DatabaseSaveError();
            }
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            try
            {
                UserDto userDto = MapUserDomainToDto(user);
                var responseUserDto = await _userRepository.UpdateUserAsync(userDto);
                return MapUserDtoToDomain(responseUserDto);
            }
            catch
            {
                throw new DatabaseSaveError();
            }
        }
        
        public async Task DeleteUserAsync(User user)
        {
            UserDto userDto = MapUserDomainToDto(user);
            await _userRepository.DeleteUserAsync(userDto);
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