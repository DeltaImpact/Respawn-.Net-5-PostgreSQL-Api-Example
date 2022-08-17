using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using RespawnCoreApiExample.Api;
using RespawnCoreApiExample.IntegrationTests.ResetPerRun.Utils;

namespace RespawnCoreApiExample.IntegrationTests.ResetPerRun.Fixtures
{
    public class ApiWebApplicationFactory : WebApplicationFactory<Startup>
    {
        private IConfigurationRoot _loadConfiguration;
        public string ConnectionString { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(config =>
            {
                _loadConfiguration = ConfigLoader.LoadConfiguration();
                ConnectionString = ConfigLoader.GetDbConnectionString(_loadConfiguration);

                config.AddConfiguration(_loadConfiguration);
            });
        }
    }
}