using Xunit;

namespace RespawnCoreApiExample.IntegrationTests.ResetPerRun.Fixtures;

[CollectionDefinition("Database collection")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
{
    
}