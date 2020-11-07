namespace SocketLibrary.Messages
{
    public class Error: Response
    {
        private int _errorId;
        public string Message { get; }

        public Error(int errorId, string message)
        {
            _errorId = errorId;
            Message = message;
        }
    }
}