using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Exceptions;
using Repositories.Dtos;
using Repositories.Interfaces;

namespace Repositories
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly IDbConnection _dbConnection;

        public PhotoRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<PhotoDto> SavePhotoDtoAsync(PhotoDto photoDto)
        {
            _dbConnection.Open();
            try
            {
                await _dbConnection.InsertAsync(photoDto);
                return photoDto;
            }
            catch (Exception e) when (e.Message.Contains("UNIQUE constraint failed"))
            {
                throw new PhotoAlreadyExists($"Photo with the name \"{photoDto.Name}\" already uploaded");
            }
            catch (Exception e) when (e.Message.Contains("FOREIGN KEY constraint failed"))
            {
                throw new UserNotFound();
            }
            finally
            {
                _dbConnection.Close();
            }
        }

        public async Task<IEnumerable<PhotoDto>> GetPhotosFromUserAsync(string username)
        {
            _dbConnection.Open();
            var photoDtoList = await _dbConnection.QueryAsync<PhotoDto>(
                sql: "SELECT * FROM Photos WHERE Username = @Username",
                param: new {Username = username}
            );
            _dbConnection.Close();

            return photoDtoList;
        }

        public async Task<PhotoDto> GetPhotoByNamePhotoAsync(string namePhoto)
        {
            _dbConnection.Open();
            var photoDto = await _dbConnection.GetAsync<PhotoDto>(namePhoto);
            _dbConnection.Close();
            return photoDto;
        }
    }
}