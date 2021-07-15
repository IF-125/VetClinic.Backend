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
using VetClinic.WebApi.ViewModels;
using Xunit;
using static VetClinic.Core.Resources.TextMessages;

namespace VetClinic.WebApi.Tests.Controllers
{
    public class AppointmentControllerTests
    {
        private readonly AppointmentService _appointmentService;
        private readonly Mock<IAppointmentRepository> _appointmentRepository = new Mock<IAppointmentRepository>();
        private readonly IMapper _mapper;
        public AppointmentControllerTests()
        {
            _appointmentService = new AppointmentService(_appointmentRepository.Object);

            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile(new AppointmentMapperProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            _mapper = mapper;
        }

        [Fact]
        public void CanGetAllAppointments()
        {
            //arrange
            var AppointmentController = new AppointmentController(_appointmentService, _mapper);

            var Appointments = AppointmentFakeData.GetAppointmentFakeData();

            _appointmentRepository.Setup(b => b.GetAsync(null, null, null, true).Result).Returns(() => Appointments);
            //act
            var result = AppointmentController.GetAllAppointmentsAsync().Result;
            //assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<AppointmentViewModel>>(viewResult.Value);
            Assert.Equal(AppointmentFakeData.GetAppointmentFakeData().Count, model.Count());
        }

        [Fact]
        public void CanReturnAppointmentById()
        {
            //arrange
            var AppointmentController = new AppointmentController(_appointmentService, _mapper);

            var Appointments = AppointmentFakeData.GetAppointmentFakeData().AsQueryable();

            int id = 6;

            _appointmentRepository.Setup(b => b.GetFirstOrDefaultAsync(b => b.Id == id, null, false).Result)
                .Returns((Expression<Func<Appointment, bool>> filter,
                Func<IQueryable<Appointment>, IIncludableQueryable<Appointment, object>> include,
                bool asNoTracking) => Appointments.FirstOrDefault(filter));
            //act
            var result = AppointmentController.GetAppointmentByIdAsync(id).Result;

            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<AppointmentViewModel>(viewResult.Value);
            //assert
            Assert.Equal(id, model.Id);
            Assert.Equal(AppointmentStatus.Opened, model.Status);
        }

        [Fact]
        public void GetAppointmentByInvalidId()
        {
            //arrange
            var AppointmentController = new AppointmentController(_appointmentService, _mapper);

            int id = 100;
            //act
            var result = AppointmentController.GetAppointmentByIdAsync(id).Result;
            //assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CanInsertAppointment()
        {
            //arrange
            AppointmentViewModel Appointment = new AppointmentViewModel
            {
                Id = 11,
                Status = AppointmentStatus.Opened,
                From = new DateTime(2021, 8, 22),
                To = new DateTime(2021, 8, 23),
                OrderProcedureId = 11
            };

            var AppointmentController = new AppointmentController(_appointmentService, _mapper);

            _appointmentRepository.Setup(b => b.InsertAsync(It.IsAny<Appointment>()));
            //act
            var result = AppointmentController.InsertAppointmentAsync(Appointment).Result;
            //assert
            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public void CanUpdateAppointment()
        {
            //arrange
            AppointmentViewModel Appointment = new AppointmentViewModel
            {
                Id = 11,
                Status = AppointmentStatus.Opened,
                From = new DateTime(2021, 8, 22),
                To = new DateTime(2021, 8, 23),
                OrderProcedureId = 11
            };

            int id = 11;

            var AppointmentController = new AppointmentController(_appointmentService, _mapper);

            _appointmentRepository.Setup(b => b.InsertAsync(It.IsAny<Appointment>()));
            //act
            var result = AppointmentController.Update(id, Appointment);
            //assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void UpdateAppointment_InvalidId()
        {
            //arrange
            Appointment AppointmentToUpdate = new Appointment
            {
                Id = 11,
                Status = AppointmentStatus.Opened,
                From = new DateTime(2021, 8, 22),
                To = new DateTime(2021, 8, 23),
                OrderProcedureId = 11
            };

            int id = 10;

            _appointmentRepository.Setup(b => b.Update(It.IsAny<Appointment>()));
            //assert
            Assert.Throws<ArgumentException>(() => _appointmentService.Update(id, AppointmentToUpdate));
        }

        [Fact]
        public void CanDeleteAppointment()
        {
            //arrange
            int id = 5;

            _appointmentRepository.Setup(b => b.GetFirstOrDefaultAsync(
                b => b.Id == id, null, false).Result)
                .Returns(new Appointment() { Id = id });

            _appointmentRepository.Setup(b => b.Delete(It.IsAny<Appointment>()));

            var AppointmentController = new AppointmentController(_appointmentService, _mapper);
            //act
            var result = AppointmentController.DeleteAppointmentAsync(id).Result;
            //assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void DeleteAppointmentByInvalidId()
        {
            //arrange
            int id = 500;

            var AppointmentController = new AppointmentController(_appointmentService, _mapper);
            //act
            var result = AppointmentController.DeleteAppointmentAsync(id).Result;
            //assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CanDeleteRange()
        {
            //arrange
            int[] ids = new int[] { 4, 8, 9 };

            var Appointments = AppointmentFakeData.GetAppointmentFakeData().AsQueryable();

            _appointmentRepository
                .Setup(b => b.GetAsync(It.IsAny<Expression<Func<Appointment, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<Appointment, bool>> filter,
                Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>> AppointmentBy,
                Func<IQueryable<Appointment>, IIncludableQueryable<Appointment, object>> include,
                bool asNoTracking) => Appointments.Where(filter).ToList());

            _appointmentRepository.Setup(b => b.DeleteRange(It.IsAny<IEnumerable<Appointment>>()));

            var AppointmentController = new AppointmentController(_appointmentService, _mapper);
            //act
            var result = AppointmentController.DeleteAppointmentsAsync(ids).Result;
            //assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void DeleteRangeWithInvalidId()
        {
            //arrange
            int[] ids = new int[] { 4, 8, 100 };

            var Appointments = AppointmentFakeData.GetAppointmentFakeData().AsQueryable();

            _appointmentRepository
                .Setup(b => b.GetAsync(It.IsAny<Expression<Func<Appointment, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<Appointment, bool>> filter,
                Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>> AppointmentBy,
                Func<IQueryable<Appointment>, IIncludableQueryable<Appointment, object>> include,
                bool asNoTracking) => Appointments.Where(filter).ToList());

            _appointmentRepository.Setup(b => b.DeleteRange(It.IsAny<IEnumerable<Appointment>>()));

            var AppointmentController = new AppointmentController(_appointmentService, _mapper);
            //act
            var result = AppointmentController.DeleteAppointmentsAsync(ids).Result;

            var badRequest = result as BadRequestObjectResult;
            //assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"{SomeEntitiesInCollectionNotFound} {nameof(Appointment)}s to delete", badRequest.Value);
        }
    }
}
