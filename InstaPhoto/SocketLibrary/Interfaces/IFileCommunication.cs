using System.Threading.Tasks;

namespace SocketLibrary.Interfaces
{
    public interface IFileCommunication
    {
        Task SendFileAsync(string path);
        Task<string> ReceiveFileAsync();
    }
}