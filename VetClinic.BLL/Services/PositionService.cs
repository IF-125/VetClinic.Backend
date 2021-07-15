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
    public class PositionService : IPositionService
    {
        private readonly IPositionRepository _positionRepository;

        public PositionService(IPositionRepository positionRepository)
        {
            _positionRepository = positionRepository;
        }

        public async Task<IList<Position>> GetPositionsAsync()
        {
            return await _positionRepository.GetAsync(asNoTracking: true);
        }

        public async Task<Position> GetByIdAsync(int id)
        {
            var position = await _positionRepository.GetFirstOrDefaultAsync(x => x.Id == id) ?? 
                throw new NotFoundException($"{nameof(Position)} {EntityWasNotFound}");

            return position;
        }

        public async Task InsertAsync(Position entity)
        {
            await _positionRepository.InsertAsync(entity);
            await _positionRepository.SaveChangesAsync();
        }

        public void Update(int id, Position entityToUpdate)
        {
            if(id != entityToUpdate.Id)
            {
                throw new BadRequestException($"{nameof(Position)} {IdsDoNotMatch}");
            }

            _positionRepository.Update(entityToUpdate);
            _positionRepository.SaveChanges();
        }

        public async Task DeleteAsync(int id)
        {
            var positionToDelete = await _positionRepository.GetFirstOrDefaultAsync(x => x.Id == id) ??
                throw new NotFoundException($"{nameof(Position)} {EntityWasNotFound}");

            _positionRepository.Delete(positionToDelete);
            _positionRepository.SaveChanges();
        }

        public async Task DeleteRangeAsync(IList<int> listOfIds)
        {
            var positionsToDelete = await _positionRepository.GetAsync(x => listOfIds.Contains(x.Id));

            if (positionsToDelete.Count() != listOfIds.Count)
            {
                throw new BadRequestException($"{SomeEntitiesInCollectionNotFound} {nameof(Position)}s to delete");
            }
            _positionRepository.DeleteRange(positionsToDelete);
            await _positionRepository.SaveChangesAsync();
        }
    }
}
