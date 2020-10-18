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
            IPageCreator pageCreator = new PageCreator();
            IPage nextPage = pageCreator.CreatePage(IPageCreator.PageId.LandingPage);

            while (nextPage != null)
            {
                ConsoleHelper.Clear();
                nextPage = await nextPage.RenderAsync();
            }
        }
    }
}