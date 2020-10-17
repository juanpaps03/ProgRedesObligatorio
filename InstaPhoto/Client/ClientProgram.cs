using System.Threading.Tasks;
using Client.Classes;
using Client.Classes.Pages;
using Client.Interfaces;

namespace Client
{
    class ClientInstaPhoto
    {
        static async Task Main(string[] args)
        {
            var console = new ConsoleHelper();
            IPage nextPage = new LandingPage(console);

            while (nextPage != null)
            {
                console.Clear();
                nextPage = await nextPage.RenderAsync();
            }
        }
    }
}