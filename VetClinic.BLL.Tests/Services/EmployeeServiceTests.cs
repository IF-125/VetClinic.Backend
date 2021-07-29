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
    public class EmployeeServiceTests
    {
        private readonly EmployeeService _employeeService;
        private readonly Mock<IEmployeeRepository> _employeeRepository = new Mock<IEmployeeRepository>();

        public EmployeeServiceTests()
        {
            _employeeService = new EmployeeService(
                _employeeRepository.Object);
        }

        [Fact]
        public async Task CanReturnAllEmployees()
        {
            //Arrange
            _employeeRepository.Setup(x => x.GetAsync(null, null, null, true).Result)
                .Returns(EmployeeFakeData.GetEmployeeFakeData());

            int expectedCount = 10;

            //Act
            IList<Employee> testEmployees = await _employeeService.GetEmployeesAsync();

            //Assert
            Assert.NotNull(testEmployees);
            Assert.Equal(expectedCount, testEmployees.Count);
        }

        [Fact]
        public async Task CanReturnEmployeeById()
        {
            //Arrange
            var id = "f1a05cca-b479-4f72-bbda-96b8979f4afe";

            var employees = EmployeeFakeData.GetEmployeeFakeData().AsQueryable();

            _employeeRepository.Setup(x => x.GetFirstOrDefaultAsync(
                x => x.Id == id, null, false).Result)
                .Returns((Expression<Func<Employee, bool>> filter,
                Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => employees.FirstOrDefault(filter));

            //Act
            var employee = await _employeeService.GetByIdAsync(id);

            //Assert
            Assert.Equal("Bob", employee.FirstName);
        }

        [Fact]
        public async Task GetEmployeeById_ShouldReturnException()
        {
            //Arrange
            var id = "fffff";

            var employees = EmployeeFakeData.GetEmployeeFakeData().AsQueryable();

            _employeeRepository.Setup(x => x.GetFirstOrDefaultAsync(
                x => x.Id == id, null, true).Result)
                .Returns((Expression<Func<Employee, bool>> filter,
                Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => employees.FirstOrDefault(filter));

            //Act, Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _employeeService.GetByIdAsync(id));

            
        }

        [Fact]
        public async Task CanInsertEmployeeAsync()
        {
            //Arrange
            var newEmployee = new Employee
            {
                Id = "f1a05cca-b479-4f72-bbda-96b8979f4123",
                FirstName = "Added",
                LastName = "User",
                Address = "9 Grayhawk Alley",
                Email = "tisacq0@unesco.org"
            };

            _employeeRepository.Setup(x => x.InsertAsync(It.IsAny<Employee>()));

            //Act
            await _employeeService.InsertAsync(newEmployee);

            //Assert
            _employeeRepository.Verify(a => a.InsertAsync(newEmployee));
        }

        [Fact]
        public void CanUpdateEmployee()
        {
            //Arrange
            var newEmployee = new Employee
            {
                Id = "f1a05cca-b479-4f72-bbda-96b8979f4123",
                FirstName = "Added",
                LastName = "User",
                Address = "9 Grayhawk Alley",
                Email = "tisacq0@unesco.org"
            };

            var id = "f1a05cca-b479-4f72-bbda-96b8979f4123";

            var employees = EmployeeFakeData.GetEmployeeFakeData().AsQueryable();

            _employeeRepository.Setup(x => x
            .IsAny(It.IsAny<Expression<Func<Employee, bool>>>()))
                .Returns(!employees.Any(x => x.Id == id));

            _employeeRepository.Setup(x => x.Update(It.IsAny<Employee>()));

            //Act
            _employeeService.Update(id, newEmployee);

            //Assert
            _employeeRepository.Verify(x => x.Update(newEmployee));
        }

        [Fact]
        public void UpdateEmployee_ThrowsException()
        {
            //Arrange
            var newEmployee = new Employee
            {
                Id = "f1a05cca-b479-4f72-bbda-96b8979f4123",
                FirstName = "Added",
                LastName = "User",
                Address = "9 Grayhawk Alley",
                Email = "tisacq0@unesco.org"
            };

            var id = "DummyIdentificator";

            var employees = EmployeeFakeData.GetEmployeeFakeData().AsQueryable();

            _employeeRepository.Setup(x => x.Update(It.IsAny<Employee>()));

            _employeeRepository.Setup(x => x
            .IsAny(x => x.Id == id))
                .Returns((Expression<Func<Employee, bool>> filter) => employees.Any(filter));

            //Act, Assert
            Assert.Throws<NotFoundException>(() => _employeeService.Update(id, newEmployee));
        }

        [Fact]
        public async Task CanDeleteEmployeeAsync()
        {
            //Arrange
            var id = "f1a05cca-b479-4f72-bbda-96b8979f4afe";

            _employeeRepository.Setup(x => x.GetFirstOrDefaultAsync(
                x => x.Id == id, null, false).Result)
                .Returns(new Employee() { Id = id });

            _employeeRepository.Setup(x => x.Delete(It.IsAny<Employee>())).Verifiable();

            //Act
            await _employeeService.DeleteAsync(id);

            //Assert
            _employeeRepository.Verify(x => x.Delete(It.IsAny<Employee>()));


        }

        [Fact]
        public async Task DeleteEmployee_WhenEmployeeDoesNotExist()
        { 
             await Assert.ThrowsAsync<NotFoundException>(async () =>
                await _employeeService.DeleteAsync("ShouldnotFind"));
        }

        [Fact]
        public void CanDeleteRangeAsync()
        {
            //Arrange
            var listOfIds = new List<string> 
            { 
                "f1a05cca-b479-4f72-bbda-96b8979f4afe",
                "6fca381a-40d0-4bf9-a076-706e1a995662",
                "804bbbca-3ffc-4d28-9b71-0d7788ddf681" 
            };

            var employees = EmployeeFakeData.GetEmployeeFakeData().AsQueryable();

            _employeeRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Employee, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<Employee, bool>> filter,
                Func<IQueryable<Employee>, IOrderedQueryable<Employee>> orderBy,
                Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => employees.Where(filter).ToList());

            _employeeRepository.Setup(x => x.DeleteRange(It.IsAny<IEnumerable<Employee>>()));

            //Act
            _employeeService.DeleteRangeAsync(listOfIds).Wait();

            //Assert
            _employeeRepository.Verify(x => x.DeleteRange(It.IsAny<IEnumerable<Employee>>()));
        }

        [Fact]
        public void DeleteRangeAsync_ShouldReturnException_BecauseOneEmployeeWasNotFound()
        {
            var listOfIds = new List<string>
            {
                "f1a05cca-b479-4f72-bbda-96b8979f4afe",
                "IDoNotExist",
                "123123"
            };

            var employees = EmployeeFakeData.GetEmployeeFakeData().AsQueryable();

            _employeeRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Employee, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<Employee, bool>> filter,
                Func<IQueryable<Employee>, IOrderedQueryable<Employee>> orderBy,
                Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => employees.Where(filter).ToList());

            _employeeRepository.Setup(x => x.DeleteRange(It.IsAny<IEnumerable<Employee>>()));

            Assert.Throws<AggregateException>(() => _employeeService.DeleteRangeAsync(listOfIds).Wait());
        }
    }
}
