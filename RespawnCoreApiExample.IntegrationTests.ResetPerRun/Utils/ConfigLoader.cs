using Microsoft.Extensions.Configuration;
using RespawnCoreApiExample.DataAccess.Contexts;

namespace RespawnCoreApiExample.IntegrationTests.ResetPerRun.Utils
{
    public static class ConfigLoader
    {
        public static readonly string ConfigPath = "appsettings.IntegrationTests.json";

        public static IConfigurationRoot LoadConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile(ConfigPath, optional: true)
                .AddEnvironmentVariables()
                .Build();

            return config;
        }

        public static string GetDbConnectionString(IConfigurationRoot configurationRoot)
        {
            return configurationRoot.GetConnectionString(nameof(ApplicationDbContext));
        }
    }
}