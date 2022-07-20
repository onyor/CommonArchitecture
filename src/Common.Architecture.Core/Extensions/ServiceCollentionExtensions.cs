using Common.Architecture.Core.Utilities.IoC;
using Common.Architecture.Core.Utilities.Security.Encryption;
using Common.Architecture.Core.Utilities.Security.JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Common.Architecture.Core.Extensions
{
    public static class ServiceCollentionExtensions
    {
        public static IServiceCollection AddDependencyResolvers
            (this IServiceCollection serviceCollention, ICoreModule[] modules)
        {
            foreach (var module in modules)
            {
                module.Load(serviceCollention);
            }

            return ServiceTool.Create(serviceCollention);
        }

        public static IServiceCollection AddAuthenticationWithJwt
            (this IServiceCollection serviceCollention, IConfiguration config)
        {
            var tokenOptions = config.GetSection("TokenOptions").Get<TokenOptions>();

            serviceCollention.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidIssuer = tokenOptions.Issuer,
                       ValidAudience = tokenOptions.Audience,
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
                   };
               });

            return serviceCollention;
        }
    }
}
