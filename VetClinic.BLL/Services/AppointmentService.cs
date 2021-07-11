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
    class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        public AppointmentService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<IList<Appointment>> GetAppointmentsAsync(
            Expression<Func<Appointment, bool>> filter = null,
            Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>> orderBy = null,
            Func<IQueryable<Appointment>, IIncludableQueryable<Appointment, object>> include = null,
            bool asNoTracking = false)
        {
            return await _appointmentRepository.GetAsync(filter, orderBy, include, asNoTracking);
        }

        public async Task<Appointment> GetByIdAsync(
            int id,
            Func<IQueryable<Appointment>, IIncludableQueryable<Appointment, object>> include = null,
            bool asNoTracking = false)
        {
            var appointment = await _appointmentRepository.GetFirstOrDefaultAsync(x => x.Id == id, include, asNoTracking);
            if (appointment == null)
            {
                throw new ArgumentException($"{nameof(Appointment)} {EntityWasNotFound}");
            }
            return appointment;
        }

        public async Task InsertAsync(Appointment entity)
        {
            await _appointmentRepository.InsertAsync(entity);
        }

        public void Update(int id, Appointment appointmentToUpdate)
        {
            if (id != appointmentToUpdate.Id)
            {
                throw new ArgumentException($"{nameof(Appointment)} {IdsDoNotMatch}");
            }
            _appointmentRepository.Update(appointmentToUpdate);

        }

        public async Task DeleteAsync(int id)
        {
            var appointmentToDelete = await _appointmentRepository.GetFirstOrDefaultAsync(x => x.Id == id);

            if (appointmentToDelete == null)
            {
                throw new ArgumentException($"{nameof(Appointment)} {EntityWasNotFound}");
            }
            _appointmentRepository.Delete(appointmentToDelete);
        }

        public async Task DeleteRangeAsync(int[] idArr)
        {
            var appointmentsToDelete = await GetAppointmentsAsync(x => idArr.Contains(x.Id));

            if (appointmentsToDelete.Count() != idArr.Length)
            {
                throw new ArgumentException($"{SomeEntitiesInCollectionNotFound} {nameof(Appointment)}s to delete");
            }
            _appointmentRepository.DeleteRange(appointmentsToDelete);
        }
    }
}
