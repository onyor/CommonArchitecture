using Microsoft.Extensions.DependencyInjection;

namespace Common.Architecture.Core.Utilities.IoC
{
    public interface ICoreModule
    {
        void Load(IServiceCollection serviceCollection);
    }
}
