using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Repositories;
using VetClinic.DAL.Context;
using VetClinic.DAL.Repositories.Base;

namespace VetClinic.DAL.Repositories
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        public ClientRepository(VetClinicDbContext context) : base(context)
        {
        }
    }
}
