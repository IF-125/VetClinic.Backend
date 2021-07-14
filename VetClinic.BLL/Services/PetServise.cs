using static VetClinic.Core.Resources.TextMessages;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Repositories;
using VetClinic.Core.Interfaces.Services;

namespace VetClinic.BLL.Services
{
    public class PetServise : IPetService
    {
        private readonly IPetRepository _petRepository;

        public PetServise(IPetRepository petRepository)
        {
            _petRepository = petRepository;
        }

        public async Task<IList<Pet>> GetPetsAsync(
            Expression<Func<Pet, bool>> filter = null,
            Func<IQueryable<Pet>, IOrderedQueryable<Pet>> orderBy = null,
            Func<IQueryable<Pet>, IIncludableQueryable<Pet, object>> include = null,
            bool asNoTracking = false)
        {
            return await _petRepository.GetAsync(filter, orderBy, include, asNoTracking);
        }

        public async Task<Pet> GetByIdAsync(
            int id, 
            Func<IQueryable<Pet>, IIncludableQueryable<Pet, object>> include = null, 
            bool asNoTracking = false)
        {
            var pet = await _petRepository.GetFirstOrDefaultAsync(x => x.Id == id, include, asNoTracking);
            if (pet == null)
                throw new ArgumentException($"{nameof(pet)} {EntityWasNotFound}");

            return pet;
        }

        public async Task InsertAsync(Pet entity)
        {
            await _petRepository.InsertAsync(entity);
        }


        public  void Update(int id, Pet petToUpdate)
        {
            if (id != petToUpdate.Id)
                throw new ArgumentException($"{nameof(petToUpdate)} {EntityWasNotFound}");
            _petRepository.Update(petToUpdate);
        }

        public async Task DeleteAsync(int id)
        {
            var petToDelete = await _petRepository.GetFirstOrDefaultAsync(x => x.Id == id);

            if (petToDelete==null)
                throw  new ArgumentException($"{nameof(petToDelete)} {EntityWasNotFound}");
            _petRepository.Delete(petToDelete);

        }

        public async Task DeleteRangeAsync(int[] idArr)
        {
            var petsToDelete = await GetPetsAsync(x => idArr.Contains(x.Id));

            if (petsToDelete.Count() != idArr.Length)
            {
                throw new ArgumentException($"{SomeEntitiesInCollectionNotFound} {nameof(Pet)}s to delete");
            }

            _petRepository.DeleteRange(petsToDelete);






        }
    }
}