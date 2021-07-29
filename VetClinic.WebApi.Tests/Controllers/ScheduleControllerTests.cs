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

namespace VetClinic.WebApi.Tests.Controllers
{
    public class ScheduleControllerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IScheduleRepository> _mockScheduleRepository;
        private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
        private readonly IScheduleService _scheduleService;
        private readonly ScheduleValidator _scheduleValidator;
        private readonly ScheduleCollectionValidator _scheduleCollectionValidator;

        public ScheduleControllerTests()
        {
            _mockScheduleRepository = new Mock<IScheduleRepository>();
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            _scheduleService = new ScheduleService(
                _mockScheduleRepository.Object,
                _mockEmployeeRepository.Object);
            _scheduleValidator = new ScheduleValidator();
            _scheduleCollectionValidator = new ScheduleCollectionValidator();

            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new ScheduleMapperProfile());
                });

                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }

        [Fact]
        public void CanGetScheduleOfEmployee()
        {
            //Arrange
            var scheduleController = new ScheduleController(
                _scheduleService,
                _mapper,
                _scheduleValidator,
                _scheduleCollectionValidator);

            var employeeId = "f1a05cca-b479-4f72-bbda-96b8979f4afe";

            var schedules = ScheduleFakeData.GetScheduleFakeData().AsQueryable();

            _mockScheduleRepository.Setup(x => x.GetAsync(
                x => x.EmployeeId == employeeId,
                null,
                null,
                false).Result)
                .Returns((Expression<Func<Schedule, bool>> filter,
                Func<IQueryable<Schedule>, IOrderedQueryable<Schedule>> orderBy,
                Func<IQueryable<Schedule>, IIncludableQueryable<Schedule, object>> include,
                bool asNoTracking) => schedules.Where(filter).ToList());

            var expectedResult = 5;

            //Act
            var result = scheduleController.GetScheduleOfEmployee(employeeId).Result;

            //Assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ScheduleViewModel>>(viewResult.Value);
            Assert.Equal(expectedResult, model.Count());
        }

        [Fact]
        public void GetScheduleOfEmployee_WhenEmployeeHasNone()
        {
            //Arrange
            var scheduleController = new ScheduleController(
                _scheduleService,
                _mapper,
                _scheduleValidator,
                _scheduleCollectionValidator);

            //this employee has no schedule
            var employeeId = "6fca381a-40d0-4bf9-a076-706e1a995662";

            var schedules = ScheduleFakeData.GetScheduleFakeData().AsQueryable();

            _mockScheduleRepository.Setup(x => x.GetAsync(
                x => x.EmployeeId == employeeId,
                null,
                null,
                false).Result)
                .Returns((Expression<Func<Schedule, bool>> filter,
                Func<IQueryable<Schedule>, IOrderedQueryable<Schedule>> orderBy,
                Func<IQueryable<Schedule>, IIncludableQueryable<Schedule, object>> include,
                bool asNoTracking) => schedules.Where(filter).ToList());

            var expectedErrorMessage = "No schedule was provided for this employee";

            //Act
            var result = scheduleController.GetScheduleOfEmployee(employeeId).Result;

            var notFoundResult = result as NotFoundObjectResult;

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(expectedErrorMessage, notFoundResult.Value);
        }

        [Fact]
        public void CanGetScheduleById()
        {
            //Arrange
            var scheduleController = new ScheduleController(
                _scheduleService,
                _mapper,
                _scheduleValidator,
                _scheduleCollectionValidator);

            var schedules = ScheduleFakeData.GetScheduleFakeData().AsQueryable();

            var id = 2;

            _mockScheduleRepository.Setup(x => x.GetFirstOrDefaultAsync(x => x.Id == id, null, false).Result)
                .Returns((Expression<Func<Schedule, bool>> filter,
                Func<IQueryable<Schedule>, IIncludableQueryable<Schedule, object>> include,
                bool asNoTracking) => schedules.FirstOrDefault(filter));

            var expectedResult = Days.Tuesday;
            //Act
            var result = scheduleController.GetSchedule(id).Result;

            //Assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<ScheduleViewModel>(viewResult.Value);

            Assert.Equal(expectedResult.ToString(), model.Day);
        }

        [Fact]
        public async Task GetScheduleById_ReturnsNotFound()
        {
            //Arrange
            var scheduleController = new ScheduleController(
                _scheduleService,
                _mapper,
                _scheduleValidator,
                _scheduleCollectionValidator);

            var schedules = ScheduleFakeData.GetScheduleFakeData().AsQueryable();

            var id = -1;

            _mockScheduleRepository.Setup(x => x.GetFirstOrDefaultAsync(x => x.Id == id, null, false))
                .ReturnsAsync((Expression<Func<Schedule, bool>> filter,
                Func<IQueryable<Schedule>, IIncludableQueryable<Schedule, object>> include,
                bool asNoTracking) => schedules.FirstOrDefault(filter));

            //Act
            var result = await scheduleController.GetSchedule(id);

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CanAssignScheduleToEmployee()
        {
            //Arrange
            var scheduleController = new ScheduleController(
                _scheduleService,
                _mapper,
                _scheduleValidator,
                _scheduleCollectionValidator);

            var scheduleVM = new ScheduleViewModel
            {
                Day = Days.Tuesday.ToString(),
                From = "12:00",
                To = "15:00"
            };

            var employees = EmployeeFakeData
                .GetEmployeeFakeData()
                .AsQueryable();

            var id = "f1a05cca-b479-4f72-bbda-96b8979f4afe";

            _mockEmployeeRepository.Setup(x => x.GetFirstOrDefaultAsync(
                x => x.Id == id,
                It.IsAny<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>(),
                false))
                .ReturnsAsync((Expression<Func<Employee, bool>> filter,
                Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => employees.FirstOrDefault(filter));

            _mockEmployeeRepository.Setup(x => x.Update(It.IsAny<Employee>()));

            _mockScheduleRepository.Setup(x => x.InsertAsync(It.IsAny<Schedule>()));

            //Act
            var result = scheduleController.AssignScheduleToEmployee(scheduleVM, id).Result;

            //Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task AssignScheduleToEmployee_ReturnsNotFound_WhenEmployeeWasNotFound()
        {
            //Arrange
            var scheduleController = new ScheduleController(
                _scheduleService,
                _mapper,
                _scheduleValidator,
                _scheduleCollectionValidator);

            var scheduleVM = new ScheduleViewModel
            {
                Day = Days.Tuesday.ToString(),
                From = "12:00",
                To = "15:00"
            };

            var employees = EmployeeFakeData
                .GetEmployeeFakeData()
                .AsQueryable();

            var id = "idonotexist";

            _mockEmployeeRepository.Setup(x => x.GetFirstOrDefaultAsync(
                x => x.Id == id,
                It.IsAny<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>(),
                false))
                .ReturnsAsync((Expression<Func<Employee, bool>> filter,
                Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => employees.FirstOrDefault(filter));

            _mockEmployeeRepository.Setup(x => x.Update(It.IsAny<Employee>()));

            _mockScheduleRepository.Setup(x => x.InsertAsync(It.IsAny<Schedule>()));

            //Act
            var result = await scheduleController.AssignScheduleToEmployee(scheduleVM, id);

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void AssignScheduleToEmployee_ReturnsBadRequest_DueToValidationErrors()
        {
            //Arrange
            var scheduleController = new ScheduleController(
                _scheduleService,
                _mapper,
                _scheduleValidator,
                _scheduleCollectionValidator);

            var scheduleVM = new ScheduleViewModel
            {
                Day = "bad input", //Wrong day
                From = "12:00",
                To = "15:00"
            };

            var employees = EmployeeFakeData
                .GetEmployeeFakeData()
                .AsQueryable();

            var id = "f1a05cca-b479-4f72-bbda-96b8979f4afe";

            _mockEmployeeRepository.Setup(x => x.GetFirstOrDefaultAsync(
                x => x.Id == id,
                It.IsAny<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>(),
                false))
                .ReturnsAsync((Expression<Func<Employee, bool>> filter,
                Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => employees.FirstOrDefault(filter));

            _mockEmployeeRepository.Setup(x => x.Update(It.IsAny<Employee>()));

            _mockScheduleRepository.Setup(x => x.InsertAsync(It.IsAny<Schedule>()));

            //Act
            var result = scheduleController.AssignScheduleToEmployee(scheduleVM, id).Result;

            //Assert
            var badRequest = result as BadRequestObjectResult;

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(badRequest.Value.GetType() == typeof(List<ValidationFailure>));
        }

        [Fact]
        public void CanAssignSchedulesToEmployee()
        {
            //Arrange
            var scheduleController = new ScheduleController(
                _scheduleService,
                _mapper,
                _scheduleValidator,
                _scheduleCollectionValidator);

            var schedulesVM = new List<ScheduleViewModel>
            {
                new ScheduleViewModel
                {
                    Day = Days.Tuesday.ToString(),
                    From = "12:00",
                    To = "15:00"
                },
                new ScheduleViewModel
                {
                    Day = Days.Tuesday.ToString(),
                    From = "12:00",
                    To = "15:00"
                },
                new ScheduleViewModel
                {
                    Day = Days.Tuesday.ToString(),
                    From = "12:00",
                    To = "15:00"
                },
                new ScheduleViewModel
                {
                    Day = Days.Tuesday.ToString(),
                    From = "12:00",
                    To = "15:00"
                },
            };

            var employees = EmployeeFakeData
                .GetEmployeeFakeData()
                .AsQueryable();

            var id = "f1a05cca-b479-4f72-bbda-96b8979f4afe";

            _mockEmployeeRepository.Setup(x => x.GetFirstOrDefaultAsync(
                x => x.Id == id,
                It.IsAny<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>(),
                false))
                .ReturnsAsync((Expression<Func<Employee, bool>> filter,
                Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => employees.FirstOrDefault(filter));

            _mockEmployeeRepository.Setup(x => x.Update(It.IsAny<Employee>()));

            _mockScheduleRepository.Setup(x => x.InsertRangeAsync(It.IsAny<IEnumerable<Schedule>>()));

            //Act
            var result = scheduleController.AssignSchedulesToEmployee(schedulesVM, id).Result;

            //Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task AssignSchedulesToEmployee_ReturnsNotFound_WhenEmployeeWasNotFound()
        {
            //Arrange
            var scheduleController = new ScheduleController(
                _scheduleService,
                _mapper,
                _scheduleValidator,
                _scheduleCollectionValidator);

            var schedulesVM = new List<ScheduleViewModel>
            {
                new ScheduleViewModel
                {
                    Day = Days.Tuesday.ToString(),
                    From = "12:00",
                    To = "15:00"
                },
                new ScheduleViewModel
                {
                    Day = Days.Tuesday.ToString(),
                    From = "12:00",
                    To = "15:00"
                },
                new ScheduleViewModel
                {
                    Day = Days.Tuesday.ToString(),
                    From = "12:00",
                    To = "15:00"
                },
                new ScheduleViewModel
                {
                    Day = Days.Tuesday.ToString(),
                    From = "12:00",
                    To = "15:00"
                },
            };

            var schedules = ScheduleFakeData
               .GetScheduleFakeData();

            var employees = EmployeeFakeData
                .GetEmployeeFakeData()
                .AsQueryable();

            var id = "idonotexist";

            _mockEmployeeRepository.Setup(x => x.GetFirstOrDefaultAsync(
                x => x.Id == id,
                It.IsAny<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>(),
                false))
                .ReturnsAsync((Expression<Func<Employee, bool>> filter,
                Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => employees.FirstOrDefault(filter));

            _mockScheduleRepository.Setup(x => x.InsertRangeAsync(schedules));

            //Act
            var result = await scheduleController.AssignSchedulesToEmployee(schedulesVM, id);

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task AssignSchedulesToEmployee_ReturnsBadRequest_DueToMapperErrors()
        {
            //Arrange
            var scheduleController = new ScheduleController(
                _scheduleService,
                _mapper,
                _scheduleValidator,
                _scheduleCollectionValidator);

            var schedulesVM = new List<ScheduleViewModel>
            {
                new ScheduleViewModel
                {
                    Day = "wrong day",
                    From = "12:00",
                    To = "15:00"
                },
                new ScheduleViewModel
                {
                    Day = Days.Tuesday.ToString(),
                    From = "12:00",
                    To = "15:00"
                },
                new ScheduleViewModel
                {
                    Day = Days.Tuesday.ToString(),
                    From = "12:00",
                    To = "15:00"
                },
                new ScheduleViewModel
                {
                    Day = Days.Tuesday.ToString(),
                    From = "12:00",
                    To = "15:00"
                },
            };

            var schedules = ScheduleFakeData
               .GetScheduleFakeData();

            var employees = EmployeeFakeData
                .GetEmployeeFakeData()
                .AsQueryable();

            var id = "f1a05cca-b479-4f72-bbda-96b8979f4afe";

            _mockEmployeeRepository.Setup(x => x.GetFirstOrDefaultAsync(
                x => x.Id == id,
                It.IsAny<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>(),
                false))
                .ReturnsAsync((Expression<Func<Employee, bool>> filter,
                Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => employees.FirstOrDefault(filter));

            _mockEmployeeRepository.Setup(x => x.Update(It.IsAny<Employee>()));

            _mockScheduleRepository.Setup(x => x.InsertRangeAsync(schedules));

            //Act
            var result = await scheduleController.AssignSchedulesToEmployee(schedulesVM, id);

            //Assert
             Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CanUpdateSchedule()
        {
            //Arrange
            var scheduleController = new ScheduleController(
                _scheduleService,
                _mapper,
                _scheduleValidator,
                _scheduleCollectionValidator);

            var scheduleVM = new ScheduleViewModel
            {
                Id = 2,
                Day = Days.Tuesday.ToString(),
                From = "12:00",
                To = "15:00"
            };

            var id = 2;

            _mockScheduleRepository.Setup(x => x.Update(It.IsAny<Schedule>()));

            //Act
            var result = scheduleController.UpdateSchedule(id, scheduleVM);

            //Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void UpdateSchedule_ReturnsBadRequest_DueToValidationError()
        {
            //Arrange
            var scheduleController = new ScheduleController(
                _scheduleService,
                _mapper,
                _scheduleValidator,
                _scheduleCollectionValidator);

            var scheduleVM = new ScheduleViewModel
            {
                Id = 2,
                Day = Days.Tuesday.ToString(),
                From = "12:00",
                To = default
            };

            var id = 2;

            _mockScheduleRepository.Setup(x => x.Update(It.IsAny<Schedule>()));

            //Act
            var result = scheduleController.UpdateSchedule(id, scheduleVM);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CanDeleteSchedule()
        {
            //Arrange
            var scheduleController = new ScheduleController(
                _scheduleService,
                _mapper,
                _scheduleValidator,
                _scheduleCollectionValidator);

            var id = 2;

            _mockScheduleRepository.Setup(x => x.GetFirstOrDefaultAsync(
                x => x.Id == id, null, false).Result)
                .Returns(new Schedule() { Id = id });

            _mockScheduleRepository.Setup(x => x.Delete(It.IsAny<Schedule>()));

            //Act
            var result = scheduleController.DeleteSchedule(id).Result;

            //Assert
            Assert.IsType<OkObjectResult>(result);

        }

        [Fact]
        public void DeleteSchedule_WhenScheduleDoesNotExist()
        {
            //Arrange
            var id = 400;

            var scheduleController = new ScheduleController(
                _scheduleService,
                _mapper,
                _scheduleValidator,
                _scheduleCollectionValidator);

            //Act
            var result = scheduleController.DeleteSchedule(id).Result;

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CanDeleteRange()
        {
            //Arrange
            var scheduleController = new ScheduleController(
                _scheduleService,
                _mapper,
                _scheduleValidator,
                _scheduleCollectionValidator);

            var idArr = new List<int> { 1, 2, 3 };

            var schedules = ScheduleFakeData
                .GetScheduleFakeData()
                .AsQueryable();

            _mockScheduleRepository
                .Setup(x => x.GetAsync(
                    It.IsAny<Expression<Func<Schedule, bool>>>(),
                    null,
                    null,
                    false).Result)
                .Returns((Expression<Func<Schedule, bool>> filter,
                Func<IQueryable<Schedule>, IOrderedQueryable<Schedule>> orderBy,
                Func<IQueryable<Schedule>, IIncludableQueryable<Schedule, object>> include,
                bool asNoTracking) => schedules.Where(filter).ToList());

            _mockScheduleRepository.Setup(x => x.DeleteRange(It.IsAny<IEnumerable<Schedule>>()));

            //Act
            var result = scheduleController.DeleteListOfSchedule(idArr).Result;

            //Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void DeleteRange_WhenSomeScheduleWasNotFound()
        {
            //Arrange
            var scheduleController = new ScheduleController(
                _scheduleService,
                _mapper,
                _scheduleValidator,
                _scheduleCollectionValidator);

            var idArr = new List<int> { 1, -4, 3 };

            var schedules = ScheduleFakeData.GetScheduleFakeData().AsQueryable();

            _mockScheduleRepository
                .Setup(x => x.GetAsync(
                    It.IsAny<Expression<Func<Schedule, bool>>>(),
                    null,
                    null,
                    false).Result)
                .Returns((Expression<Func<Schedule, bool>> filter,
                Func<IQueryable<Schedule>, IOrderedQueryable<Schedule>> orderBy,
                Func<IQueryable<Schedule>, IIncludableQueryable<Schedule, object>> include,
                bool asNoTracking) => schedules.Where(filter).ToList());

            _mockScheduleRepository.Setup(x => x.DeleteRange(It.IsAny<IEnumerable<Schedule>>()));

            //Act
            var result = scheduleController.DeleteListOfSchedule(idArr).Result;

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
