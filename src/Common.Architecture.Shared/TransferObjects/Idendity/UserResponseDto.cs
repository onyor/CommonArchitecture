using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Architecture.Shared.TransferObjects.Idendity
{
    public class UserResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Token { get; set; }
        public int ExpirationMinutes { get; set; }

        public string UserRolesCsv { get; set; }
        public Guid? CurrentRoleId { get; set; }
        public string CurrentRoleName { get; set; }

        public long? PersonelTanimId { get; set; }
        public long? PersonelSicilNo { get; set; }
        public string SorumluBirimList { get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool EmailConfirmed { get; set; }


        public Collection<UserRoleDto> UserRoles { get; set; }

        //public List<UserOperationClaim> UserOperationClaims { get; set; }

    }
}
