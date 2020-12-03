using System;

namespace Exceptions
{
    public class ConnectionLost : Exception
    {
        public ConnectionLost() : base("Connection lost")
        {
        }
    }
}