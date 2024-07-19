﻿using System.ComponentModel.DataAnnotations;

namespace Hng.Domain.Entities.Models
{
    public class BaseModel
    {
        [Key]
        public long Id { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now.ToUniversalTime();
        public DateTimeOffset UpdatedAt { get; set;} = DateTimeOffset.Now.ToUniversalTime();
    }
}
