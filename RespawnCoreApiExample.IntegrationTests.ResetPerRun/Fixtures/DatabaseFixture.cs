using Respawn;
using RespawnCoreApiExample.IntegrationTests.ResetPerRun.Utils;

namespace RespawnCoreApiExample.IntegrationTests.ResetPerRun.Fixtures;

public class DatabaseFixture
{
    private readonly Checkpoint _checkpoint = RespawnHelper.GetCheckpoint();
    
    public DatabaseFixture()
    {
        var configuration = ConfigLoader.LoadConfiguration();
        var connectionString = ConfigLoader.GetDbConnectionString(configuration);
        
        RespawnHelper.ResetDbAsync(_checkpoint, connectionString).Wait();
    }
}