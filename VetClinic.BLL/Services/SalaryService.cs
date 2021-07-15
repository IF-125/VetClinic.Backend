using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Errors.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Repositories;
using VetClinic.Core.Interfaces.Services;
using static VetClinic.Core.Resources.TextMessages;

namespace VetClinic.BLL.Services
{
    public class SalaryService : ISalaryService
    {
        private readonly ISalaryRepository _salaryRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public SalaryService(ISalaryRepository salaryRepository,
            IEmployeeRepository employeeRepository)
        {
            _salaryRepository = salaryRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<Salary> GetByIdAsync(int id)
        {
            var salary = await _salaryRepository.GetFirstOrDefaultAsync(x => x.Id == id) ??
                throw new NotFoundException($"{nameof(Salary)} {EntityWasNotFound}");

            return salary;
        }

        public async Task InsertAsync(Salary entity)
        {
            await _salaryRepository.InsertAsync(entity);
            await _salaryRepository.SaveChangesAsync();
        }

        public void Update(int id, Salary entityToUpdate)
        { 
            if (id != entityToUpdate.Id)
            {
                throw new BadRequestException($"{nameof(Salary)} {IdsDoNotMatch}");
            }

            _salaryRepository.Update(entityToUpdate);
            _salaryRepository.SaveChanges();
        }

        public async Task DeleteAsync(int id)
        {
            var salaryToDelete = await _salaryRepository.GetFirstOrDefaultAsync(x => x.Id == id) ??
                 throw new NotFoundException($"{nameof(Salary)} {EntityWasNotFound}");

            _salaryRepository.Delete(salaryToDelete);
            await _salaryRepository.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IList<int> listOfIds)
        {
            var salaryToDelete = await _salaryRepository.GetAsync(x => listOfIds.Contains(x.Id));

            if (salaryToDelete.Count() != listOfIds.Count)
            {
                throw new BadRequestException($"{SomeEntitiesInCollectionNotFound} {nameof(Salary)}s to delete");
            }
            _salaryRepository.DeleteRange(salaryToDelete);
            await _salaryRepository.SaveChangesAsync();
        }

        public async Task AssignSalaryToEmployee(string employeeId, Salary salary)
        {
            var employee = await _employeeRepository
                .GetFirstOrDefaultAsync(x => x.Id == employeeId,
                include: x => x
                .Include(x => x.EmployeePosition)
                .ThenInclude(x => x.Salaries)) ??
                throw new NotFoundException($"{nameof(Employee)} {EntityWasNotFound}");
            
            if(salary == null)
            {
                throw new BadRequestException("Salary was null");
            }

            await _salaryRepository.InsertAsync(salary);
            employee.EmployeePosition.Salaries.Add(salary);
            _employeeRepository.Update(employee);
            await _salaryRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Salary>> GetSalariesOfEmployee(int employeePositionId)
        {
            var salaries = await _salaryRepository.GetAsync(x => x.EmployeePositionId == employeePositionId);

            if (salaries == null || !salaries.Any())
            {
                throw new BadRequestException("Employee has no salaries");
            }

            return salaries;
        }
    }
}
