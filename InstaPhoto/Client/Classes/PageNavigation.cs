using System;
using System.Collections.Generic;
using Client.Classes.Pages;
using Client.Interfaces;
using SocketLibrary;

namespace Client.Classes
{
    public class PageNavigation : IPageNavigation
    {
        private readonly Stack<IPage> _stack = new Stack<IPage>();
        private readonly ProtocolCommunication _protocolCommunication;

        public PageNavigation(ProtocolCommunication protocolCommunication)
        {
            _protocolCommunication = protocolCommunication;
        }

        public void GoToPage(string page, Dictionary<string, string> parameters = null)
        {
            switch (page)
            {
                case IPageNavigation.LandingPage:
                    _stack.Push(new LandingPage(this));
                    break;
                case IPageNavigation.LoginPage:
                    _stack.Push(new LoginPage(this, _protocolCommunication));
                    break;
                case IPageNavigation.SignUpPage:
                    _stack.Push(new SignUpPage(this));
                    break;
                case IPageNavigation.HomePage:
                    _stack.Push(new HomePage(this));
                    break;
                case IPageNavigation.UploadPhotoPage:
                    _stack.Push(new UploadPhotoPage(this, _protocolCommunication));
                    break;
                case IPageNavigation.PhotoListPage:
                    _stack.Push(new PhotoListPage(this, parameters, _protocolCommunication));
                    break;
                default:
                    throw new Exception($"Page {page} not found");
            }
        }

        public void Back()
        {
            if (_stack.Count > 0)
                _stack.Pop();
        }

        public IPage Top()
        {
            return _stack.Count > 0 ? _stack.Peek() : null;
        }

        public void Exit()
        {
            _stack.Clear();
        }
    }
}