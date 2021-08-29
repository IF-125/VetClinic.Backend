//using Microsoft.EntityFrameworkCore.Query;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Threading.Tasks;
//using VetClinic.BLL.Services;
//using VetClinic.BLL.Tests.FakeData;
//using VetClinic.Core.Entities;
//using VetClinic.Core.Interfaces.Repositories;
//using VetClinic.Core.Interfaces.Services;
//using Xunit;

//namespace VetClinic.BLL.Tests.Services
//{
//    public class OrderProcedureServiceTests
//    {
//        private readonly OrderProcedureService _orderProcedureService;
//        private readonly Mock<IPetService> _mockPetService = new Mock<IPetService>();
//        private readonly Mock<IProcedureService> _mockProcedureService = new Mock<IProcedureService>();
//        private readonly Mock<IOrderRepository> _mockOrderRepository = new Mock<IOrderRepository>();
//        private readonly Mock<IOrderProcedureRepository> _mockOrderProcedureRepository = new Mock<IOrderProcedureRepository>();
//        public OrderProcedureServiceTests()
//        {
//            _orderProcedureService = new OrderProcedureService(
//                _mockOrderProcedureRepository.Object,
//                _mockPetService.Object,
//                _mockProcedureService.Object,
//                _mockOrderRepository.Object);
//        }

//        [Fact]
//        public async Task CanReturnAllOrderProcedures()
//        {
//            //arrange
//            _mockOrderProcedureRepository.Setup(b => b.GetAsync(null, null, null, true).Result)
//                .Returns(OrderProcedureFakeData.GetOrderProcedureFakeData());
//            //act
//            IList<OrderProcedure> OrderProcedures = await _orderProcedureService.GetOrderProceduresAsync();
//            //assert
//            Assert.NotNull(OrderProcedures);
//            Assert.Equal(10, OrderProcedures.Count);
//        }

//        [Fact]
//        public async Task CanReturnOrderProcedureById()
//        {
//            //arrange
//            int id = 3;

//            var OrderProcedures = OrderProcedureFakeData.GetOrderProcedureFakeData().AsQueryable();

//            _mockOrderProcedureRepository.Setup(b => b.GetFirstOrDefaultAsync(
//                b => b.Id == id, null, false).Result)
//                .Returns((Expression<Func<OrderProcedure, bool>> filter,
//                Func<IQueryable<OrderProcedure>, IIncludableQueryable<OrderProcedure, object>> include,
//                bool asNoTracking) => OrderProcedures.FirstOrDefault(filter));
//            //act
//            var OrderProcedure = await _orderProcedureService.GetByIdAsync(id);
//            //assert
//            Assert.Equal("Procedure was unsuccessful.", OrderProcedure.Conclusion);
//        }

//        [Fact]
//        public void GetOrderProcedureByInvalidId()
//        {
//            //arrange
//            int id = 45;

//            var OrderProcedures = OrderProcedureFakeData.GetOrderProcedureFakeData().AsQueryable();

//            _mockOrderProcedureRepository.Setup(b => b.GetFirstOrDefaultAsync(
//                b => b.Id == id, null, true).Result)
//                .Returns((Expression<Func<OrderProcedure, bool>> filter,
//                Func<IQueryable<OrderProcedure>, IIncludableQueryable<OrderProcedure, object>> include,
//                bool asNoTracking) => OrderProcedures.FirstOrDefault(filter));
//            //act
//            var orderProcedure = _orderProcedureService.GetByIdAsync(id);
//            //assert
//            Assert.Throws<AggregateException>(() => orderProcedure.Result);
//        }

//        [Fact]
//        public async Task CanInsertOrderProcedure()
//        {
//            //arrange
//            OrderProcedure OrderProcedureToInsert = new OrderProcedure
//            {
//                Id = 11,
//                Conclusion = "Procedure was conducted successfully.",
//                Details = "The patient appearts to be stable.",
//                OrderId = 11,
//                ProcedureId = 3,
//                PetId = 3,
//                EmployeeId = "f1a05cca-b479-4f72-bbda-96b8979f4afe"
//            };

//            _mockOrderProcedureRepository.Setup(b => b.InsertAsync(It.IsAny<OrderProcedure>()));
//            //act
//            await _orderProcedureService.InsertAsync(OrderProcedureToInsert);
//            //assert
//            _mockOrderProcedureRepository.Verify(b => b.InsertAsync(OrderProcedureToInsert));
//        }

