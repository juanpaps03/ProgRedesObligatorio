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

        private readonly string _namePhoto;
        private Menu _commentListMenu;

        public CommentListPage(
            PageNavigation navigation,
            IDictionary<string, string> parameters,
            IProtocolCommunication protocolCommunication
        )
        {
            _navigation = navigation;
            _protocolCommunication = protocolCommunication;
            parameters.TryGetValue("namePhoto", out _namePhoto);

            if (_namePhoto == null)
            {
                throw new Exception("Parameter \"namePhoto\" required");
            }
        }

        public async Task RenderAsync()
        {
            ConsoleHelper.WriteLine($"Photo list from {_namePhoto}\n", ConsoleColor.Cyan);

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

            var response = await _protocolCommunication.SendRequestAsync(new CommentListRequest(_namePhoto));

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
                        commentList.Add((comment.NamePhoto, $"{comment.Text}"));
                    }
                    
                    _commentListMenu = new Menu(
                        options: commentList,
                        onSelect: s => { }, // TODO: ADD NEW PAGE TO SEE COMMENTS AND ADD NEW
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