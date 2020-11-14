using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace Services.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<Comment>> GetCommentsAsync();
        Task<Comment> GetCommentByPhotoNameAsync(string namePhoto);
        Task<Comment> SaveCommentAsync(Comment comment);
    }
}