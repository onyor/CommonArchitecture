using Common.Architecture.Core.CrossCuttingConcerns.Abstract;
using Common.Architecture.Core.CrossCuttingConcerns.Microsoft;
using Common.Architecture.Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace Common.Architecture.Core.DependencyResolvers
{
    public class CoreModule : ICoreModule
    {
        public void Load(IServiceCollection serviceCollection)
        {
            serviceCollection.AddMemoryCache();
            serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            serviceCollection.AddSingleton<ICacheService, MemoryCacheService>();
            serviceCollection.AddSingleton<Stopwatch>();
        }
    }
}
