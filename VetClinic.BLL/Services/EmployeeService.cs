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
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<IList<Employee>> GetEmployeesAsync()
        {
            return await _employeeRepository.GetAsync(asNoTracking: true);
        }   

        public async Task<Employee> GetByIdAsync(string id)
        {
            var employee = await _employeeRepository.GetFirstOrDefaultAsync(x => x.Id == id) ??
                throw new NotFoundException($"{nameof(Employee)} {EntityWasNotFound}");

            return employee;
        }

        public async Task InsertAsync(Employee entity)
        {
            await _employeeRepository.InsertAsync(entity);
            await _employeeRepository.SaveChangesAsync();
        }

        public void Update(string id, Employee employeeToUpdate)
        {
            var employee = _employeeRepository.GetFirstOrDefaultAsync(x => x.Id == id) ??
                throw new NotFoundException($"{nameof(Employee)} {EntityWasNotFound}");
       
            _employeeRepository.Update(employeeToUpdate);
            _employeeRepository.SaveChanges();
        }

        public async Task DeleteAsync(string id)
        {
            var employeeToDelete = await _employeeRepository.GetFirstOrDefaultAsync(x => x.Id == id);

            if(employeeToDelete == null)
            {
                throw new NotFoundException($"{nameof(Employee)} {EntityWasNotFound}");
            }

            _employeeRepository.Delete(employeeToDelete);
            await _employeeRepository.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IList<string> listOfIds)
        {
            var employeesToDelete = await _employeeRepository.GetAsync(x => listOfIds.Contains(x.Id));

            if(employeesToDelete.Count() != listOfIds.Count)
            {
                throw new BadRequestException($"{SomeEntitiesInCollectionNotFound} {nameof(Employee)}s to delete");
            }
            _employeeRepository.DeleteRange(employeesToDelete);
            await _employeeRepository.SaveChangesAsync();
        }
    }
}
