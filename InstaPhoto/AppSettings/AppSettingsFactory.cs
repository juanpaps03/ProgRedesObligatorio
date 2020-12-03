using System.IO;
using Microsoft.Extensions.Configuration;

namespace AppSettings
{
    public class AppSettingsFactory
    {
        public static IConfiguration GetAppSettings()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            IConfigurationRoot configuration = builder.Build();

            return configuration.GetSection("AppSettings");
        }
    }
}