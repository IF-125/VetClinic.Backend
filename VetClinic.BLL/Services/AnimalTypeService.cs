using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Repositories;
using VetClinic.Core.Interfaces.Services;
using static VetClinic.Core.Resources.TextMessages;

namespace VetClinic.BLL.Services
{
    public class AnimalTypeService:IAnimalTypeService
    {
        private readonly IAnimalTypeRepository _AnimalTypeRepository;

        public AnimalTypeService(IAnimalTypeRepository AnimalTypeRepository)
        {
            _AnimalTypeRepository = AnimalTypeRepository;
        }

        public async Task<IList<AnimalType>> GetAnimalTypesAsync()
        {
            return await _AnimalTypeRepository.GetAsync(asNoTracking: true);
        }

        public async Task<AnimalType> GetByIdAsync(int id)
        {
            var AnimalType = await _AnimalTypeRepository.GetFirstOrDefaultAsync(filter: x => x.Id == id);
            if (AnimalType == null)
            {
                throw new NotFoundException($"{nameof(AnimalType)} {EntityWasNotFound}");
            }

            return AnimalType;
        }

        public async Task InsertAsync(AnimalType entity)
        {
            await _AnimalTypeRepository.InsertAsync(entity);
            await _AnimalTypeRepository.SaveChangesAsync();
        }


        public void Update(int id, AnimalType AnimalTypeToUpdate)
        {
            if (id != AnimalTypeToUpdate.Id)
                throw new NotFoundException($"{nameof(AnimalTypeToUpdate)} {EntityWasNotFound}");
            _AnimalTypeRepository.Update(AnimalTypeToUpdate);
            _AnimalTypeRepository.SaveChanges();
        }

        public async Task DeleteAsync(int id)
        {
            var AnimalTypeToDelete = await _AnimalTypeRepository.GetFirstOrDefaultAsync(x => x.Id == id);

            if (AnimalTypeToDelete == null)
                throw new NotFoundException($"{nameof(AnimalTypeToDelete)} {EntityWasNotFound}");
            _AnimalTypeRepository.Delete(AnimalTypeToDelete);
            await _AnimalTypeRepository.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IList<int> listOfIds)
        {
            var AnimalTypesToDelete = await _AnimalTypeRepository.GetAsync(x => listOfIds.Contains(x.Id));

            if (AnimalTypesToDelete.Count() != listOfIds.Count)
            {
                throw new BadRequestException($"{SomeEntitiesInCollectionNotFound} {nameof(AnimalType)}s to delete");
            }

            _AnimalTypeRepository.DeleteRange(AnimalTypesToDelete);
            await _AnimalTypeRepository.SaveChangesAsync();
        }
    }
}
