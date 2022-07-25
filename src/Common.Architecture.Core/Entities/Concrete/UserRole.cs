using Common.Architecture.Core.Entities.Interface;
using Microsoft.AspNetCore.Identity;
using System;

namespace Common.Architecture.Core.Entities.Concrete
{
    public class UserRole : IdentityUserRole<Guid>, IEntity
    {
        public User User { get; set; }
        public Role Role { get; set; }

        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
