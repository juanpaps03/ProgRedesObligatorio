using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Interfaces;

namespace Client.Classes.Pages
{
    public class LandingPage : IPage
    {
        private readonly IPageNavigation _navigation;
        private readonly Menu _menu;

        public LandingPage(IPageNavigation navigation)
        {
            _navigation = navigation;
            _menu = new Menu(
                options: new List<(string, string)>
                {
                    (IPageNavigation.LoginPage, "Login"),
                    (IPageNavigation.SignUpPage, "Sign up"),
                    (null, "Exit"),
                },
                Navigate
            );
        }

        public async Task RenderAsync()
        {
            ConsoleHelper.WriteLine("\n>> Welcome to InstaPhoto! <<\n", ConsoleColor.Cyan);
            _menu.Render();
        }

        private void Navigate(string page)
        {
            if (page != null)
                _navigation.GoToPage(page);
            else
                _navigation.Exit();
        }
    }
}