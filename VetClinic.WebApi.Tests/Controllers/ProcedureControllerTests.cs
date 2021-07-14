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
    public class ProcedureControllerTests
    {
        private readonly ProcedureService _procedureService;
        private readonly Mock<IProcedureRepository> _procedureRepository = new Mock<IProcedureRepository>();
        private readonly IMapper _mapper;
        public ProcedureControllerTests()
        {
            _procedureService = new ProcedureService(_procedureRepository.Object);

            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile(new ProcedureMapperProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            _mapper = mapper;
        }

        [Fact]
        public void CanGetAllProcedures()
        {
            var ProcedureController = new ProcedureController(_procedureService, _mapper);

            var Procedures = ProcedureFakeData.GetProcedureFakeData();

            _procedureRepository.Setup(b => b.GetAsync(null, null, null, true).Result).Returns(() => Procedures);

            var result = ProcedureController.GetAllProceduresAsync().Result;

            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ProcedureViewModel>>(viewResult.Value);
            Assert.Equal(ProcedureFakeData.GetProcedureFakeData().Count, model.Count());
        }

        [Fact]
        public void CanReturnProcedureById()
        {
            var ProcedureController = new ProcedureController(_procedureService, _mapper);

            var Procedures = ProcedureFakeData.GetProcedureFakeData().AsQueryable();

            int id = 6;

            _procedureRepository.Setup(b => b.GetFirstOrDefaultAsync(b => b.Id == id, null, false).Result)
                .Returns((Expression<Func<Procedure, bool>> filter,
                Func<IQueryable<Procedure>, IIncludableQueryable<Procedure, object>> include,
                bool asNoTracking) => Procedures.FirstOrDefault(filter));

            var result = ProcedureController.GetProcedureByIdAsync(id).Result;

            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<ProcedureViewModel>(viewResult.Value);

            Assert.Equal(id, model.Id);
            Assert.Equal("Procedure 6", model.Title);
        }

        [Fact]
        public void GetProcedureByInvalidId()
        {
            var ProcedureController = new ProcedureController(_procedureService, _mapper);

            int id = 100;

            var result = ProcedureController.GetProcedureByIdAsync(id).Result;

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CanInsertProcedure()
        {
            ProcedureViewModel Procedure = new ProcedureViewModel
            {
                Id = 11,
                Title = "Procedure 11",
                Description = "Surgical procedure.",
                Duration = new TimeSpan(hours: 0, minutes: 45, seconds: 0),
                Price = 1600
            };

            var ProcedureController = new ProcedureController(_procedureService, _mapper);

            _procedureRepository.Setup(b => b.InsertAsync(It.IsAny<Procedure>()));

            var result = ProcedureController.InsertProcedureAsync(Procedure).Result;

            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public void CanUpdateProcedure()
        {
            ProcedureViewModel Procedure = new ProcedureViewModel
            {
                Id = 11,
                Title = "Procedure 11",
                Description = "Surgical procedure.",
                Duration = new TimeSpan(hours: 0, minutes: 45, seconds: 0),
                Price = 1600
            };

            int id = 11;

            var ProcedureController = new ProcedureController(_procedureService, _mapper);

            _procedureRepository.Setup(b => b.InsertAsync(It.IsAny<Procedure>()));

            var result = ProcedureController.Update(id, Procedure);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void UpdateProcedure_InvalidId()
        {
            Procedure ProcedureToUpdate = new Procedure
            {
                Id = 11,
                Title = "Procedure 11",
                Description = "Surgical procedure.",
                Duration = new TimeSpan(hours: 0, minutes: 45, seconds: 0),
                Price = 1600
            };

            int id = 10;

            _procedureRepository.Setup(b => b.Update(It.IsAny<Procedure>()));

            Assert.Throws<ArgumentException>(() => _procedureService.Update(id, ProcedureToUpdate));
        }

        [Fact]
        public void CanDeleteProcedure()
        {
            int id = 5;

            _procedureRepository.Setup(b => b.GetFirstOrDefaultAsync(
                b => b.Id == id, null, false).Result)
                .Returns(new Procedure() { Id = id });

            _procedureRepository.Setup(b => b.Delete(It.IsAny<Procedure>()));

            var ProcedureController = new ProcedureController(_procedureService, _mapper);

            var result = ProcedureController.DeleteProcedureAsync(id).Result;

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void DeleteProcedureByInvalidId()
        {
            int id = 500;

            var ProcedureController = new ProcedureController(_procedureService, _mapper);

            var result = ProcedureController.DeleteProcedureAsync(id).Result;

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CanDeleteRange()
        {
            int[] ids = new int[] { 4, 8, 9 };

            var Procedures = ProcedureFakeData.GetProcedureFakeData().AsQueryable();

            _procedureRepository
                .Setup(b => b.GetAsync(It.IsAny<Expression<Func<Procedure, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<Procedure, bool>> filter,
                Func<IQueryable<Procedure>, IOrderedQueryable<Procedure>> ProcedureBy,
                Func<IQueryable<Procedure>, IIncludableQueryable<Procedure, object>> include,
                bool asNoTracking) => Procedures.Where(filter).ToList());

            _procedureRepository.Setup(b => b.DeleteRange(It.IsAny<IEnumerable<Procedure>>()));

            var ProcedureController = new ProcedureController(_procedureService, _mapper);

            var result = ProcedureController.DeleteProceduresAsync(ids).Result;

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void DeleteRangeWithInvalidId()
        {
            int[] ids = new int[] { 4, 8, 100 };

            var Procedures = ProcedureFakeData.GetProcedureFakeData().AsQueryable();

            _procedureRepository
                .Setup(b => b.GetAsync(It.IsAny<Expression<Func<Procedure, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<Procedure, bool>> filter,
                Func<IQueryable<Procedure>, IOrderedQueryable<Procedure>> ProcedureBy,
                Func<IQueryable<Procedure>, IIncludableQueryable<Procedure, object>> include,
                bool asNoTracking) => Procedures.Where(filter).ToList());

            _procedureRepository.Setup(b => b.DeleteRange(It.IsAny<IEnumerable<Procedure>>()));

            var ProcedureController = new ProcedureController(_procedureService, _mapper);

            var result = ProcedureController.DeleteProceduresAsync(ids).Result;

            var badRequest = result as BadRequestObjectResult;

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"{SomeEntitiesInCollectionNotFound} {nameof(Procedure)}s to delete", badRequest.Value);
        }
    }
}
