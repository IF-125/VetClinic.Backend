using System;
using System.Linq.Expressions;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Repositories.Base;

namespace VetClinic.Core.Interfaces.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee> 
    {
        public bool IsAny(Expression<Func<Employee, bool>> filter);
    }
}
