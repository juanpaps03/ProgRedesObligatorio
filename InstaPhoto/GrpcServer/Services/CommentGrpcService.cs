using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Services.Interfaces;

namespace GrpcServer.Services
{
    public class CommentGrpcService : Comments.CommentsBase
    {
        private readonly ILogger<CommentGrpcService> _logger;

        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;

        public CommentGrpcService(ILogger<CommentGrpcService> logger, ICommentService commentService)
        {
            _logger = logger;
            _commentService = commentService;

            var config = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<CommentMessage, Comment>();
                    cfg.CreateMap<Comment, CommentMessage>();
                }
            );
            _mapper = config.CreateMapper();
        }

        public override async Task<GetCommentsReply> GetComments(GetCommentsRequest request, ServerCallContext context)
        {
            var commentList = await _commentService.GetCommentsAsync();
            return new GetCommentsReply
            {
                CommentList = {_mapper.Map<IEnumerable<CommentMessage>>(commentList)}
            };
        }

        public override async Task<GetCommentsByPhotoNameReply> GetCommentsByPhotoName(
            GetCommentsByPhotoNameRequest request,
            ServerCallContext context
        )
        {
            var commentList = await _commentService.GetCommentsByNamePhotoAsync(
                request.Username,
                request.PhotoName
            );
            return new GetCommentsByPhotoNameReply
            {
                CommentList = {_mapper.Map<IEnumerable<CommentMessage>>(commentList)}
            };
        }

        public override async Task<SaveCommentReply> SaveComment(SaveCommentRequest request, ServerCallContext context)
        {
            var comment = _mapper.Map<Comment>(request.Comment);
            var responseComment = await _commentService.SaveCommentAsync(comment);
            return new SaveCommentReply
            {
                Comment = _mapper.Map<CommentMessage>(responseComment)
            };
        }
    }
}