using System.Collections.Generic;
using RespawnCoreApiExample.Domain.Models.Base;

namespace RespawnCoreApiExample.Domain.Models.Entities
{
    public class Book : BaseEntity, INamed
    {
        public string Name { get; set; }

        public ICollection<Genre> Genres { get; set; }

        public ICollection<Author> Authors { get; set; }
    }
}