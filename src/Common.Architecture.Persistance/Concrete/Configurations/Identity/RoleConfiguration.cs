using Common.Architecture.Core.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Common.Architecture.Persistance.Concrete.Configurations.Identity

{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");
            builder.Property(c => c.Id)
                .IsRequired();
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);
            //builder.Property(c => c.PasswordSalt)
            //    .IsRequired()
            //    .HasMaxLength(500);

            var dateTime = new DateTime(2021, 1, 1);
        }
    }
}
