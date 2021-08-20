using System;
using System.Collections.Generic;
using System.Text;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Repositories;
using VetClinic.DAL.Context;
using VetClinic.DAL.Repositories.Base;

namespace VetClinic.DAL.Repositories
{
    public class PetImageRepository: Repository<PetImage>, IPetImageRepository
    {
        public PetImageRepository(VetClinicDbContext context) : base(context)
        {

        }
    }
}
