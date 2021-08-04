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
    public class ProcedureService : IProcedureService
    {
        private readonly IProcedureRepository _procedureRepository;
        public ProcedureService(IProcedureRepository procedureRepository)
        {
            _procedureRepository = procedureRepository;
        }

        public async Task<IList<Procedure>> GetProceduresAsync()
        {
            return await _procedureRepository.GetAsync(asNoTracking: true);
        }

        public async Task<Procedure> GetByIdAsync(int id)
        {
            var procedure = await _procedureRepository
                .GetFirstOrDefaultAsync(x => x.Id == id) ??
                    throw new NotFoundException($"{nameof(Procedure)} {EntityWasNotFound}");

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
                throw new BadRequestException($"{nameof(Procedure)} {IdsDoNotMatch}");
            }
            _procedureRepository.Update(procedureToUpdate);
            _procedureRepository.SaveChanges();

        }

        public async Task DeleteAsync(int id)
        {
            var procedureToDelete = await _procedureRepository
                .GetFirstOrDefaultAsync(x => x.Id == id) ??
                    throw new NotFoundException($"{nameof(Procedure)} {EntityWasNotFound}");

            _procedureRepository.Delete(procedureToDelete);
            await _procedureRepository.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IList<int> listOfIds)
        {
            var proceduresToDelete = await _procedureRepository.GetAsync(x => listOfIds.Contains(x.Id));

            if (proceduresToDelete.Count() != listOfIds.Count)
            {
                throw new BadRequestException($"{SomeEntitiesInCollectionNotFound} {nameof(Procedure)}s to delete");
            }
            _procedureRepository.DeleteRange(proceduresToDelete);
            await _procedureRepository.SaveChangesAsync();
        }
    }
}
