using SendGrid.Helpers.Errors.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Repositories;
using VetClinic.Core.Interfaces.Services;
using static VetClinic.Core.Resources.TextMessages;

namespace VetClinic.BLL.Services
{
    public class AnimalTypeService : IAnimalTypeService
    {
        private readonly IAnimalTypeRepository _animalTypeRepository;

        public AnimalTypeService(IAnimalTypeRepository AnimalTypeRepository)
        {
            _animalTypeRepository = AnimalTypeRepository;
        }

        public async Task<IList<AnimalType>> GetAnimalTypesAsync()
        {
            return await _animalTypeRepository.GetAsync(asNoTracking: true);
        }

        public async Task<AnimalType> GetByIdAsync(int id)
        {
            var AnimalType = await _animalTypeRepository.GetFirstOrDefaultAsync(filter: x => x.Id == id);
            if (AnimalType == null)
            {
                throw new NotFoundException($"{nameof(AnimalType)} {EntityWasNotFound}");
            }

            return AnimalType;
        }

        public async Task InsertAsync(AnimalType entity)
        {
            await _animalTypeRepository.InsertAsync(entity);
            await _animalTypeRepository.SaveChangesAsync();
        }


        public void Update(int id, AnimalType AnimalTypeToUpdate)
        {
            if (id != AnimalTypeToUpdate.Id)
                throw new NotFoundException($"{nameof(AnimalTypeToUpdate)} {EntityWasNotFound}");
            _animalTypeRepository.Update(AnimalTypeToUpdate);
            _animalTypeRepository.SaveChanges();
        }

        public async Task DeleteAsync(int id)
        {
            var AnimalTypeToDelete = await _animalTypeRepository.GetFirstOrDefaultAsync(x => x.Id == id);

            if (AnimalTypeToDelete == null)
                throw new NotFoundException($"{nameof(AnimalTypeToDelete)} {EntityWasNotFound}");
            _animalTypeRepository.Delete(AnimalTypeToDelete);
            await _animalTypeRepository.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IList<int> listOfIds)
        {
            var animalTypesToDelete = await GetAnimalTypes(listOfIds);

            if (animalTypesToDelete.Count() != listOfIds.Count)
            {
                throw new BadRequestException($"{SomeEntitiesInCollectionNotFound} {nameof(AnimalType)}s to delete");
            }

            _animalTypeRepository.DeleteRange(animalTypesToDelete);
            await _animalTypeRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<AnimalType>> GetAnimalTypesByIds(IList<int> listOfIds)
        {
            var animalTypes = await GetAnimalTypes(listOfIds);

            if (animalTypes.Count() != listOfIds.Count)
            {
                throw new BadRequestException($"{SomeEntitiesInCollectionNotFound} {nameof(AnimalType)}s to delete");
            }

            return animalTypes;
        }

        private async Task<IEnumerable<AnimalType>> GetAnimalTypes(IList<int> listOfIds)
        {
            return await _animalTypeRepository.GetAsync(x => listOfIds.Contains(x.Id));
        }
    }
}
