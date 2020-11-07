using System;
using System.IO;
using SocketLibrary.Constants;

namespace SocketLibrary.Messages.CreatePhoto
{
    public class CreatePhotoRequest : Request
    {
        public string Name { get; }
        public string FilePath { get; }

        public CreatePhotoRequest(string name, string filePath) : base(MessageId.CreatePhoto)
        {
            Name = name;
            
            if (!File.Exists(filePath))
                throw new Exception("File does not exist"); // TODO: CREATE CUSTOM EXCEPTION
            
            FilePath = filePath;
        }
    }
}