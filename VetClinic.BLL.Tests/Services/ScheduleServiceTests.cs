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
    public class ScheduleServiceTests
    {
        private readonly ScheduleService _scheduleService;
        private readonly Mock<IScheduleRepository> _scheduleRepository = new Mock<IScheduleRepository>();
        private readonly Mock<IEmployeeRepository> _employeeRepository = new Mock<IEmployeeRepository>();

        public ScheduleServiceTests()
        {
            _scheduleService = new ScheduleService(
                _scheduleRepository.Object,
                _employeeRepository.Object);
        }

        [Fact]
        public async Task CanReturnScheduleOfEmployee()
        {
            //Arrange
            var employeeId = "f1a05cca-b479-4f72-bbda-96b8979f4afe";

            var scheduleList = ScheduleFakeData.GetScheduleFakeData().AsQueryable();

            _scheduleRepository.Setup(x => x.GetAsync(
                x => x.EmployeeId == employeeId, null, null, false).Result)
                .Returns((Expression<Func<Schedule, bool>> filter,
                Func<IQueryable<Schedule>, IOrderedQueryable<Schedule>> orderBy,
                Func<IQueryable<Schedule>, IIncludableQueryable<Schedule, object>> include,
                bool asNoTracking) => scheduleList.Where(filter).ToList());

            int expectedCount = 5;

            //Act
            var schedule = await _scheduleService.GetScheduleOfEmployee(employeeId);

            //Assert
            Assert.NotNull(schedule);
            Assert.Equal(expectedCount, schedule.Count());
        }

        [Fact]
        public async Task CanReturnScheduleById()
        {
            //Arrange
            var id = 2;

            var schedule = ScheduleFakeData.GetScheduleFakeData().AsQueryable();

            _scheduleRepository.Setup(x => x.GetFirstOrDefaultAsync(
                x => x.Id == id, null, false).Result)
                .Returns((Expression<Func<Schedule, bool>> filter,
                Func<IQueryable<Schedule>, IIncludableQueryable<Schedule, object>> include,
                bool asNoTracking) => schedule.FirstOrDefault(filter));

            //Act
            var testSchedule = await _scheduleService.GetByIdAsync(id);

            //Assert
            Assert.Equal(Days.Tuesday, testSchedule.Day);
        }

        [Fact]
        public async Task GetScheduleById_ShouldReturnException()
        {
            //Arrange
            int id = 500;

            var schedule = ScheduleFakeData.GetScheduleFakeData().AsQueryable();

            _scheduleRepository.Setup(x => x.GetFirstOrDefaultAsync(
                x => x.Id == id, null, false))
                .ReturnsAsync((Expression<Func<Schedule, bool>> filter,
                Func<IQueryable<Schedule>, IIncludableQueryable<Schedule, object>> include,
                bool asNoTracking) => schedule.FirstOrDefault(filter));

            //Act, Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await _scheduleService.GetByIdAsync(id));
        }

        [Fact]
        public async Task CanAssignScheduleToEmployee()
        {
            //Arrange
            var employees = EmployeeFakeData
                .GetEmployeeFakeData()
                .AsQueryable();

            var id = "f1a05cca-b479-4f72-bbda-96b8979f4afe";

            _employeeRepository.Setup(x => x.GetFirstOrDefaultAsync(
                x => x.Id == id,
                It.IsAny<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>(),
                false))
                .ReturnsAsync((Expression<Func<Employee, bool>> filter,
                Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => employees.FirstOrDefault(filter));

            _employeeRepository.Setup(x => x.Update(It.IsAny<Employee>()));

            _scheduleRepository.Setup(x => x.InsertAsync(It.IsAny<Schedule>()));

            //Act
            await _scheduleService.AssignScheduleToEmployeeAsync(It.IsAny<Schedule>(), id);

            //Assert
            _employeeRepository.Verify(a => a.Update(It.IsAny<Employee>()));

            _scheduleRepository.Verify(a => a.InsertAsync(It.IsAny<Schedule>()));
        }

        [Fact]
        public async Task AssignScheduleToEmployee_ReturnsNotFound_WhenEmployeeWasNotFound()
        {
            //Arrange
            var employees = EmployeeFakeData
                .GetEmployeeFakeData()
                .AsQueryable();

            var id = "idonotexist";

            _employeeRepository.Setup(x => x.GetFirstOrDefaultAsync(
                x => x.Id == id,
                It.IsAny<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>(),
                false))
                .ReturnsAsync((Expression<Func<Employee, bool>> filter,
                Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => employees.FirstOrDefault(filter));

            _employeeRepository.Setup(x => x.Update(It.IsAny<Employee>()));

            _scheduleRepository.Setup(x => x.InsertAsync(It.IsAny<Schedule>()));

            //Act, Assert
            await Assert.ThrowsAsync<NotFoundException>(() => 
            _scheduleService.AssignScheduleToEmployeeAsync(It.IsAny<Schedule>(), id));
        }

        [Fact]
        public async Task CanAssignSchedulesToEmployee()
        {
            //Arrange
            var employees = EmployeeFakeData
                .GetEmployeeFakeData()
                .AsQueryable();

            var schedules = ScheduleFakeData
                .GetScheduleFakeData();

            var id = "f1a05cca-b479-4f72-bbda-96b8979f4afe";

            _employeeRepository.Setup(x => x.GetFirstOrDefaultAsync(
                x => x.Id == id,
                It.IsAny<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>(),
                false))
                .ReturnsAsync((Expression<Func<Employee, bool>> filter,
                Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => employees.FirstOrDefault(filter));

            _employeeRepository.Setup(x => x.Update(It.IsAny<Employee>()));

            _scheduleRepository.Setup(x => x.InsertRangeAsync(schedules));

            //Act
            await _scheduleService.AssignMultipleSchedulesToEmployeeAsync(schedules, id);

            //Assert
            _employeeRepository.Verify(a => a.Update(It.IsAny<Employee>()));

            _scheduleRepository.Verify(a => a.InsertRangeAsync(schedules));
        }

        [Fact]
        public async Task AssignSchedulesToEmployee_ReturnsNotFound_WhenEmployeeWasNotFound()
        {
            //Arrange
            var employees = EmployeeFakeData
                .GetEmployeeFakeData()
                .AsQueryable();

            var schedules = ScheduleFakeData
                .GetScheduleFakeData();

            var id = "idonotexist";

            _employeeRepository.Setup(x => x.GetFirstOrDefaultAsync(
                x => x.Id == id,
                It.IsAny<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>(),
                false))
                .ReturnsAsync((Expression<Func<Employee, bool>> filter,
                Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => employees.FirstOrDefault(filter));

            _employeeRepository.Setup(x => x.Update(It.IsAny<Employee>()));

            _scheduleRepository.Setup(x => x.InsertRangeAsync(schedules));

            //Act, Assert
            await Assert.ThrowsAsync<NotFoundException>(() =>
            _scheduleService.AssignMultipleSchedulesToEmployeeAsync(schedules, id));
        }

        [Fact]
        public void CanUpdateSchedule()
        {
            //Arrange
            var id = 2;

            var schedule = ScheduleFakeData.GetScheduleFakeData().FirstOrDefault(x => x.Id == 2);

            _scheduleRepository.Setup(x => x.Update(It.IsAny<Schedule>()));

            //Act
            _scheduleService.Update(id, schedule);

            //Assert
            _scheduleRepository.Verify(x => x.Update(schedule));
        }

        [Fact]
        public void UpdateSchedule_ThrowsException()
        {
            //Arrange
            var id = 12;

            var schedule = ScheduleFakeData.GetScheduleFakeData().FirstOrDefault(x => x.Id == 2);

            _scheduleRepository.Setup(x => x.Update(It.IsAny<Schedule>()));

            //Act, Assert
            Assert.Throws<BadRequestException>(() => _scheduleService.Update(id, schedule));
        }

        [Fact]
        public async Task CanDeleteScheduleAsync()
        {
            //Arrange
            var id = 2;

            _scheduleRepository.Setup(x => x.GetFirstOrDefaultAsync(
                x => x.Id == id, null, false).Result)
                .Returns(new Schedule() { Id = id });

            _scheduleRepository.Setup(x => x.Delete(It.IsAny<Schedule>())).Verifiable();

            //Act
            await _scheduleService.DeleteAsync(id);

            //Assert
            _scheduleRepository.Verify(x => x.Delete(It.IsAny<Schedule>()));


        }

        [Fact]
        public async Task DeleteSchedule_WhenScheduleDoesNotExist()
        {
            await Assert.ThrowsAsync<NotFoundException>(async () =>
             await _scheduleService.DeleteAsync(500));
        }

        [Fact]
        public void CanDeleteRangeAsync()
        {
            //Arrange
            var listOfIds = new List<int> { 2, 3, 4 };

            var schedule = ScheduleFakeData.GetScheduleFakeData().AsQueryable();

            _scheduleRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Schedule, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<Schedule, bool>> filter,
                Func<IQueryable<Schedule>, IOrderedQueryable<Schedule>> orderBy,
                Func<IQueryable<Schedule>, IIncludableQueryable<Schedule, object>> include,
                bool asNoTracking) => schedule.Where(filter).ToList());

            _scheduleRepository.Setup(x => x.DeleteRange(It.IsAny<IEnumerable<Schedule>>()));

            //Act
            _scheduleService.DeleteRangeAsync(listOfIds).Wait();

            //Assert
            _scheduleRepository.Verify(x => x.DeleteRange(It.IsAny<IEnumerable<Schedule>>()));
        }

        [Fact]
        public async Task DeleteRangeAsync_ShouldReturnException_BecauseOneScheduleWasNotFound()
        {
            //Arrange
            var listOfIds = new List<int> { 2, 400, 3 };

            var schedule = ScheduleFakeData.GetScheduleFakeData().AsQueryable();

            _scheduleRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Schedule, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<Schedule, bool>> filter,
                Func<IQueryable<Schedule>, IOrderedQueryable<Schedule>> orderBy,
                Func<IQueryable<Schedule>, IIncludableQueryable<Schedule, object>> include,
                bool asNoTracking) => schedule.Where(filter).ToList());

            _scheduleRepository.Setup(x => x.DeleteRange(It.IsAny<IEnumerable<Schedule>>()));

            //Act, Assert
            await Assert.ThrowsAsync<BadRequestException>(async () =>
                await _scheduleService.DeleteRangeAsync(listOfIds));
        }
    }
}
