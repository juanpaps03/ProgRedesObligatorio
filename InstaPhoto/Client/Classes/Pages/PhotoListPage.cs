using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Client.Interfaces;
using SocketLibrary.Interfaces;
using SocketLibrary.Messages.Error;
using SocketLibrary.Messages.PhotoList;

namespace Client.Classes.Pages
{
    public class PhotoListPage : IPage
    {
        private readonly PageNavigation _navigation;
        private readonly IProtocolCommunication _protocolCommunication;

        private readonly string _username;
        private Menu _photoListMenu;

        public PhotoListPage(
            PageNavigation navigation,
            IDictionary<string, string> parameters,
            IProtocolCommunication protocolCommunication
        )
        {
            _navigation = navigation;
            _protocolCommunication = protocolCommunication;
            parameters.TryGetValue("username", out _username);

            if (_username == null)
            {
                throw new Exception("Parameter \"username\" required");
            }
        }

        public async Task RenderAsync()
        {
            ConsoleHelper.WriteLine($"Photo list from {_username}\n", ConsoleColor.Cyan);

            if (_photoListMenu == null)
            {
                await LoadPage();
            }
            else
            {
                _photoListMenu.Render();
            }
        }

        private async Task LoadPage()
        {
            ConsoleHelper.WriteLine("Loading...\n", ConsoleColor.Yellow);

            var response = await _protocolCommunication.SendRequestAsync(new PhotoListRequest(_username));

            switch (response)
            {
                case ErrorResponse errorResponse:
                    ConsoleHelper.WriteLine($"Error {errorResponse.ErrorId}: {errorResponse.Message}");
                    ConsoleHelper.ReadKey(); // Pause
                    _navigation.Back();
                    break;
                case PhotoListResponse photoListResponse:
                    var cacheLocation = $"cache/{_username}";
                    if (Directory.Exists(cacheLocation))
                        Directory.Delete(cacheLocation, true);
                    Directory.CreateDirectory(cacheLocation);

                    // Move photos to cache
                    var photoList = new List<(string, string)>();
                    foreach (var photo in photoListResponse.Photos)
                    {
                        var photoPath = $"{cacheLocation}/{photo.Name}";
                        File.Move(photo.File, photoPath);

                        photoList.Add((photo.Name, $"{photo.Name} - {photoPath}"));
                    }
                    
                    _photoListMenu = new Menu(
                        options: photoList,
                        onSelect: s => { }, // TODO: ADD NEW PAGE TO SEE COMMENTS AND ADD NEW
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