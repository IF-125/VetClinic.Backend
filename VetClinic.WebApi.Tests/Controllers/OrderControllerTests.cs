using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
using VetClinic.Core.Interfaces.Services;
using VetClinic.WebApi.Controllers;
using VetClinic.WebApi.Mappers;
using VetClinic.WebApi.ViewModels;
using Xunit;
using static VetClinic.Core.Resources.TextMessages;

namespace VetClinic.WebApi.Tests.Controllers
{
    public class OrderControllerTests
    {
        private readonly OrderService _orderService;
        private readonly Mock<IOrderRepository> _orderRepository = new Mock<IOrderRepository>();
        private readonly IMapper _mapper;
        public OrderControllerTests()
        {
            _orderService = new OrderService(_orderRepository.Object);

            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile(new OrderMapperProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            _mapper = mapper;
        }

        [Fact]
        public void CanGetAllOrders()
        {
            //arrange
            var orderController = new OrderController(_orderService, _mapper);

            var orders = OrderFakeData.GetOrderFakeData();

            _orderRepository.Setup(b => b.GetAsync(null, null, null, true).Result).Returns(() => orders);
            //act
            var result = orderController.GetAllOrdersAsync().Result;
            //assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<OrderViewModel>>(viewResult.Value);
            Assert.Equal(OrderFakeData.GetOrderFakeData().Count, model.Count());
        }

        [Fact]
        public void CanReturnOrderById()
        {
            //arrange
            var orderController = new OrderController(_orderService, _mapper);

            var orders = OrderFakeData.GetOrderFakeData().AsQueryable();

            int id = 6;

            _orderRepository.Setup(b => b.GetFirstOrDefaultAsync(b => b.Id == id, null, false).Result)
                .Returns((Expression<Func<Order, bool>> filter,
                Func<IQueryable<Order>, IIncludableQueryable<Order, object>> include,
                bool asNoTracking) => orders.FirstOrDefault(filter));
            //act
            var result = orderController.GetOrderByIdAsync(id).Result;
            //assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<OrderViewModel>(viewResult.Value);

            Assert.Equal(id, model.Id);
            Assert.Equal(new DateTime(2021, 1, 25), model.CreatedAt);
        }

        [Fact]
        public void GetOrderByInvalidId()
        {
            //arrange
            var orderController = new OrderController(_orderService, _mapper);

            int id = 100;
            //act
            var result = orderController.GetOrderByIdAsync(id).Result;
            //assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CanInsertOrder()
        {
            //arrange
            OrderViewModel order = new OrderViewModel
            {
                Id = 11,
                IsPaid = false,
                OrderProcedureId = 11,
                CreatedAt = new DateTime(2021, 6, 11)
            };

            var orderController = new OrderController(_orderService, _mapper);

            _orderRepository.Setup(b => b.InsertAsync(It.IsAny<Order>()));
            //act
            var result = orderController.InsertOrderAsync(order).Result;
            //assert
            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public void CanUpdateOrder()
        {
            //arrange
            OrderViewModel order = new OrderViewModel
            {
                Id = 11,
                IsPaid = false,
                OrderProcedureId = 11,
                CreatedAt = new DateTime(2021, 6, 11)
            };

            int id = 11;

            var orderController = new OrderController(_orderService, _mapper);

            _orderRepository.Setup(b => b.InsertAsync(It.IsAny<Order>()));
            //act
            var result = orderController.Update(id, order);
            //assert
            Assert.IsType<OkResult>(result);
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
        public void CanDeleteOrder()
        {
            //arrange
            int id = 5;

            _orderRepository.Setup(b => b.GetFirstOrDefaultAsync(
                b => b.Id == id, null, false).Result)
                .Returns(new Order() { Id = id });

            _orderRepository.Setup(b => b.Delete(It.IsAny<Order>()));

            var orderController = new OrderController(_orderService, _mapper);
            //act
            var result = orderController.DeleteOrderAsync(id).Result;
            //assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void DeleteOrderByInvalidId()
        {
            //arrange
            int id = 500;

            var orderController = new OrderController(_orderService, _mapper);
            //act
            var result = orderController.DeleteOrderAsync(id).Result;
            //assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CanDeleteRange()
        {
            //arrange
            int[] ids = new int[]{4, 8, 9};

            var orders = OrderFakeData.GetOrderFakeData().AsQueryable();

            _orderRepository
                .Setup(b => b.GetAsync(It.IsAny<Expression<Func<Order, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<Order, bool>> filter,
                Func<IQueryable<Order>, IOrderedQueryable<Order>> orderBy,
                Func<IQueryable<Order>, IIncludableQueryable<Order, object>> include,
                bool asNoTracking) => orders.Where(filter).ToList());

            _orderRepository.Setup(b => b.DeleteRange(It.IsAny<IEnumerable<Order>>()));

            var orderController = new OrderController(_orderService, _mapper);
            //act
            var result = orderController.DeleteOrdersAsync(ids).Result;
            //assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void DeleteRangeWithInvalidId()
        {
            //arrange
            int[] ids = new int[] { 4, 8, 100 };

            var orders = OrderFakeData.GetOrderFakeData().AsQueryable();

            _orderRepository
                .Setup(b => b.GetAsync(It.IsAny<Expression<Func<Order, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<Order, bool>> filter,
                Func<IQueryable<Order>, IOrderedQueryable<Order>> orderBy,
                Func<IQueryable<Order>, IIncludableQueryable<Order, object>> include,
                bool asNoTracking) => orders.Where(filter).ToList());

            _orderRepository.Setup(b => b.DeleteRange(It.IsAny<IEnumerable<Order>>()));

            var orderController = new OrderController(_orderService, _mapper);
            //act
            var result = orderController.DeleteOrdersAsync(ids).Result;

            var badRequest = result as BadRequestObjectResult;
            //assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"{SomeEntitiesInCollectionNotFound} {nameof(Order)}s to delete", badRequest.Value);
        }
    }
}
