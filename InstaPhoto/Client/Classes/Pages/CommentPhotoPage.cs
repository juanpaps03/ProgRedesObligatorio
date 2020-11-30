using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Client.Interfaces;
using SocketLibrary.Interfaces;
using SocketLibrary.Messages;
using SocketLibrary.Messages.CommentPhoto;
using SocketLibrary.Messages.CreatePhoto;
using SocketLibrary.Messages.Error;

namespace Client.Classes.Pages
{
    public class CommentPhotoPage : IPage
    {
        private const string RetryAction = "retry";
        private const string GoBackAction = "goBack";

        private bool _retry;
        private readonly string _username;
        private readonly string _photoName;
        private string _text;
        private string _error;

        private readonly IPageNavigation _navigation;
        private readonly IProtocolCommunication _protocolCommunication;

        private readonly Menu _retryMenu;

        public CommentPhotoPage(
            IPageNavigation navigation,
            IDictionary<string, string> parameters,
            IProtocolCommunication protocolCommunication
        )
        {
            _navigation = navigation;
            _protocolCommunication = protocolCommunication;

            parameters.TryGetValue("username", out _username);
            if (_username == null)
            {
                throw new Exception("Parameter \"username\" required");
            }

            parameters.TryGetValue("photoName", out _photoName);
            if (_photoName == null)
            {
                throw new Exception("Parameter \"photoName\" required");
            }

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
            ConsoleHelper.WriteLine("Comment photo\n");

            if (!_retry)
                await RenderCommentPhotoAsync();
            else
                await RenderRetryAsync();
        }

        private async Task RenderCommentPhotoAsync()
        {
            ConsoleHelper.Write("Comment: ");
            _text = ConsoleHelper.ReadLine();

            ConsoleHelper.Write("\nUploading... ", ConsoleColor.Yellow);
            Response response = await _protocolCommunication.SendRequestAsync(
                new CommentPhotoRequest(_photoName, _username, _text)
            );

            switch (response)
            {
                case ErrorResponse errorResponse:
                    ConsoleHelper.WriteLine($"Error {errorResponse.ErrorId}: {errorResponse.Message}");
                    ConsoleHelper.ReadKey(); // Pause
                    _navigation.Back();
                    break;
                case CommentPhotoResponse commentPhotoResponse:
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

        private async Task RenderRetryAsync()
        {
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