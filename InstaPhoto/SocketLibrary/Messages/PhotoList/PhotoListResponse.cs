using System.Collections.Generic;
using Domain;
using SocketLibrary.Constants;

namespace SocketLibrary.Messages.PhotoList
{
    public class PhotoListResponse: Response
    {
        public ICollection<Photo> Photos { get; }
        
        public PhotoListResponse(ICollection<Photo> photos) : base(MessageId.PhotoList)
        {
            Photos = photos;
        }
    }
}