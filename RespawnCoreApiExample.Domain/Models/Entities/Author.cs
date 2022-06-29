using System.Collections.Generic;
using RespawnCoreApiExample.Domain.Models.Base;

namespace RespawnCoreApiExample.Domain.Models.Entities;

public class Author : BaseEntity
{
    public string FullName { get; set; }
        
    public ICollection<Book> Books { get; set; }
}