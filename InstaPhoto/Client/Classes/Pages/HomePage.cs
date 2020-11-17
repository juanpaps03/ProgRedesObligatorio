using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Interfaces;

namespace Client.Classes.Pages
{
    public class HomePage : IPage
    {
        private const string UploadPhotoAction = "U";
        private const string UsersListAction = "L";
        private const string LogOutAction = "X";
        private const string CommentPhotoAction = "C";
        private const string CommentListAction = "M";

        private readonly IPageNavigation _navigation;
        private readonly Menu _menu;

        public HomePage(IPageNavigation navigation)
        {
            _navigation = navigation;
            _menu = new Menu(
                options: new List<(string, string)>
                {
                    (UploadPhotoAction, "Upload new photo"),
                    (UsersListAction, "Show users"),
                    (CommentPhotoAction, "Comment Photo"),
                    (CommentListAction, "Show comments photo"),
                    (LogOutAction, "Logout"),
                },
                Navigate
            );
        }

        private void Navigate(string option)
        {
            switch (option)
            {
                case UploadPhotoAction:
                    _navigation.GoToPage(IPageNavigation.UploadPhotoPage);
                    break;
                case UsersListAction:
                    _navigation.GoToPage(IPageNavigation.UserListPage);
                    break;
                case CommentPhotoAction:
                    _navigation.GoToPage(IPageNavigation.CommentPhotoPage);
                    break;
                case CommentListAction:
                    _navigation.GoToPage(IPageNavigation.CommentListPage);
                    break;
                default:
                    _navigation.Exit();
                    break;
            }
        }

        public async Task RenderAsync()
        {
            ConsoleHelper.WriteLine("Home: \n", ConsoleColor.Cyan);

            _menu.Render();
        }
    }
}