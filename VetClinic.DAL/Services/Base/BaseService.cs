using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VetClinic.Core.Interfaces.Repositories.Base;
using VetClinic.Core.Interfaces.Services.Base;

namespace VetClinic.DAL.Services.Base
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
    {
        private readonly IRepository<TEntity> _repository;

        public BaseService(IRepository<TEntity> repository)
        {
            _repository = repository;
        }
        public async Task<IList<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool asNoTracking = false)
        {
            return await _repository
                .GetAsync(filter, orderBy, include, asNoTracking);
        }

        public async Task<TEntity> GetFirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool asNoTracking = false)
        {
            return await _repository
                .GetFirstOrDefaultAsync(filter, include, asNoTracking);
        }
        
        public async Task InsertAsync(TEntity entity)
        {
            await _repository.InsertAsync(entity);
        }

        public async Task InsertRangeAsync(IEnumerable<TEntity> entities)
        {
            await _repository.InsertRangeAsync(entities);
        }

        public void Update(TEntity entityToUpdate)
        {
            _repository.Update(entityToUpdate);
        }

        public void Delete(TEntity entityToDelete)
        {
            _repository.Delete(entityToDelete);
        }

        public void DeleteRange(IEnumerable<TEntity> entitiesToDelete)
        {
            _repository.DeleteRange(entitiesToDelete);
        }

    }
}
