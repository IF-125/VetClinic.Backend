using static VetClinic.Core.Resources.TextMessages;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Repositories;
using VetClinic.Core.Interfaces.Services;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace VetClinic.BLL.Services
{
    public class SalaryService : ISalaryService
    {
        private readonly ISalaryRepository _salaryRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeePositionRepository _employeePositionRepository;

        public SalaryService(ISalaryRepository salaryRepository,
            IEmployeeRepository employeeRepository,
            IEmployeePositionRepository employeePositionRepository)
        {
            _salaryRepository = salaryRepository;
            _employeeRepository = employeeRepository;
            _employeePositionRepository = employeePositionRepository;
        }

        public async Task<Salary> GetByIdAsync(
            int id,
            Func<IQueryable<Salary>, IIncludableQueryable<Salary, object>> include = null,
            bool asNoTracking = false)
        {
            var salary = await _salaryRepository.GetFirstOrDefaultAsync(x => x.Id == id, include, asNoTracking);
            if(salary == null)
            {
                throw new ArgumentException($"{nameof(Salary)} {EntityWasNotFound}");
            }

            return salary;
        }

        public async Task InsertAsync(Salary entity)
        {
            await _salaryRepository.InsertAsync(entity); 
        }

        public void Update(int id, Salary entityToUpdate)
        { 
            if (id != entityToUpdate.Id)
            {
                throw new ArgumentException($"{nameof(Salary)} {IdsDoNotMatch}");
            }

            _salaryRepository.Update(entityToUpdate);
        }

        public async Task DeleteAsync(int id)
        {
            var salaryToDelete = await _salaryRepository.GetFirstOrDefaultAsync(x => x.Id == id);

            if (salaryToDelete == null)
            {
                throw new ArgumentException($"{nameof(Salary)} {EntityWasNotFound}");
            }

            _salaryRepository.Delete(salaryToDelete);
        }

        public async Task DeleteRangeAsync(int[] idArr)
        {
            var salaryToDelete = await _salaryRepository.GetAsync(x => idArr.Contains(x.Id));

            if (salaryToDelete.Count() != idArr.Length)
            {
                throw new ArgumentException($"{SomeEntitiesInCollectionNotFound} {nameof(Salary)}s to delete");
            }
            _salaryRepository.DeleteRange(salaryToDelete);
        }

        public async Task AssignSalaryToEmployee(string employeeId, Salary salary)
        {
            var employee = await _employeeRepository
                .GetFirstOrDefaultAsync(x => x.Id == employeeId,
                include: x => x.Include(x => x.EmployeePosition).ThenInclude(x => x.Salaries));

            if(employee == null)
            {
                throw new ArgumentException($"{nameof(Employee)} {EntityWasNotFound}");
            }
            else if(salary == null)
            {
                throw new ArgumentNullException(string.Empty, "Salary was null");
            }

            await _salaryRepository.InsertAsync(salary);
            employee.EmployeePosition.Salaries.Add(salary);
            _employeeRepository.Update(employee);
        }

        public async Task<IEnumerable<Salary>> GetSalariesOfEmployee(int employeePositionId)
        {
            var salaries = (await _employeePositionRepository
                .GetFirstOrDefaultAsync(x => x.Id == employeePositionId,
                include: x => x.Include(s => s.Salaries)))
                .Salaries;

            if(salaries == null)
            {
                throw new ArgumentException("Employee has no salaries");
            }

            return salaries;
        }
    }
}
