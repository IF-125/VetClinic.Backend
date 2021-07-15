using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Repositories;
using VetClinic.DAL.Context;
using VetClinic.DAL.Repositories.Base;

namespace VetClinic.DAL.Repositories
{
    public class OrderProcedureRepository : Repository<OrderProcedure>, IOrderProcedureRepository
    {
        public OrderProcedureRepository(VetClinicDbContext context) : base(context)
        {

        }
    }
}
