using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Client.Interfaces;
using SocketLibrary.Interfaces;
using SocketLibrary.Messages.CommentList;
using SocketLibrary.Messages.Error;
using SocketLibrary.Messages.PhotoList;

namespace Client.Classes.Pages
{
    public class CommentListPage : IPage
    {
        private readonly PageNavigation _navigation;
        private readonly IProtocolCommunication _protocolCommunication;

        private readonly string _username;
        private readonly string _photoName;
        private Menu _commentListMenu;

        public CommentListPage(
            PageNavigation navigation,
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
        }

        public async Task RenderAsync()
        {
            ConsoleHelper.WriteLine(
                $"Comment list from \"{_photoName}\" from \"{_username}\"\n",
                ConsoleColor.Cyan
            );

            if (_commentListMenu == null)
            {
                await LoadPage();
            }
            else
            {
                _commentListMenu.Render();
            }
        }

        private async Task LoadPage()
        {
            ConsoleHelper.WriteLine("Loading...\n", ConsoleColor.Yellow);

            var response = await _protocolCommunication.SendRequestAsync(
                new CommentListRequest(_username, _photoName)
            );

            switch (response)
            {
                case ErrorResponse errorResponse:
                    ConsoleHelper.WriteLine($"Error {errorResponse.ErrorId}: {errorResponse.Message}");
                    ConsoleHelper.ReadKey(); // Pause
                    _navigation.Back();
                    break;
                case CommentListResponse commentListResponse:
                    // Move comments to cache
                    var commentList = new List<(string, string)>();
                    foreach (var comment in commentListResponse.Comments)
                    {
                        commentList.Add(($"{comment.Username},{comment.PhotoName}", $"{comment.Text}"));
                    }

                    _commentListMenu = new Menu(
                        options: commentList,
                        onSelect: s => { },
                        onEscPressed: () => _navigation.Back(),
                        escapeActionName: "Go back"
                    );
                    break;
                default:
                    ConsoleHelper.WriteLine($"Error: Unrecognized command {response.Id}");
                    ConsoleHelper.ReadKey(); // Pause
                    _navigation.Back();
                    break;
            }
        }
    }
}