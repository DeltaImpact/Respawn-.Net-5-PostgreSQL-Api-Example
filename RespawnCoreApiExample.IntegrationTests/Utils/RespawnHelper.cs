using System.Threading.Tasks;
using Npgsql;
using Respawn;
using Respawn.Graph;

namespace RespawnCoreApiExample.IntegrationTests.Utils
{
    public class RespawnHelper
    {
        public static Checkpoint GetCheckpoint()
        {
            return new Checkpoint
            {
                SchemasToInclude = new[]
                {
                    "public"
                },
                TablesToIgnore = new Table[]
                {
                    "Genres",
                    "__EFMigrationsHistory"
                },
                DbAdapter = DbAdapter.Postgres
            };
        }

        public static async Task ResetDbAsync(Checkpoint checkpoint, string connectionString)
        {
            using (var npgsqlConnection = new NpgsqlConnection(connectionString))
            {
                await npgsqlConnection.OpenAsync();
                await checkpoint.Reset(npgsqlConnection);
                await npgsqlConnection.CloseAsync();
            }
        }
    }
}