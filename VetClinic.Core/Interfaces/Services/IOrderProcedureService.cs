using System.Collections.Generic;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Services.Base;

namespace VetClinic.Core.Interfaces.Services
{
    public interface IOrderProcedureService : IBaseService<OrderProcedure, int>
    {
        public Task<IList<OrderProcedure>> GetOrderProceduresAsync();

        public Task<IEnumerable<OrderProcedure>> GetOrderProceduresOfDoctorAsync(string doctorId);
    }
}
