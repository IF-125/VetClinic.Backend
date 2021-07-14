using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VetClinic.BLL.Services;
using VetClinic.BLL.Tests.FakeData;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Repositories;
using VetClinic.WebApi.Controllers;
using VetClinic.WebApi.Mappers;
using VetClinic.WebApi.ViewModels;
using Xunit;
using static VetClinic.Core.Resources.TextMessages;

namespace VetClinic.WebApi.Tests.Controllers
{
    public class OrderProcedureControllerTests
    {
        private readonly OrderProcedureService _orderProcedureService;
        private readonly Mock<IOrderProcedureRepository> _orderProcedureRepository = new Mock<IOrderProcedureRepository>();
        private readonly IMapper _mapper;
        public OrderProcedureControllerTests()
        {
            _orderProcedureService = new OrderProcedureService(_orderProcedureRepository.Object);

            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile(new OrderProcedureMapperProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            _mapper = mapper;
        }

        [Fact]
        public void CanGetAllOrderProcedures()
        {
            var OrderProcedureController = new OrderProcedureController(_orderProcedureService, _mapper);

            var OrderProcedures = OrderProcedureFakeData.GetOrderProcedureFakeData();

            _orderProcedureRepository.Setup(b => b.GetAsync(null, null, null, true).Result).Returns(() => OrderProcedures);

            var result = OrderProcedureController.GetAllOrderProceduresAsync().Result;

            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<OrderProcedureViewModel>>(viewResult.Value);
            Assert.Equal(OrderProcedureFakeData.GetOrderProcedureFakeData().Count, model.Count());
        }

        [Fact]
        public void CanReturnOrderProcedureById()
        {
            var OrderProcedureController = new OrderProcedureController(_orderProcedureService, _mapper);

            var OrderProcedures = OrderProcedureFakeData.GetOrderProcedureFakeData().AsQueryable();

            int id = 6;

            _orderProcedureRepository.Setup(b => b.GetFirstOrDefaultAsync(b => b.Id == id, null, false).Result)
                .Returns((Expression<Func<OrderProcedure, bool>> filter,
                Func<IQueryable<OrderProcedure>, IIncludableQueryable<OrderProcedure, object>> include,
                bool asNoTracking) => OrderProcedures.FirstOrDefault(filter));

            var result = OrderProcedureController.GetOrderProcedureByIdAsync(id).Result;

            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<OrderProcedureViewModel>(viewResult.Value);

            Assert.Equal(id, model.Id);
            Assert.Equal("Procedure was conducted successfully.", model.Conclusion);
        }

        [Fact]
        public void GetOrderProcedureByInvalidId()
        {
            var OrderProcedureController = new OrderProcedureController(_orderProcedureService, _mapper);

            int id = 100;

            var result = OrderProcedureController.GetOrderProcedureByIdAsync(id).Result;

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CanInsertOrderProcedure()
        {
            OrderProcedureViewModel OrderProcedure = new OrderProcedureViewModel
            {
                Id = 11,
                Count = 1,
                Time = new TimeSpan(hours: 1, minutes: 27, seconds: 0),
                Conclusion = "Procedure was conducted successfully.",
                Details = "The patient appearts to be stable.",
                OrderId = 11,
                AppointmentId = 11,
                ProcedureId = 3,
                PetId = 3,
                EmployeeId = 9
            };

            var OrderProcedureController = new OrderProcedureController(_orderProcedureService, _mapper);

            _orderProcedureRepository.Setup(b => b.InsertAsync(It.IsAny<OrderProcedure>()));

            var result = OrderProcedureController.InsertOrderProcedureAsync(OrderProcedure).Result;

            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public void CanUpdateOrderProcedure()
        {
            OrderProcedureViewModel OrderProcedure = new OrderProcedureViewModel
            {
                Id = 11,
                Count = 1,
                Time = new TimeSpan(hours: 1, minutes: 27, seconds: 0),
                Conclusion = "Procedure was conducted successfully.",
                Details = "The patient appearts to be stable.",
                OrderId = 11,
                AppointmentId = 11,
                ProcedureId = 3,
                PetId = 3,
                EmployeeId = 9
            };

            int id = 11;

            var OrderProcedureController = new OrderProcedureController(_orderProcedureService, _mapper);

            _orderProcedureRepository.Setup(b => b.InsertAsync(It.IsAny<OrderProcedure>()));

            var result = OrderProcedureController.Update(id, OrderProcedure);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void UpdateOrderProcedure_InvalidId()
        {
            OrderProcedure OrderProcedureToUpdate = new OrderProcedure
            {
                Id = 11,
                Count = 1,
                Time = new TimeSpan(hours: 1, minutes: 27, seconds: 0),
                Conclusion = "Procedure was conducted successfully.",
                Details = "The patient appearts to be stable.",
                OrderId = 11,
                AppointmentId = 11,
                ProcedureId = 3,
                PetId = 3,
                EmployeeId = 9
            };

            int id = 10;

            _orderProcedureRepository.Setup(b => b.Update(It.IsAny<OrderProcedure>()));

            Assert.Throws<ArgumentException>(() => _orderProcedureService.Update(id, OrderProcedureToUpdate));
        }

        [Fact]
        public void CanDeleteOrderProcedure()
        {
            int id = 5;

            _orderProcedureRepository.Setup(b => b.GetFirstOrDefaultAsync(
                b => b.Id == id, null, false).Result)
                .Returns(new OrderProcedure() { Id = id });

            _orderProcedureRepository.Setup(b => b.Delete(It.IsAny<OrderProcedure>()));

            var OrderProcedureController = new OrderProcedureController(_orderProcedureService, _mapper);

            var result = OrderProcedureController.DeleteOrderProcedureAsync(id).Result;

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void DeleteOrderProcedureByInvalidId()
        {
            int id = 500;

            var OrderProcedureController = new OrderProcedureController(_orderProcedureService, _mapper);

            var result = OrderProcedureController.DeleteOrderProcedureAsync(id).Result;

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CanDeleteRange()
        {
            int[] ids = new int[] { 4, 8, 9 };

            var OrderProcedures = OrderProcedureFakeData.GetOrderProcedureFakeData().AsQueryable();

            _orderProcedureRepository
                .Setup(b => b.GetAsync(It.IsAny<Expression<Func<OrderProcedure, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<OrderProcedure, bool>> filter,
                Func<IQueryable<OrderProcedure>, IOrderedQueryable<OrderProcedure>> OrderProcedureBy,
                Func<IQueryable<OrderProcedure>, IIncludableQueryable<OrderProcedure, object>> include,
                bool asNoTracking) => OrderProcedures.Where(filter).ToList());

            _orderProcedureRepository.Setup(b => b.DeleteRange(It.IsAny<IEnumerable<OrderProcedure>>()));

            var OrderProcedureController = new OrderProcedureController(_orderProcedureService, _mapper);

            var result = OrderProcedureController.DeleteOrderProceduresAsync(ids).Result;

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void DeleteRangeWithInvalidId()
        {
            int[] ids = new int[] { 4, 8, 100 };

            var OrderProcedures = OrderProcedureFakeData.GetOrderProcedureFakeData().AsQueryable();

            _orderProcedureRepository
                .Setup(b => b.GetAsync(It.IsAny<Expression<Func<OrderProcedure, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<OrderProcedure, bool>> filter,
                Func<IQueryable<OrderProcedure>, IOrderedQueryable<OrderProcedure>> OrderProcedureBy,
                Func<IQueryable<OrderProcedure>, IIncludableQueryable<OrderProcedure, object>> include,
                bool asNoTracking) => OrderProcedures.Where(filter).ToList());

            _orderProcedureRepository.Setup(b => b.DeleteRange(It.IsAny<IEnumerable<OrderProcedure>>()));

            var OrderProcedureController = new OrderProcedureController(_orderProcedureService, _mapper);

            var result = OrderProcedureController.DeleteOrderProceduresAsync(ids).Result;

            var badRequest = result as BadRequestObjectResult;

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"{SomeEntitiesInCollectionNotFound} {nameof(OrderProcedure)}s to delete", badRequest.Value);
        }
    }
}
