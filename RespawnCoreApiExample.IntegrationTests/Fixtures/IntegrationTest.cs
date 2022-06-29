using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Respawn;
using RespawnCoreApiExample.DataAccess.Contexts;
using RespawnCoreApiExample.IntegrationTests.Utils;

namespace RespawnCoreApiExample.IntegrationTests.Fixtures
{
    public class IntegrationTestFactory : ApiWebApplicationFactory
    {
        private readonly Checkpoint _checkpoint = RespawnHelper.GetCheckpoint();

        public readonly HttpClient Client;

        public ApplicationDbContext Context { get; private set; }

        public IntegrationTestFactory()
        {
            Client = CreateClient();

            SetContext();
        }

        public async Task ResetDb()
        {
            await RespawnHelper.ResetDbAsync(_checkpoint, ConnectionString);
        }

        private void SetContext()
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseNpgsql(ConnectionString);
            Context = new ApplicationDbContext(builder.Options);
        }
    }
}