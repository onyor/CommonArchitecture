using Castle.DynamicProxy;
using Common.Architecture.Core.Extensions;
using Common.Architecture.Core.Utilities.Interceptors;
using Common.Architecture.Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Common.Architecture.Infrastructure.Business.BusinessAspect.Autofac
{
    public class SecuredOperation : MethodInterception
    {
        private readonly string[] _roles;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SecuredOperation(params string[] roles)
        {
            _roles = roles;
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
        }
        protected override void OnBefore(IInvocation invocation)
        {
            var roleClaims = _httpContextAccessor.HttpContext.User.ClaimRoles();

            foreach (var role in _roles)
            {
                if (roleClaims.Contains(role))
                {
                    return;
                }
            }
            throw new Exception("Yetkiniz yok");
        }
    }
}
