using System;

namespace RespawnCoreApiExample.Domain.Models.Base
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
    }
}