using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Repositories.Dtos;
using Repositories.Interfaces;

namespace Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly IDbConnection _dbConnection;
    
        public CommentRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
    
        public async Task<IEnumerable<CommentDto>> GetCommentsAsync()
        {
            _dbConnection.Open();
            IEnumerable<CommentDto> comments = await _dbConnection.GetAllAsync<CommentDto>();
            _dbConnection.Close();
            return comments;
        }
    
        public async Task<IEnumerable<CommentDto>> GetCommentByNamePhotoAsync(string namePhoto)
        {
            _dbConnection.Open();
            var commentDtoList = await _dbConnection.QueryAsync<CommentDto>(
                sql: "SELECT * FROM Comment WHERE NamePhoto = @NamePhoto",
                param: new {NamePhoto = namePhoto});
            _dbConnection.Close();
            return commentDtoList;
        }

        public async Task<CommentDto> SaveCommentAsync(CommentDto commentDto)
        {
            _dbConnection.Open();
            int namePhoto = await _dbConnection.InsertAsync(commentDto);
            CommentDto responseCommentDto = await _dbConnection.GetAsync<CommentDto>(namePhoto);
            _dbConnection.Close();
            return responseCommentDto;
        }

    }
}