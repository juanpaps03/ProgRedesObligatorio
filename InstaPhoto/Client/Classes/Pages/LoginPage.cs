using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Client.Interfaces;
using SocketLibrary.Interfaces;
using SocketLibrary.Messages;
using SocketLibrary.Messages.Error;
using SocketLibrary.Messages.Login;

namespace Client.Classes.Pages
{
    public class LoginPage : IPage
    {
        private const string RetryAction = "R";
        private const string BackAction = "Z";
        private const string ExitAction = "X";

        private string _username;
        private string _password;

        private bool _askRetry;
        private bool _validLogin;

        private readonly IPageNavigation _navigation;
        private readonly IProtocolCommunication _protocolCommunication;
        
        private readonly Menu _menu;

        public LoginPage(IPageNavigation navigation, IProtocolCommunication protocolCommunication)
        {
            _navigation = navigation;
            _protocolCommunication = protocolCommunication;
            _menu = new Menu(
                options: new List<(string, string)>
                {
                    (RetryAction, "Try Again"),
                    (BackAction, "Back"),
                    (ExitAction, "Exit"),
                },
                RetryActionSelected
            );
        }

        public async Task RenderAsync()
        {
            ConsoleHelper.WriteLine("Login\n", ConsoleColor.Cyan);

            if (!_askRetry)
                await RenderLoginAsync();
            else
                RenderRetry();
        }

        private async Task RenderLoginAsync()
        {
            ConsoleHelper.Write("Username: ");
            _username = ConsoleHelper.ReadLine();
            ConsoleHelper.Write("Password: ");
            _password = ConsoleHelper.ReadLine();

            await DoLogin();

            if (_validLogin)
                _navigation.GoToPage(IPageNavigation.HomePage);

            // Wrong login, retry
            _askRetry = true;
        }

        private async Task DoLogin()
        {
            ConsoleHelper.WriteLine("\nLogging in...");

            Response response = await _protocolCommunication.SendRequestAsync(
                new LoginRequest(_username, _password)
            );
            switch (response)
            {
                case ErrorResponse errorResponse:
                    ConsoleHelper.WriteLine(errorResponse.Message);
                    _validLogin = false;
                    RenderRetry();
                    break;
                case LoginResponse loginResponse:
                    ConsoleHelper.WriteLine("Success Login!\n", ConsoleColor.Green);
                    ConsoleHelper.WriteLine($"Welcome {_username}!\n");
                    _validLogin = true;
                    break;
            }

        }

        private void RenderRetry()
        {
            ConsoleHelper.WriteLine($"Username: {_username}");
            ConsoleHelper.WriteLine($"Password: {_password}");

            _menu.Render();
        }

        private void RetryActionSelected(string selectedOption)
        {
            switch (selectedOption)
            {
                case RetryAction:
                    _askRetry = false;
                    break;
                case BackAction:
                    _navigation.Back();
                    break;
                case ExitAction:
                    _navigation.Exit();
                    break;
            }
        }
    }
}