using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Errors.Model;
using System;
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

        public PetServise(IPetRepository petRepository)
        {
            _petRepository = petRepository;
        }

        public async Task<IList<Pet>> GetPetsAsync()
        {
            return await _petRepository.GetAsync(include:x=>x
                .Include(y=>y.AnimalType),
                asNoTracking: true);
        }

        public async Task<Pet> GetByIdAsync(int id)
        {
            var pet = await _petRepository.GetFirstOrDefaultAsync(filter: x => x.Id == id);
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
            var petToDelete = await _petRepository.GetFirstOrDefaultAsync(x => x.Id == id);

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