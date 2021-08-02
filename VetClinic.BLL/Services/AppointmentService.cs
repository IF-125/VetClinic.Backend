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
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        public AppointmentService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<IList<Appointment>> GetAppointmentsAsync()
        {
            return await _appointmentRepository.GetAsync(asNoTracking: true);
        }

        public async Task<Appointment> GetByIdAsync(int id)
        {
            var appointment = await _appointmentRepository
                .GetFirstOrDefaultAsync(x => x.Id == id) ??
                    throw new NotFoundException($"{nameof(Appointment)} {EntityWasNotFound}");
            
            return appointment;
        }

        public async Task InsertAsync(Appointment entity)
        {
            await _appointmentRepository.InsertAsync(entity);
            await _appointmentRepository.SaveChangesAsync();
        }

        public void Update(int id, Appointment appointmentToUpdate)
        {
            if (id != appointmentToUpdate.Id)
            {
                throw new BadRequestException($"{nameof(Appointment)} {IdsDoNotMatch}");
            }
            _appointmentRepository.Update(appointmentToUpdate);
            _appointmentRepository.SaveChanges();

        }

        public async Task DeleteAsync(int id)
        {
            var appointmentToDelete = await _appointmentRepository
                .GetFirstOrDefaultAsync(x => x.Id == id) ??
                    throw new NotFoundException($"{nameof(Appointment)} {EntityWasNotFound}");
            
            _appointmentRepository.Delete(appointmentToDelete);
            await _appointmentRepository.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IList<int> listOfIds)
        {
            var appointmentsToDelete = await _appointmentRepository.GetAsync(x => listOfIds.Contains(x.Id));

            if (appointmentsToDelete.Count() != listOfIds.Count)
            {
                throw new BadRequestException($"{SomeEntitiesInCollectionNotFound} {nameof(Appointment)}s to delete");
            }
            _appointmentRepository.DeleteRange(appointmentsToDelete);
            await _appointmentRepository.SaveChangesAsync();
        }
    }
}
