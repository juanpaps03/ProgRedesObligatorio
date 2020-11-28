using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Client.Interfaces;
using SocketLibrary.Interfaces;
using SocketLibrary.Messages.Error;
using SocketLibrary.Messages.PhotoList;

namespace Client.Classes.Pages
{
    public class PhotoDetailsPage : IPage
    {
        private const string ShowCommentsAction = "S";
        private const string CommentAction = "C";

        private readonly PageNavigation _navigation;

        private readonly string _username;
        private readonly string _photoName;
        private readonly Menu _menu;

        public PhotoDetailsPage(
            PageNavigation navigation,
            IDictionary<string, string> parameters
        )
        {
            _navigation = navigation;

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

            _menu = new Menu(
                options: new List<(string, string)>
                {
                    (ShowCommentsAction, "Show comments"),
                    (CommentAction, "Comments")
                },
                onSelect: OnMenuSelect,
                onEscPressed: () => _navigation.Back(),
                escapeActionName: "Go back"
            );
        }

        public async Task RenderAsync()
        {
            ConsoleHelper.WriteLine(
                $"Actions for photo \"{_photoName}\" from user \"{_username}\"\n",
                ConsoleColor.Cyan
            );

            _menu.Render();
        }

        private void OnMenuSelect(string option)
        {
            var parameters = new Dictionary<string, string>
            {
                {"username", _username},
                {"photoName", _photoName},
            };

            switch (option)
            {
                case CommentAction:
                    _navigation.GoToPage(IPageNavigation.CommentPhotoPage, parameters);
                    break;
                case ShowCommentsAction:
                    _navigation.GoToPage(IPageNavigation.CommentListPage, parameters);
                    break;
            }
        }
    }
}