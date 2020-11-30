using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Repositories.Dtos;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<IEnumerable<Comment>> GetCommentsAsync()
        {
            IEnumerable<CommentDto> commentsDto = await _commentRepository.GetCommentsAsync();
            return commentsDto.Select(commentDto => MapCommentDtoToDomain(commentDto)).ToList();
        }

        public async Task<IEnumerable<Comment>> GetCommentsByNamePhotoAsync(string username, string photoName)
        {
            var commentDtoList = await _commentRepository.GetCommentByNamePhotoAsync(
                username, photoName
            );
            return commentDtoList.Select(commentDto => MapCommentDtoToDomain(commentDto)).ToList();
        }

        public async Task<Comment> SaveCommentAsync(Comment comment)
        {
            CommentDto commentDto = MapCommentDomainToDto(comment);
            var responseCommentDto = await _commentRepository.SaveCommentAsync(commentDto);
            return MapCommentDtoToDomain(responseCommentDto);
        }

        private CommentDto MapCommentDomainToDto(Comment comment)
        {
            return new CommentDto
            {
                PhotoName = comment.PhotoName,
                Username = comment.Username,
                Text = comment.Text
            };
        }

        private Comment MapCommentDtoToDomain(CommentDto commentDto)
        {
            return new Comment
            {
                PhotoName = commentDto.PhotoName,
                Username = commentDto.Username,
                Text = commentDto.Text
            };
        }
    }
}