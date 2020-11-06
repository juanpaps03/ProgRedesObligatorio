using System.Threading.Tasks;

namespace Client.Interfaces
{
    public interface IPage
    {
        /**
         * Shows the current page and returns the next page, or null for a graceful exit.
         */
        Task RenderAsync();
    }
}