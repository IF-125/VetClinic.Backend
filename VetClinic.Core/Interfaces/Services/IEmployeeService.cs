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
    public interface IEmployeeService : IBaseService<Employee, string>
    {
        public Task<IList<Employee>> GetEmployees(
            Expression<Func<Employee, bool>> filter = null,
            Func<IQueryable<Employee>, IOrderedQueryable<Employee>> orderBy = null,
            Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>> include = null,
            bool asNoTracking = false);
    }
}
