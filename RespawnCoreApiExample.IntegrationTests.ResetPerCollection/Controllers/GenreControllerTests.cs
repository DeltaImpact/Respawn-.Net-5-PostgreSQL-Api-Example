using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using RespawnCoreApiExample.Domain.Models.Entities;
using RespawnCoreApiExample.IntegrationTests.ResetPerCollection.Fixtures;
using RespawnCoreApiExample.IntegrationTests.ResetPerCollection.Utils;
using Xunit;

namespace RespawnCoreApiExample.IntegrationTests.ResetPerCollection.Controllers
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

        [Fact]
        public async Task Artificial_Delay()
        {
            await Task.Delay(1000);
        }
    }
}