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
    public interface IEmployeePositionService : IBaseService<EmployeePosition, int>
    {
        public Task AssignPositionToEmployeeAsync(EmployeePosition employeePosition);

        public Task<IList<EmployeePosition>> GetEmployeePositionsAsync(
            Expression<Func<EmployeePosition, bool>> filter = null,
            Func<IQueryable<EmployeePosition>, IOrderedQueryable<EmployeePosition>> orderBy = null,
            Func<IQueryable<EmployeePosition>, IIncludableQueryable<EmployeePosition, object>> include = null,
            bool asNoTracking = false);
    }
}
