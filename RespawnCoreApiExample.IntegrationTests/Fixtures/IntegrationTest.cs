using System;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Respawn;
using RespawnCoreApiExample.DataAccess.Contexts;
using Xunit;

namespace RespawnCoreApiExample.IntegrationTests.Fixtures
{
    public class IntegrationTest : IClassFixture<ApiWebApplicationFactory>
    {
         private readonly Checkpoint _checkpoint = new Checkpoint
        {
            SchemasToInclude = new[]
            {
                "public"
            },
            TablesToIgnore = new []
            {
                "Genres",
                "__EFMigrationsHistory"
            },
            DbAdapter = DbAdapter.Postgres
        };

        protected readonly ApiWebApplicationFactory Factory;

        protected readonly HttpClient Client;

        private string _connectionString;

        protected RespawnExampleDbContext Context { get; private set; }

        public IntegrationTest(ApiWebApplicationFactory fixture)
        {
            Factory = fixture;
            Client = Factory.CreateClient();
            LoadDbConnectionString(fixture.ConfigPath);
            SetupContext();
            SetupCheckpoint();
        }

        private void LoadDbConnectionString(string configPath)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(configPath)
                .Build();
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private void SetupContext()
        {  
            var builder = new DbContextOptionsBuilder<RespawnExampleDbContext>();
            builder.UseNpgsql(_connectionString);
            Context = new RespawnExampleDbContext(builder.Options);
        }

        private void SetupCheckpoint()
        {
            using (var npgsqlConnection = new NpgsqlConnection(_connectionString))
            {
                npgsqlConnection.OpenAsync().Wait();
                _checkpoint.Reset(npgsqlConnection).Wait();
            }
        }
    }
}