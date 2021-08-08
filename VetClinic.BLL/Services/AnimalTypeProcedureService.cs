using System.Collections.Generic;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Services;

namespace VetClinic.BLL.Services
{
    public class AnimalTypeProcedureService : IAnimalTypeProcedureService
    {
        private readonly IAnimalTypeService _animalTypeService;
        private readonly IProcedureService _procedureService;
        public AnimalTypeProcedureService(IAnimalTypeService animalTypeService,
            IProcedureService procedureService)
        {
            _animalTypeService = animalTypeService;
            _procedureService = procedureService;
        }
        public async Task InsertAsync(Procedure procedure, IList<int> listOfAnimalTypesIds)
        {
            var animalTypes = await _animalTypeService.GetAnimalTypesByIds(listOfAnimalTypesIds);

            foreach (var a in animalTypes)
            {
                procedure.AnimalTypesProcedures.Add(new AnimalTypeProcedure { AnimalType = a });
            }

            await _procedureService.InsertAsync(procedure);
        }
    }
}
