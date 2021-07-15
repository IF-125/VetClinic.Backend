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
    public interface IProcedureService : IBaseService<Procedure, int>
    {
        public Task<IList<Procedure>> GetProceduresAsync(
            Expression<Func<Procedure, bool>> filter = null,
            Func<IQueryable<Procedure>, IOrderedQueryable<Procedure>> orderBy = null,
            Func<IQueryable<Procedure>, IIncludableQueryable<Procedure, object>> include = null,
            bool asNoTracking = false);
    }
}
