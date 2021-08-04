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
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IList<Order>> GetOrdersAsync()
        {
            return await _orderRepository.GetAsync(asNoTracking: true);
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            var order = await _orderRepository
                .GetFirstOrDefaultAsync(x => x.Id == id) ??
                    throw new NotFoundException($"{nameof(Order)} {EntityWasNotFound}");

            return order;
        }

        public async Task InsertAsync(Order entity)
        {
            await _orderRepository.InsertAsync(entity);
            await _orderRepository.SaveChangesAsync();
        }

        public void Update(int id, Order orderToUpdate)
        {
            if (id != orderToUpdate.Id)
            {
                throw new BadRequestException($"{nameof(Order)} {IdsDoNotMatch}");
            }
            _orderRepository.Update(orderToUpdate);
            _orderRepository.SaveChanges();

        }

        public async Task DeleteAsync(int id)
        {
            var orderToDelete = await _orderRepository
                .GetFirstOrDefaultAsync(x => x.Id == id) ??
                    throw new NotFoundException($"{nameof(Order)} {EntityWasNotFound}");
            
            _orderRepository.Delete(orderToDelete);
            await _orderRepository.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IList<int> listOfIds)
        {
            var ordersToDelete = await _orderRepository.GetAsync(x => listOfIds.Contains(x.Id));

            if (ordersToDelete.Count() != listOfIds.Count)
            {
                throw new BadRequestException($"{SomeEntitiesInCollectionNotFound} {nameof(Order)}s to delete");
            }
            _orderRepository.DeleteRange(ordersToDelete);
            await _orderRepository.SaveChangesAsync();
        }
    }
}
