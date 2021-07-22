using AutoMapper;
using FluentValidation.Results;
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
using VetClinic.Core.Interfaces.Services;
using VetClinic.WebApi.Controllers;
using VetClinic.WebApi.Mappers;
using VetClinic.WebApi.Validators.EntityValidators;
using VetClinic.WebApi.ViewModels;
using Xunit;
using static VetClinic.Core.Resources.TextMessages;

namespace VetClinic.WebApi.Tests.Controllers
{
    public class EmployeesControllerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
        private readonly IEmployeeService _employeeService;
        private readonly EmployeeValidator _employeeValidator;

        public EmployeesControllerTests()
        {
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            _employeeService = new EmployeeService(_mockEmployeeRepository.Object);
            _employeeValidator = new EmployeeValidator();

            if(_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new EmployeeMapperProfile());
                });

                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }

        [Fact]
        public void CanGetAllEmployees()
        {
            //Arrange
            var employeeController = new EmployeesController(
                _employeeService,
                _mapper,
                _employeeValidator);

            var employees = EmployeeFakeData.GetEmployeeFakeData();

            var expectedResult = employees.Count;

            _mockEmployeeRepository.Setup(x => x
            .GetAsync(null, null, null, true).Result).Returns(() => employees);

            //Act
            var result = employeeController.GetAllEmployeesAsync().Result;

            //Assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<EmployeeViewModel>>(viewResult.Value);
            Assert.Equal(expectedResult, model.Count());
        }

        [Fact]
        public void CanGetEmployeeById()
        {
            //Arrange
            var employeeController = new EmployeesController(
                _employeeService, 
                _mapper,
                _employeeValidator);

            var employees = EmployeeFakeData
                .GetEmployeeFakeData()
                .AsQueryable();

            var id = "f1a05cca-b479-4f72-bbda-96b8979f4afe";

            var expectedResult = "Bob";

            _mockEmployeeRepository.Setup(x => x
            .GetFirstOrDefaultAsync(
                x => x.Id == id,
                null,
                false).Result)
                .Returns(
                (Expression<Func<Employee, bool>> filter,
                Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => employees.FirstOrDefault(filter));

            //Act
            var result = employeeController.GetEmployeeByIdAsync(id).Result;

            //Assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<EmployeeViewModel>(viewResult.Value);

            Assert.Equal(expectedResult, model.FirstName);
        }

        [Fact]
        public void GetEmployeeById_ReturnsNotFound()
        {
            //Arrange
            var employeeController = new EmployeesController(
                _employeeService, 
                _mapper, 
                _employeeValidator);

            var id = "IDoNotExist";

            //Act
            var result = employeeController.GetEmployeeByIdAsync(id).Result;

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CanInsertEmployeeAsync()
        {
            //Arrange
            var employeeController = new EmployeesController(
                _employeeService,
                _mapper,
                _employeeValidator);
                
            var employeeVM = new EmployeeViewModel
            {
                FirstName = "Added",
                LastName = "User",
                Address = "9 Grayhawk Alley",
                Email = "tisacq0@unesco.org"
            };

            _mockEmployeeRepository.Setup(x => x.InsertAsync(It.IsAny<Employee>()));

            //Act
            var result = employeeController.InsertEmployeeAsync(employeeVM).Result;

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void InsertEmployeeAsync_ShouldReturnValidationError()
        {
            //Arrange
            var employeeController = new EmployeesController(
                _employeeService,
                _mapper,
                _employeeValidator);

            var employeeVM = new EmployeeViewModel
            {
                FirstName = "Added",
                LastName = "User",
                Address = string.Empty,
                Email = "tisacq0@unesco.org"
            };

            _mockEmployeeRepository.Setup(x => x.InsertAsync(It.IsAny<Employee>()));

            //Act
            var result = employeeController.InsertEmployeeAsync(employeeVM).Result;

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CanUpdateEmployee()
        { 
            //Arrange
            var employeeController = new EmployeesController(
                _employeeService,
                _mapper,
                _employeeValidator);

            var employeeVM = new EmployeeViewModel
            {
                FirstName = "Added",
                LastName = "User",
                Address = "9 Grayhawk Alley",
                Email = "tisacq0@unesco.org"
            };

            var id = "f1a05cca-b479-4f72-bbda-96b8979f4123";

            var employees = EmployeeFakeData
                .GetEmployeeFakeData()
                .AsQueryable();

            _mockEmployeeRepository.Setup(x => x.Update(It.IsAny<Employee>()));

            _mockEmployeeRepository.Setup(x => x
            .IsAny(x => x.Id == id))
                .Returns((Expression<Func<Employee, bool>> filter) => !employees.Any(filter));

            //Act
            var result = employeeController.UpdateEmployee(id, employeeVM);

            //Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void UpdateEmployee_ReturnsBadRequest_DueToValidationError()
        { 
            //Arrange
            var employeeController = new EmployeesController(
                _employeeService,
                _mapper,
                _employeeValidator);

            var employeeVM = new EmployeeViewModel
            {
                FirstName = "Added",
                LastName = "User",
                Address = string.Empty,
                Email = "tisacq0@unesco.org"
            };

            var id = "f1a05cca-b479-4f72-bbda-96b8979f4123";

            _mockEmployeeRepository.Setup(x => x.Update(It.IsAny<Employee>()));

            //Act
            var result = employeeController.UpdateEmployee(id, employeeVM);

            //Assert
            var badRequest = result as BadRequestObjectResult;

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(badRequest.Value.GetType() == typeof(List<ValidationFailure>));
        }

        [Fact]
        public void CanDeleteEmployee()
        {
            //Arrange
            var employeeController = new EmployeesController(
                _employeeService,
                _mapper,
                _employeeValidator);

            var id = "f1a05cca-b479-4f72-bbda-96b8979f4afe";

            _mockEmployeeRepository.Setup(x => x
            .GetFirstOrDefaultAsync(
                x => x.Id == id,
                null,
                false).Result)
                .Returns(new Employee() { Id = id });

            _mockEmployeeRepository.Setup(x => x.Delete(It.IsAny<Employee>()));

            //Act
            var result = employeeController.DeleteEmployeeAsync(id).Result;

            //Assert
            Assert.IsType<OkObjectResult>(result);

        }

        [Fact]
        public void DeleteEmployee_WhenEmployeeDoesNotExist()
        {
            //Arrange
            var employeeController = new EmployeesController(
                _employeeService,
                _mapper,
                _employeeValidator);

            var id = "IDoNotExist";

            //Act
            var result = employeeController.DeleteEmployeeAsync(id).Result;

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CanDeleteRange()
        {
            //Arrange
            var employeeController = new EmployeesController(
                _employeeService,
                _mapper,
                _employeeValidator);

            var idArr = new List<string>()
            {
                "f1a05cca-b479-4f72-bbda-96b8979f4afe",
                "6fca381a-40d0-4bf9-a076-706e1a995662",
                "804bbbca-3ffc-4d28-9b71-0d7788ddf681"
            };

            var employees = EmployeeFakeData
                .GetEmployeeFakeData()
                .AsQueryable();

            _mockEmployeeRepository
                .Setup(x => x
                .GetAsync(
                    It.IsAny<Expression<Func<Employee, bool>>>(),
                    null,
                    null,
                    false).Result)
                .Returns(
                (Expression<Func<Employee, bool>> filter,
                Func<IQueryable<Employee>, IOrderedQueryable<Employee>> orderBy,
                Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => employees.Where(filter).ToList());

            _mockEmployeeRepository.Setup(x => x.DeleteRange(It.IsAny<IEnumerable<Employee>>()));

            //Act
            var result = employeeController.DeleteEmployeesAsync(idArr).Result;

            //Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void DeleteRange_WhenSomeEmployeeWasNotFound()
        {
            //Arrange
            var employeeController = new EmployeesController(
                _employeeService,
                _mapper,
                _employeeValidator);

            var idArr = new List<string>()
           {
                "f1a05cca-b479-4f72-bbda-96b8979f4afe",
                "IDoNotExist",
                "804bbbca-3ffc-4d28-9b71-0d7788ddf681"
           };

            var employees = EmployeeFakeData
                .GetEmployeeFakeData()
                .AsQueryable();

            _mockEmployeeRepository
                .Setup(x => x
                .GetAsync(
                    It.IsAny<Expression<Func<Employee, bool>>>(),
                    null,
                    null,
                    false).Result)
                .Returns(
                (Expression<Func<Employee, bool>> filter,
                Func<IQueryable<Employee>, IOrderedQueryable<Employee>> orderBy,
                Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => employees.Where(filter).ToList());

            _mockEmployeeRepository.Setup(x => x.DeleteRange(It.IsAny<IEnumerable<Employee>>()));

            //Act
            var result = employeeController.DeleteEmployeesAsync(idArr).Result;

            //Assert
            var badRequest = result as BadRequestObjectResult;

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"{SomeEntitiesInCollectionNotFound} {nameof(Employee)}s to delete", badRequest.Value);
        }
    }
}
