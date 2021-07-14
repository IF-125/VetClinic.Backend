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
    public class OrderProcedureServiceTests
    {
        private readonly OrderProcedureService _orderProcedureService;
        private readonly Mock<IOrderProcedureRepository> _orderProcedureRepository = new Mock<IOrderProcedureRepository>();
        public OrderProcedureServiceTests()
        {
            _orderProcedureService = new OrderProcedureService(
                _orderProcedureRepository.Object);
        }

        [Fact]
        public async Task CanReturnAllOrderProcedures()
        {
            _orderProcedureRepository.Setup(b => b.GetAsync(null, null, null, true).Result)
                .Returns(OrderProcedureFakeData.GetOrderProcedureFakeData());

            IList<OrderProcedure> OrderProcedures = await _orderProcedureService.GetOrderProceduresAsync(null, null, null, asNoTracking: true);

            Assert.NotNull(OrderProcedures);
            Assert.Equal(10, OrderProcedures.Count);
        }

        [Fact]
        public async Task CanReturnOrderProcedureById()
        {
            int id = 3;

            var OrderProcedures = OrderProcedureFakeData.GetOrderProcedureFakeData().AsQueryable();

            _orderProcedureRepository.Setup(b => b.GetFirstOrDefaultAsync(
                b => b.Id == id, null, true).Result)
                .Returns((Expression<Func<OrderProcedure, bool>> filter,
                Func<IQueryable<OrderProcedure>, IIncludableQueryable<OrderProcedure, object>> include,
                bool asNoTracking) => OrderProcedures.FirstOrDefault(filter));

            var OrderProcedure = await _orderProcedureService.GetByIdAsync(id, null, true);

            Assert.Equal("Procedure was unsuccessful.", OrderProcedure.Conclusion);
        }

        [Fact]
        public void GetOrderProcedureByInvalidId()
        {
            int id = 45;

            var OrderProcedures = OrderProcedureFakeData.GetOrderProcedureFakeData().AsQueryable();

            _orderProcedureRepository.Setup(b => b.GetFirstOrDefaultAsync(
                b => b.Id == id, null, true).Result)
                .Returns((Expression<Func<OrderProcedure, bool>> filter,
                Func<IQueryable<OrderProcedure>, IIncludableQueryable<OrderProcedure, object>> include,
                bool asNoTracking) => OrderProcedures.FirstOrDefault(filter));

            Assert.Throws<AggregateException>(() => _orderProcedureService.GetByIdAsync(id).Result);
        }

        [Fact]
        public async Task CanInsertOrderProcedure()
        {
            OrderProcedure OrderProcedureToInsert = new OrderProcedure
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

            _orderProcedureRepository.Setup(b => b.InsertAsync(It.IsAny<OrderProcedure>()));

            await _orderProcedureService.InsertAsync(OrderProcedureToInsert);

            _orderProcedureRepository.Verify(b => b.InsertAsync(OrderProcedureToInsert));
        }

        [Fact]
        public void CanUpdateOrderProcedure()
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

            int id = 11;

            _orderProcedureRepository.Setup(b => b.Update(It.IsAny<OrderProcedure>()));

            _orderProcedureService.Update(id, OrderProcedureToUpdate);

            _orderProcedureRepository.Verify(b => b.Update(OrderProcedureToUpdate));
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
        public async Task CanDeleteOrderProcedure()
        {
            var id = 10;

            _orderProcedureRepository.Setup(b => b.GetFirstOrDefaultAsync(
                b => b.Id == id, null, false).Result)
                .Returns(new OrderProcedure() { Id = id });

            _orderProcedureRepository.Setup(b => b.Delete(It.IsAny<OrderProcedure>())).Verifiable();

            await _orderProcedureService.DeleteAsync(id);

            _orderProcedureRepository.Verify(b => b.Delete(It.IsAny<OrderProcedure>()));
        }

        [Fact]
        public void DeleteOrderProcedureByInvalidId()
        {
            Assert.Throws<AggregateException>(() => _orderProcedureService.DeleteAsync(100).Wait());
        }

        [Fact]
        public void CanDeleteRange()
        {
            int[] ids = new int[] { 8, 9, 10 };

            var OrderProcedures = OrderProcedureFakeData.GetOrderProcedureFakeData().AsQueryable();

            _orderProcedureRepository.Setup(b => b.GetAsync(It.IsAny<Expression<Func<OrderProcedure, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<OrderProcedure, bool>> filter,
                Func<IQueryable<OrderProcedure>, IOrderedQueryable<OrderProcedure>> OrderProcedureBy,
                Func<IQueryable<OrderProcedure>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => OrderProcedures.Where(filter).ToList());

            _orderProcedureRepository.Setup(b => b.DeleteRange(It.IsAny<IEnumerable<OrderProcedure>>()));

            _orderProcedureService.DeleteRangeAsync(ids).Wait();

            _orderProcedureRepository.Verify(b => b.DeleteRange(It.IsAny<IEnumerable<OrderProcedure>>()));
        }

        [Fact]
        public void DeleteRangeWithInvalidId()
        {
            int[] ids = new int[] { 8, 9, 100 };

            var OrderProcedures = OrderProcedureFakeData.GetOrderProcedureFakeData().AsQueryable();

            _orderProcedureRepository.Setup(b => b.GetAsync(It.IsAny<Expression<Func<OrderProcedure, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<OrderProcedure, bool>> filter,
                Func<IQueryable<OrderProcedure>, IOrderedQueryable<OrderProcedure>> OrderProcedureBy,
                Func<IQueryable<OrderProcedure>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => OrderProcedures.Where(filter).ToList());

            _orderProcedureRepository.Setup(b => b.DeleteRange(It.IsAny<IEnumerable<OrderProcedure>>()));

            Assert.Throws<AggregateException>(() => _orderProcedureService.DeleteRangeAsync(ids).Wait());
        }
    }
}
