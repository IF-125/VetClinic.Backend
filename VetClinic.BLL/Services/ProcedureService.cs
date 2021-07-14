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
    public class ProcedureService : IProcedureService
    {
        private readonly IProcedureRepository _procedureRepository;
        public ProcedureService(IProcedureRepository procedureRepository)
        {
            _procedureRepository = procedureRepository;
        }

        public async Task<IList<Procedure>> GetProceduresAsync(
            Expression<Func<Procedure, bool>> filter = null,
            Func<IQueryable<Procedure>, IOrderedQueryable<Procedure>> orderBy = null,
            Func<IQueryable<Procedure>, IIncludableQueryable<Procedure, object>> include = null,
            bool asNoTracking = false)
        {
            return await _procedureRepository.GetAsync(filter, orderBy, include, asNoTracking);
        }

        public async Task<Procedure> GetByIdAsync(
            int id,
            Func<IQueryable<Procedure>, IIncludableQueryable<Procedure, object>> include = null,
            bool asNoTracking = false)
        {
            var procedure = await _procedureRepository.GetFirstOrDefaultAsync(x => x.Id == id, include, asNoTracking);
            if (procedure == null)
            {
                throw new ArgumentException($"{nameof(Procedure)} {EntityWasNotFound}");
            }
            return procedure;
        }

        public async Task InsertAsync(Procedure entity)
        {
            await _procedureRepository.InsertAsync(entity);
        }

        public void Update(int id, Procedure procedureToUpdate)
        {
            if (id != procedureToUpdate.Id)
            {
                throw new ArgumentException($"{nameof(Procedure)} {IdsDoNotMatch}");
            }
            _procedureRepository.Update(procedureToUpdate);

        }

        public async Task DeleteAsync(int id)
        {
            var procedureToDelete = await _procedureRepository.GetFirstOrDefaultAsync(x => x.Id == id);

            if (procedureToDelete == null)
            {
                throw new ArgumentException($"{nameof(Procedure)} {EntityWasNotFound}");
            }
            _procedureRepository.Delete(procedureToDelete);
        }

        public async Task DeleteRangeAsync(int[] idArr)
        {
            var proceduresToDelete = await GetProceduresAsync(x => idArr.Contains(x.Id));

            if (proceduresToDelete.Count() != idArr.Length)
            {
                throw new ArgumentException($"{SomeEntitiesInCollectionNotFound} {nameof(Procedure)}s to delete");
            }
            _procedureRepository.DeleteRange(proceduresToDelete);
        }
    }
}
