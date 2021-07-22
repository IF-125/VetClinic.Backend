using System;
using System.Collections.Generic;
using System.Text;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Repositories;
using VetClinic.DAL.Context;
using VetClinic.DAL.Repositories.Base;

namespace VetClinic.DAL.Repositories
{
    public class AnimalTypeRepository:Repository<AnimalType>,IAnimalTypeRepository
    {
        public AnimalTypeRepository(VetClinicDbContext context):base(context)
        {

        }
    }
}
