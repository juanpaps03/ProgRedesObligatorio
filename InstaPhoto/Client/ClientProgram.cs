using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Client.Classes;
using Client.Interfaces;
using SocketLibrary;
using SocketLibrary.Constants;

namespace Client
{
    class ClientInstaPhoto
    {
        static async Task Main(string[] args)
        {
            ConsoleHelper.WriteLine("Starting client...", ConsoleColor.Yellow);
            var clientIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 0);
            var serverIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), ProtocolSpecification.Port);
            TcpClient client = new TcpClient(clientIpEndPoint);
            
            ConsoleHelper.WriteLine("Attempting connection to server...", ConsoleColor.Yellow);
            client.Connect(serverIpEndPoint);
            NetworkStream stream = client.GetStream();
            var protocolCommunication = new ProtocolCommunication(stream);
            
            IPageNavigation navigation = new PageNavigation(protocolCommunication);
            navigation.GoToPage(IPageNavigation.LandingPage);

            while (navigation.Top() != null)
            {
                ConsoleHelper.Clear();
                await navigation.Top().RenderAsync();
            }
        }
    }
}