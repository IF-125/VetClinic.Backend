using AutoMapper;
using FluentValidation.Results;
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
using static VetClinic.Core.Resources.TextMessages;

namespace VetClinic.WebApi.Tests.Controllers
{
    public class EmployeePositionsControllerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IEmployeePositionRepository> _mockEmployeePositionRepository;
        private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
        private readonly Mock<IPositionRepository> _mockPositionRepository;
        private readonly IEmployeePositionService _employeePositionService;
        private readonly EmployeePositionValidator _employeePositionValidator;

        public EmployeePositionsControllerTests()
        {
            _mockEmployeePositionRepository = new Mock<IEmployeePositionRepository>();
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            _mockPositionRepository = new Mock<IPositionRepository>();

            _employeePositionService = new EmployeePositionService(
                _mockEmployeePositionRepository.Object,
                _mockEmployeeRepository.Object,
                _mockPositionRepository.Object);

            _employeePositionValidator = new EmployeePositionValidator();

            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new EmployeePositionMapperProfile());
                });

                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }

        [Fact]
        public void CanGetAllEmployeePositions()
        {
            //Arrange
            var employeePositionsController = new EmployeePositionsController(
                _employeePositionService,
                _mapper,
                _employeePositionValidator);

            var employeePositions = EmployeePositionFakeData.GetEmployeePositionFakeData();

            _mockEmployeePositionRepository.Setup(x => x
            .GetAsync(
                null,
                null,
                It.IsAny<Func<IQueryable<EmployeePosition>, IIncludableQueryable<EmployeePosition, object>>>(),
                true).Result)
                .Returns(() => employeePositions);

            var expectedResult = 5;

            //Act
            var result = employeePositionsController.GetAllEmployeePositions().Result;

            //Assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<EmployeePositionViewModel>>(viewResult.Value);
            Assert.Equal(expectedResult, model.Count());
        }

        [Fact]
        public void CanGetEmployeePositionById()
        {
            //Arrange
            var employeePositionsController = new EmployeePositionsController(
                _employeePositionService,
                _mapper, 
                _employeePositionValidator);

            var employeePositions = EmployeePositionFakeData
                .GetEmployeePositionFakeData()
                .AsQueryable();

            var id = 2;

            var expectedResult = 10002;

            _mockEmployeePositionRepository.Setup(x => x
            .GetFirstOrDefaultAsync(
                x => x.Id == id,
                It.IsAny<Func<IQueryable<EmployeePosition>, IIncludableQueryable<EmployeePosition, object>>>(),
                false).Result)
                .Returns((
                    Expression<Func<EmployeePosition, bool>> filter,
                    Func<IQueryable<EmployeePosition>, IIncludableQueryable<EmployeePosition, object>> include,
                    bool asNoTracking) => employeePositions.FirstOrDefault(filter));

            //Act
            var result = employeePositionsController.GetEmployeePosition(id).Result;

            //Assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<EmployeePositionViewModel>(viewResult.Value);

            Assert.Equal(expectedResult, model.CurrentBaseSalary);
        }

        [Fact]
        public async Task GetEmployeePositionById_ReturnsNotFound()
        {
            //Arrange
            var employeePositionsController = new EmployeePositionsController(
                _employeePositionService,
                _mapper,
                _employeePositionValidator);

            var employeePositions = EmployeePositionFakeData
                .GetEmployeePositionFakeData()
                .AsQueryable();

            var id = -4;

            _mockEmployeePositionRepository.Setup(x => x
            .GetFirstOrDefaultAsync(
                x => x.Id == id,
                It.IsAny<Func<IQueryable<EmployeePosition>, IIncludableQueryable<EmployeePosition, object>>>(),
                false))
                .ReturnsAsync((
                    Expression<Func<EmployeePosition, bool>> filter,
                    Func<IQueryable<EmployeePosition>, IIncludableQueryable<EmployeePosition, object>> include,
                    bool asNoTracking) => employeePositions.FirstOrDefault(filter));

            //Act
            var result = await employeePositionsController.GetEmployeePosition(id);

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task CanAssignPositionToEmployee()
        {
            //Arrange
            var employeePositionsController = new EmployeePositionsController(
                _employeePositionService,
                _mapper,
                _employeePositionValidator);

            var employees = EmployeeFakeData.GetEmployeeFakeData().AsQueryable();
            var positions = PositionFakeData.GetPositionFakeData().AsQueryable(); 

            var employeePositionVM = new EmployeePositionViewModel
            {
                CurrentBaseSalary = 124,
                Rate = 12,
                HierdDate = DateTime.Now,
                DismissedDate = null,
                EmployeeId = "f1a05cca-b479-4f72-bbda-96b8979f4afe",
                PositionId = 1,
            };

            _mockEmployeeRepository.Setup(x => x
            .GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Employee, bool>>>(),
                null,
                false))
                .ReturnsAsync(
                (Expression<Func<Employee, bool>> filter,
                Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => employees.FirstOrDefault(filter));

            _mockPositionRepository.Setup(x => x
            .GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Position, bool>>>(),
                null,
                false))
                .ReturnsAsync(
                (Expression<Func<Position, bool>> filter,
                Func<IQueryable<Position>, IIncludableQueryable<Position, object>> include,
                bool asNoTracking) => positions.FirstOrDefault(filter));

            _mockEmployeePositionRepository.Setup(x => x.InsertAsync(It.IsAny<EmployeePosition>()));

            //Act
            var result = await employeePositionsController.AssignPositionToEmployee(employeePositionVM);

            //Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task AssignPositionToEmployee_ShouldReturnValidationErrors()
        {
            //Arrange
            var employeePositionsController = new EmployeePositionsController(
                _employeePositionService,
                _mapper,
                _employeePositionValidator);

            var employees = EmployeeFakeData.GetEmployeeFakeData().AsQueryable();
            var positions = PositionFakeData.GetPositionFakeData().AsQueryable();

            var employeePositionVM = new EmployeePositionViewModel
            {
                CurrentBaseSalary = 124,
                Rate = 12,
                HierdDate = DateTime.Now,
                DismissedDate = null,
                EmployeeId = null,
                PositionId = default,
            };

            _mockEmployeeRepository.Setup(x => x
            .GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Employee, bool>>>(),
                null,
                false))
                .ReturnsAsync(
                (Expression<Func<Employee, bool>> filter,
                Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => employees.FirstOrDefault(filter));

            _mockPositionRepository.Setup(x => x
            .GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Position, bool>>>(),
                null,
                false))
                .ReturnsAsync(
                (Expression<Func<Position, bool>> filter,
                Func<IQueryable<Position>, IIncludableQueryable<Position, object>> include,
                bool asNoTracking) => positions.FirstOrDefault(filter));

            _mockEmployeePositionRepository.Setup(x => x.InsertAsync(It.IsAny<EmployeePosition>()));

            //Act
            var result = await employeePositionsController
                .AssignPositionToEmployee(employeePositionVM);

            //Assert
            var badRequest = result as BadRequestObjectResult;

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(badRequest.Value.GetType() == typeof(List<ValidationFailure>));
        }

        [Fact]
        public async Task AssignPositionToEmployee_ShouldReturn_NotFound()
        {
            //Arrange
            var employeePositionsController = new EmployeePositionsController(
                _employeePositionService, _mapper, _employeePositionValidator);

            var employees = EmployeeFakeData.GetEmployeeFakeData().AsQueryable();
            var positions = PositionFakeData.GetPositionFakeData().AsQueryable();

            var employeePositionVM = new EmployeePositionViewModel
            {
                CurrentBaseSalary = 124,
                Rate = 12,
                HierdDate = DateTime.Now,
                DismissedDate = null,
                EmployeeId = "iDoNotExist",
                PositionId = 1,
            };

            _mockEmployeeRepository.Setup(x => x
            .GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Employee, bool>>>(),
                null,
                false))
                .ReturnsAsync(
                (Expression<Func<Employee, bool>> filter,
                Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => employees.FirstOrDefault(filter));

            _mockPositionRepository.Setup(x => x
            .GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Position, bool>>>(),
                null,
                false))
                .ReturnsAsync(
                (Expression<Func<Position, bool>> filter,
                Func<IQueryable<Position>, IIncludableQueryable<Position, object>> include,
                bool asNoTracking) => positions.FirstOrDefault(filter));

            _mockEmployeePositionRepository.Setup(x => x.InsertAsync(It.IsAny<EmployeePosition>()));

            //Act
            var result = await employeePositionsController
                .AssignPositionToEmployee(employeePositionVM);

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CanUpdateEmployeePosition()
        {
            //Arrange
            var employeePositionsController = new EmployeePositionsController(
                _employeePositionService,
                _mapper,
                _employeePositionValidator);

            var employeePositionVM = new EmployeePositionViewModel
            {
                Id = 1,
                CurrentBaseSalary = 1241,
                Rate = 12,
                HierdDate = DateTime.Now,
                DismissedDate = null,
                EmployeeId = "f1a05cca-b479-4f72-bbda-96b8979f4afe",
                PositionId = 2,
            };

            var id = 1;

            _mockEmployeePositionRepository.Setup(x => x.Update(It.IsAny<EmployeePosition>()));

            //Act
            var result = employeePositionsController
                .UpdateEmployeePosition(id, employeePositionVM);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void UpdateEmployeePosition_ReturnsBadRequest_DueToValidationError()
        {
            //Arrange
            var employeePositionsController = new EmployeePositionsController(
                _employeePositionService,
                _mapper,
                _employeePositionValidator);

            var employeePositionVM = new EmployeePositionViewModel
            {
                CurrentBaseSalary = -124,
                Rate = -12,
                HierdDate = DateTime.Now,
                DismissedDate = null,
                EmployeeId = "f1a05cca-b479-4f72-bbda-96b8979f4afe",
                PositionId = 2,
            };

            var id = 2;

            _mockEmployeePositionRepository.Setup(x => x.Update(It.IsAny<EmployeePosition>()));

            //Act
            var result = employeePositionsController
                .UpdateEmployeePosition(id, employeePositionVM);

            //Assert
            var badRequest = result as BadRequestObjectResult;

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(badRequest.Value.GetType() == typeof(List<ValidationFailure>));
        }

        [Fact]
        public void UpdateEmployeePosition_ReturnsBadRequest_DueToIdsMismatch()
        {
            //Arrange
            var employeePositionsController = new EmployeePositionsController(
                _employeePositionService,
                _mapper,
                _employeePositionValidator);

            var employeePositionVM = new EmployeePositionViewModel
            {
                CurrentBaseSalary = 124,
                Rate = 12,
                HierdDate = DateTime.Now,
                DismissedDate = null,
                EmployeeId = "f1a05cca-b479-4f72-bbda-96b8979f4afe",
                PositionId = 2,
            };

            var id = 2;

            _mockEmployeePositionRepository.Setup(x => x.Update(It.IsAny<EmployeePosition>()));

            //Act
            var result = employeePositionsController
                .UpdateEmployeePosition(id, employeePositionVM);

            //Assert
            var badRequest = result as BadRequestObjectResult;

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("EmployeePosition id and passed id did not match", badRequest.Value);
        }

        [Fact]
        public void CanDeleteEmployeePosition()
        {
            //Arrange
            var employeePositionController = new EmployeePositionsController(
                _employeePositionService,
                _mapper,
                _employeePositionValidator);

            var id = 2;

            _mockEmployeePositionRepository.Setup(x => x
            .GetFirstOrDefaultAsync(
                x => x.Id == id,
                null,
                false).Result)
                .Returns(new EmployeePosition() { Id = id });

            _mockEmployeePositionRepository.Setup(x => x.Delete(It.IsAny<EmployeePosition>()));

            //Act
            var result = employeePositionController
                .DeleteEmployeePosition(id).Result;

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void DeleteEmployeePosition_WhenPositionDoesNotExist()
        {
            //Arrange
            var employeePositionController = new EmployeePositionsController(
                _employeePositionService,
                _mapper,
                _employeePositionValidator);

            var id = 400;

            //Act
            var result = employeePositionController.DeleteEmployeePosition(id).Result;

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CanDeleteRange()
        {
            //Arrange
            var employeePositionController = new EmployeePositionsController(
                _employeePositionService,
                _mapper,
                _employeePositionValidator);

            var idArr = new List<int> { 1, 2, 3 };

            var employeePositions = EmployeePositionFakeData
                .GetEmployeePositionFakeData()
                .AsQueryable();

            _mockEmployeePositionRepository
                .Setup(x => x
                .GetAsync(
                    It.IsAny<Expression<Func<EmployeePosition, bool>>>(),
                    null,
                    null,
                    false).Result)
                .Returns(
                (Expression<Func<EmployeePosition, bool>> filter,
                Func<IQueryable<EmployeePosition>, IOrderedQueryable<EmployeePosition>> orderBy,
                Func<IQueryable<EmployeePosition>, IIncludableQueryable<EmployeePosition, object>> include,
                bool asNoTracking) => employeePositions.Where(filter).ToList());

            _mockEmployeePositionRepository.Setup(x => x
            .DeleteRange(It.IsAny<IEnumerable<EmployeePosition>>()));

            //Act
            var result = employeePositionController.
                DeleteEmployeePositions(idArr).Result;

            //Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void DeleteRange_WhenSomePositionWasNotFound()
        {
            //Arrange
            var employeePositionController = new EmployeePositionsController(
                _employeePositionService,
                _mapper,
                _employeePositionValidator);

            var idArr = new List<int> { 1, -10000, 3 };

            var employeePositions = EmployeePositionFakeData
                .GetEmployeePositionFakeData()
                .AsQueryable();

            _mockEmployeePositionRepository
                .Setup(x => x
                .GetAsync(
                    It.IsAny<Expression<Func<EmployeePosition, bool>>>(),
                    null,
                    null,
                    false).Result)
                .Returns(
                (Expression<Func<EmployeePosition, bool>> filter,
                Func<IQueryable<EmployeePosition>, IOrderedQueryable<EmployeePosition>> orderBy,
                Func<IQueryable<EmployeePosition>, IIncludableQueryable<EmployeePosition, object>> include,
                bool asNoTracking) => employeePositions.Where(filter).ToList());

            _mockEmployeePositionRepository.Setup(x => x
            .DeleteRange(It.IsAny<IEnumerable<EmployeePosition>>()));

            //Act
            var result = employeePositionController.
                DeleteEmployeePositions(idArr).Result;

            //Assert
            var badRequest = result as BadRequestObjectResult;

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"{SomeEntitiesInCollectionNotFound} {nameof(EmployeePosition)}s to delete",
                badRequest.Value);
        }
    }
}
