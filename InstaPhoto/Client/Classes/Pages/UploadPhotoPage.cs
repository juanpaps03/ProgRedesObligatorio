using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Client.Interfaces;
using SocketLibrary.Interfaces;
using SocketLibrary.Messages;
using SocketLibrary.Messages.CreatePhoto;
using SocketLibrary.Messages.Error;

namespace Client.Classes.Pages
{
    public class UploadPhotoPage : IPage
    {
        private const string RetryAction = "retry";
        private const string GoBackAction = "goBack";

        private bool _retry;
        private string _path;
        private string _error;

        private readonly IPageNavigation _navigation;
        private readonly IProtocolCommunication _protocolCommunication;

        private readonly Menu _retryMenu;

        public UploadPhotoPage(
            IPageNavigation navigation,
            IProtocolCommunication protocolCommunication
        )
        {
            _navigation = navigation;
            _protocolCommunication = protocolCommunication;

            _retryMenu = new Menu(
                options: new List<(string, string)>
                {
                    (RetryAction, "Retry"),
                    (GoBackAction, "Back")
                },
                onSelect: RetryOptionSelected
            );
        }

        public async Task RenderAsync()
        {
            ConsoleHelper.WriteLine("Upload photo\n");

            if (!_retry)
                await RenderUploadPhotoAsync();
            else
                await RenderRetryAsync();
        }

        private async Task RenderUploadPhotoAsync()
        {
            ConsoleHelper.Write("Image path: ");
            _path = ConsoleHelper.ReadLine();

            if (!File.Exists(_path))
            {
                _error = "File does not exist";
                _retry = true;
            }
            else
            {
                ConsoleHelper.Write("\nUploading... ", ConsoleColor.Yellow);
                var fileInfo = new FileInfo(_path);
                Response response = await _protocolCommunication.SendRequestAsync(
                    new CreatePhotoRequest(fileInfo.Name, _path)
                );

                switch (response)
                {
                    case ErrorResponse errorResponse:
                        _error = errorResponse.Message;
                        _retry = true;
                        break;
                    case CreatePhotoResponse createPhotoResponse:
                        ConsoleHelper.WriteLine("Success!\n", ConsoleColor.Green);
                        ConsoleHelper.WriteLine("Press anything to continue");
                        ConsoleHelper.ReadKey();
                        _navigation.Back();
                        break;
                    default:
                        _error = $"Unrecognized response Id={response.Id}";
                        _retry = true;
                        break;
                }
            }
        }

        private async Task RenderRetryAsync()
        {
            ConsoleHelper.WriteLine($"Image path: {_path}");
            ConsoleHelper.WriteLine($"\nError: {_error}\n", ConsoleColor.Red);

            _retryMenu.Render();
        }

        private void RetryOptionSelected(string retryOption)
        {
            switch (retryOption)
            {
                case RetryAction:
                    _retry = false;
                    _error = null;
                    break;
                case GoBackAction:
                    _navigation.Back();
                    break;
            }
        }
    }
}