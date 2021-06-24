using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace VetClinic.Core.Interfaces.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        Task<IList<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            bool asNoTracking = false);

        Task<TEntity> GetByIdAsync(object id);

        Task<TEntity> GetFirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> filter = null,
            string includeProperties = "",
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
