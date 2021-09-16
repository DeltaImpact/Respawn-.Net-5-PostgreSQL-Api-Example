using System.Collections.Generic;

namespace RespawnCoreApiExample.Domain.Db.Entities
{
    public class Genre : BaseEntity, INamed
    {
        public string Name { get; set; }
        
        public ICollection<Book> Books { get; set; }
    }
}