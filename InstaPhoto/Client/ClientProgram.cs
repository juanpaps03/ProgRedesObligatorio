using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using AppSettings;
using Client.Classes;
using Client.Interfaces;
using Microsoft.Extensions.Configuration;
using SocketLibrary;
using SocketLibrary.Constants;
using SocketLibrary.Messages.Logout;

namespace Client
{
    class ClientInstaPhoto
    {
        static async Task Main(string[] args)
        {
            IConfiguration configuration = AppSettingsFactory.GetAppSettings();
            
            ConsoleHelper.WriteLine("Starting client...", ConsoleColor.Yellow);
            var clientIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 0);
            var serverIpEndPoint = new IPEndPoint(IPAddress.Parse(configuration["ServerIpAddress"]), ProtocolSpecification.Port);
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

            await protocolCommunication.SendRequestAsync(new LogoutRequest());
        }
    }
}