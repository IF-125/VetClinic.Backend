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
    public interface IOrderProcedureService : IBaseService<OrderProcedure, int>
    {
        public Task<IList<OrderProcedure>> GetOrderProceduresAsync(
            Expression<Func<OrderProcedure, bool>> filter = null,
            Func<IQueryable<OrderProcedure>, IOrderedQueryable<OrderProcedure>> orderBy = null,
            Func<IQueryable<OrderProcedure>, IIncludableQueryable<OrderProcedure, object>> include = null,
            bool asNoTracking = false);
    }
}
