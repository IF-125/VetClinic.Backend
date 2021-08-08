using System.Collections.Generic;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Services.Base;

namespace VetClinic.Core.Interfaces.Services
{
    public  interface IAnimalTypeService:IBaseService<AnimalType,int>
    {
        public Task<IList<AnimalType>> GetAnimalTypesAsync();

        public Task<IEnumerable<AnimalType>> GetAnimalTypesByIds(IList<int> listOfIds);
    }
}
