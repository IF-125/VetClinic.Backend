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
            //arrange
            var OrderProcedureController = new OrderProcedureController(_orderProcedureService, _mapper);

            var OrderProcedures = OrderProcedureFakeData.GetOrderProcedureFakeData();

            _orderProcedureRepository.Setup(b => b.GetAsync(null, null, null, true).Result).Returns(() => OrderProcedures);
            //act
            var result = OrderProcedureController.GetAllOrderProceduresAsync().Result;
            //assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<OrderProcedureViewModel>>(viewResult.Value);
            Assert.Equal(OrderProcedureFakeData.GetOrderProcedureFakeData().Count, model.Count());
        }

        [Fact]
        public void CanReturnOrderProcedureById()
        {
            //arrange
            var OrderProcedureController = new OrderProcedureController(_orderProcedureService, _mapper);

            var OrderProcedures = OrderProcedureFakeData.GetOrderProcedureFakeData().AsQueryable();

            int id = 6;

            _orderProcedureRepository.Setup(b => b.GetFirstOrDefaultAsync(b => b.Id == id, null, false).Result)
                .Returns((Expression<Func<OrderProcedure, bool>> filter,
                Func<IQueryable<OrderProcedure>, IIncludableQueryable<OrderProcedure, object>> include,
                bool asNoTracking) => OrderProcedures.FirstOrDefault(filter));
            //act
            var result = OrderProcedureController.GetOrderProcedureByIdAsync(id).Result;
            //assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<OrderProcedureViewModel>(viewResult.Value);

            Assert.Equal(id, model.Id);
            Assert.Equal("Procedure was conducted successfully.", model.Conclusion);
        }

        [Fact]
        public void GetOrderProcedureByInvalidId()
        {
            //arrange
            var OrderProcedureController = new OrderProcedureController(_orderProcedureService, _mapper);

            int id = 100;
            //act
            var result = OrderProcedureController.GetOrderProcedureByIdAsync(id).Result;
            //assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CanInsertOrderProcedure()
        {
            //arrange
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
            //act
            var result = OrderProcedureController.InsertOrderProcedureAsync(OrderProcedure).Result;
            //assert
            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public void CanUpdateOrderProcedure()
        {
            //arrange
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
            //act
            var result = OrderProcedureController.Update(id, OrderProcedure);
            //assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void UpdateOrderProcedure_InvalidId()
        {
            //arrange
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
            //assert
            Assert.Throws<ArgumentException>(() => _orderProcedureService.Update(id, OrderProcedureToUpdate));
        }

        [Fact]
        public void CanDeleteOrderProcedure()
        {
            //arrange
            int id = 5;

            _orderProcedureRepository.Setup(b => b.GetFirstOrDefaultAsync(
                b => b.Id == id, null, false).Result)
                .Returns(new OrderProcedure() { Id = id });

            _orderProcedureRepository.Setup(b => b.Delete(It.IsAny<OrderProcedure>()));

            var OrderProcedureController = new OrderProcedureController(_orderProcedureService, _mapper);
            //act
            var result = OrderProcedureController.DeleteOrderProcedureAsync(id).Result;
            //assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void DeleteOrderProcedureByInvalidId()
        {
            //arrange
            int id = 500;

            var OrderProcedureController = new OrderProcedureController(_orderProcedureService, _mapper);
            //act
            var result = OrderProcedureController.DeleteOrderProcedureAsync(id).Result;
            //assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CanDeleteRange()
        {
            //arrange
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
            //act
            var result = OrderProcedureController.DeleteOrderProceduresAsync(ids).Result;
            //assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void DeleteRangeWithInvalidId()
        {
            //arrange
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
            //act
            var result = OrderProcedureController.DeleteOrderProceduresAsync(ids).Result;

            var badRequest = result as BadRequestObjectResult;
            //assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"{SomeEntitiesInCollectionNotFound} {nameof(OrderProcedure)}s to delete", badRequest.Value);
        }
    }
}
