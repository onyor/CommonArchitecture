using Common.Architecture.Core.Entities.Interface;
using Common.Architecture.Core.Helpers;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.Architecture.Core.DataAccess.Interface
{
    public interface IEntityRepository<TEntity> where TEntity : class, IEntity, new()
    {
        void ChangeTable(string table);
        Task<TEntity> GetFirstOrDefaultAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
             Expression<Func<TEntity, bool>> predicate = null,
             Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
             Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
             bool disableTracking = true,
             bool ignoreQueryFilters = false);

        Task<IList<TEntity>> GetAllAsync();
        Task<IList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null,
                                               Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                               Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                               bool disableTracking = true,
                                               bool ignoreQueryFilters = false);
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
       
    }
}
