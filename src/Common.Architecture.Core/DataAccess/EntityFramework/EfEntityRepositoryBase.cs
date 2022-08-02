using Common.Architecture.Core.DataAccess.Interface;
using Common.Architecture.Core.Entities.Concrete;
using Common.Architecture.Core.Entities.Interface;
using Common.Architecture.Core.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
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
        private IncludeHelpers<TEntity> includeHelpers;
        public EfEntityRepositoryBase()
        {
            using (TContext context = new TContext())
            {
                _dbSet = context.Set<TEntity>();
            }
            includeHelpers = new IncludeHelpers<TEntity>();


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
                foreach (var include in includeProperties)
                {
                    //query = query.Include(include).ThenInclude(x=>x.);
                }
            }

            return await query.SingleOrDefaultAsync();

        }



        /// <summary>
        /// Gets the first or default entity based on a predicate, orderby delegate and include delegate. This method default no-tracking query.
        /// </summary>
        /// <param name="selector">The selector for projection.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <returns>An <see cref="IPagedList{TEntity}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        public TResult GetFirstOrDefault<TResult>(Expression<Func<TEntity, TResult>> selector,
                                                  Expression<Func<TEntity, bool>> predicate = null,
                                                  Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                  Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                  bool disableTracking = true)
        {
            IQueryable<TEntity> query = _dbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).Select(selector).FirstOrDefault();
            }
            else
            {
                return query.Select(selector).FirstOrDefault();
            }
        }
    }
}
