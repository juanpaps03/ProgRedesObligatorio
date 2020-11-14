using System;
using System.IO;
using SocketLibrary.Constants;

namespace SocketLibrary.Messages.CreatePhoto
{
    public class CommentPhotoRequest : Request
    {
        public string NamePhoto { get; }
        public string UserName { get; }
        public string Text { get; }

        public CommentPhotoRequest(string namePhoto, string userName, string text) : base(MessageId.CommentPhoto)
        {
            NamePhoto = namePhoto;
            UserName = userName;
            Text = text;
        }
    }
}