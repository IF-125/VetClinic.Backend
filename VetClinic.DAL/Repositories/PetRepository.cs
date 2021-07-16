using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Repositories;
using VetClinic.DAL.Context;
using VetClinic.DAL.Repositories.Base;

namespace VetClinic.DAL.Repositories
{
    public class PetRepository:Repository<Pet>, IPetRepository
    {
        public PetRepository(VetClinicDbContext context):base(context)
        {

        }
    }
}
