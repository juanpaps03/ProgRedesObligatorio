using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Interfaces;
using SocketLibrary.Interfaces;
using SocketLibrary.Messages.Error;
using SocketLibrary.Messages.UserList;

namespace Client.Classes.Pages
{
    public class UserListPage : IPage
    {
        private readonly PageNavigation _navigation;
        private readonly IProtocolCommunication _protocolCommunication;

        private Menu _userListMenu;

        public UserListPage(
            PageNavigation navigation,
            IProtocolCommunication protocolCommunication
        )
        {
            _navigation = navigation;
            _protocolCommunication = protocolCommunication;
        }

        public async Task RenderAsync()
        {
            ConsoleHelper.WriteLine(
                $"List of users\n",
                ConsoleColor.Cyan
            );
            
            if (_userListMenu == null)
            {
                await LoadPage();
            }
            else
            {
                _userListMenu.Render();
            }
        }

        private async Task LoadPage()
        {
            ConsoleHelper.WriteLine("Loading...\n", ConsoleColor.Yellow);

            var response = await _protocolCommunication.SendRequestAsync(new UserListRequest());

            switch (response)
            {
                case ErrorResponse errorResponse:
                    ConsoleHelper.WriteLine($"Error {errorResponse.ErrorId}: {errorResponse.Message}");
                    ConsoleHelper.ReadKey(); // Pause
                    _navigation.Back();
                    break;
                case UserListResponse userListResponse:
                    // Move users to cache
                    var userList = new List<(string, string)>();
                    foreach (var user in userListResponse.Users)
                    {
                        userList.Add((user.Username, user.Username));
                    }

                    _userListMenu = new Menu(
                        options: userList,
                        onSelect: username =>
                        {
                            _navigation.GoToPage(
                                IPageNavigation.PhotoListPage,
                                new Dictionary<string, string> {{"username", username}}
                            );
                        },
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