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
    public class ScheduleControllerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IScheduleRepository> _mockScheduleRepository;
        private readonly IScheduleService _scheduleService;
        private readonly ScheduleValidator _scheduleValidator;

        public ScheduleControllerTests()
        {
            _mockScheduleRepository = new Mock<IScheduleRepository>();
            _scheduleService = new ScheduleService(_mockScheduleRepository.Object);
            _scheduleValidator = new ScheduleValidator();

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
            var scheduleController = new ScheduleController(_scheduleService, _mapper, _scheduleValidator);

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
            var scheduleController = new ScheduleController(_scheduleService, _mapper, _scheduleValidator);

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

            var expectedErrorMessage = "No schedule was provided for this emplouee";

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
            var scheduleController = new ScheduleController(_scheduleService, _mapper, _scheduleValidator);

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
            var scheduleController = new ScheduleController(_scheduleService, _mapper, _scheduleValidator);

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
        public void CanInsertSchedule()
        {
            //Arrange
            var scheduleController = new ScheduleController(_scheduleService, _mapper, _scheduleValidator);

            var scheduleVM = new ScheduleViewModel
            {
                Day = Days.Tuesday.ToString(),
                From = TimeSpan.FromHours(12),
                To = TimeSpan.FromHours(20)
            };

            _mockScheduleRepository.Setup(x => x.InsertAsync(It.IsAny<Schedule>()));

            //Act
            var result = scheduleController.InsertSchedule(scheduleVM).Result;

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void InsertSchedule_ShouldReturnValidationError()
        {
            //Arrange
            var scheduleController = new ScheduleController(_scheduleService, _mapper, _scheduleValidator);

            var scheduleVM = new ScheduleViewModel
            {
                Day = string.Empty,
                From = default,
                To = TimeSpan.FromHours(20)
            };

            _mockScheduleRepository.Setup(x => x.InsertAsync(It.IsAny<Schedule>()));

            //Act
            var result = scheduleController.InsertSchedule(scheduleVM).Result;

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CanUpdateSchedule()
        {
            //Arrange
            var scheduleController = new ScheduleController(_scheduleService, _mapper, _scheduleValidator);

            var scheduleVM = new ScheduleViewModel
            {
                Id = 2,
                Day = Days.Tuesday.ToString(),
                From = TimeSpan.FromHours(12),
                To = TimeSpan.FromHours(20)
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
            var scheduleController = new ScheduleController(_scheduleService, _mapper, _scheduleValidator);

            var scheduleVM = new ScheduleViewModel
            {
                Id = 2,
                Day = Days.Tuesday.ToString(),
                From = TimeSpan.FromHours(12),
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
            var id = 2;

            _mockScheduleRepository.Setup(x => x.GetFirstOrDefaultAsync(
                x => x.Id == id, null, false).Result)
                .Returns(new Schedule() { Id = id });

            _mockScheduleRepository.Setup(x => x.Delete(It.IsAny<Schedule>()));

            var scheduleController = new ScheduleController(_scheduleService, _mapper, _scheduleValidator);

            //Act
            var result = scheduleController.DeleteSchedule(id).Result;

            //Assert
            Assert.IsType<OkObjectResult>(result);

        }

        [Fact]
        public void DeletePosition_WhenScheduleDoesNotExist()
        {
            //Arrange
            var id = 400;

            var scheduleController = new ScheduleController(_scheduleService, _mapper, _scheduleValidator);

            //Act
            var result = scheduleController.DeleteSchedule(id).Result;

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CanDeleteRange()
        {
            //Arrange
            var idArr = new List<int> { 1, 2, 3 };

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

            var scheduleController = new ScheduleController(_scheduleService, _mapper, _scheduleValidator);

            //Act
            var result = scheduleController.DeleteListOfSchedule(idArr).Result;

            //Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void DeleteRange_WhenSomePositionWasNotFound()
        {
            //Arrange
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

            var scheduleController = new ScheduleController(_scheduleService, _mapper, _scheduleValidator);

            //Act
            var result = scheduleController.DeleteListOfSchedule(idArr).Result;

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
