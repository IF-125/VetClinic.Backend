using static VetClinic.Core.Resources.TextMessages;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Repositories;
using VetClinic.Core.Interfaces.Services;
using SendGrid.Helpers.Errors.Model;

namespace VetClinic.BLL.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;

        public ScheduleService(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
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

            return schedule;
        }
    }
}
