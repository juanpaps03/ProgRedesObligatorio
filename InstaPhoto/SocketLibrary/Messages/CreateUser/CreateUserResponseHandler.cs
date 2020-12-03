using System.Threading.Tasks;
using SocketLibrary.Interfaces;

namespace SocketLibrary.Messages.CreateUser
{
    public class CreateUserResponseHandler: IContentHandler<CreateUserResponse>
    {
        private INetworkCommunication _networkStream;

        public CreateUserResponseHandler(INetworkCommunication networkStream)
        {
            _networkStream = networkStream;
        }

        public async Task SendMessageAsync(CreateUserResponse loginResponse)
        {
        }

        public async Task<CreateUserResponse> ReceiveMessageAsync()
        {
            return new CreateUserResponse();
        }
    }
}