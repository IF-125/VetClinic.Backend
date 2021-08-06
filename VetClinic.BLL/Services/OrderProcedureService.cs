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
    public class OrderProcedureService : IOrderProcedureService
    {
        private readonly IOrderProcedureRepository _orderProcedureRepository;
        public OrderProcedureService(IOrderProcedureRepository orderProcedureRepository)
        {
            _orderProcedureRepository = orderProcedureRepository;
        }

        public async Task<IList<OrderProcedure>> GetOrderProceduresAsync()
        {
            return await _orderProcedureRepository
                .GetAsync(asNoTracking: true);
        }

        public async Task<OrderProcedure> GetByIdAsync(int id)
        {
            var orderProcedure = await _orderProcedureRepository
                .GetFirstOrDefaultAsync(x => x.Id == id) ??
                    throw new NotFoundException($"{nameof(OrderProcedure)} {EntityWasNotFound}");
           
            return orderProcedure;
        }

        public async Task InsertAsync(OrderProcedure entity)
        {
            await _orderProcedureRepository.InsertAsync(entity);
            await _orderProcedureRepository.SaveChangesAsync();
        }

        public void Update(int id, OrderProcedure orderProcedureToUpdate)
        {
            if (id != orderProcedureToUpdate.Id)
            {
                throw new BadRequestException($"{nameof(OrderProcedure)} {IdsDoNotMatch}");
            }
            _orderProcedureRepository.Update(orderProcedureToUpdate);
            _orderProcedureRepository.SaveChanges();

        }

        public async Task DeleteAsync(int id)
        {
            var orderProcedureToDelete = await _orderProcedureRepository
                .GetFirstOrDefaultAsync(x => x.Id == id) ??
                throw new NotFoundException($"{nameof(OrderProcedure)} {EntityWasNotFound}");

            _orderProcedureRepository.Delete(orderProcedureToDelete);
            await _orderProcedureRepository.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IList<int> listOfIds)
        {
            var orderProceduresToDelete = await _orderProcedureRepository.GetAsync(x => listOfIds.Contains(x.Id));

            if (orderProceduresToDelete.Count() != listOfIds.Count)
            {
                throw new BadRequestException($"{SomeEntitiesInCollectionNotFound} {nameof(OrderProcedure)}s to delete");
            }
            _orderProcedureRepository.DeleteRange(orderProceduresToDelete);
            await _orderProcedureRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderProcedure>> GetOrderProceduresOfDoctorAsync(string doctorId)
        {
            return await _orderProcedureRepository.GetAsync(
                filter: x => x.EmployeeId == doctorId,
                include: y => y
                    .Include(p => p.Procedure)
                    .Include(p => p.Pet)
                    .ThenInclude(a => a.AnimalType),
                asNoTracking: true);
        }

        public async Task<IList<OrderProcedure>> GetMedicalCardOfPetAsync(int petId)
        {
            return await _orderProcedureRepository.GetAsync(
                filter: x => x.PetId == petId,
                include: i => i
                    .Include(p => p.Procedure)
                    .Include(o => o.Order),
                asNoTracking: true
                );
        }
    }
}
