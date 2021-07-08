using static VetClinic.Core.Resources.TextMessages;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Repositories;
using VetClinic.Core.Interfaces.Services;

namespace VetClinic.BLL.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<IList<Employee>> GetEmployees(
            Expression<Func<Employee, bool>> filter = null,
            Func<IQueryable<Employee>, IOrderedQueryable<Employee>> orderBy = null,
            Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>> include = null,
            bool asNoTracking = false)
        {
            return await _employeeRepository.GetAsync(filter, orderBy, include, asNoTracking);
        }

        public async Task<Employee> GetByIdAsync(
            string id,
            Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>> include = null,
            bool asNoTracking = false)
        {
            var employee = await _employeeRepository.GetFirstOrDefaultAsync(x => x.Id == id, include, asNoTracking);
            if(employee == null)
            {
                throw new ArgumentException($"{nameof(Employee)} {EntityWasNotFound}");
            }
            return employee;
        }

        public async Task InsertAsync(Employee entity)
        {
            await _employeeRepository.InsertAsync(entity);
        }

        public void Update(Employee entityToUpdate)
        {
            throw new System.NotImplementedException();
        }

        public async Task DeleteAsync(string id)
        {
            var employeeToDelete = await _employeeRepository.GetFirstOrDefaultAsync(x => x.Id == id);

            if(employeeToDelete == null)
            {
                throw new ArgumentException($"{nameof(Employee)} {EntityWasNotFound}");
            }
            _employeeRepository.Delete(employeeToDelete);
        }

        public async Task DeleteRangeAsync(string[] idArr)
        {
            var employeesToDelete = await GetEmployees(x => idArr.Contains(x.Id));

            if(employeesToDelete.Any(i => i == null))
            {
                throw new ArgumentException($"{SomeEntitiesInCollectionNotFound} {nameof(Employee)}s to delete");
            }
            _employeeRepository.DeleteRange(employeesToDelete);
        }
    }
}
