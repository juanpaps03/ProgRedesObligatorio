using System;
using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;
using Domain;
using Grpc.Net.Client;
using Repositories;
using Repositories.Interfaces;
using Services;
using Services.Interfaces;

namespace InstaPhoto
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            IUserService userService = new UserServiceGrpcClient(channel);

            var users = await userService.GetUsersAsync();

            foreach (var user in users)
            {
                Console.WriteLine($"{user.Username} - Pass: {user.Password}");
            }


            // string connectionString = @"Data Source=/home/diego/ORT/ProgRedes/ProgRedesObligatorio/dbInstaPhoto.db;foreign keys=true;Version=3;";
            //
            // IDbConnection connection = new SQLiteConnection(connectionString);
            // IPhotoRepository photoRepository = new PhotoRepository(connection);
            // IPhotoService photoService = new PhotoService(photoRepository);
            //
            // var photoList = await photoService.GetPhotosFromUserAsync("asdf");
            //
            // foreach (var photo in photoList)
            // {
            //     Console.WriteLine($"{photo.Name} - {photo.File} - {photo.Username}");
            // }
            // Console.WriteLine("Hello World!");
        }
    }
}