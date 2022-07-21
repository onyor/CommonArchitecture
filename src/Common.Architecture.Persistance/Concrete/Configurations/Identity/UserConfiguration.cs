using Common.Architecture.Core.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Common.Architecture.Persistance.Concrete.Configurations.Identity
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(c => c.Surname)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(50);
            //builder.Property(c => c.PasswordSalt)
            //    .IsRequired()
            //    .HasMaxLength(500);

            var dateTime = new DateTime(2021, 1, 1);
        }
    }
}
