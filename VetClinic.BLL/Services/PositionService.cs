using static VetClinic.Core.Resources.TextMessages;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Repositories;
using VetClinic.Core.Interfaces.Services;

namespace VetClinic.BLL.Services
{
    public class PositionService : IPositionService
    {
        private readonly IPositionRepository _positionRepository;

        public PositionService(IPositionRepository positionRepository)
        {
            _positionRepository = positionRepository;
        }

        public async Task<IList<Position>> GetPositionsAsync(
            Expression<Func<Position, bool>> filter = null,
            Func<IQueryable<Position>, IOrderedQueryable<Position>> orderBy = null,
            Func<IQueryable<Position>, IIncludableQueryable<Position, object>> include = null,
            bool asNoTracking = false)
        {
            return await _positionRepository.GetAsync(filter, orderBy, include, asNoTracking);
        }

        public async Task<Position> GetByIdAsync(
            int id)
        {
            var position = await _positionRepository.GetFirstOrDefaultAsync(x => x.Id == id);
            if(position == null)
            {
                throw new ArgumentException($"{nameof(Position)} {EntityWasNotFound}");
            }

            return position;
        }

        public async Task InsertAsync(Position entity)
        {
            await _positionRepository.InsertAsync(entity);
        }

        public void Update(int id, Position entityToUpdate)
        {
            if(id != entityToUpdate.Id)
            {
                throw new ArgumentException($"{nameof(Position)} {IdsDoNotMatch}");
            }

            _positionRepository.Update(entityToUpdate);
        }

        public async Task DeleteAsync(int id)
        {
            var positionToDelete = await _positionRepository.GetFirstOrDefaultAsync(x => x.Id == id);

            if (positionToDelete == null)
            {
                throw new ArgumentException($"{nameof(Position)} {EntityWasNotFound}");
            }

            _positionRepository.Delete(positionToDelete);
        }

        public async Task DeleteRangeAsync(IList<int> listOfIds)
        {
            var positionsToDelete = await GetPositionsAsync(x => listOfIds.Contains(x.Id));

            if (positionsToDelete.Count() != listOfIds.Count)
            {
                throw new ArgumentException($"{SomeEntitiesInCollectionNotFound} {nameof(Position)}s to delete");
            }
            _positionRepository.DeleteRange(positionsToDelete);
        }
    }
}
