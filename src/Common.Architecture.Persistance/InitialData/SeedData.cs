using Common.Architecture.Core.Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Common.Architecture.Persistance.InitialData
{
    public class SeedData
    {
        Guid guidIdUser1 = Guid.NewGuid();
        Guid guidIdUser2 = Guid.NewGuid();
        Guid guidIdRole1 = Guid.NewGuid();
        Guid guidIdRole2 = Guid.NewGuid();

        public void SeedUsers(ModelBuilder builder)
        {
            var admin = new User
            {
                Id = guidIdUser1,
                Name = "System",
                Surname = "Administrator",
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
                EmailConfirmed = true,
                PhoneNumber = "905322223344"
            };

            // Normal User
            var firstUser = new User
            {
                Id = guidIdUser2,
                Name = "Onur",
                Surname = "YILDIZ",
                UserName = "onuryldz008@gmail.com",
                Email = "onuryldz008@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "905315875631"
            };


            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
            passwordHasher.HashPassword(admin, "Test1234");
            builder.Entity<User>().HasData(admin);

            passwordHasher.HashPassword(firstUser, "Test1234");
            builder.Entity<User>().HasData(firstUser);
        }


        public void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole<Guid>>().HasData(
                new IdentityRole<Guid>() { Id = guidIdRole1, Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
                new IdentityRole<Guid>() { Id = guidIdRole2, Name = "Uye", ConcurrencyStamp = "2", NormalizedName = "Human Resource" }
                );
        }

        public void SeedUserRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityUserRole<Guid>>().HasData(
                new IdentityUserRole<Guid>()
                {
                    RoleId = guidIdRole1,
                    UserId = guidIdUser1
                });

            builder.Entity<IdentityUserRole<Guid>>().HasData(
                new IdentityUserRole<Guid>()
                {
                    RoleId = guidIdRole2,
                    UserId = guidIdUser2
                });
        }
    }
}
