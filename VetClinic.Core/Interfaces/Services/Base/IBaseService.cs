using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VetClinic.Core.Entities;

namespace VetClinic.Core.Interfaces.Services.Base
{
    public interface IBaseService<TEntity, TIdType> 
        where TEntity : class
    {
        public Task<TEntity> GetByIdAsync(
            TIdType id,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool asNoTracking = false);

        public Task InsertAsync(TEntity entity);

        public void Update(TIdType id, TEntity entityToUpdate);

        public Task DeleteAsync(TIdType id);

        public Task DeleteRangeAsync(TIdType[] idArr);
    }
}
