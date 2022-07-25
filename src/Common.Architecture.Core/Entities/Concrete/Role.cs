using Common.Architecture.Core.Entities.Interface;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Architecture.Core.Entities.Concrete
{
    public class Role : IdentityRole<Guid>, IEntity
    {
        [StringLength(255)]
        [Required]
        public string Description { get; set; }

        [NotMapped]
        public string[] Claims { get; set; }

        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public List<UserRole> UserRoles { get; set; }
    }
}
