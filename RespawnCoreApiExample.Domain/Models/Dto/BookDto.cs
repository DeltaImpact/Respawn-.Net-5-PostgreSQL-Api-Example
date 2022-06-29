using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RespawnCoreApiExample.Domain.Models.Entities;

namespace RespawnCoreApiExample.Domain.Models.Dto;

public class BookDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<AuthorDto> Authors { get; set; }
    public ICollection<string> Genres { get; set; }
}