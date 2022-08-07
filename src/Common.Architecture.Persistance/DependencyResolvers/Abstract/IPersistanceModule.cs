using Common.Architecture.Core.DataAccess.Interface;
using Common.Architecture.Core.Entities.Interface;
using Common.Architecture.Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Architecture.Persistance.DependencyResolvers.Abstract
{
    public interface IPersistanceModule : IModule
    {
        void LoadCustomRepository<TEntity, TRepository>(IServiceCollection serviceCollection)
            where TEntity : class, IEntity, new()
            where TRepository : class, IEntityRepository<TEntity>;
    }
}
