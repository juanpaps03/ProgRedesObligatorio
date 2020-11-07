using System.Threading.Tasks;
using Client.Classes;
using Client.Interfaces;

namespace Client
{
    class ClientInstaPhoto
    {
        static async Task Main(string[] args)
        {
            IPageNavigation navigation = new PageNavigation();
            navigation.GoToPage(IPageNavigation.LandingPage);

            while (navigation.Top() != null)
            {
                ConsoleHelper.Clear();
                await navigation.Top().RenderAsync();
            }
        }
    }
}