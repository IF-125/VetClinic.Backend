using static VetClinic.Core.Resources.TextMessages;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Repositories;
using VetClinic.Core.Interfaces.Services;

namespace VetClinic.BLL.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;

        public ScheduleService(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        public async Task<Schedule> GetByIdAsync(
            int id,
            Func<IQueryable<Schedule>, IIncludableQueryable<Schedule, object>> include = null,
            bool asNoTracking = false)
        {
            var schedude = await _scheduleRepository.GetFirstOrDefaultAsync(x => x.Id == id, include, asNoTracking);
            if(schedude == null)
            {
                throw new ArgumentException($"{nameof(Schedule)} {EntityWasNotFound}");
            }

            return schedude;
        }

        public async Task InsertAsync(Schedule entity)
        {
            await _scheduleRepository.InsertAsync(entity);
        }

        public async Task InsertRangeAsync(IEnumerable<Schedule> schedule)
        {
            await _scheduleRepository.InsertRangeAsync(schedule);
        }

        public void Update(int id, Schedule entityToUpdate)
        {
            if (id != entityToUpdate.Id)
            {
                throw new ArgumentException($"{nameof(Schedule)} {IdsDoNotMatch}");
            }

            _scheduleRepository.Update(entityToUpdate);
        }

        public async Task DeleteAsync(int id)
        {
            var scheduleToDelete = await _scheduleRepository.GetFirstOrDefaultAsync(x => x.Id == id);

            if(scheduleToDelete == null)
            {
                throw new ArgumentException($"{nameof(Schedule)} {EntityWasNotFound}");
            }

            _scheduleRepository.Delete(scheduleToDelete);
        }

        public async Task DeleteRangeAsync(int[] idArr)
        {
            var scheduleToDelete = await _scheduleRepository.GetAsync(x => idArr.Contains(x.Id));

            if (scheduleToDelete.Count() != idArr.Length)
            {
                throw new ArgumentException($"{SomeEntitiesInCollectionNotFound} {nameof(Schedule)}s to delete");
            }
            _scheduleRepository.DeleteRange(scheduleToDelete);
        }

        public async Task<IEnumerable<Schedule>> GetScheduleOfEmployee(string emoloyeeId)
        {
            var schedule = await _scheduleRepository.GetAsync(x => x.EmployeeId == emoloyeeId);

            if(schedule == null)
            {
                throw new ArgumentException($"No schedule for employee of id {emoloyeeId} provided");
            }

            return schedule;
        }
    }
}
