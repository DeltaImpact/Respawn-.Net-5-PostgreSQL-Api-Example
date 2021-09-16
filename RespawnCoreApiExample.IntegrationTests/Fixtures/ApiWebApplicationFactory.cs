using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace RespawnCoreApiExample.IntegrationTests.Fixtures
{
    public class ApiWebApplicationFactory : WebApplicationFactory<Api.Startup>
    {
        public readonly string ConfigPath = "appsettings.IntegrationTests.json";
        
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(config =>
            {
                var integrationConfig = new ConfigurationBuilder()
                    .AddJsonFile(ConfigPath)
                    .Build();
                
                config.AddConfiguration(integrationConfig);
            });
        }
    }
}