using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using RespawnCoreApiExample.Domain.Models.Entities;
using RespawnCoreApiExample.IntegrationTests.ResetPerRun.Fixtures;
using RespawnCoreApiExample.IntegrationTests.ResetPerRun.Utils;
using Xunit;

namespace RespawnCoreApiExample.IntegrationTests.ResetPerRun.Controllers
{
    [Collection("Database collection")]
    public class GenreControllerTests : IntegrationTestFactory
    {
        private static string BaseUrl => "/api/Genre";

        [Fact]
        public async Task Get_ShouldReturnAllGenres()
        {
            var response = await Client.GetAsync(BaseUrl);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var genres = await response.Content.DeserializeAsync<List<Genre>>();
            genres.Count.Should().Be(5);
        }

        [Fact]
        public async Task Artificial_Delay()
        {
            await Task.Delay(1000);
        }
    }
}