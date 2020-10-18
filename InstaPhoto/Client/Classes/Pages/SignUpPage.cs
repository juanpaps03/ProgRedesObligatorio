using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Client.Interfaces;

namespace Client.Classes.Pages
{
    public class SignUpPage : IPage
    {
        private const char RetryAction = 'R';
        private const char BackAction = 'Z';
        private const char ExitAction = 'X';
        
        private static readonly Dictionary<char, string> RetryOptions =
            new Dictionary<char, string>
            {
                {RetryAction, "Try Again"},
                {BackAction, "Back"},
                {ExitAction, "Exit"},
            };

        private string _username;
        private string _password;

        private bool _askRetry;
        private bool _retryOptionError;
        private char _selectedOption;
        private readonly IPageCreator _pageCreator;

        public SignUpPage(IPageCreator pageCreator)
        {
            _pageCreator = pageCreator;
        }

        public async Task<IPage> RenderAsync()
        {
            ConsoleHelper.WriteLine("Sign up\n", ConsoleColor.Cyan);

            return !_askRetry ? await RenderLoginAsync() : RenderRetry();
        }

        private async Task<IPage> RenderLoginAsync()
        {
            ConsoleHelper.Write("Username: ");
            _username = ConsoleHelper.Read();
            ConsoleHelper.Write("Password: ");
            _password = ConsoleHelper.Read();

            var validSignUp = await DoSignUp();

            if (validSignUp)
                return _pageCreator.CreatePage(IPageCreator.PageId.HomePage);

            // Wrong login, retry
            _askRetry = true;
            return this;
        }

        private async Task<bool> DoSignUp()
        {
            ConsoleHelper.WriteLine("\nSigning up...");

            // TODO: CALL Sign up
            Thread.Sleep(500);
            return _username != "admin";
        }

        private IPage RenderRetry()
        {
            ConsoleHelper.WriteLine($"Username: {_username}");
            ConsoleHelper.WriteLine($"Password: {_password}");

            ConsoleHelper.WriteLine("\nUsername Already in use!\n", ConsoleColor.Red);
            if (_retryOptionError)
            {
                ConsoleHelper.WriteLine($"\nUnrecognized option <{_selectedOption}>\n", ConsoleColor.Red);
            }

            _selectedOption = ConsoleHelper.ShowMenu(RetryOptions);

            switch (_selectedOption)
            {
                case RetryAction:
                    _askRetry = false;
                    return this;
                case BackAction:
                    return _pageCreator.CreatePage(IPageCreator.PageId.LandingPage);
                case ExitAction:
                    return null;
                default:
                    _retryOptionError = true;
                    return this;
            }
        }
    }
}