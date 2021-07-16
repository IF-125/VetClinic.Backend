using Microsoft.EntityFrameworkCore.Query;
using Moq;
using SendGrid.Helpers.Errors.Model;
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
            //Arrange
            _positionRepository.Setup(x => x.GetAsync(null, null, null, true).Result)
                .Returns(PositionFakeData.GetPositionFakeData());

            int expectedCount = 4;
            
            //Act
            IList<Position> testEmployees = await _positionService.GetPositionsAsync();

            //Assert
            Assert.NotNull(testEmployees);
            Assert.Equal(expectedCount, testEmployees.Count);
        }

        [Fact]
        public async Task CanReturnPositionById()
        {
            //Arrange
            var id = 2;

            var positions = PositionFakeData.GetPositionFakeData().AsQueryable();

            _positionRepository.Setup(x => x.GetFirstOrDefaultAsync(
                x => x.Id == id, null, false).Result)
                .Returns((Expression<Func<Position, bool>> filter,
                Func<IQueryable<Employee>, IIncludableQueryable<Position, object>> include,
                bool asNoTracking) => positions.FirstOrDefault(filter));

            //Act
            var employee = await _positionService.GetByIdAsync(id);

            //Assert
            Assert.Equal("Anesthetist", employee.Title);
        }

        [Fact]
        public async Task GetPositionById_ShouldReturnException()
        {
            //Arrange
            var id = 1234;

            var positions = PositionFakeData.GetPositionFakeData().AsQueryable();

            _positionRepository.Setup(x => x.GetFirstOrDefaultAsync(
                x => x.Id == id, null, true).Result)
                .Returns((Expression<Func<Position, bool>> filter,
                Func<IQueryable<Position>, IIncludableQueryable<Position, object>> include,
                bool asNoTracking) => positions.FirstOrDefault(filter));

            //Act, Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _positionService.GetByIdAsync(id));


        }

        [Fact]
        public async Task CanInsertPositionAsync()
        {
            //Arrange
            var newPosition = new Position
            {
                Id = 5,
                Title = "New Position",
            };

            _positionRepository.Setup(x => x.InsertAsync(It.IsAny<Position>()));

            //Act
            await _positionService.InsertAsync(newPosition);

            //Assert
            _positionRepository.Verify(a => a.InsertAsync(newPosition));
        }

        [Fact]
        public void CanUpdatePosition()
        {
            //Arrange
            var id = 2;

            var position = PositionFakeData.GetPositionFakeData().FirstOrDefault(x => x.Id == 2);

            _positionRepository.Setup(x => x.Update(It.IsAny<Position>()));

            //Act
            _positionService.Update(id, position);

            //Assert
            _positionRepository.Verify(x => x.Update(position));
        }

        [Fact]
        public void UpdatePosition_ThrowsException()
        {
            //Arrange
            var id = 12;

            var position = PositionFakeData.GetPositionFakeData().FirstOrDefault(x => x.Id == 2);

            _positionRepository.Setup(x => x.Update(It.IsAny<Position>()));

            //Act, Assert
            Assert.Throws<BadRequestException>(() => _positionService.Update(id, position));
        }

        [Fact]
        public async Task CanDeletePositionAsync()
        {
            //Arrange
            var id = 2;

            _positionRepository.Setup(x => x.GetFirstOrDefaultAsync(
                x => x.Id == id, null, false).Result)
                .Returns(new Position() { Id = id });

            _positionRepository.Setup(x => x.Delete(It.IsAny<Position>())).Verifiable();

            //Act
            await _positionService.DeleteAsync(id);

            //Assert
            _positionRepository.Verify(x => x.Delete(It.IsAny<Position>()));


        }

        [Fact]
        public async Task DeletePosition_WhenPositionDoesNotExist()
        {
           await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _positionService.DeleteAsync(500));
        }

        [Fact]
        public void CanDeleteRangeAsync()
        {
            //Arrange
            var listOfIds = new List<int> { 2, 3, 4 };

            var positions = PositionFakeData.GetPositionFakeData().AsQueryable();

            _positionRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Position, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<Position, bool>> filter,
                Func<IQueryable<Position>, IOrderedQueryable<Position>> orderBy,
                Func<IQueryable<Position>, IIncludableQueryable<Position, object>> include,
                bool asNoTracking) => positions.Where(filter).ToList());

            _positionRepository.Setup(x => x.DeleteRange(It.IsAny<IEnumerable<Position>>()));

            //Act
            _positionService.DeleteRangeAsync(listOfIds).Wait();

            //Assert
            _positionRepository.Verify(x => x.DeleteRange(It.IsAny<IEnumerable<Position>>()));
        }

        [Fact]
        public async Task DeleteRangeAsync_ShouldReturnException_BecauseOneEmployeeWasNotFound()
        {
            //Arrange
            var listOfIds = new List<int> { 2, 400, 3 };

            var positions = PositionFakeData.GetPositionFakeData().AsQueryable();

            _positionRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Position, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<Position, bool>> filter,
                Func<IQueryable<Position>, IOrderedQueryable<Position>> orderBy,
                Func<IQueryable<Position>, IIncludableQueryable<Position, object>> include,
                bool asNoTracking) => positions.Where(filter).ToList());

            _positionRepository.Setup(x => x.DeleteRange(It.IsAny<IEnumerable<Position>>()));

            //Act, Assert
            await Assert.ThrowsAsync<BadRequestException>(async () => 
                await _positionService.DeleteRangeAsync(listOfIds));
        }
    }
}
