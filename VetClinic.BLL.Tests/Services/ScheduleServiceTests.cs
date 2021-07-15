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
        public ScheduleServiceTests()
        {
            _scheduleService = new ScheduleService(
                _scheduleRepository.Object);
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
        public async Task CanInsertScheduleAsync()
        {
            //Arrange
            var newSchedule = new Schedule
            {
                Id = 12,
                Day = Days.Saturday,
                From = TimeSpan.FromHours(12),
                To = TimeSpan.FromHours(14),
                EmployeeId = "6fca381a-40d0-4bf9-a076-706e1a995662"
            };

            _scheduleRepository.Setup(x => x.InsertAsync(It.IsAny<Schedule>()));

            //Act
            await _scheduleService.InsertAsync(newSchedule);

            //Assert
            _scheduleRepository.Verify(a => a.InsertAsync(newSchedule));
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
