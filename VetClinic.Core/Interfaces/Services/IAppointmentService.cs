using System.Collections.Generic;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Services.Base;

namespace VetClinic.Core.Interfaces.Services
{
    public interface IAppointmentService : IBaseService<Appointment, int>
    {
        public Task<IList<Appointment>> GetAppointmentsAsync();

        public Task<IEnumerable<Appointment>> GetAppointmentsOfDoctorAsync(string doctorId);
    }
}
