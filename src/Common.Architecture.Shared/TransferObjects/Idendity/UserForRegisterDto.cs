using Common.Architecture.Core.Entities.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Architecture.Shared.TransferObjects.Idendity
{
    public class UserForRegisterDto : IDto
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
        public string LdapKod { get; set; }

        public UserRoleDto[] UserRoles { get; set; }
        public long? PersonelTanimId { get; set; }
        public long? PersonelSicilNo { get; set; }
        public string SorumluBirimList { get; set; }

        // This properties are required when login the system. For this function -->  _signInManager.CheckPasswordSignInAsync()
        public string PasswordHash { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}
