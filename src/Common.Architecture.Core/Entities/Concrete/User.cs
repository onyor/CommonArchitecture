using Common.Architecture.Core.Entities.Interface;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Architecture.Core.Entities.Concrete
{
    public class User : IdentityUser<Guid>, IEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Fullname => this.Name + " " + this.Surname;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
    }
}
