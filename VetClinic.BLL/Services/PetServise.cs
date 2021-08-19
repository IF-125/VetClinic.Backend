using Microsoft.EntityFrameworkCore;
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
    public class PetServise : IPetService
    {
        private readonly IPetRepository _petRepository;
        private readonly IBlobService _blobService;
        private readonly string _containerName;
        private readonly string _containerPath;

        public PetServise(IPetRepository petRepository, IBlobService blobService)
        {
            _petRepository = petRepository;
            _blobService = blobService;
            _containerName = "testcontainer";
            _containerPath = "https://blobuploadsample21.blob.core.windows.net/testcontainer/";

        }

        public async Task<IList<Pet>> GetPetsAsync()
        {
            return await _petRepository.GetAsync(
                include: x => x.Include(y => y.AnimalType)
                                .Include(y=>y.PetImages),
                asNoTracking: true);

           
        }

        public async Task<IList<Pet>> GetPetsByClientId(string clientId)
        {
            return await _petRepository.GetAsync(x => x.ClientId == clientId,
                include: x => x.Include(y => y.AnimalType)
                                .Include(y => y.PetImages),
            asNoTracking: true);
        }

        public async Task<Pet> GetByIdAsync(int id)
        {
            var pet = await _petRepository.GetFirstOrDefaultAsync(filter: x => x.Id == id,
                include: x => x.Include(y => y.AnimalType)
                                .Include(y => y.PetImages));
            if (pet == null)
            {
                throw new NotFoundException($"{nameof(Pet)} {EntityWasNotFound}");
            }

            return pet;
        }

        public async Task InsertAsync(Pet entity)
        {
            await _petRepository.InsertAsync(entity);
            await _petRepository.SaveChangesAsync();
        }

        public  void Update(int id, Pet petToUpdate)
        {
            if (id != petToUpdate.Id)
                throw new NotFoundException($"{nameof(petToUpdate)} {EntityWasNotFound}");
            _petRepository.Update(petToUpdate);
            _petRepository.SaveChanges();
        }

        public async Task DeleteAsync(int id)
        {
            var petToDelete = await _petRepository.GetFirstOrDefaultAsync(filter: x => x.Id == id,
                include: x => x.Include(y => y.AnimalType)
                                .Include(y => y.PetImages));

            if (petToDelete.PetImages!=null && petToDelete.PetImages.Count > 0)
            {
                foreach (var petImage in petToDelete.PetImages)
                {
                    var petImageBlobName = petImage.Path.Replace(_containerPath, default);
                    await _blobService.DeleteBlob(petImageBlobName, _containerName);
                }
            }

            if (petToDelete==null)
                throw  new NotFoundException($"{nameof(petToDelete)} {EntityWasNotFound}");
            _petRepository.Delete(petToDelete);
            await _petRepository.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IList<int> listOfIds)
        {
            var petsToDelete = await _petRepository.GetAsync(x => listOfIds.Contains(x.Id));
      
            if (petsToDelete.Count() != listOfIds.Count)
            {
                throw new BadRequestException($"{SomeEntitiesInCollectionNotFound} {nameof(Pet)}s to delete");
            }

            _petRepository.DeleteRange(petsToDelete);
            await _petRepository.SaveChangesAsync();
        }
    }
}