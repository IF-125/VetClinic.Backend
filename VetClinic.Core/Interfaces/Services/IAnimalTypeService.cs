using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Services.Base;

namespace VetClinic.Core.Interfaces.Services
{
    public  interface IAnimalTypeService:IBaseService<AnimalType,int>
    {
        public Task<IList<AnimalType>> GetAnimalTypesAsync();
    }
}
