using  System;

namespace DataBase
{
    public class DBException
    {
        public class DBException : Exception
        {
            public DBException(string message) : base(message) { }
            public DBException(string message, Exception ex) : base(message, ex) { }
        }
    }
}