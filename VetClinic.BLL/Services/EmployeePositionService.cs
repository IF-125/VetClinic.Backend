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
    public class EmployeePositionService : IEmployeePositionService
    {
        private readonly IEmployeePositionRepository _employeePositionRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPositionRepository _positionRepository;

        public EmployeePositionService(IEmployeePositionRepository employeePositionRepository,
            IEmployeeRepository employeeRepository,
            IPositionRepository positionRepository)
        {
            _employeePositionRepository = employeePositionRepository;
            _employeeRepository = employeeRepository;
            _positionRepository = positionRepository;
        }

        public async Task<IList<EmployeePosition>> GetEmployeePositionsAsync()
        {
            return await _employeePositionRepository.GetAsync(asNoTracking: true,
                include: x => x
                .Include(e => e.Employee)
                .Include(p => p.Position));
        }

        public async Task<EmployeePosition> GetByIdAsync(int id)
        {
            var employeePosition = await _employeePositionRepository
                .GetFirstOrDefaultAsync(x => x.Id == id,
                include: x => x
                .Include(e => e.Employee)
                .Include(p => p.Position)) ??
                throw new NotFoundException($"{nameof(EmployeePosition)} {EntityWasNotFound}");

            return employeePosition;
        }

        public async Task InsertAsync(EmployeePosition entity)
        {
            await _employeePositionRepository.InsertAsync(entity);
            await _employeePositionRepository.SaveChangesAsync();
        }

        public void Update(int id, EmployeePosition entityToUpdate)
        {
            if (id != entityToUpdate.Id)
            {
                throw new BadRequestException($"{nameof(EmployeePosition)} {IdsDoNotMatch}");
            }

            _employeePositionRepository.Update(entityToUpdate);
            _employeePositionRepository.SaveChanges();
        }

        public async Task DeleteAsync(int id)
        {
            var employeePositionToDelete = await _employeePositionRepository
                .GetFirstOrDefaultAsync(x => x.Id == id) ??
                throw new NotFoundException($"EmployeePosition {EntityWasNotFound}");

            _employeePositionRepository.Delete(employeePositionToDelete);
            await _employeePositionRepository.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IList<int> listOfIds)
        {
            var employeePositionToDelete = await _employeePositionRepository.GetAsync(x => listOfIds.Contains(x.Id));

            if (employeePositionToDelete.Count() != listOfIds.Count)
            {
                throw new BadRequestException($"{SomeEntitiesInCollectionNotFound} {nameof(EmployeePosition)}s to delete");
            }
            _employeePositionRepository.DeleteRange(employeePositionToDelete);
            await _employeePositionRepository.SaveChangesAsync();
        }

        public async Task AssignPositionToEmployeeAsync(EmployeePosition employeePosition)
        {
            var employee = await _employeeRepository
                .GetFirstOrDefaultAsync(x => x.Id == employeePosition.EmployeeId) ??
                throw new NotFoundException($"Employee {EntityWasNotFound}");

            var position = await _positionRepository
                .GetFirstOrDefaultAsync(x => x.Id == employeePosition.PositionId) ??
                throw new NotFoundException($"Position {EntityWasNotFound}");

            employeePosition.Employee = employee;
            employeePosition.Position = position;
            await _employeePositionRepository.InsertAsync(employeePosition);
            await _employeePositionRepository.SaveChangesAsync();
        }
    }
}
