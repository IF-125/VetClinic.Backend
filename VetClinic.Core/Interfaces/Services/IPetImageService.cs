using System.Collections.Generic;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Services.Base;

namespace VetClinic.Core.Interfaces.Services
{
    public interface IPetImageService:IBaseService<PetImage,int>
    {
        public Task<PetImage> InsertAsyncWithId(PetImage entity);
        public  Task<IList<PetImage>> GetPetImagesByPetId(int petId);
    }
}