//        [Fact]
//        public void CanUpdateOrderProcedure()
//        {
//            //arrange
//            OrderProcedure OrderProcedureToUpdate = new OrderProcedure
//            {
//                Id = 11,
//                Conclusion = "Procedure was conducted successfully.",
//                Details = "The patient appearts to be stable.",
//                OrderId = 11,
//                ProcedureId = 3,
//                PetId = 3,
//                EmployeeId = "f1a05cca-b479-4f72-bbda-96b8979f4afe"
//            };

//            int id = 11;

//            _mockOrderProcedureRepository.Setup(b => b.Update(It.IsAny<OrderProcedure>()));
//            //act
//            _orderProcedureService.Update(id, OrderProcedureToUpdate);
//            //assert
//            _mockOrderProcedureRepository.Verify(b => b.Update(OrderProcedureToUpdate));
//        }

//        [Fact]
//        public void UpdateOrderProcedure_InvalidId()
//        {
//            //arrange
//            OrderProcedure OrderProcedureToUpdate = new OrderProcedure
//            {
//                Id = 11,
//                Conclusion = "Procedure was conducted successfully.",
//                Details = "The patient appearts to be stable.",
//                OrderId = 11,
//                ProcedureId = 3,
//                PetId = 3,
//                EmployeeId = "f1a05cca-b479-4f72-bbda-96b8979f4afe"
//            };

//            int id = 10;

//            _mockOrderProcedureRepository.Setup(b => b.Update(It.IsAny<OrderProcedure>()));
//            //assert
//            Assert.Throws<ArgumentException>(() => _orderProcedureService.Update(id, OrderProcedureToUpdate));
//        }

//        [Fact]
//        public async Task CanDeleteOrderProcedure()
//        {
//            //arrange
//            var id = 10;

//            _mockOrderProcedureRepository.Setup(b => b.GetFirstOrDefaultAsync(
//                b => b.Id == id, null, false).Result)
//                .Returns(new OrderProcedure() { Id = id });

//            _mockOrderProcedureRepository.Setup(b => b.Delete(It.IsAny<OrderProcedure>())).Verifiable();
//            //act
//            await _orderProcedureService.DeleteAsync(id);
//            //assert
//            _mockOrderProcedureRepository.Verify(b => b.Delete(It.IsAny<OrderProcedure>()));
//        }

//        [Fact]
//        public void DeleteOrderProcedureByInvalidId()
//        {
//            //assert
//            Assert.Throws<AggregateException>(() => _orderProcedureService.DeleteAsync(100).Wait());
//        }

//        [Fact]
//        public void CanDeleteRange()
//        {
//            //arrange
//            List<int> ids = new List<int>() { 8, 9, 10 };

//            var OrderProcedures = OrderProcedureFakeData.GetOrderProcedureFakeData().AsQueryable();

//            _mockOrderProcedureRepository.Setup(b => b.GetAsync(It.IsAny<Expression<Func<OrderProcedure, bool>>>(), null, null, false).Result)
//                .Returns((Expression<Func<OrderProcedure, bool>> filter,
//                Func<IQueryable<OrderProcedure>, IOrderedQueryable<OrderProcedure>> OrderProcedureBy,
//                Func<IQueryable<OrderProcedure>, IIncludableQueryable<Employee, object>> include,
//                bool asNoTracking) => OrderProcedures.Where(filter).ToList());

//            _mockOrderProcedureRepository.Setup(b => b.DeleteRange(It.IsAny<IEnumerable<OrderProcedure>>()));
//            //act
//            _orderProcedureService.DeleteRangeAsync(ids).Wait();
//            //assert
//            _mockOrderProcedureRepository.Verify(b => b.DeleteRange(It.IsAny<IEnumerable<OrderProcedure>>()));
//        }

//        [Fact]
//        public void DeleteRangeWithInvalidId()
//        {
//            //arrange
//            List<int> ids = new List<int>() { 8, 9, 100 };

//            var OrderProcedures = OrderProcedureFakeData.GetOrderProcedureFakeData().AsQueryable();

//            _mockOrderProcedureRepository.Setup(b => b.GetAsync(It.IsAny<Expression<Func<OrderProcedure, bool>>>(), null, null, false).Result)
//                .Returns((Expression<Func<OrderProcedure, bool>> filter,
//                Func<IQueryable<OrderProcedure>, IOrderedQueryable<OrderProcedure>> OrderProcedureBy,
//                Func<IQueryable<OrderProcedure>, IIncludableQueryable<Employee, object>> include,
//                bool asNoTracking) => OrderProcedures.Where(filter).ToList());

//            _mockOrderProcedureRepository.Setup(b => b.DeleteRange(It.IsAny<IEnumerable<OrderProcedure>>()));
//            //assert
//            Assert.Throws<AggregateException>(() => _orderProcedureService.DeleteRangeAsync(ids).Wait());
//        }
//    }
//}
