﻿using static VetClinic.Core.Resources.TextMessages;
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
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IList<Order>> GetOrdersAsync(
            Expression<Func<Order, bool>> filter = null,
            Func<IQueryable<Order>, IOrderedQueryable<Order>> orderBy = null,
            Func<IQueryable<Order>, IIncludableQueryable<Order, object>> include = null,
            bool asNoTracking = false)
        {
            return await _orderRepository.GetAsync(filter, orderBy, include, asNoTracking);
        }

        public async Task<Order> GetByIdAsync(
            int id,
            Func<IQueryable<Order>, IIncludableQueryable<Order, object>> include = null,
            bool asNoTracking = false)
        {
            var order = await _orderRepository.GetFirstOrDefaultAsync(x => x.Id == id, include, asNoTracking);
            if (order == null)
            {
                throw new ArgumentException($"{nameof(Order)} {EntityWasNotFound}");
            }
            return order;
        }

        public async Task InsertAsync(Order entity)
        {
            await _orderRepository.InsertAsync(entity);
        }

        public void Update(int id, Order orderToUpdate)
        {
            if (id != orderToUpdate.Id)
            {
                throw new ArgumentException($"{nameof(Order)} {IdsDoNotMatch}");
            }
            _orderRepository.Update(orderToUpdate);

        }

        public async Task DeleteAsync(int id)
        {
            var orderToDelete = await _orderRepository.GetFirstOrDefaultAsync(x => x.Id == id);

            if (orderToDelete == null)
            {
                throw new ArgumentException($"{nameof(Order)} {EntityWasNotFound}");
            }
            _orderRepository.Delete(orderToDelete);
        }

        public async Task DeleteRangeAsync(int[] idArr)
        {
            var ordersToDelete = await GetOrdersAsync(x => idArr.Contains(x.Id));

            if (ordersToDelete.Count() != idArr.Length)
            {
                throw new ArgumentException($"{SomeEntitiesInCollectionNotFound} {nameof(Order)}s to delete");
            }
            _orderRepository.DeleteRange(ordersToDelete);
        }
    }
}