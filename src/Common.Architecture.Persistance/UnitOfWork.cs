using Common.Architecture.Core.DataAccess.EntityFramework;
using Common.Architecture.Core.DataAccess.Interface;
using Common.Architecture.Persistance.Abstract;
using Common.Architecture.Persistance.Concrete.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Architecture.Persistance
{
    public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        private readonly TContext _context;
        private Dictionary<Type, object> repositories;
        private bool disposed = false;


        private EfUserDal _userDal;
        public IUserDal Users => _userDal ?? new EfUserDal();
        public UnitOfWork(TContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void ChangeDatabase(string database)
        {
            throw new NotImplementedException();
        }

        //public IEntityRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : class
        //{
        //    if (repositories == null)
        //    {
        //        repositories = new Dictionary<Type, object>();
        //    }

        //    // what's the best way to support custom reposity?
        //    if (hasCustomRepository)
        //    {
        //        var customRepo = _context.GetService<IEntityRepository<TEntity>>();
        //        if (customRepo != null)
        //        {
        //            return customRepo;
        //        }
        //    }

        //    var type = typeof(TEntity);
        //    if (!repositories.ContainsKey(type))
        //    {
        //        repositories[type] = new EfEntityRepositoryBase<TEntity, TContext>();
        //    }

        //    return (IEntityRepository<TEntity>)repositories[type];
        //}

        public int SaveChanges(bool ensureAutoHistory = false)
        {
            throw new NotImplementedException();
        }

        public async Task<int> SaveChangesAsync(bool ensureAutoHistory = false)
        {

            return await _context.SaveChangesAsync();
        }

        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] parameters) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public void TrackGraph(object rootEntity, Action<EntityEntryGraphNode> callback)
        {
            throw new NotImplementedException();
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }
}
