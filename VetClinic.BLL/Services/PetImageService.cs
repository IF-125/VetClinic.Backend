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
    public class PetImageService:IPetImageService
    {
        private readonly IPetImageRepository _petImageRepository;

        public PetImageService(IPetImageRepository petImageRepository)
        {
            _petImageRepository = petImageRepository;
        }

        public async Task<IList<PetImage>> GetPetImagesAsync()
        {
            return await _petImageRepository.GetAsync(asNoTracking: true);
        }

        public async Task<IList<PetImage>> GetPetImagesByPetId(int petId)
        {
            return await _petImageRepository.GetAsync(x => x.PetId == petId, asNoTracking: true);
        }

        public async Task<PetImage> GetByIdAsync(int id)
        {
            var petImage = await _petImageRepository.GetFirstOrDefaultAsync(filter: x => x.Id == id);
            if (petImage == null)
            {
                throw new NotFoundException($"{nameof(PetImage)} {EntityWasNotFound}");
            }

            return petImage;
        }

        public async Task InsertAsync(PetImage entity)
        {
            await _petImageRepository.InsertAsync(entity);
            await _petImageRepository.SaveChangesAsync();
        }

        public async Task<PetImage> InsertAsyncWithId(PetImage entity)
        {
            await _petImageRepository.InsertAsync(entity);
            await _petImageRepository.SaveChangesAsync();
            return entity;
        }

        public void Update(int id, PetImage petImageToUpdate)
        {
            if (id != petImageToUpdate.Id)
                throw new NotFoundException($"{nameof(petImageToUpdate)} {EntityWasNotFound}");
            _petImageRepository.Update(petImageToUpdate);
            _petImageRepository.SaveChanges();
        }

        public async Task DeleteAsync(int id)
        {
            var petImageToDelete = await _petImageRepository.GetFirstOrDefaultAsync(x => x.Id == id);

            if (petImageToDelete == null)
                throw new NotFoundException($"{nameof(petImageToDelete)} {EntityWasNotFound}");
            _petImageRepository.Delete(petImageToDelete);
            await _petImageRepository.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IList<int> listOfIds)
        {
            var petImagesToDelete = await _petImageRepository.GetAsync(x => listOfIds.Contains(x.Id));

            if (petImagesToDelete.Count() != listOfIds.Count)
            {
                throw new BadRequestException($"{SomeEntitiesInCollectionNotFound} {nameof(PetImage)}s to delete");
            }

            _petImageRepository.DeleteRange(petImagesToDelete);
            await _petImageRepository.SaveChangesAsync();
        }
    }

}

