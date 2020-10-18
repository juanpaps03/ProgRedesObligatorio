using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly Dictionary<char, IPage> _nextPage;

        private bool _error;
        private char _optionSelected = '\0';

        public HomePage(IPageCreator pageCreator)
        {
            _nextPage = new Dictionary<char, IPage>
            {
                {UploadPhotoAction, null},
                {ListUsersAction, null},
                {LogOutAction, null},
            };
        }

        public async Task<IPage> RenderAsync()
        {
            ConsoleHelper.WriteLine("Home: \n", ConsoleColor.Cyan);

            if (_error)
            {
                ConsoleHelper.WriteLine($"Option <{_optionSelected}> not recognized!\n", ConsoleColor.Red);
            }

            _optionSelected = ConsoleHelper.ShowMenu(MenuOptions);
            if (_nextPage.ContainsKey(_optionSelected))
                return _nextPage[_optionSelected];

            // Wrong option, re render the page with error
            _error = true;
            return this;
        }
    }
}