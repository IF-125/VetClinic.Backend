using System;
using System.Collections.Generic;
using System.Text;
using VetClinic.Core.Entities;
using VetClinic.DAL.Repositories.Base;
using VetClinic.Core.Interfaces.Repositories;
using VetClinic.DAL.Context;

namespace VetClinic.DAL.Repositories
{
    public class PetRepository:Repository<Pet>, IPetRepository
    {
        public PetRepository(VetClinicDbContext context):base(context)
        {

        }
    }
}
