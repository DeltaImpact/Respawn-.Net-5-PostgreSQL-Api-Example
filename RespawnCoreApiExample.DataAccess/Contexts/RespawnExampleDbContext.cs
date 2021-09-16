using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RespawnCoreApiExample.DataAccess.Extensions;
using RespawnCoreApiExample.Domain.Db.Entities;

namespace RespawnCoreApiExample.DataAccess.Contexts
{
    public class RespawnExampleDbContext : DbContext
    {
        public RespawnExampleDbContext(DbContextOptions<RespawnExampleDbContext> options) : base(options)
        {
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            ChangeTracker.ApplyAuditInformation();

            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }
    }
}