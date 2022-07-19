using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Architecture.Shared.TransferObjects.Idendity
{
    public class UserRoleDto
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }

        public string UserName { get; set; }
        public string RoleName { get; set; }
    }
}
