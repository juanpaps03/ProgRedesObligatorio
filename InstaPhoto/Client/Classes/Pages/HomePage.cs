using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Interfaces;

namespace Client.Classes.Pages
{
    public class HomePage : IPage
    {
        private const char UploadPhotoAction = 'U';
        private const char ListUsersAction = 'L';
        private const char LogOutAction = 'X';

        private static readonly Dictionary<char, string> MenuOptions =
            new Dictionary<char, string>
            {
                {UploadPhotoAction, "Upload new photo"},
                {ListUsersAction, "Show users"},
                {LogOutAction, "Logout"},
            };

        private readonly Dictionary<char, string> _nextPage;

        private bool _error;
        private char _optionSelected = '\0';

        private readonly IPageNavigation _navigation;
        public HomePage(IPageNavigation navigation)
        {
            _navigation = navigation;
            _nextPage = new Dictionary<char, string>
            {
                {UploadPhotoAction, null},
                {ListUsersAction, null},
                {LogOutAction, null},
            };
        }

        public async Task RenderAsync()
        {
            ConsoleHelper.WriteLine("Home: \n", ConsoleColor.Cyan);

            if (_error)
            {
                ConsoleHelper.WriteLine($"Option <{_optionSelected}> not recognized!\n", ConsoleColor.Red);
            }

            _optionSelected = ConsoleHelper.ShowMenu(MenuOptions);
            if (_nextPage.ContainsKey(_optionSelected))
            {
                var pageSelected = _nextPage[_optionSelected];
                if (pageSelected != null)
                    _navigation.GoToPage(pageSelected);
                else
                    _navigation.Exit();
            }

            // Wrong option, re render the page with error
            _error = true;
        }
    }
}