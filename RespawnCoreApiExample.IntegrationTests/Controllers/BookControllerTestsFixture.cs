using System.Threading.Tasks;
using RespawnCoreApiExample.Domain.Models.Entities;
using RespawnCoreApiExample.IntegrationTests.Fixtures;
using Xunit;

namespace RespawnCoreApiExample.IntegrationTests.Controllers
{
    public class BookControllerTestsFixture : IntegrationTestFactory, IAsyncLifetime
    {
        public Author Author;

        public async Task InitializeAsync()
        {
            await ResetDb();
            await DatabaseSetupAsync();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        private async Task DatabaseSetupAsync()
        {
            Author = (await Context.Authors.AddAsync(new Author
                {
                    FullName = "Oswald Orange",
                }
            )).Entity;
            await Context.SaveChangesAsync();
        }
    }
}