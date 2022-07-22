using Common.Architecture.Core.DataAccess.Interface;
using Common.Architecture.Core.Entities.Concrete;
using Common.Architecture.Core.Entities.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.Architecture.Core.DataAccess.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : DbContext, new()
    {
        private DbSet<TEntity> _dbSet;

        public EfEntityRepositoryBase()
        {
            using (TContext context = new TContext())
            {
                _dbSet = context.Set<TEntity>();
            }
        }
        public async Task AddAsync(TEntity entity)
        {
            //using (TContext context = new TContext())
            //{
            //    var addedEntity = context.Entry(entity);
            //    addedEntity.State = EntityState.Added;
            //    await context.SaveChangesAsync();
            //}

            await _dbSet.AddAsync(entity);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            //using (TContext context = new TContext())
            //{
            //    return await context.Set<TEntity>().AnyAsync(predicate);
            //}

            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            //using (TContext context = new TContext())
            //{
            //    return await context.Set<TEntity>().CountAsync(predicate);
            //}
            return await _dbSet.CountAsync(predicate);
        }

        public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            //using (TContext context = new TContext())
            //{
            //    var deletedEntity = context.Entry(entity);
            //    deletedEntity.State = EntityState.Deleted;
            //    await context.SaveChangesAsync();
            //}

            var entityToDelete = await GetAsync(predicate);
            using (TContext context = new TContext())
            {
                if (context.Entry(entityToDelete).State == EntityState.Detached) //Concurrency için 
                {
                    _dbSet.Attach(entityToDelete);
                }

                _dbSet.Remove(entityToDelete);
            }
        }

        public async Task<IList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            using (TContext context = new TContext())
            {
                IQueryable<TEntity> query = context.Set<TEntity>();

                if (predicate != null)
                {
                    query = query.Where(predicate);
                }

                if (includeProperties.Any())
                {
                    foreach (var includeProperty in includeProperties)
                    {
                        query = query.Include(includeProperty);
                    }
                }

                return await query.ToListAsync();
            }
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            //using (TContext context = new TContext())
            //{
            //    IQueryable<TEntity> query = context.Set<TEntity>();

            //    if (predicate != null)
            //    {
            //        query = query.Where(predicate);
            //    }

            //    if (includeProperties.Any())
            //    {
            //        foreach (var includeProperty in includeProperties)
            //        {
            //            query = query.Include(includeProperty);
            //        }
            //    }

            //    return await query.SingleOrDefaultAsync();
            //}

            IQueryable<TEntity> query = _dbSet;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (includeProperties.Any())
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.SingleOrDefaultAsync();

        }

        public Task UpdateAsync(TEntity entity)
        {
            //using (TContext context = new TContext())
            //{
            //    var modifiedEntity = context.Entry(entity);
            //    modifiedEntity.State = EntityState.Modified;
            //    await context.SaveChangesAsync();
            //}

            using (TContext context = new TContext())
            {
                _dbSet.Attach(entity);
                context.Entry(entity).State = EntityState.Modified;
            }

            return Task.CompletedTask;
        }
    }
}
