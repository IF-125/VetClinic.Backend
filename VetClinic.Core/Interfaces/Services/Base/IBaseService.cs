﻿using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace VetClinic.Core.Interfaces.Services.Base
{
    public interface IBaseService<TEntity> 
        where TEntity : class
    {
        public Task<IList<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool asNoTracking = false);

        public Task<TEntity> GetFirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool asNoTracking = false);
        public Task InsertAsync(TEntity entity);

        public Task InsertRangeAsync(IEnumerable<TEntity> entities);

        public void Update(TEntity entityToUpdate);

        public void Delete(TEntity entityToDelete);

        public void DeleteRange(IEnumerable<TEntity> entitiesToDelete);
    }
}
