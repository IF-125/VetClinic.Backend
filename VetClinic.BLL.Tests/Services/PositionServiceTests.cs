using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System;
using System.Collections;
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
    public class PositionServiceTests
    {
        private readonly PositionService _positionService;
        private readonly Mock<IPositionRepository> _positionRepository = new Mock<IPositionRepository>();
        public PositionServiceTests()
        {
            _positionService = new PositionService(
                _positionRepository.Object);
        }

        [Fact]
        public async Task CanReturnAllPositions()
        {
            _positionRepository.Setup(x => x.GetAsync(null, null, null, true).Result)
                .Returns(PositionFakeData.GetPositionFakeData());

            IList<Position> testEmployees = await _positionService.GetPositionsAsync(null, null, null, asNoTracking: true);

            Assert.NotNull(testEmployees);
            Assert.Equal(4, testEmployees.Count);
        }

        [Fact]
        public async Task CanReturnPositionById()
        {
            var id = 2;

            var positions = PositionFakeData.GetPositionFakeData().AsQueryable();

            _positionRepository.Setup(x => x.GetFirstOrDefaultAsync(
                x => x.Id == id, null, true).Result)
                .Returns((Expression<Func<Position, bool>> filter,
                Func<IQueryable<Employee>, IIncludableQueryable<Position, object>> include,
                bool asNoTracking) => positions.FirstOrDefault(filter));

            var employee = await _positionService.GetByIdAsync(id, null, true);

            Assert.Equal("Anesthetist", employee.Title);
        }

        [Fact]
        public void GetPositionById_ShouldReturnException()
        {
            var id = 1234;

            var positions = PositionFakeData.GetPositionFakeData().AsQueryable();

            _positionRepository.Setup(x => x.GetFirstOrDefaultAsync(
                x => x.Id == id, null, true).Result)
                .Returns((Expression<Func<Position, bool>> filter,
                Func<IQueryable<Position>, IIncludableQueryable<Position, object>> include,
                bool asNoTracking) => positions.FirstOrDefault(filter));

            Assert.Throws<AggregateException>(() => _positionService.GetByIdAsync(id).Result);


        }

        [Fact]
        public async Task CanInsertPositionAsync()
        {
            var newPosition = new Position
            {
                Id = 5,
                Title = "New Position",
            };

            _positionRepository.Setup(x => x.InsertAsync(It.IsAny<Position>()));

            await _positionService.InsertAsync(newPosition);

            _positionRepository.Verify(a => a.InsertAsync(newPosition));
        }

        [Fact]
        public void CanUpdatePosition()
        {
            var position = PositionFakeData.GetPositionFakeData().FirstOrDefault(x => x.Id == 2);

            var id = 2;

            _positionRepository.Setup(x => x.Update(It.IsAny<Position>()));

            _positionService.Update(id, position);

            _positionRepository.Verify(x => x.Update(position));
        }

        [Fact]
        public void UpdatePosition_ThrowsException()
        {
            var position = PositionFakeData.GetPositionFakeData().FirstOrDefault(x => x.Id == 2);

            var id = 12;

            _positionRepository.Setup(x => x.Update(It.IsAny<Position>()));

            Assert.Throws<ArgumentException>(() => _positionService.Update(id, position));
        }

        [Fact]
        public async Task CanDeletePositionAsync()
        {
            var id = 2;

            _positionRepository.Setup(x => x.GetFirstOrDefaultAsync(
                x => x.Id == id, null, false).Result)
                .Returns(new Position() { Id = id });

            _positionRepository.Setup(x => x.Delete(It.IsAny<Position>())).Verifiable();

            await _positionService.DeleteAsync(id);

            _positionRepository.Verify(x => x.Delete(It.IsAny<Position>()));


        }

        [Fact]
        public void DeletePosition_WhenPositionDoesNotExist()
        {
            Assert.Throws<AggregateException>(() => _positionService.DeleteAsync(500).Wait());
        }

        [Fact]
        public void CanDeleteRangeAsync()
        {
            var idArr = new int[] { 2, 3, 4 };

            var positions = PositionFakeData.GetPositionFakeData().AsQueryable();

            _positionRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Position, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<Position, bool>> filter,
                Func<IQueryable<Position>, IOrderedQueryable<Position>> orderBy,
                Func<IQueryable<Position>, IIncludableQueryable<Position, object>> include,
                bool asNoTracking) => positions.Where(filter).ToList());

            _positionRepository.Setup(x => x.DeleteRange(It.IsAny<IEnumerable<Position>>()));

            _positionService.DeleteRangeAsync(idArr).Wait();

            _positionRepository.Verify(x => x.DeleteRange(It.IsAny<IEnumerable<Position>>()));
        }

        [Fact]
        public void DeleteRangeAsync_ShouldReturnException_BecauseOneEmployeeWasNotFound()
        {
            var idArr = new int[] { 2, 400, 3 };

            var positions = PositionFakeData.GetPositionFakeData().AsQueryable();

            _positionRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Position, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<Position, bool>> filter,
                Func<IQueryable<Position>, IOrderedQueryable<Position>> orderBy,
                Func<IQueryable<Position>, IIncludableQueryable<Position, object>> include,
                bool asNoTracking) => positions.Where(filter).ToList());

            _positionRepository.Setup(x => x.DeleteRange(It.IsAny<IEnumerable<Position>>()));

            Assert.Throws<AggregateException>(() => _positionService.DeleteRangeAsync(idArr).Wait());
        }
    }
}
