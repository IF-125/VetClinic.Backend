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
    public class EmployeePositionServiceTests
    {
        private readonly EmployeePositionService _employeePositionService;
        private readonly Mock<IEmployeePositionRepository> _mockEmployeePositionRepository = new Mock<IEmployeePositionRepository>();
        private readonly Mock<IEmployeeRepository> _mockEmployeeRepository = new Mock<IEmployeeRepository>();
        private readonly Mock<IPositionRepository> _mockPositionRepository = new Mock<IPositionRepository>();
        public EmployeePositionServiceTests()
        {
            _employeePositionService = new EmployeePositionService(
                _mockEmployeePositionRepository.Object,
                _mockEmployeeRepository.Object,
                _mockPositionRepository.Object);
        }

        [Fact]
        public async Task CanReturnAllEmployeePositions()
        {
            //Arrange
            _mockEmployeePositionRepository.Setup(x => x.GetAsync(
                null,
                null,
                It.IsAny<Func<IQueryable<EmployeePosition>, IIncludableQueryable<EmployeePosition, object>>>(),
                true).Result)
                .Returns(EmployeePositionFakeData.GetEmployeePositionFakeData());

            int expectedCount = 5;

            //Act
            IList<EmployeePosition> testEmployeePositions = await _employeePositionService.GetEmployeePositionsAsync();

            //Assert
            Assert.NotNull(testEmployeePositions);
            Assert.Equal(expectedCount, testEmployeePositions.Count);
        }

        [Fact]
        public async Task CanReturnEmployeePositionById()
        {
            //Arrange
            var id = 2;

            var employeePositions = EmployeePositionFakeData.GetEmployeePositionFakeData().AsQueryable();

            _mockEmployeePositionRepository.Setup(x => x.GetFirstOrDefaultAsync(
                x => x.Id == id,
                It.IsAny<Func<IQueryable<EmployeePosition>, IIncludableQueryable<EmployeePosition, object>>>(),
                false).Result)
                .Returns((Expression<Func<EmployeePosition, bool>> filter,
                Func<IQueryable<EmployeePosition>, IIncludableQueryable<EmployeePosition, object>> include,
                bool asNoTracking) => employeePositions.FirstOrDefault(filter));

            var expectedResult = 10002; 

            //Act
            var testEmployeePosition = await _employeePositionService.GetByIdAsync(id);

            //Assert
            Assert.Equal(expectedResult, testEmployeePosition.CurrentBaseSalary);
        }

        [Fact]
        public async Task GetEmployeePositionById_ShouldReturnException()
        {
            //Arrange
            var id = 1234;

            var employeePositions = EmployeePositionFakeData.GetEmployeePositionFakeData().AsQueryable();

            _mockEmployeePositionRepository.Setup(x => x.GetFirstOrDefaultAsync(
                x => x.Id == id,
                It.IsAny<Func<IQueryable<EmployeePosition>, IIncludableQueryable<EmployeePosition, object>>>(),
                false))
                .ReturnsAsync((Expression<Func<EmployeePosition, bool>> filter,
                Func<IQueryable<EmployeePosition>, IIncludableQueryable<EmployeePosition, object>> include,
                bool asNoTracking) => employeePositions.FirstOrDefault(filter));

            //Act, Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _employeePositionService.GetByIdAsync(id));


        }

        [Fact]
        public async Task CanInsertEmployeePositionAsync()
        {
            //Arrange
            var newEmployeePosition = new EmployeePosition
            {
                Id = 12,
                CurrentBaseSalary = 102,
                DismissedDate = null,
                HierdDate = new DateTime(2021, 5, 2),
                EmployeeId = "f1a05cca-b479-4f72-bbda-96b8979f4afe",
                PositionId = 2,
                Rate = 12
            };

            _mockEmployeePositionRepository.Setup(x => x.InsertAsync(It.IsAny<EmployeePosition>()));

            //Act
            await _employeePositionService.InsertAsync(newEmployeePosition);

            //Assert
            _mockEmployeePositionRepository.Verify(a => a.InsertAsync(newEmployeePosition));
        }

        [Fact]
        public void CanUpdateEmployeePosition()
        {
            //Arrange
            var id = 2;

            var employeePosition = EmployeePositionFakeData
                .GetEmployeePositionFakeData()
                .FirstOrDefault(x => x.Id == 2);

            _mockEmployeePositionRepository.Setup(x => x.Update(It.IsAny<EmployeePosition>()));

            //Act
            _employeePositionService.Update(id, employeePosition);

            //Assert
            _mockEmployeePositionRepository.Verify(x => x.Update(employeePosition));
        }

        [Fact]
        public void UpdateEmployeePosition_ThrowsException()
        {
            //Arrange
            var id = 12;

            var employeePosition = EmployeePositionFakeData
                .GetEmployeePositionFakeData()
                .FirstOrDefault(x => x.Id == 2);

            _mockEmployeePositionRepository.Setup(x => x.Update(It.IsAny<EmployeePosition>()));

            //Act, Assert
            Assert.Throws<BadRequestException>(() => _employeePositionService.Update(id, employeePosition));
        }

        [Fact]
        public async Task CanDeletePositionAsync()
        {
            //Arrange
            var id = 2;

            _mockEmployeePositionRepository.Setup(x => x.GetFirstOrDefaultAsync(
                x => x.Id == id, null, false).Result)
                .Returns(new EmployeePosition() { Id = id });

            _mockEmployeePositionRepository.Setup(x => x.Delete(It.IsAny<EmployeePosition>())).Verifiable();

            //Act
            await _employeePositionService.DeleteAsync(id);

            //Assert
            _mockEmployeePositionRepository.Verify(x => x.Delete(It.IsAny<EmployeePosition>()));


        }

        [Fact]
        public async Task DeletePosition_WhenPositionDoesNotExist()
        {
            await Assert.ThrowsAsync<NotFoundException>(async () =>
             await _employeePositionService.DeleteAsync(500));
        }

        [Fact]
        public void CanDeleteRangeAsync()
        {
            //Arrange
            var listOfIds = new List<int> { 2, 3, 4 };

            var employeePositions = EmployeePositionFakeData.GetEmployeePositionFakeData().AsQueryable();

            _mockEmployeePositionRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<EmployeePosition, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<EmployeePosition, bool>> filter,
                Func<IQueryable<EmployeePosition>, IOrderedQueryable<EmployeePosition>> orderBy,
                Func<IQueryable<EmployeePosition>, IIncludableQueryable<EmployeePosition, object>> include,
                bool asNoTracking) => employeePositions.Where(filter).ToList());

            _mockEmployeePositionRepository.Setup(x => x.DeleteRange(It.IsAny<IEnumerable<EmployeePosition>>()));

            //Act
            _employeePositionService.DeleteRangeAsync(listOfIds).Wait();

            //Assert
            _mockEmployeePositionRepository.Verify(x => x.DeleteRange(It.IsAny<IEnumerable<EmployeePosition>>()));
        }

        [Fact]
        public async Task DeleteRangeAsync_ShouldReturnException_BecauseOneEmployeeWasNotFound()
        {
            //Arrange
            var listOfIds = new List<int> { 2, 400, 3 };

            var employeePositions = EmployeePositionFakeData.GetEmployeePositionFakeData().AsQueryable();

            _mockEmployeePositionRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<EmployeePosition, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<EmployeePosition, bool>> filter,
                Func<IQueryable<EmployeePosition>, IOrderedQueryable<EmployeePosition>> orderBy,
                Func<IQueryable<EmployeePosition>, IIncludableQueryable<EmployeePosition, object>> include,
                bool asNoTracking) => employeePositions.Where(filter).ToList());

            _mockEmployeePositionRepository.Setup(x => x.DeleteRange(It.IsAny<IEnumerable<EmployeePosition>>()));

            //Act, Assert
            await Assert.ThrowsAsync<BadRequestException>(async () =>
                await _employeePositionService.DeleteRangeAsync(listOfIds));
        }

        [Fact]
        public async Task CanAssignPositionToEmployee()
        {
            //Arrange
            var employeePosition = new EmployeePosition
            {
                Id = 1,
                CurrentBaseSalary = 1000,
                DismissedDate = null,
                HierdDate = new System.DateTime(2021, 5, 12),
                EmployeeId = "6fca381a-40d0-4bf9-a076-706e1a995662",
                PositionId = 1,
                Rate = 12
            };

            _mockEmployeeRepository.Setup(x => x
            .GetFirstOrDefaultAsync(x => x.Id == employeePosition.EmployeeId, null, false))
                .ReturnsAsync(new Employee { Id = employeePosition.EmployeeId });

            _mockPositionRepository.Setup(x => x
            .GetFirstOrDefaultAsync(x => x.Id == employeePosition.PositionId, null, false))
                .ReturnsAsync(new Position { Id = employeePosition.PositionId });

            _mockEmployeePositionRepository.Setup(x => x.InsertAsync(employeePosition));

            //Act
            await _employeePositionService.AssignPositionToEmployeeAsync(employeePosition);

            //Assert
            _mockEmployeePositionRepository.Verify(x => x.InsertAsync(employeePosition));
        }

        [Fact]
        public async Task AssignPositionToEmployee_ThrowsNotFoundException_BecauseEmployeeWasNotFound()
        {
            //Arrange
            var employeePosition = new EmployeePosition
            {
                Id = 1,
                CurrentBaseSalary = 1000,
                DismissedDate = null,
                HierdDate = new System.DateTime(2021, 5, 12),
                EmployeeId = "6fca381a-40d0-4bf9-a076-706e1a995662",
                PositionId = 1,
                Rate = 12
            };

            Employee employee = null;

            _mockEmployeeRepository.Setup(x => x
            .GetFirstOrDefaultAsync(x => x.Id == employeePosition.EmployeeId, null, false))
                .ReturnsAsync(employee);

            _mockPositionRepository.Setup(x => x
            .GetFirstOrDefaultAsync(x => x.Id == employeePosition.PositionId, null, false))
                .ReturnsAsync(new Position { Id = employeePosition.PositionId });

            _mockEmployeePositionRepository.Setup(x => x.InsertAsync(employeePosition));

            //Act, Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _employeePositionService.AssignPositionToEmployeeAsync(employeePosition));
        }

        [Fact]
        public async Task AssignPositionToEmployee_ThrowsNotFoundException_BecausePositionWasNotFound()
        {
            //Arrange
            var employeePosition = new EmployeePosition
            {
                Id = 1,
                CurrentBaseSalary = 1000,
                DismissedDate = null,
                HierdDate = new System.DateTime(2021, 5, 12),
                EmployeeId = "6fca381a-40d0-4bf9-a076-706e1a995662",
                PositionId = 1,
                Rate = 12
            };

            Position position = null;

            _mockEmployeeRepository.Setup(x => x
            .GetFirstOrDefaultAsync(x => x.Id == employeePosition.EmployeeId, null, false))
                .ReturnsAsync(new Employee { Id = employeePosition.EmployeeId });

            _mockPositionRepository.Setup(x => x
            .GetFirstOrDefaultAsync(x => x.Id == employeePosition.PositionId, null, false))
                .ReturnsAsync(position);

            _mockEmployeePositionRepository.Setup(x => x.InsertAsync(employeePosition));

            //Act, Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _employeePositionService.AssignPositionToEmployeeAsync(employeePosition));
        }
    }
}
