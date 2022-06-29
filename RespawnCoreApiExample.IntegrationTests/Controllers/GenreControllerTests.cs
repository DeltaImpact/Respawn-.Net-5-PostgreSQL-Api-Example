using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using RespawnCoreApiExample.Domain.Models.Entities;
using RespawnCoreApiExample.IntegrationTests.Fixtures;
using RespawnCoreApiExample.IntegrationTests.Utils;
using Xunit;

namespace RespawnCoreApiExample.IntegrationTests.Controllers
{
    public class GenreControllerTests : IClassFixture<IntegrationTestFactory>
    {
        private readonly IntegrationTestFactory _fixture;
        private static string BaseUrl => "/api/Genre";
        
        public GenreControllerTests(IntegrationTestFactory fixture)
        {
            _fixture = fixture;
        }
    
        [Fact]
        public async Task Get_ShouldReturnAllGenres()
        {
            var response = await _fixture.Client.GetAsync(BaseUrl);
    
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var genres = await response.Content.DeserializeAsync<List<Genre>>();
            genres.Count.Should().Be(5);
        }
    }
}