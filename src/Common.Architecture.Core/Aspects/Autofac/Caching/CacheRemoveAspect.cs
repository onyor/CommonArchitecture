using Castle.DynamicProxy;
using Common.Architecture.Core.CrossCuttingConcerns.Abstract;
using Common.Architecture.Core.Utilities.Interceptors;
using Common.Architecture.Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Architecture.Core.Aspects.Autofac.Caching
{
    public class CacheRemoveAspect : MethodInterception
    {
        private string _pattern;
        private ICacheService _cacheManager;

        public CacheRemoveAspect(string pattern)
        {
            _pattern = pattern;
            _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheService>();
        }

        protected override void OnSuccess(IInvocation invocation)
        {
            _cacheManager.RemoveByPattern(_pattern);
        }
    }
}
