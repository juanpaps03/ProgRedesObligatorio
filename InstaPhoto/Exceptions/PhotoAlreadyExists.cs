using System;

namespace Exceptions
{
    public class PhotoAlreadyExists: Exception
    {
        public PhotoAlreadyExists(): base("Nombre de imagen repetido")
        {
        }
    }
}