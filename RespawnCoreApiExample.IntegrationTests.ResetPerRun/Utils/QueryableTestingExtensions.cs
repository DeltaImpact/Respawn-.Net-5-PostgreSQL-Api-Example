using Microsoft.EntityFrameworkCore;

namespace RespawnCoreApiExample.IntegrationTests.ResetPerRun.Utils
{
    public static class QueryableTestingExtensions
    {
        public static void WipeTable<TEntity>(this DbSet<TEntity> dbSet) where TEntity : class
        {
            dbSet.RemoveRange(dbSet);
        }
    }
}