using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using SocketLibrary.Interfaces;

namespace SocketLibrary
{
    public class FileCommunication: IFileCommunication
    {
        private readonly INetworkCommunication _networkCommunication;

        public FileCommunication(NetworkStream stream)
        {
            _networkCommunication = new NetworkCommunication(stream); // TODO: DEPENDENCY INJECTION
        }

        public async Task SendFileAsync(string path)
        {
            if (!File.Exists(path))
                throw new Exception("File not found"); // TODO: CHANGE FOR CUSTOM IMPLEMENTATION
            
            // Send file length
            var fileInfo = new FileInfo(path);
            await _networkCommunication.SendLongAsync(fileInfo.Length);
            
            // Send file chunks
            await using var fileStream = File.OpenRead(path);
            
            var sent = 0;
            var buffer = new byte[ProtocolSpecification.FileChunkSize];
            while (fileInfo.Length - sent > ProtocolSpecification.FileChunkSize)
            {
                await fileStream.ReadAsync(buffer, 0, ProtocolSpecification.FileChunkSize);
                await _networkCommunication.SendBytesAsync(buffer);
                sent += ProtocolSpecification.FileChunkSize;
            }
            
            buffer = new byte[fileInfo.Length - sent];
            await fileStream.ReadAsync(buffer, 0, (int) fileInfo.Length - sent);
            await _networkCommunication.SendBytesAsync(buffer);
        }

        public async Task<string> ReceiveFileAsync()
        {
            // Receive file length
            var fileSize = await _networkCommunication.ReceiveLongAsync();
            
            // Receive file chunks
            var path = Path.GetTempFileName();
            await using var fileStream = File.OpenRead(path);
            
            var read = 0;
            byte[] buffer = null;
            while (fileSize - read > ProtocolSpecification.FileChunkSize)
            {
                buffer = await _networkCommunication.ReceiveBytesAsync(ProtocolSpecification.FileChunkSize);
                await fileStream.WriteAsync(buffer, 0, ProtocolSpecification.FileChunkSize);
                read += ProtocolSpecification.FileChunkSize;
            }
            
            buffer = await _networkCommunication.ReceiveBytesAsync((int) (fileSize - read));
            await fileStream.WriteAsync(buffer, 0, (int) (fileSize - read));

            return path;
        }
    }
}