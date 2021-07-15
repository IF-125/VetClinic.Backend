using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VetClinic.BLL.Services;
using VetClinic.BLL.Tests.FakeData;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Repositories;
using Xunit;

namespace VetClinic.BLL.Tests.Services
{
    public class OrderServiceTests
    {
        private readonly OrderService _orderService;
        private readonly Mock<IOrderRepository> _orderRepository = new Mock<IOrderRepository>();
        public OrderServiceTests()
        {
            _orderService = new OrderService(
                _orderRepository.Object);
        }

        [Fact]
        public async Task CanReturnAllOrders()
        {
            //arrange
            _orderRepository.Setup(b => b.GetAsync(null, null, null, true).Result)
                .Returns(OrderFakeData.GetOrderFakeData());
            //act
            IList<Order> orders = await _orderService.GetOrdersAsync(null, null, null, asNoTracking: true);
            //assert
            Assert.NotNull(orders);
            Assert.Equal(10, orders.Count);
        }

        [Fact]
        public async Task CanReturnOrderById()
        {
            //arrange
            int id = 4;

            var orders = OrderFakeData.GetOrderFakeData().AsQueryable();

            _orderRepository.Setup(b => b.GetFirstOrDefaultAsync(
                b => b.Id == id, null, true).Result)
                .Returns((Expression<Func<Order, bool>> filter,
                Func<IQueryable<Order>, IIncludableQueryable<Order, object>> include,
                bool asNoTracking) => orders.FirstOrDefault(filter));
            //act
            var order = await _orderService.GetByIdAsync(id, null, true);
            //assert
            Assert.Equal(new DateTime(2020, 10, 16), order.CreatedAt);
        }

        [Fact]
        public void GetOrderByInvalidId()
        {
            //arrange
            int id = 45;

            var orders = OrderFakeData.GetOrderFakeData().AsQueryable();

            _orderRepository.Setup(b => b.GetFirstOrDefaultAsync(
                b => b.Id == id, null, true).Result)
                .Returns((Expression<Func<Order, bool>> filter,
                Func<IQueryable<Order>, IIncludableQueryable<Order, object>> include,
                bool asNoTracking) => orders.FirstOrDefault(filter));
            //act
            var order = _orderService.GetByIdAsync(id);
            //assert
            Assert.Throws<AggregateException>(() => order.Result);
        }

        [Fact]
        public async Task CanInsertOrder()
        {
            //arrange
            Order orderToInsert = new Order
            {
                Id = 11,
                IsPaid = false,
                OrderProcedureId = 11,
                CreatedAt = new DateTime(2021, 6, 11)
            };

            _orderRepository.Setup(b => b.InsertAsync(It.IsAny<Order>()));
            //act
            await _orderService.InsertAsync(orderToInsert);
            //assert
            _orderRepository.Verify(b => b.InsertAsync(orderToInsert));
        }

        [Fact]
        public void CanUpdateOrder()
        {
            //arrange
            Order orderToUpdate = new Order
            {
                Id = 11,
                IsPaid = false,
                OrderProcedureId = 11,
                CreatedAt = new DateTime(2021, 6, 11)
            };

            int id = 11;

            _orderRepository.Setup(b => b.Update(It.IsAny<Order>()));
            //act
            _orderService.Update(id, orderToUpdate);
            //assert
            _orderRepository.Verify(b => b.Update(orderToUpdate));
        }

        [Fact]
        public void UpdateOrder_InvalidId()
        {
            //arrange
            Order orderToUpdate = new Order
            {
                Id = 11,
                IsPaid = false,
                OrderProcedureId = 11,
                CreatedAt = new DateTime(2021, 6, 11)
            };

            int id = 10;

            _orderRepository.Setup(b => b.Update(It.IsAny<Order>()));
            //assert
            Assert.Throws<ArgumentException>(() => _orderService.Update(id, orderToUpdate));
        }

        [Fact]
        public async Task CanDeleteOrder()
        {
            //arrange
            var id = 10;

            _orderRepository.Setup(b => b.GetFirstOrDefaultAsync(
                b => b.Id == id, null, false).Result)
                .Returns(new Order() { Id = id });

            _orderRepository.Setup(b => b.Delete(It.IsAny<Order>())).Verifiable();
            //act
            await _orderService.DeleteAsync(id);
            //assert
            _orderRepository.Verify(b => b.Delete(It.IsAny<Order>()));
        }

        [Fact]
        public void DeleteOrderByInvalidId()
        {
            //assert
            Assert.Throws<AggregateException>(() => _orderService.DeleteAsync(100).Wait());
        }

        [Fact]
        public void CanDeleteRange()
        {
            //arrange
            int[] ids = new int[]{8, 9, 10};

            var orders = OrderFakeData.GetOrderFakeData().AsQueryable();

            _orderRepository.Setup(b => b.GetAsync(It.IsAny<Expression<Func<Order, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<Order, bool>> filter,
                Func<IQueryable<Order>, IOrderedQueryable<Order>> orderBy,
                Func<IQueryable<Order>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => orders.Where(filter).ToList());

            _orderRepository.Setup(b => b.DeleteRange(It.IsAny<IEnumerable<Order>>()));
            //act
            _orderService.DeleteRangeAsync(ids).Wait();
            //assert
            _orderRepository.Verify(b => b.DeleteRange(It.IsAny<IEnumerable<Order>>()));
        }

        [Fact]
        public void DeleteRangeWithInvalidId()
        {
            //arrange
            int[] ids = new int[]{8, 9, 100};

            var orders = OrderFakeData.GetOrderFakeData().AsQueryable();

            _orderRepository.Setup(b => b.GetAsync(It.IsAny<Expression<Func<Order, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<Order, bool>> filter,
                Func<IQueryable<Order>, IOrderedQueryable<Order>> orderBy,
                Func<IQueryable<Order>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => orders.Where(filter).ToList());

            _orderRepository.Setup(b => b.DeleteRange(It.IsAny<IEnumerable<Order>>()));
            //assert
            Assert.Throws<AggregateException>(() => _orderService.DeleteRangeAsync(ids).Wait());
        }
    }
}
