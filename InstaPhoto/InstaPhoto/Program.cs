using System;
using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;
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
            string connectionString = @"Data Source=/home/diego/ORT/ProgRedes/ProgRedesObligatorio/dbInstaPhoto.db;Version=3;";
            
            IDbConnection connection = new SQLiteConnection(connectionString);
            
            IUserRepository userRepository = new UserRepository(connection);
            
            IUserService userService = new UserService(userRepository);

            var users = await userService.GetUsersAsync();

            foreach (var user in users)
            {
                Console.WriteLine($"{user.Username} - Password: {user.Password} - Admin: {user.Admin}");
            }
            Console.WriteLine("Hello World!");
        }
    }
}