using System.Collections.Generic;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Services.Base;

namespace VetClinic.Core.Interfaces.Services
{
    public interface IPetService: IBaseService<Pet, int>
    {
        public Task<IList<Pet>> GetPetsAsync();

        public Task<IList<Pet>> GetPetsByClientId(string clientId);
    }
}
