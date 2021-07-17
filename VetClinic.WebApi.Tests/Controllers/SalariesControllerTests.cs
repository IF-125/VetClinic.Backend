using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
using VetClinic.Core.Interfaces.Services;
using VetClinic.WebApi.Controllers;
using VetClinic.WebApi.Mappers;
using VetClinic.WebApi.Validators.EntityValidators;
using VetClinic.WebApi.ViewModels;
using Xunit;

namespace VetClinic.WebApi.Tests.Controllers
{
    public class SalariesControllerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ISalaryRepository> _mockSalaryRepository;
        private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
        private readonly ISalaryService _salaryService;
        private readonly SalaryValidator _salaryValidator;

        public SalariesControllerTests()
        {
            _mockSalaryRepository = new Mock<ISalaryRepository>();
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            _salaryService = new SalaryService(_mockSalaryRepository.Object, _mockEmployeeRepository.Object);
            _salaryValidator = new SalaryValidator();

            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new SalaryMapperProfile());
                });

                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }

        [Fact]
        public void CanGetSalariesOfEmployee()
        {
            //Arrange
            var salariesController = new SalariesController(
                _salaryService,
                _mapper,
                _salaryValidator);

            var employeePostionId = 2;

            var salaries = SalaryFakeData
                .GetFakeSalaryData()
                .AsQueryable();

            _mockSalaryRepository.Setup(x => x.GetAsync(
                x => x.EmployeePositionId == employeePostionId,
                null,
                null,
                false).Result)
                .Returns((Expression<Func<Salary, bool>> filter,
                Func<IQueryable<Salary>, IOrderedQueryable<Salary>> orderBy,
                Func<IQueryable<Salary>, IIncludableQueryable<Salary, object>> include,
                bool asNoTracking) => salaries.Where(filter).ToList());

            var expectedResult = 3;

            //Act
            var result = salariesController.GetSalariesOfEmployee(employeePostionId).Result;

            //Assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<SalaryViewModel>>(viewResult.Value);
            Assert.Equal(expectedResult, model.Count());
        }

        [Fact]
        public void GetSalariesOfEmployee_WhenEmployeeHasNone()
        {
            //Arrange
            var salariesController = new SalariesController(
                _salaryService,
                _mapper,
                _salaryValidator);

            var employeePostionId = -1;

            var salaries = SalaryFakeData
                .GetFakeSalaryData()
                .AsQueryable();

            _mockSalaryRepository.Setup(x => x
            .GetAsync(
                x => x.EmployeePositionId == employeePostionId,
                null,
                null,
                false).Result)
                .Returns(
                (Expression<Func<Salary, bool>> filter,
                Func<IQueryable<Salary>, IOrderedQueryable<Salary>> orderBy,
                Func<IQueryable<Salary>, IIncludableQueryable<Salary, object>> include,
                bool asNoTracking) => salaries.Where(filter).ToList());

            //Act
            var result = salariesController.GetSalariesOfEmployee(employeePostionId).Result;

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CanGetSalaryById()
        {
            //Arrange
            var salariesController = new SalariesController(
                _salaryService,
                _mapper,
                _salaryValidator);

            var id = 1;

            var salaries = SalaryFakeData
                .GetFakeSalaryData()
                .AsQueryable();

            _mockSalaryRepository.Setup(x => x
            .GetFirstOrDefaultAsync(
                x => x.Id == id,
                null,
                false).Result)
                .Returns(
                (Expression<Func<Salary, bool>> filter,
                Func<IQueryable<Salary>, IIncludableQueryable<Salary, object>> include,
                bool asNoTracking) => salaries.FirstOrDefault(filter));

            var expectedResult = 1000;

            //Act
            var result = salariesController.GetSalary(id).Result;

            //Assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<SalaryViewModel>(viewResult.Value);
            Assert.Equal(expectedResult, model.Amount);
        }

        [Fact]
        public async Task GetSalaryById_ReturnsNotFound()
        {
            //Arrange
            var salariesController = new SalariesController(
                _salaryService,
                _mapper,
                _salaryValidator);

            var id = -4;

            var salaries = SalaryFakeData
                .GetFakeSalaryData()
                .AsQueryable();

            _mockSalaryRepository.Setup(x => x
            .GetFirstOrDefaultAsync(
                x => x.Id == id,
                null,
                false))
                .ReturnsAsync(
                (Expression<Func<Salary, bool>> filter,
                Func<IQueryable<Salary>, IIncludableQueryable<Salary, object>> include,
                bool asNoTracking) => salaries.FirstOrDefault(filter));

            //Act
            var result = await salariesController.GetSalary(id);

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CanAssignSalaryToEmployee()
        {
            //Arrange
            var salariesController = new SalariesController(
                _salaryService,
                _mapper,
                _salaryValidator);

            var employeeId = "f1a05cca-b479-4f72-bbda-96b8979f4afe";

            var salaryVM = new SalaryViewModel
            {
                Amount = 10000,
                Bonus = 10,
                Date = new System.DateTime(2021, 8, 6),
                EmployeePositionId = 2
            };

            var employees = EmployeeFakeData
                .GetEmployeeFakeData()
                .AsQueryable();

            _mockEmployeeRepository.Setup(x => x
            .GetFirstOrDefaultAsync(
                x => x.Id == employeeId,
                It.IsAny<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>(),
                false).Result)
                .Returns(
                (Expression<Func<Employee, bool>> filter,
                Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => employees.FirstOrDefault(filter));

            _mockSalaryRepository.Setup(x => x.InsertAsync(It.IsAny<Salary>())).Verifiable();

            _mockEmployeeRepository.Setup(x => x.Update(It.IsAny<Employee>())).Verifiable();

            //Act
            var result = salariesController.AssignSalaryToEmployee(employeeId, salaryVM).Result;

            //Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void AssignSalaryToEmployee_ShouldReturn_ValidationError()
        {
            //Arrange
            var salariesController = new SalariesController(
                _salaryService,
                _mapper,
                _salaryValidator);

            var employeeId = "f1a05cca-b479-4f72-bbda-96b8979f4afe";

            var salaryVM = new SalaryViewModel
            {
                Amount = -10,
                Bonus = 10,
                Date = new DateTime(2021, 8, 6),
                EmployeePositionId = 2
            };

            var employees = EmployeeFakeData
                .GetEmployeeFakeData()
                .AsQueryable();

            _mockEmployeeRepository.Setup(x => x
            .GetFirstOrDefaultAsync(
                x => x.Id == employeeId,
                It.IsAny<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>(),
                false).Result)
                .Returns(
                (Expression<Func<Employee, bool>> filter,
                Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => employees.FirstOrDefault(filter));

            _mockSalaryRepository.Setup(x => x.InsertAsync(It.IsAny<Salary>())).Verifiable();

            _mockEmployeeRepository.Setup(x => x.Update(It.IsAny<Employee>())).Verifiable();

            //Act
            var result = salariesController.AssignSalaryToEmployee(employeeId, salaryVM).Result;

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task AssignSalaryToEmployee_ShouldReturn_NotFound()
        {
            //Arrange
            var salariesController = new SalariesController(
                _salaryService,
                _mapper,
                _salaryValidator);

            var employeeId = "idonotexist";

            var salaryVM = new SalaryViewModel
            {
                Amount = 1000,
                Bonus = 10,
                Date = new DateTime(2021, 8, 6),
                EmployeePositionId = 2
            };

            var employees = EmployeeFakeData
                .GetEmployeeFakeData()
                .AsQueryable();

            _mockEmployeeRepository.Setup(x => x
            .GetFirstOrDefaultAsync(
                x => x.Id == employeeId,
                It.IsAny<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>(),
                false))
                .ReturnsAsync(
                (Expression<Func<Employee, bool>> filter,
                Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => employees.FirstOrDefault(filter));

            _mockSalaryRepository.Setup(x => x.InsertAsync(It.IsAny<Salary>())).Verifiable();

            _mockEmployeeRepository.Setup(x => x.Update(It.IsAny<Employee>())).Verifiable();

            //Act
            var result = await salariesController.AssignSalaryToEmployee(employeeId, salaryVM);

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CanUpdateSalary()
        {
            //Arrange
            var salariesController = new SalariesController(
                _salaryService,
                _mapper,
                _salaryValidator);

            var salaryVM = new SalaryViewModel
            {
                Id = 2,
                Amount = 1000,
                Bonus = 10,
                Date = new System.DateTime(2021, 8, 6),
                EmployeePositionId = 2
            };

            var id = 2;

            _mockSalaryRepository.Setup(x => x.Update(It.IsAny<Salary>()));

            //Act
            var result = salariesController.UpdateSalary(id, salaryVM);

            //Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void UpdateSalary_ReturnsBadRequest_DueToValidationError()
        {
            //Arrange
            var salariesController = new SalariesController(
                _salaryService,
                _mapper,
                _salaryValidator);

            var salaryVM = new SalaryViewModel
            {
                Id = 2,
                Amount = -1234,
                Bonus = 10,
                Date = new DateTime(2021, 8, 6),
                EmployeePositionId = 2
            };

            var id = 2;

            _mockSalaryRepository.Setup(x => x.Update(It.IsAny<Salary>()));

            //Act
            var result = salariesController.UpdateSalary(id, salaryVM);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void UpdateSalary_ReturnsBadRequest_DueToIdsMismatch()
        {
            //Arrange
            var salariesController = new SalariesController(
                _salaryService,
                _mapper,
                _salaryValidator);

            var salaryVM = new SalaryViewModel
            {
                Id = 2,
                Amount = 1234,
                Bonus = 10,
                Date = new DateTime(2021, 8, 6),
                EmployeePositionId = 2
            };

            var id = 12;

            _mockSalaryRepository.Setup(x => x.Update(It.IsAny<Salary>()));

            //Act
            var result = salariesController.UpdateSalary(id, salaryVM);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CanDeleteSalary()
        {
            //Arrange
            var salariesController = new SalariesController(
                _salaryService,
                _mapper,
                _salaryValidator);

            var id = 2;

            _mockSalaryRepository.Setup(x => x
            .GetFirstOrDefaultAsync(
                x => x.Id == id, 
                null,
                false).Result)
                .Returns(new Salary() { Id = id });

            _mockSalaryRepository.Setup(x => x.Delete(It.IsAny<Salary>()));

            //Act
            var result = salariesController.DeleteSalary(id).Result;

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void DeleteSalary_WhenSalaryDoesNotExist()
        {
            //Arrange
            var salariesController = new SalariesController(
                _salaryService,
                _mapper,
                _salaryValidator);
            
            var id = 400;

            //Act
            var result = salariesController.DeleteSalary(id).Result;

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CanDeleteRange()
        {
            //Arrange
            var salariesController = new SalariesController(
                _salaryService, 
                _mapper,
                _salaryValidator);

            var idArr = new List<int> { 1, 2, 3 };

            var salaries = SalaryFakeData
                .GetFakeSalaryData()
                .AsQueryable();

            _mockSalaryRepository.Setup(x => x
            .GetAsync(
                It.IsAny<Expression<Func<Salary, bool>>>(),
                null,
                null,
                false).Result)
                .Returns(
                (Expression<Func<Salary, bool>> filter,
                Func<IQueryable<Salary>, IOrderedQueryable<Salary>> orderBy,
                Func<IQueryable<Salary>, IIncludableQueryable<Salary, object>> include,
                bool asNoTracking) => salaries.Where(filter).ToList());

            _mockSalaryRepository.Setup(x => x.DeleteRange(It.IsAny<IEnumerable<Salary>>()));

            //Act
            var result = salariesController.DeleteListOfSalaries(idArr).Result;

            //Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void DeleteRange_WhenSomeSalaryWasNotFound()
        {
            //Arrange
            var salariesController = new SalariesController(
                _salaryService,
                _mapper,
                _salaryValidator);

            var idArr = new List<int> { 1, -2, 3 };

            var salaries = SalaryFakeData
                .GetFakeSalaryData()
                .AsQueryable();

            _mockSalaryRepository.Setup(x => x
            .GetAsync(
                It.IsAny<Expression<Func<Salary, bool>>>(),
                null,
                null,
                false).Result)
                .Returns(
                (Expression<Func<Salary, bool>> filter,
                Func<IQueryable<Salary>, IOrderedQueryable<Salary>> orderBy,
                Func<IQueryable<Salary>, IIncludableQueryable<Salary, object>> include,
                bool asNoTracking) => salaries.Where(filter).ToList());

            _mockSalaryRepository.Setup(x => x.DeleteRange(It.IsAny<IEnumerable<Salary>>()));

            //Act
            var result = salariesController.DeleteListOfSalaries(idArr).Result;

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
