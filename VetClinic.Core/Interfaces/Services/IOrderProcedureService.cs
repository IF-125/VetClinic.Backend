using System.Collections.Generic;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Entities.Enums;
using VetClinic.Core.Interfaces.Services.Base;

namespace VetClinic.Core.Interfaces.Services
{
    public interface IOrderProcedureService : IBaseService<OrderProcedure, int>
    {
        public Task<IList<OrderProcedure>> GetOrderProceduresAsync();

        public Task<IEnumerable<OrderProcedure>> GetOrderProceduresOfDoctorAsync(string doctorId);

        public Task<OrderProcedure> GenerateOrderProcedureAsync(int petId, int procedureId, PaymentOption paymentOption);

        public Task AddAppointmentAndDoctorToOrderProcedureAsync(int orderProcedureId, string employeeId, Appointment appointment);
    }
}
