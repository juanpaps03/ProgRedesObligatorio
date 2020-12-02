using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using Grpc.Core;
using GrpcServer;
using Services.Interfaces;

namespace Services
{
    public class CommentServiceRemote : ICommentService
    {
        private readonly Comments.CommentsClient _client;
        private readonly IMapper _mapper;

        public CommentServiceRemote(ChannelBase channel)
        {
            _client = new Comments.CommentsClient(channel);

            var config = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<CommentMessage, Comment>();
                    cfg.CreateMap<Comment, CommentMessage>();
                });
            _mapper = config.CreateMapper();
        }

        public async Task<IEnumerable<Comment>> GetCommentsAsync()
        {
            var reply = await _client.GetCommentsAsync(new GetCommentsRequest());
            return _mapper.Map<IEnumerable<Comment>>(reply.CommentList);
        }

        public async Task<IEnumerable<Comment>> GetCommentsByNamePhotoAsync(string username, string photoName)
        {
            var reply = await _client.GetCommentsByPhotoNameAsync(
                new GetCommentsByPhotoNameRequest {Username = username, PhotoName = photoName}
            );
            return _mapper.Map<IEnumerable<Comment>>(reply.CommentList);
        }

        public async Task<Comment> SaveCommentAsync(Comment comment)
        {
            var request = new SaveCommentRequest
            {
                Comment = _mapper.Map<CommentMessage>(comment)
            };
            var reply = await _client.SaveCommentAsync(request);
            return _mapper.Map<Comment>(reply.Comment);
        }
    }
}