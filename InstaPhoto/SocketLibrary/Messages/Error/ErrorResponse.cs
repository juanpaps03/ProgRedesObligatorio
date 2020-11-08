using SocketLibrary.Constants;

namespace SocketLibrary.Messages.Error
{
    public class ErrorResponse : Response
    {
        public ErrorId ErrorId { get; }
        public string Message { get; }

        public ErrorResponse(ErrorId errorId, string message) : base(MessageId.Error)
        {
            ErrorId = errorId;
            Message = message;
        }
    }
}