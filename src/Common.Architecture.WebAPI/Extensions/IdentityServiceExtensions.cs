using Common.Architecture.Core.Entities.Concrete;
using Common.Architecture.Persistance;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace HRM.WebAPI.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(
            this IServiceCollection services, IConfiguration config)
        {
            services.AddIdentityCore<User>(opt =>
            {
                opt.Password.RequiredLength = 8;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireDigit = false;
                opt.Password.RequireNonAlphanumeric = false;

                opt.User.RequireUniqueEmail = true;
                opt.SignIn.RequireConfirmedEmail = false; // TODO: security risk !!!
                opt.Tokens.EmailConfirmationTokenProvider = "emailconfirmation";

                opt.Lockout.AllowedForNewUsers = true;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                opt.Lockout.MaxFailedAccessAttempts = 3;
            })
                .AddSignInManager<SignInManager<User>>()
                //.AddRoles<IdentityRole>()
                //.AddRoleManager<RoleManager<IdentityRole>>()
                //.AddRoleValidator<RoleValidator<IdentityRole>>()
                .AddEntityFrameworkStores<CommonDBContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}