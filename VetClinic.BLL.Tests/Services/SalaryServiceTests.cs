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
    public class SalaryServiceTests
    {
        private readonly SalaryService _salaryService;
        private readonly Mock<ISalaryRepository> _mockSalaryRepository = new Mock<ISalaryRepository>();
        private readonly Mock<IEmployeeRepository> _mockEmployeeRepository = new Mock<IEmployeeRepository>();
        public SalaryServiceTests()
        {
            _salaryService = new SalaryService(
                _mockSalaryRepository.Object,
                _mockEmployeeRepository.Object);
        }

        [Fact]
        public async Task CanReturnSalaryById()
        {
            //Arrange
            var id = 2;

            var salaries = SalaryFakeData.GetFakeSalaryData().AsQueryable();

            _mockSalaryRepository.Setup(x => x
            .GetFirstOrDefaultAsync(
                x => x.Id == id,
                null,
                false).Result)
                .Returns((Expression<Func<Salary, bool>> filter,
                Func<IQueryable<Salary>, IIncludableQueryable<Salary, object>> include,
                bool asNoTracking) => salaries.FirstOrDefault(filter));

            var expectedValue = 10100;

            //Act
            var salary = await _salaryService.GetByIdAsync(id);

            //Assert
            Assert.Equal(expectedValue, salary.Amount);
        }

        [Fact]
        public async Task GetSalaryById_ShouldReturnException()
        {
            //Arrange
            int id = -2;

            var salaries = SalaryFakeData.GetFakeSalaryData().AsQueryable();

            _mockSalaryRepository.Setup(x => x
            .GetFirstOrDefaultAsync(
                x => x.Id == id,
                null,
                false))
                .ReturnsAsync((Expression<Func<Salary, bool>> filter,
                Func<IQueryable<Salary>, IIncludableQueryable<Salary, object>> include,
                bool asNoTracking) => salaries.FirstOrDefault(filter));

            //Act, Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await _salaryService.GetByIdAsync(id));
        }

        [Fact]
        public async Task CanInsertSalaryAsync()
        {
            //Arrange
            var newSalary = new Salary
            {
                Id = 1,
                Amount = 1000,
                Bonus = 10,
                Date = new System.DateTime(2021, 8, 6),
                EmployeePositionId = 1
            };

            _mockSalaryRepository.Setup(x => x.InsertAsync(It.IsAny<Salary>()));

            //Act
            await _salaryService.InsertAsync(newSalary);

            //Assert
            _mockSalaryRepository.Verify(a => a.InsertAsync(newSalary));
        }

        [Fact]
        public void CanUpdateSalary()
        {
            //Arrange
            var id = 2;

            var salary = SalaryFakeData.GetFakeSalaryData().FirstOrDefault(x => x.Id == 2);

            _mockSalaryRepository.Setup(x => x.Update(It.IsAny<Salary>()));

            //Act
            _salaryService.Update(id, salary);

            //Assert
            _mockSalaryRepository.Verify(x => x.Update(salary));
        }

        [Fact]
        public void UpdateSalary_ThrowsException()
        {
            //Arrange
            var id = 12;

            var salary = SalaryFakeData.GetFakeSalaryData().FirstOrDefault(x => x.Id == 2);

            _mockSalaryRepository.Setup(x => x.Update(It.IsAny<Salary>()));

            //Act, Assert
            Assert.Throws<BadRequestException>(() => _salaryService.Update(id, salary));
        }

        [Fact]
        public async Task CanDeleteSalaryAsync()
        {
            //Arrange
            var id = 2;

            _mockSalaryRepository.Setup(x => x.GetFirstOrDefaultAsync(
                x => x.Id == id, null, false).Result)
                .Returns(new Salary() { Id = id });

            _mockSalaryRepository.Setup(x => x.Delete(It.IsAny<Salary>())).Verifiable();

            //Act
            await _salaryService.DeleteAsync(id);

            //Assert
            _mockSalaryRepository.Verify(x => x.Delete(It.IsAny<Salary>()));


        }

        [Fact]
        public async Task DeletePosition_WhenPositionDoesNotExist()
        {
            await Assert.ThrowsAsync<NotFoundException>(async () =>
             await _salaryService.DeleteAsync(500));
        }

        [Fact]
        public void CanDeleteRangeAsync()
        {
            //Arrange
            var listOfIds = new List<int> { 2, 3, 4 };

            var salaries = SalaryFakeData.GetFakeSalaryData().AsQueryable();

            _mockSalaryRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Salary, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<Salary, bool>> filter,
                Func<IQueryable<Salary>, IOrderedQueryable<Salary>> orderBy,
                Func<IQueryable<Salary>, IIncludableQueryable<Salary, object>> include,
                bool asNoTracking) => salaries.Where(filter).ToList());

            _mockSalaryRepository.Setup(x => x.DeleteRange(It.IsAny<IEnumerable<Salary>>()));

            //Act
            _salaryService.DeleteRangeAsync(listOfIds).Wait();

            //Assert
            _mockSalaryRepository.Verify(x => x.DeleteRange(It.IsAny<IEnumerable<Salary>>()));
        }

        [Fact]
        public async Task DeleteRangeAsync_ShouldReturnException_BecauseOneSalaryWasNotFound()
        {
            //Arrange
            var listOfIds = new List<int> { 2, 400, 3 };

            var salaries = SalaryFakeData.GetFakeSalaryData().AsQueryable();

            _mockSalaryRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Salary, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<Salary, bool>> filter,
                Func<IQueryable<Salary>, IOrderedQueryable<Salary>> orderBy,
                Func<IQueryable<Salary>, IIncludableQueryable<Salary, object>> include,
                bool asNoTracking) => salaries.Where(filter).ToList());

            _mockSalaryRepository.Setup(x => x.DeleteRange(It.IsAny<IEnumerable<Salary>>()));

            //Act, Assert
            await Assert.ThrowsAsync<BadRequestException>(async () =>
                await _salaryService.DeleteRangeAsync(listOfIds));
        }

        [Fact]
        public async Task CanGetSalariesOfEmployee()
        {
            //Arrange
            var employeePositionId = 2;

            var salaries = SalaryFakeData.GetFakeSalaryData().AsQueryable();

            _mockSalaryRepository.Setup(x => x
            .GetAsync(
                x => x.EmployeePositionId == employeePositionId,
                null,
                null,
                false))
            .ReturnsAsync((Expression<Func<Salary, bool>> filter,
                Func<IQueryable<Salary>, IOrderedQueryable<Salary>> orderBy,
                Func<IQueryable<Salary>, IIncludableQueryable<Salary, object>> include,
                bool asNoTracking) => salaries.Where(filter).ToList());

            var expectedResult = 3;

            //Act
            var testSalaries = await _salaryService.GetSalariesOfEmployee(employeePositionId);

            //Assert
            Assert.Equal(expectedResult, testSalaries.Count());
        }

        [Fact]
        public async Task GetSalariesOfEmployee_ReturnsBadRequestException()
        {
            //Arrange
            var employeePositionId = -1;

            var salaries = SalaryFakeData.GetFakeSalaryData().AsQueryable();

            _mockSalaryRepository.Setup(x => x
            .GetAsync(
                x => x.EmployeePositionId == employeePositionId,
                null,
                null,
                false))
            .ReturnsAsync((Expression<Func<Salary, bool>> filter,
                Func<IQueryable<Salary>, IOrderedQueryable<Salary>> orderBy,
                Func<IQueryable<Salary>, IIncludableQueryable<Salary, object>> include,
                bool asNoTracking) => salaries.Where(filter).ToList());

            //Act, Assert
            await Assert.ThrowsAsync<BadRequestException>(async () =>
                await _salaryService.GetSalariesOfEmployee(employeePositionId));
        }

        [Fact]
        public async Task CanAssignSalaryToEmployee()
        {
            //Arrange
            var employeeId = "70efa05d-a06d-45b2-b116-af1ccb602d2e";
            var salary = new Salary
            {
                Id = 4,
                Amount = 10000,
                Bonus = 10,
                Date = new System.DateTime(2021, 8, 6),
                EmployeePositionId = 2
            };

            _mockEmployeeRepository.Setup(x => x
            .GetFirstOrDefaultAsync(
                x => x.Id == employeeId,
                It.IsAny<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>(),
                false).Result)
                .Returns(new Employee { Id = employeeId });

            _mockSalaryRepository.Setup(x => x.InsertAsync(It.IsAny<Salary>())).Verifiable();

            _mockEmployeeRepository.Setup(x => x.Update(It.IsAny<Employee>())).Verifiable();

            //Act
            await _salaryService.AssignSalaryToEmployee(employeeId, salary);

            //Assert
            _mockSalaryRepository.Verify(x => x.InsertAsync(salary));
            _mockEmployeeRepository.Verify(x => x.Update(It.IsAny<Employee>()));
        }
    }
}
