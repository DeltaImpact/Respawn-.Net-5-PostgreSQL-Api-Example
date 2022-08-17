using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using RespawnCoreApiExample.DataAccess.Contexts;

namespace RespawnCoreApiExample.IntegrationTests.ResetPerRun.Fixtures
{
    public class IntegrationTestFactory : ApiWebApplicationFactory
    {
        public readonly HttpClient Client;

        public ApplicationDbContext Context { get; private set; }

        public IntegrationTestFactory()
        {
            Client = CreateClient();

            SetContext();
        }

        private void SetContext()
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseNpgsql(ConnectionString);
            Context = new ApplicationDbContext(builder.Options);
        }
    }
}