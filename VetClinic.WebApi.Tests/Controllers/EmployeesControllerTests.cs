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

        public EmployeesControllerTests()
        {
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            _employeeService = new EmployeeService(_mockEmployeeRepository.Object);

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

        private List<Employee> GetTestEmployees()
        {
            return new List<Employee>
            {
                new Employee
                {
                    Id = "f1a05cca-b479-4f72-bbda-96b8979f4afe",
                    FirstName = "Bob",
                    LastName = "Roberts",
                    Address = "9 Grayhawk Alley",
                    Email = "tisacq0@unesco.org"
                },
                new Employee
                {
                    Id = "6fca381a-40d0-4bf9-a076-706e1a995662",
                    FirstName = "Roselia",
                    LastName = "Isacq",
                    Address = "3 Menomonie Point",
                    Email = "dbrandle2@arizona.edu"
                },
                new Employee
                {
                    Id = "804bbbca-3ffc-4d28-9b71-0d7788ddf681",
                    FirstName = "Bob",
                    LastName = "Roberts",
                    Address = "9 Grayhawk Alley",
                    Email = "tisacq0@unesco.org"
                },
                new Employee
                {
                    Id = "0e216f00-f03b-4655-9f06-f166828d35df",
                    FirstName = "Mill",
                    LastName = "Hixson",
                    Address = "9170 Arapahoe Junction",
                    Email = "mhixson3@tmall.com"
                },
                new Employee
                {
                    Id = "ada82298-b807-4267-a9a2-c3955e975294",
                    FirstName = "Bryna",
                    LastName = "McTrustey",
                    Address = "2 Roth Point",
                    Email = "bmctrustey4@weather.com"
                },
                new Employee
                {
                    Id = "70efa05d-a06d-45b2-b116-af1ccb602d2e",
                    FirstName = "Erek",
                    LastName = "Dosdale",
                    Address = "5357 Schmedeman Drive",
                    Email = "edosdalef@google.nl"
                },
                new Employee
                {
                    Id = "7a7dc85b-ee14-4643-9782-4e4e98855d41",
                    FirstName = "Philbert",
                    LastName = "Gauthorpp",
                    Address = "55 Cottonwood Circle",
                    Email = "rgladebeck7@myspace.com"
                },
                new Employee
                {
                    Id = "982ed974-1d5e-449d-a94c-09a68f01f7e4",
                    FirstName = "Noel",
                    LastName = "Pont",
                    Address = "9 Grayhawk Alley",
                    Email = "npontm@upenn.edu"
                },
                new Employee
                {
                    Id = "b8e7017a-bec3-4654-9c40-538758b55917",
                    FirstName = "Martin",
                    LastName = "Roberts",
                    Address = "9 Grayhawk Alley",
                    Email = "smereweathern@elpais.com"
                },
                new Employee
                {
                    Id = "b8e7017a-bec3-4654-9c40-538758b55917",
                    FirstName = "Derek",
                    LastName = "Hussy",
                    Address = "1 Katie Court",
                    Email = "khussyo@elegantthemes.com"
                }
            };
        }

        [Fact]
        public void CanGetAllEmployees()
        {
            var employeeController = new EmployeesController(_employeeService, _mapper);

            var employees = GetTestEmployees();

            _mockEmployeeRepository.Setup(x => x.GetAsync(null, null, null, true).Result).Returns(() => employees);

            var result = employeeController.GetAllEmployeesAsync().Result;

            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<EmployeeViewModel>>(viewResult.Value);
            Assert.Equal(GetTestEmployees().Count, model.Count());
        }

        [Fact]
        public void CanGetEmployeeById()
        {
            var employeeController = new EmployeesController(_employeeService, _mapper);

            var employees = GetTestEmployees().AsQueryable();

            var id = "f1a05cca-b479-4f72-bbda-96b8979f4afe";

            _mockEmployeeRepository.Setup(x => x.GetFirstOrDefaultAsync(x => x.Id == id, null, false).Result)
                .Returns((Expression<Func<Employee, bool>> filter,
                Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => employees.FirstOrDefault(filter));

            var result = employeeController.GetEmployeeByIdAsync(id).Result;

            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<EmployeeViewModel>(viewResult.Value);

            Assert.Equal(id, model.Id);
            Assert.Equal("Bob", model.FirstName);
        }

        [Fact]
        public void GetEmployeeById_ReturnsNotFound()
        {
            var employeeController = new EmployeesController(_employeeService, _mapper);

            var employees = GetTestEmployees().AsQueryable();

            var id = "IDoNotExist";

            var result = employeeController.GetEmployeeByIdAsync(id).Result;

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CanInsertEmployeeAsync()
        {
            var employeeController = new EmployeesController(_employeeService, _mapper);
                
            var employeeVM = new EmployeeViewModel
            {
                Id = "f1a05cca-b479-4f72-bbda-96b8979f4123",
                FirstName = "Added",
                LastName = "User",
                Address = "9 Grayhawk Alley",
                Email = "tisacq0@unesco.org"
            };

            _mockEmployeeRepository.Setup(x => x.InsertAsync(It.IsAny<Employee>()));

            var result = employeeController.InsertEmployeeAsync(employeeVM).Result;

            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public void InsertEmployeeAsync_ShouldReturnValidationError()
        {
            var employeeController = new EmployeesController(_employeeService, _mapper);

            var employeeVM = new EmployeeViewModel
            {
                Id = "f1a05cca-b479-4f72-bbda-96b8979f4123",
                FirstName = "Added",
                LastName = "User",
                Address = string.Empty,
                Email = "tisacq0@unesco.org"
            };

            _mockEmployeeRepository.Setup(x => x.InsertAsync(It.IsAny<Employee>()));

            var result = employeeController.InsertEmployeeAsync(employeeVM).Result;

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CanUpdateEmployee()
        { 
            var employeeController = new EmployeesController(_employeeService, _mapper);

            var employeeVM = new EmployeeViewModel
            {
                Id = "f1a05cca-b479-4f72-bbda-96b8979f4123",
                FirstName = "Added",
                LastName = "User",
                Address = "9 Grayhawk Alley",
                Email = "tisacq0@unesco.org"
            };

            var id = "f1a05cca-b479-4f72-bbda-96b8979f4123";

            _mockEmployeeRepository.Setup(x => x.Update(It.IsAny<Employee>()));

            var result = employeeController.Update(id, employeeVM);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void UpdateEmployee_ReturnsBadRequest_DueToValidationError()
        { 
            var employeeController = new EmployeesController(_employeeService, _mapper);

            var employeeVM = new EmployeeViewModel
            {
                Id = "f1a05cca-b479-4f72-bbda-96b8979f4123",
                FirstName = "Added",
                LastName = "User",
                Address = string.Empty,
                Email = "tisacq0@unesco.org"
            };

            var id = "f1a05cca-b479-4f72-bbda-96b8979f4123";

            _mockEmployeeRepository.Setup(x => x.Update(It.IsAny<Employee>()));

            var result = employeeController.Update(id, employeeVM);

            var badRequest = result as BadRequestObjectResult;

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(badRequest.Value.GetType() == typeof(List<ValidationFailure>));
        }

        [Fact]
        public void UpdateEmployee_ReturnsBadRequest_DueToIdsMismatch()
        {
            var employeeController = new EmployeesController(_employeeService, _mapper);

            var employeeVM = new EmployeeViewModel
            {
                Id = "f1a05cca-b479-4f72-bbda-96b8979f4123",
                FirstName = "Added",
                LastName = "User",
                Address = "Good in here",
                Email = "tisacq0@unesco.org"
            };

            var id = "DummyId";

            _mockEmployeeRepository.Setup(x => x.Update(It.IsAny<Employee>()));

            var result = employeeController.Update(id, employeeVM);

            var badRequest = result as BadRequestObjectResult;

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Employee id and passed id did not match", badRequest.Value);
        }

        [Fact]
        public void CanDeleteEmployee()
        {
            var id = "f1a05cca-b479-4f72-bbda-96b8979f4afe";

            _mockEmployeeRepository.Setup(x => x.GetFirstOrDefaultAsync(
                x => x.Id == id, null, false).Result)
                .Returns(new Employee() { Id = id });

            _mockEmployeeRepository.Setup(x => x.Delete(It.IsAny<Employee>()));

            var employeeController = new EmployeesController(_employeeService, _mapper);

            var result = employeeController.DeleteEmployeeAsync(id).Result;

            Assert.IsType<OkObjectResult>(result);

        }

        [Fact]
        public void DeleteEmployee_WhenEmployeeDoesNotExist()
        {
            var id = "IDoNotExist";

            var employeeController = new EmployeesController(_employeeService, _mapper);

            var result = employeeController.DeleteEmployeeAsync(id).Result;

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CanDeleteRange()
        {
            var idArr = new string[]
            {
                "f1a05cca-b479-4f72-bbda-96b8979f4afe",
                "6fca381a-40d0-4bf9-a076-706e1a995662",
                "804bbbca-3ffc-4d28-9b71-0d7788ddf681"
            };

            var employees = GetTestEmployees().AsQueryable();

            _mockEmployeeRepository
                .Setup(x => x.GetAsync(It.IsAny<Expression<Func<Employee, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<Employee, bool>> filter,
                Func<IQueryable<Employee>, IOrderedQueryable<Employee>> orderBy,
                Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => employees.Where(filter).ToList());

            _mockEmployeeRepository.Setup(x => x.DeleteRange(It.IsAny<IEnumerable<Employee>>()));

            var employeeController = new EmployeesController(_employeeService, _mapper);

            var result = employeeController.DeleteEmployeesAsync(idArr).Result;

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void DeleteRange_WhenSomeEmployeeWasNotFound()
        {
            var idArr = new string[]
           {
                "f1a05cca-b479-4f72-bbda-96b8979f4afe",
                "IDoNotExist",
                "804bbbca-3ffc-4d28-9b71-0d7788ddf681"
           };

            var employees = GetTestEmployees().AsQueryable();

            _mockEmployeeRepository
                .Setup(x => x.GetAsync(It.IsAny<Expression<Func<Employee, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<Employee, bool>> filter,
                Func<IQueryable<Employee>, IOrderedQueryable<Employee>> orderBy,
                Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => employees.Where(filter).ToList());

            _mockEmployeeRepository.Setup(x => x.DeleteRange(It.IsAny<IEnumerable<Employee>>()));

            var employeeController = new EmployeesController(_employeeService, _mapper);

            var result = employeeController.DeleteEmployeesAsync(idArr).Result;

            var badRequest = result as BadRequestObjectResult;

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"{SomeEntitiesInCollectionNotFound} {nameof(Employee)}s to delete", badRequest.Value);
        }
    }
}
