using System.Collections.Generic;

namespace RespawnCoreApiExample.Domain.Db.Entities
{
    public class Book : BaseEntity, INamed
    {
        public string Name { get; set; }
        
        public ICollection<Genre> Genres { get; set; }
    }
}