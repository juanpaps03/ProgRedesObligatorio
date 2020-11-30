using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace Services.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<Comment>> GetCommentsAsync();
        Task<IEnumerable<Comment>> GetCommentsByNamePhotoAsync(string username, string photoName);
        Task<Comment> SaveCommentAsync(Comment comment);
    }
}