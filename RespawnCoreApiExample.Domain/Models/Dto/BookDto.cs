using System;
using System.Collections.Generic;

namespace RespawnCoreApiExample.Domain.Models.Dto;

public class BookDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<AuthorDto> Authors { get; set; }
    public ICollection<string> Genres { get; set; }
}