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

        private readonly IConsoleHelper _console;

        private string _username;
        private string _password;

        private bool _askRetry;
        private bool _retryOptionError;
        private char _selectedOption;

        public SignUpPage(IConsoleHelper console)
        {
            _console = console;
        }

        public async Task<IPage> RenderAsync()
        {
            _console.WriteLine("Sign up\n", ConsoleColor.Cyan);

            return !_askRetry ? await RenderLoginAsync() : RenderRetry();
        }

        private async Task<IPage> RenderLoginAsync()
        {
            _console.Write("Username: ");
            _username = _console.Read();
            _console.Write("Password: ");
            _password = _console.Read();

            var validSignUp = await DoSignUp();

            if (validSignUp)
                return null;

            // Wrong login, retry
            _askRetry = true;
            return this;
        }

        private async Task<bool> DoSignUp()
        {
            _console.WriteLine("\nSigning up...");

            // TODO: CALL Sign up
            Thread.Sleep(500);
            return _username != "admin";
        }

        private IPage RenderRetry()
        {
            _console.WriteLine($"Username: {_username}");
            _console.WriteLine($"Password: {_password}");

            _console.WriteLine("\nUsername Already in use!\n", ConsoleColor.Red);
            if (_retryOptionError)
            {
                _console.WriteLine($"\nUnrecognized option <{_selectedOption}>\n", ConsoleColor.Red);
            }

            _selectedOption = _console.ShowMenu(RetryOptions);

            switch (_selectedOption)
            {
                case RetryAction:
                    _askRetry = false;
                    return this;
                case BackAction:
                    return new LandingPage(_console);
                case ExitAction:
                    return null;
                default:
                    _retryOptionError = true;
                    return this;
            }
        }
    }
}