using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Services.Base;

namespace VetClinic.Core.Interfaces.Services
{
    public interface IPositionService : IBaseService<Position, int>
    {
        public Task<IList<Position>> GetPositionsAsync(
            Expression<Func<Position, bool>> filter = null,
            Func<IQueryable<Position>, IOrderedQueryable<Position>> orderBy = null,
            Func<IQueryable<Position>, IIncludableQueryable<Position, object>> include = null,
            bool asNoTracking = false);
    }
}
