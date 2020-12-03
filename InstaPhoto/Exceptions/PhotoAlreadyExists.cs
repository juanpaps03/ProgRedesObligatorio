using System;

namespace Exceptions
{
    public class PhotoAlreadyExists: Exception
    {
        public PhotoAlreadyExists(string message): base(message)
        {
        }
    }
}