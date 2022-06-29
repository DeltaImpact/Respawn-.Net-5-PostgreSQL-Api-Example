using System.Collections.Generic;
using RespawnCoreApiExample.Domain.Models.Base;

namespace RespawnCoreApiExample.Domain.Models.Entities
{
    public class Genre : BaseEntity, INamed
    {
        public string Name { get; set; }
        
        public ICollection<Book> Books { get; set; }
    }
}