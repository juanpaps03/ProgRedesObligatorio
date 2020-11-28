using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Dtos;

namespace Repositories.Interfaces
{
    public interface ICommentRepository
    {
        Task<IEnumerable<CommentDto>> GetCommentsAsync();
        Task<IEnumerable<CommentDto>> GetCommentByNamePhotoAsync(string username, string photoName);
        Task<CommentDto> SaveCommentAsync(CommentDto commentDto);

    }
}