using System;

namespace Exceptions
{
    public class PhotoNotExist: Exception
    {
        public PhotoNotExist(): base("Foto inexistente")
        {
        }
    }
}