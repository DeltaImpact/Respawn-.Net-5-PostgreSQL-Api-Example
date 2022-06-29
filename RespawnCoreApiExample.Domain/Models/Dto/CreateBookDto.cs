using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace RespawnCoreApiExample.Domain.Models.Dto;

public class CreateBookDto
{
    [BindRequired]
    public string Name { get; set; }

    public List<Guid> Authors { get; set; } = new();

    [Required, MinLength(1)]
    public ICollection<string> Genres { get; set; }
}