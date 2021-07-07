using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VetClinic.Core.Interfaces.Repositories.Base;
using VetClinic.DAL.Context;

namespace VetClinic.DAL.Repositories.Base
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        private readonly VetClinicDbContext _context;
        public Repository(VetClinicDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets a list of elements that satisfy the specific condition
        /// </summary>
        /// <param name="filter">A condition for selecting elements</param>
        /// <param name="orderBy">A function to order elements</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="asNoTracking"><c>false</c>To disable changing tracking; Otherwise, <c>true</c></param>
        /// <returns>The list of elements that satisfy the condition specified by <paramref name="filter"/>.</returns>
        public async Task<IList<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool asNoTracking = false)
        {
            IQueryable<TEntity> query = GetConfiguredSelection(filter, include, asNoTracking);
            //consider using ternary operator
            if(orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            return await query.ToListAsync();
        }

        /// <summary>
        /// Gets first or default entity based on a filter, orderby and include delegates.
        /// </summary>
        /// <param name="filter">A condition for selecting elements</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="asNoTracking"><c>false</c>To disable changing tracking; Otherwise, <c>true</c></param>
        /// <returns>First or default element that satisfy the condition specified by <paramref name="filter"/>.</returns>
        public async Task<TEntity> GetFirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool asNoTracking = false)
        {
            IQueryable<TEntity> query = GetConfiguredSelection(filter, include, asNoTracking);
            return await query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Determines if there are any elements in a collection that satisfy the condition
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<bool> IsAnyAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return await _context.Set<TEntity>().AnyAsync(filter);
        }

        public async Task InsertAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await SaveAsync();
        }

        public async Task InsertRangeAsync(IEnumerable<TEntity> entities)
        {
            await _context.Set<TEntity>().AddRangeAsync(entities);
            await SaveAsync();
        }

        public void Update(TEntity entityToUpdate)
        {
            _context.Set<TEntity>().Update(entityToUpdate);
            SaveChanges();
        }

        public void Delete(TEntity entityToDelete)
        {
            _context.Set<TEntity>().Remove(entityToDelete);
            SaveChanges();
        }

        public void DeleteRange(IEnumerable<TEntity> entitiesToDelete)
        {
            _context.Set<TEntity>().RemoveRange(entitiesToDelete);
            SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        private void SaveChanges()
        {
            _context.SaveChanges();
        }

        private IQueryable<TEntity> GetConfiguredSelection(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool asNoTracking = false)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query;
        }
    }
}
