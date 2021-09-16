using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using RespawnCoreApiExample.Domain.Db.Entities;
using RespawnCoreApiExample.IntegrationTests.Extensions;
using RespawnCoreApiExample.IntegrationTests.Fixtures;
using Xunit;

namespace RespawnCoreApiExample.IntegrationTests.Controllers
{
    public class GenreControllerTests : IntegrationTest
    {
        public GenreControllerTests(ApiWebApplicationFactory fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task Get_ShouldReturnAllGenres()
        {
            var response = await Client.GetAsync("/api/genre");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var genres = await response.Content.DeserializeAsync<List<Genre>>();
            genres.Count.Should().Be(5);
        }
    }
}