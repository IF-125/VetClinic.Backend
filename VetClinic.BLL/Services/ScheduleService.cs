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
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public ScheduleService(IScheduleRepository scheduleRepository, 
            IEmployeeRepository employeeRepository)
        {
            _scheduleRepository = scheduleRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<Schedule> GetByIdAsync(int id)
        {
            var schedude = await _scheduleRepository.GetFirstOrDefaultAsync(x => x.Id == id) ?? 
                throw new NotFoundException($"{nameof(Schedule)} {EntityWasNotFound}");

            return schedude;
        }

        public async Task InsertAsync(Schedule entity)
        {
            await _scheduleRepository.InsertAsync(entity);
            await _scheduleRepository.SaveChangesAsync();
        }

        public async Task InsertRangeAsync(IEnumerable<Schedule> schedule)
        {
            await _scheduleRepository.InsertRangeAsync(schedule);
            await _scheduleRepository.SaveChangesAsync();
        }

        public void Update(int id, Schedule entityToUpdate)
        {
            if (id != entityToUpdate.Id)
            {
                throw new BadRequestException($"{nameof(Schedule)} {IdsDoNotMatch}");
            }

            _scheduleRepository.Update(entityToUpdate);
            _scheduleRepository.SaveChanges();
        }

        public async Task DeleteAsync(int id)
        {
            var scheduleToDelete = await _scheduleRepository.GetFirstOrDefaultAsync(x => x.Id == id) ??
                throw new NotFoundException($"{nameof(Schedule)} {EntityWasNotFound}");

            _scheduleRepository.Delete(scheduleToDelete);
            await _scheduleRepository.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IList<int> listOfIds)
        {
            var scheduleToDelete = await _scheduleRepository.GetAsync(x => listOfIds.Contains(x.Id));

            if (scheduleToDelete.Count() != listOfIds.Count)
            {
                throw new BadRequestException($"{SomeEntitiesInCollectionNotFound} {nameof(Schedule)}s to delete");
            }
            _scheduleRepository.DeleteRange(scheduleToDelete);
            await _scheduleRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Schedule>> GetScheduleOfEmployee(string emoloyeeId)
        {
            var schedule = await _scheduleRepository.GetAsync(x => x.EmployeeId == emoloyeeId);

            if(schedule == null || !schedule.Any())
            {
                throw new NotFoundException("No schedule was provided for this employee");
            }

            return schedule;
        }

        public async Task AssignScheduleToEmployeeAsync(Schedule schedule, string employeeId)
        {
            var employee = await GetEmployeeAsync(employeeId);

            await _scheduleRepository.InsertAsync(schedule);

            employee.Schedule.Add(schedule);

            _employeeRepository.Update(employee);
            await _scheduleRepository.SaveChangesAsync();
        }

        public async Task AssignMultipleSchedulesToEmployeeAsync(IEnumerable<Schedule> schedule, string employeeId)
        {
            var employee = await GetEmployeeAsync(employeeId);

            await _scheduleRepository.InsertRangeAsync(schedule);

            employee.Schedule = schedule.ToList();

            _employeeRepository.Update(employee);
            await _scheduleRepository.SaveChangesAsync();
        }

        private async Task<Employee> GetEmployeeAsync(string employeeId)
        {
            return await _employeeRepository.GetFirstOrDefaultAsync(x => x.Id == employeeId, include: x => x
            .Include(s => s.Schedule)) ??
                throw new NotFoundException($"{nameof(Employee)} {EntityWasNotFound}");
        }
    }
}
