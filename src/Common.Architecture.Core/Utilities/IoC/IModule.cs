using Microsoft.Extensions.DependencyInjection;

namespace Common.Architecture.Core.Utilities.IoC
{
    public interface IModule
    {
        void Load(IServiceCollection serviceCollection);
    }
}
