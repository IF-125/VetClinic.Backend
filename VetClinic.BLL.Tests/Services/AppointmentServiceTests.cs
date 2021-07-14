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
    public class AppointmentServiceTests
    {
        private readonly AppointmentService _AppointmentService;
        private readonly Mock<IAppointmentRepository> _appointmentRepository = new Mock<IAppointmentRepository>();
        public AppointmentServiceTests()
        {
            _AppointmentService = new AppointmentService(
                _appointmentRepository.Object);
        }

        [Fact]
        public async Task CanReturnAllAppointments()
        {
            _appointmentRepository.Setup(b => b.GetAsync(null, null, null, true).Result)
                .Returns(AppointmentFakeData.GetAppointmentFakeData());

            IList<Appointment> Appointments = await _AppointmentService.GetAppointmentsAsync(null, null, null, asNoTracking: true);

            Assert.NotNull(Appointments);
            Assert.Equal(10, Appointments.Count);
        }

        [Fact]
        public async Task CanReturnAppointmentById()
        {
            int id = 4;

            var Appointments = AppointmentFakeData.GetAppointmentFakeData().AsQueryable();

            _appointmentRepository.Setup(b => b.GetFirstOrDefaultAsync(
                b => b.Id == id, null, true).Result)
                .Returns((Expression<Func<Appointment, bool>> filter,
                Func<IQueryable<Appointment>, IIncludableQueryable<Appointment, object>> include,
                bool asNoTracking) => Appointments.FirstOrDefault(filter));

            var Appointment = await _AppointmentService.GetByIdAsync(id, null, true);

            Assert.Equal(AppointmentStatus.Closed, Appointment.Status);
        }

        [Fact]
        public void GetAppointmentByInvalidId()
        {
            int id = 45;

            var Appointments = AppointmentFakeData.GetAppointmentFakeData().AsQueryable();

            _appointmentRepository.Setup(b => b.GetFirstOrDefaultAsync(
                b => b.Id == id, null, true).Result)
                .Returns((Expression<Func<Appointment, bool>> filter,
                Func<IQueryable<Appointment>, IIncludableQueryable<Appointment, object>> include,
                bool asNoTracking) => Appointments.FirstOrDefault(filter));

            Assert.Throws<AggregateException>(() => _AppointmentService.GetByIdAsync(id).Result);
        }

        [Fact]
        public async Task CanInsertAppointment()
        {
            Appointment AppointmentToInsert = new Appointment
            {
                Id = 11,
                Status = AppointmentStatus.Opened,
                From = new DateTime(2021, 11, 10),
                To = new DateTime(2021, 11, 11),
                OrderProcedureId = 11
            };

            _appointmentRepository.Setup(b => b.InsertAsync(It.IsAny<Appointment>()));

            await _AppointmentService.InsertAsync(AppointmentToInsert);

            _appointmentRepository.Verify(b => b.InsertAsync(AppointmentToInsert));
        }

        [Fact]
        public void CanUpdateAppointment()
        {
            Appointment AppointmentToUpdate = new Appointment
            {
                Id = 11,
                Status = AppointmentStatus.Opened,
                From = new DateTime(2021, 11, 10),
                To = new DateTime(2021, 11, 11),
                OrderProcedureId = 11
            };

            int id = 11;

            _appointmentRepository.Setup(b => b.Update(It.IsAny<Appointment>()));

            _AppointmentService.Update(id, AppointmentToUpdate);

            _appointmentRepository.Verify(b => b.Update(AppointmentToUpdate));
        }

        [Fact]
        public void UpdateAppointment_InvalidId()
        {
            Appointment AppointmentToUpdate = new Appointment
            {
                Id = 11,
                Status = AppointmentStatus.Opened,
                From = new DateTime(2021, 11, 10),
                To = new DateTime(2021, 11, 11),
                OrderProcedureId = 11
            };

            int id = 10;

            _appointmentRepository.Setup(b => b.Update(It.IsAny<Appointment>()));

            Assert.Throws<ArgumentException>(() => _AppointmentService.Update(id, AppointmentToUpdate));
        }

        [Fact]
        public async Task CanDeleteAppointment()
        {
            var id = 10;

            _appointmentRepository.Setup(b => b.GetFirstOrDefaultAsync(
                b => b.Id == id, null, false).Result)
                .Returns(new Appointment() { Id = id });

            _appointmentRepository.Setup(b => b.Delete(It.IsAny<Appointment>())).Verifiable();

            await _AppointmentService.DeleteAsync(id);

            _appointmentRepository.Verify(b => b.Delete(It.IsAny<Appointment>()));
        }

        [Fact]
        public void DeleteAppointmentByInvalidId()
        {
            Assert.Throws<AggregateException>(() => _AppointmentService.DeleteAsync(100).Wait());
        }

        [Fact]
        public void CanDeleteRange()
        {
            int[] ids = new int[] { 8, 9, 10 };

            var Appointments = AppointmentFakeData.GetAppointmentFakeData().AsQueryable();

            _appointmentRepository.Setup(b => b.GetAsync(It.IsAny<Expression<Func<Appointment, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<Appointment, bool>> filter,
                Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>> AppointmentBy,
                Func<IQueryable<Appointment>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => Appointments.Where(filter).ToList());

            _appointmentRepository.Setup(b => b.DeleteRange(It.IsAny<IEnumerable<Appointment>>()));

            _AppointmentService.DeleteRangeAsync(ids).Wait();

            _appointmentRepository.Verify(b => b.DeleteRange(It.IsAny<IEnumerable<Appointment>>()));
        }

        [Fact]
        public void DeleteRangeWithInvalidId()
        {
            int[] ids = new int[] { 8, 9, 100 };

            var Appointments = AppointmentFakeData.GetAppointmentFakeData().AsQueryable();

            _appointmentRepository.Setup(b => b.GetAsync(It.IsAny<Expression<Func<Appointment, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<Appointment, bool>> filter,
                Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>> AppointmentBy,
                Func<IQueryable<Appointment>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => Appointments.Where(filter).ToList());

            _appointmentRepository.Setup(b => b.DeleteRange(It.IsAny<IEnumerable<Appointment>>()));

            Assert.Throws<AggregateException>(() => _AppointmentService.DeleteRangeAsync(ids).Wait());
        }
    }
}
