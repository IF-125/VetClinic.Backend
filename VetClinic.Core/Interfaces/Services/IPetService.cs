using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Services.Base;

namespace VetClinic.Core.Interfaces.Services
{
    public interface IPetService:IBaseService<Pet, int>
    {
        public Task<IList<Pet>> GetPetsAsync(
            Expression<Func<Pet, bool>> filter = null,
            Func<IQueryable<Pet>, IOrderedQueryable<Pet>> orderBy = null,
            Func<IQueryable<Pet>, IIncludableQueryable<Pet, object>> include = null,
            bool asNoTracking = false);
    }
}
