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

        public async Task<IEnumerable<CommentDto>> GetCommentByNamePhotoAsync(string username, string photoName)
        {
            _dbConnection.Open();
            var commentDtoList = await _dbConnection.QueryAsync<CommentDto>(
                sql: "SELECT * FROM Comments WHERE Username = @Username AND PhotoName = @PhotoName",
                param: new
                {
                    Username = username,
                    PhotoName = photoName
                }
            );
            _dbConnection.Close();
            return commentDtoList;
        }

        public async Task<CommentDto> SaveCommentAsync(CommentDto commentDto)
        {
            _dbConnection.Open();
            var id = await _dbConnection.InsertAsync(commentDto);
            // For some stupid reason the GetAsync does not work
            var responseCommentDto = await _dbConnection.QuerySingleAsync<CommentDto>(
                "SELECT * FROM Comments WHERE Id = @Id",
                param: new {Id = id}
            );
            _dbConnection.Close();
            return responseCommentDto;
        }
    }
}