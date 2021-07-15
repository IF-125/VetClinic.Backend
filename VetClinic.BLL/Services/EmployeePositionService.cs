using static VetClinic.Core.Resources.TextMessages;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Repositories;
using VetClinic.Core.Interfaces.Services;
using System.Collections.Generic;
using System.Linq.Expressions;

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

        public async Task<IList<EmployeePosition>> GetEmployeePositionsAsync(
            Expression<Func<EmployeePosition, bool>> filter = null,
            Func<IQueryable<EmployeePosition>, IOrderedQueryable<EmployeePosition>> orderBy = null,
            Func<IQueryable<EmployeePosition>, IIncludableQueryable<EmployeePosition, object>> include = null,
            bool asNoTracking = false)
        {
            return await _employeePositionRepository.GetAsync(filter, orderBy, include, asNoTracking);
        }

        public async Task<EmployeePosition> GetByIdAsync(
            int id)
        {
            var employeePosition = await _employeePositionRepository.GetFirstOrDefaultAsync(x => x.Id == id);
            if (employeePosition == null)
            {
                throw new ArgumentException($"{nameof(EmployeePosition)} {EntityWasNotFound}");
            }

            return employeePosition;
        }

        public async Task InsertAsync(EmployeePosition entity)
        {
            await _employeePositionRepository.InsertAsync(entity);
        }

        public void Update(int id, EmployeePosition entityToUpdate)
        {
            if (id != entityToUpdate.Id)
            {
                throw new ArgumentException($"{nameof(EmployeePosition)} {IdsDoNotMatch}");
            }

            _employeePositionRepository.Update(entityToUpdate);
        }

        public async Task DeleteAsync(int id)
        {
            var employeePositionToDelete = await _employeePositionRepository.GetFirstOrDefaultAsync(x => x.Id == id);

            ThrowExceptionIfNotFound(nameof(EmployeePosition), employeePositionToDelete);

            _employeePositionRepository.Delete(employeePositionToDelete);
        }

        public async Task DeleteRangeAsync(IList<int> listOfIds)
        {
            var employeePositionToDelete = await _employeePositionRepository.GetAsync(x => listOfIds.Contains(x.Id));

            if (employeePositionToDelete.Count() != listOfIds.Count)
            {
                throw new ArgumentException($"{SomeEntitiesInCollectionNotFound} {nameof(EmployeePosition)}s to delete");
            }
            _employeePositionRepository.DeleteRange(employeePositionToDelete);
        }

        public async Task AssignPositionToEmployeeAsync(EmployeePosition employeePosition)
        {
            var employee = await _employeeRepository.GetFirstOrDefaultAsync(x => x.Id == employeePosition.EmployeeId);
            var position = await _positionRepository.GetFirstOrDefaultAsync(x => x.Id == employeePosition.PositionId);

            //TODO: Find out if it is a good approach
            ThrowExceptionIfNotFound(nameof(Employee), employee);
            ThrowExceptionIfNotFound(nameof(Position), position);

            employeePosition.Employee = employee;
            employeePosition.Position = position;
            await InsertAsync(employeePosition);
        }

        private void ThrowExceptionIfNotFound<T>(string entityName, T entity)
            where T : class
        {
            if(entity == null)
            {
                throw new ArgumentException($"{entityName} {EntityWasNotFound}");
            }
        }
    }
}
