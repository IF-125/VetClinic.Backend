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
    public class OrderProcedureService : IOrderProcedureService
    {
        private readonly IOrderProcedureRepository _orderProcedureRepository;
        public OrderProcedureService(IOrderProcedureRepository orderProcedureRepository)
        {
            _orderProcedureRepository = orderProcedureRepository;
        }

        public async Task<IList<OrderProcedure>> GetOrderProceduresAsync(
            Expression<Func<OrderProcedure, bool>> filter = null,
            Func<IQueryable<OrderProcedure>, IOrderedQueryable<OrderProcedure>> orderBy = null,
            Func<IQueryable<OrderProcedure>, IIncludableQueryable<OrderProcedure, object>> include = null,
            bool asNoTracking = false)
        {
            return await _orderProcedureRepository.GetAsync(filter, orderBy, include, asNoTracking);
        }

        public async Task<OrderProcedure> GetByIdAsync(
            int id,
            Func<IQueryable<OrderProcedure>, IIncludableQueryable<OrderProcedure, object>> include = null,
            bool asNoTracking = false)
        {
            var orderProcedure = await _orderProcedureRepository.GetFirstOrDefaultAsync(x => x.Id == id, include, asNoTracking);
            if (orderProcedure == null)
            {
                throw new ArgumentException($"{nameof(OrderProcedure)} {EntityWasNotFound}");
            }
            return orderProcedure;
        }

        public async Task InsertAsync(OrderProcedure entity)
        {
            await _orderProcedureRepository.InsertAsync(entity);
        }

        public void Update(int id, OrderProcedure orderProcedureToUpdate)
        {
            if (id != orderProcedureToUpdate.Id)
            {
                throw new ArgumentException($"{nameof(OrderProcedure)} {IdsDoNotMatch}");
            }
            _orderProcedureRepository.Update(orderProcedureToUpdate);

        }

        public async Task DeleteAsync(int id)
        {
            var orderProcedureToDelete = await _orderProcedureRepository.GetFirstOrDefaultAsync(x => x.Id == id);

            if (orderProcedureToDelete == null)
            {
                throw new ArgumentException($"{nameof(OrderProcedure)} {EntityWasNotFound}");
            }
            _orderProcedureRepository.Delete(orderProcedureToDelete);
        }

        public async Task DeleteRangeAsync(int[] idArr)
        {
            var orderProceduresToDelete = await GetOrderProceduresAsync(x => idArr.Contains(x.Id));

            if (orderProceduresToDelete.Count() != idArr.Length)
            {
                throw new ArgumentException($"{SomeEntitiesInCollectionNotFound} {nameof(OrderProcedure)}s to delete");
            }
            _orderProcedureRepository.DeleteRange(orderProceduresToDelete);
        }
    }
}
