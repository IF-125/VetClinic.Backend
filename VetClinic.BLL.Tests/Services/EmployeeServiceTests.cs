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
            _employeeRepository.Setup(x => x.GetAsync(null, null, null, true).Result)
                .Returns(EmployeeFakeData.GetEmployeeFakeData());

            IList<Employee> testEmployees = await _employeeService.GetEmployeesAsync(null, null, null, asNoTracking: true);

            Assert.NotNull(testEmployees);
            Assert.Equal(10, testEmployees.Count);
        }

        [Fact]
        public async Task CanReturnEmployeeById()
        {
            var id = "f1a05cca-b479-4f72-bbda-96b8979f4afe";

            var employees = EmployeeFakeData.GetEmployeeFakeData().AsQueryable();

            _employeeRepository.Setup(x => x.GetFirstOrDefaultAsync(
                x => x.Id == id, null, true).Result)
                .Returns((Expression<Func<Employee, bool>> filter,
                Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => employees.FirstOrDefault(filter));

            var employee = await _employeeService.GetByIdAsync(id);

            Assert.Equal("Bob", employee.FirstName);
        }

        [Fact]
        public void GetEmployeeById_ShouldReturnException()
        {
            var id = "fffff";

            var employees = EmployeeFakeData.GetEmployeeFakeData().AsQueryable();

            _employeeRepository.Setup(x => x.GetFirstOrDefaultAsync(
                x => x.Id == id, null, true).Result)
                .Returns((Expression<Func<Employee, bool>> filter,
                Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => employees.FirstOrDefault(filter));

            Assert.Throws<AggregateException>(() => _employeeService.GetByIdAsync(id).Result);

            
        }

        [Fact]
        public async Task CanInsertEmployeeAsync()
        {
            var newEmployee = new Employee
            {
                Id = "f1a05cca-b479-4f72-bbda-96b8979f4123",
                FirstName = "Added",
                LastName = "User",
                Address = "9 Grayhawk Alley",
                Email = "tisacq0@unesco.org"
            };

            _employeeRepository.Setup(x => x.InsertAsync(It.IsAny<Employee>()));

            await _employeeService.InsertAsync(newEmployee);

            _employeeRepository.Verify(a => a.InsertAsync(newEmployee));
        }

        [Fact]
        public void CanUpdateEmployee()
        {
            var newEmployee = new Employee
            {
                Id = "f1a05cca-b479-4f72-bbda-96b8979f4123",
                FirstName = "Added",
                LastName = "User",
                Address = "9 Grayhawk Alley",
                Email = "tisacq0@unesco.org"
            };

            var id = "f1a05cca-b479-4f72-bbda-96b8979f4123";

            _employeeRepository.Setup(x => x.Update(It.IsAny<Employee>()));

            _employeeService.Update(id, newEmployee);

            _employeeRepository.Verify(x => x.Update(newEmployee));
        }

        [Fact]
        public void UpdateEmployee_ThrowsException()
        {
            var newEmployee = new Employee
            {
                Id = "f1a05cca-b479-4f72-bbda-96b8979f4123",
                FirstName = "Added",
                LastName = "User",
                Address = "9 Grayhawk Alley",
                Email = "tisacq0@unesco.org"
            };

            var id = "DummyIdentificator";

            _employeeRepository.Setup(x => x.Update(It.IsAny<Employee>()));

            Assert.Throws<ArgumentException>(() => _employeeService.Update(id, newEmployee));
        }

        [Fact]
        public async Task CanDeleteEmployeeAsync()
        {
            var id = "f1a05cca-b479-4f72-bbda-96b8979f4afe";

            _employeeRepository.Setup(x => x.GetFirstOrDefaultAsync(
                x => x.Id == id, null, false).Result)
                .Returns(new Employee() { Id = id });

            _employeeRepository.Setup(x => x.Delete(It.IsAny<Employee>())).Verifiable();

            await _employeeService.DeleteAsync(id);

            _employeeRepository.Verify(x => x.Delete(It.IsAny<Employee>()));


        }

        [Fact]
        public void DeleteEmployee_WhenEmployeeDoesNotExist()
        { 
             Assert.Throws<AggregateException>(() => _employeeService.DeleteAsync("ShouldnotFind").Wait());
        }

        [Fact]
        public void CanDeleteRangeAsync()
        {
            var idArr = new string[] 
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

            _employeeService.DeleteRangeAsync(idArr).Wait();

            _employeeRepository.Verify(x => x.DeleteRange(It.IsAny<IEnumerable<Employee>>()));
        }

        [Fact]
        public void DeleteRangeAsync_ShouldReturnException_BecauseOneEmployeeWasNotFound()
        {
            var idArr = new string[]
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

            Assert.Throws<AggregateException>(() => _employeeService.DeleteRangeAsync(idArr).Wait());
        }
    }
}
