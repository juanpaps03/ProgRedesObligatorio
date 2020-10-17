using System.Threading.Tasks;
using Client.Classes;
using Client.Interfaces;

namespace Client
{
    class ClientInstaPhoto
    {
        static async Task Main(string[] args)
        {
            IPage nextPage = new LandingPage(new ConsoleHelper());

            while (nextPage != null)
            {
                nextPage = await nextPage.RenderAsync();
            }
        }
    }
}