using System;

namespace Exceptions
{
    public class UserNotFound: Exception
    {
        public UserNotFound(): base("Usuario inexistente")
        {
        }
    }
}