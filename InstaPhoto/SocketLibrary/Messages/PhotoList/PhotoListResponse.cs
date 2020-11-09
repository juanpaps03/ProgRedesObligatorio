using System.Collections.Generic;
using Domain;
using SocketLibrary.Constants;

namespace SocketLibrary.Messages.PhotoList
{
    public class PhotoListResponse: Response
    {
        public string Username { get; }
        public ICollection<Photo> Photos { get; }
        
        public PhotoListResponse(string username, ICollection<Photo> photos) : base(MessageId.PhotoList)
        {
            Photos = photos;
            Username = username;
        }
    }
}