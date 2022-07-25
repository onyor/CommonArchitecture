using Common.Architecture.Core.Entities.Concrete;
using Common.Architecture.Persistance.InitialData;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;

namespace Common.Architecture.Persistance
{
    public class CommonDBContext : IdentityDbContext<
            User,
            Role,
            Guid,
            IdentityUserClaim<Guid>,
            UserRole,
            IdentityUserLogin<Guid>,
            IdentityRoleClaim<Guid>,
            IdentityUserToken<Guid>>
    {
        private readonly string _connectionString;

        private SeedData seedData = new SeedData();
        public CommonDBContext(DbContextOptions<CommonDBContext> options)
          : base(options)
        {
        }

        public CommonDBContext(string connectionString) => _connectionString = connectionString;

        public CommonDBContext()
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_connectionString != null)
                optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);

            this.seedData.SeedUsers(builder);
            this.seedData.SeedRoles(builder);
            this.seedData.SeedUserRoles(builder);
        }
    }
}
