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

        public async Task<Procedure> GetByIdAsync(int id)
        {
            var procedure = await _procedureRepository.GetFirstOrDefaultAsync(x => x.Id == id);
            if (procedure == null)
            {
                throw new ArgumentException($"{nameof(Procedure)} {EntityWasNotFound}");
            }
            return procedure;
        }

        public async Task InsertAsync(Procedure entity)
        {
            await _procedureRepository.InsertAsync(entity);
            await _procedureRepository.SaveChangesAsync();
        }

        public void Update(int id, Procedure procedureToUpdate)
        {
            if (id != procedureToUpdate.Id)
            {
                throw new ArgumentException($"{nameof(Procedure)} {IdsDoNotMatch}");
            }
            _procedureRepository.Update(procedureToUpdate);
            _procedureRepository.SaveChanges();

        }

        public async Task DeleteAsync(int id)
        {
            var procedureToDelete = await _procedureRepository.GetFirstOrDefaultAsync(x => x.Id == id);

            if (procedureToDelete == null)
            {
                throw new ArgumentException($"{nameof(Procedure)} {EntityWasNotFound}");
            }
            _procedureRepository.Delete(procedureToDelete);
            await _procedureRepository.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IList<int> listOfIds)
        {
            var proceduresToDelete = await GetProceduresAsync(x => listOfIds.Contains(x.Id));

            if (proceduresToDelete.Count() != listOfIds.Count)
            {
                throw new ArgumentException($"{SomeEntitiesInCollectionNotFound} {nameof(Procedure)}s to delete");
            }
            _procedureRepository.DeleteRange(proceduresToDelete);
            await _procedureRepository.SaveChangesAsync();
        }
    }
}
