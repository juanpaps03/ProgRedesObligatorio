using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Interfaces;

namespace Client.Classes.Pages
{
    public class HomePage : IPage
    {
        private const string UploadPhotoAction = "U";
        private const string ListUsersAction = "L";
        private const string LogOutAction = "X";

        private readonly IPageNavigation _navigation;
        private readonly Menu _menu;

        public HomePage(IPageNavigation navigation)
        {
            _navigation = navigation;
            _menu = new Menu(
                options: new List<(string, string)>
                {
                    (UploadPhotoAction, "Upload new photo"),
                    (ListUsersAction, "Show users"),
                    (LogOutAction, "Logout"),
                },
                Navigate
            );
        }

        private void Navigate(string option)
        {
            _navigation.Exit();
        }

        public async Task RenderAsync()
        {
            ConsoleHelper.WriteLine("Home: \n", ConsoleColor.Cyan);

            _menu.Render();
        }
    }
}