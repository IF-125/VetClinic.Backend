using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Errors.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Entities.Enums;
using VetClinic.Core.Interfaces.Repositories;
using VetClinic.Core.Interfaces.Services;
using static VetClinic.Core.Resources.TextMessages;

namespace VetClinic.BLL.Services
{
    public class PetServise : IPetService
    {
        private readonly IPetRepository _petRepository;
        private readonly IOrderProcedureRepository _orderProcedureRepository;
        private readonly IBlobService _blobService;
        private readonly string _containerName;
        private readonly string _containerPath;

        public PetServise(IPetRepository petRepository,
            IBlobService blobService,
            IOrderProcedureRepository orderProcedureRepository)
        {
            _petRepository = petRepository;
            _orderProcedureRepository = orderProcedureRepository;
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
                                .Include(y => y.PetImages)) ??
                    throw new NotFoundException($"{nameof(Pet)} {EntityWasNotFound}");

            if (petToDelete.PetImages != null && petToDelete.PetImages.Count > 0)
            {
                foreach (var petImage in petToDelete.PetImages)
                {
                    var petImageBlobName = petImage.Path.Replace(_containerPath, default);
                    await _blobService.DeleteBlob(petImageBlobName, _containerName);
                }
            }

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

        public async Task<Pet> GetMedicalCardOfPetAsync(int petId)
        {
            var pet = await _petRepository.GetFirstOrDefaultAsync(
                filter: x => x.Id == petId,
                include: i => i
                    .Include(a => a.AnimalType)
                    .Include(c => c.Client)
                    .Include(p => p.OrderProcedures)
                    .ThenInclude(p => p.Procedure)
                    .Include(p => p.OrderProcedures)
                    .ThenInclude(o => o.Order),
                asNoTracking: true
                );

            pet.OrderProcedures = pet.OrderProcedures.Where(x => x.Status != OrderProcedureStatus.NotAssigned).ToList();

            return pet;
        }

        public async Task<IEnumerable<Pet>> GetPetsToTreat(string doctorId)
        {
            return (await _orderProcedureRepository
                .GetAsync(
                    filter: x => x.EmployeeId == doctorId && x.Status == OrderProcedureStatus.Assigned,
                    include: x => x.Include(p => p.Pet).ThenInclude(a => a.AnimalType)))
                .Select(p => p.Pet)
                .Distinct();
        }
    }
}