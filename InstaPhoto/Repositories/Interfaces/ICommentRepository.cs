using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Dtos;

namespace Repositories.Interfaces
{
    public interface ICommentRepository
    {
        Task<IEnumerable<CommentDto>> GetCommentsAsync();
        Task<CommentDto> GetCommentByPhotoNameAsync(string namePhoto);
        Task<CommentDto> SaveCommentAsync(CommentDto commentDto);

    }
}