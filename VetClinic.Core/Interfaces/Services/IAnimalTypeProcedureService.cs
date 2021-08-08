using System.Collections.Generic;
using System.Threading.Tasks;
using VetClinic.Core.Entities;

namespace VetClinic.Core.Interfaces.Services
{
    public interface IAnimalTypeProcedureService
    {
        public Task InsertAsync(Procedure procedure, IList<int> listOfAnimalTypesIds);
    }
}
