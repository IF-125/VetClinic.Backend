using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace VetClinic.Core.Interfaces.Repositories.Base
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        Task<IList<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool asNoTracking = false);

        Task<TEntity> GetFirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>,
                IIncludableQueryable<TEntity, object>> include = null,
            bool asNoTracking = false);

        Task<bool> IsAnyAsync(Expression<Func<TEntity, bool>> filter = null);

        Task InsertAsync(TEntity entity);

        Task InsertRangeAsync(IEnumerable<TEntity> entities);

        void Update(TEntity entityToUpdate);

        void Delete(TEntity entityToDelete);

        void DeleteRange(IEnumerable<TEntity> entitiesToDelete);

        Task SaveAsync();
    }

}
