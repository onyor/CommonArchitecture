using Common.Architecture.Core.DataAccess.Abstract;
using Common.Architecture.Core.DataAccess.Interface;
using Common.Architecture.Core.Entities.Interface;
using Common.Architecture.Persistance.Abstract;
using Common.Architecture.Persistance.DependencyResolvers.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Common.Architecture.Persistance.DependencyResolvers
{
    public class PersistanceModule : IPersistanceModule
    {
        //public void Load<TContext>(IServiceCollection serviceCollection) where TContext : DbContext
        //{
        //    serviceCollection.AddScoped<IRepositoryFactory, UnitOfWork<TContext>>();
        //    serviceCollection.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
        //    serviceCollection.AddScoped<IUnitOfWork<TContext>, UnitOfWork<TContext>>();
        //}

        public void Load(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IRepositoryFactory, UnitOfWork<CommonDBContext>>();
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork<CommonDBContext>>();
            serviceCollection.AddScoped<IUnitOfWork<CommonDBContext>, UnitOfWork<CommonDBContext>>();
        }

        public void LoadCustomRepository<TEntity, TRepository>(IServiceCollection serviceCollection)
            where TEntity : class, IEntity, new()
            where TRepository : class, IEntityRepository<TEntity>
        {
            serviceCollection.AddScoped<IEntityRepository<TEntity>, TRepository>();
        }
    }
}