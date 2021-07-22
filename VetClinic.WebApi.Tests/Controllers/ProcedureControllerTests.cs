using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VetClinic.BLL.Services;
using VetClinic.BLL.Tests.FakeData;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Repositories;
using VetClinic.WebApi.Controllers;
using VetClinic.WebApi.Mappers;
using VetClinic.WebApi.Validators.EntityValidators;
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
        private readonly ProcedureValidator _validator;
        public ProcedureControllerTests()
        {
            _procedureService = new ProcedureService(_procedureRepository.Object);

            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile(new ProcedureMapperProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            _mapper = mapper;
            _validator = new ProcedureValidator();
        }

        [Fact]
        public void CanGetAllProcedures()
        {
            //arrange
            var ProcedureController = new ProcedureController(_procedureService, _mapper, _validator);

            var Procedures = ProcedureFakeData.GetProcedureFakeData();

            _procedureRepository.Setup(b => b.GetAsync(null, null, null, true).Result).Returns(() => Procedures);
            //act
            var result = ProcedureController.GetAllProcedures().Result;
            //assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ProcedureViewModel>>(viewResult.Value);
            Assert.Equal(ProcedureFakeData.GetProcedureFakeData().Count, model.Count());
        }

        [Fact]
        public void CanReturnProcedureById()
        {
            //arrange
            var ProcedureController = new ProcedureController(_procedureService, _mapper, _validator);

            var Procedures = ProcedureFakeData.GetProcedureFakeData().AsQueryable();

            int id = 6;

            _procedureRepository.Setup(b => b.GetFirstOrDefaultAsync(b => b.Id == id, null, false).Result)
                .Returns((Expression<Func<Procedure, bool>> filter,
                Func<IQueryable<Procedure>, IIncludableQueryable<Procedure, object>> include,
                bool asNoTracking) => Procedures.FirstOrDefault(filter));
            //act
            var result = ProcedureController.GetProcedure(id).Result;
            //assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<ProcedureViewModel>(viewResult.Value);

            Assert.Equal(id, model.Id);
            Assert.Equal("Procedure 6", model.Title);
        }

        [Fact]
        public void GetProcedureByInvalidId()
        {
            //arrange
            var ProcedureController = new ProcedureController(_procedureService, _mapper, _validator);

            int id = 100;
            //act
            var result = ProcedureController.GetProcedure(id).Result;
            //assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CanInsertProcedure()
        {
            //arrange
            ProcedureViewModel Procedure = new ProcedureViewModel
            {
                Id = 11,
                Title = "Procedure 11",
                Description = "Surgical procedure.",
                Duration = new TimeSpan(hours: 0, minutes: 45, seconds: 0).ToString(),
                Price = 1600
            };

            var ProcedureController = new ProcedureController(_procedureService, _mapper, _validator);

            _procedureRepository.Setup(b => b.InsertAsync(It.IsAny<Procedure>()));
            //act
            var result = ProcedureController.InsertProcedure(Procedure).Result;
            //assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void CanUpdateProcedure()
        {
            //arrange
            ProcedureViewModel Procedure = new ProcedureViewModel
            {
                Id = 11,
                Title = "Procedure 11",
                Description = "Surgical procedure.",
                Duration = new TimeSpan(hours: 0, minutes: 45, seconds: 0).ToString(),
                Price = 1600
            };

            int id = 11;

            var ProcedureController = new ProcedureController(_procedureService, _mapper, _validator);

            _procedureRepository.Setup(b => b.InsertAsync(It.IsAny<Procedure>()));
            //act
            var result = ProcedureController.Update(id, Procedure);
            //assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void UpdateProcedure_InvalidId()
        {
            //arrange
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
            //assert
            Assert.Throws<ArgumentException>(() => _procedureService.Update(id, ProcedureToUpdate));
        }

        [Fact]
        public void CanDeleteProcedure()
        {
            //arrange
            int id = 5;

            _procedureRepository.Setup(b => b.GetFirstOrDefaultAsync(
                b => b.Id == id, null, false).Result)
                .Returns(new Procedure() { Id = id });

            _procedureRepository.Setup(b => b.Delete(It.IsAny<Procedure>()));

            var ProcedureController = new ProcedureController(_procedureService, _mapper, _validator);
            //act
            var result = ProcedureController.DeleteProcedure(id).Result;
            //assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void DeleteProcedureByInvalidId()
        {
            //arrange
            int id = 500;

            var ProcedureController = new ProcedureController(_procedureService, _mapper, _validator);
            //act
            var result = ProcedureController.DeleteProcedure(id).Result;
            //assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CanDeleteRange()
        {
            //arrange
            List<int> ids = new List<int>() { 4, 8, 9 };

            var Procedures = ProcedureFakeData.GetProcedureFakeData().AsQueryable();

            _procedureRepository
                .Setup(b => b.GetAsync(It.IsAny<Expression<Func<Procedure, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<Procedure, bool>> filter,
                Func<IQueryable<Procedure>, IOrderedQueryable<Procedure>> ProcedureBy,
                Func<IQueryable<Procedure>, IIncludableQueryable<Procedure, object>> include,
                bool asNoTracking) => Procedures.Where(filter).ToList());

            _procedureRepository.Setup(b => b.DeleteRange(It.IsAny<IEnumerable<Procedure>>()));

            var ProcedureController = new ProcedureController(_procedureService, _mapper, _validator);
            //act
            var result = ProcedureController.DeleteProcedures(ids).Result;
            //assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void DeleteRangeWithInvalidId()
        {
            //arrange
            List<int> ids = new List<int>() { 4, 8, 100 };

            var Procedures = ProcedureFakeData.GetProcedureFakeData().AsQueryable();

            _procedureRepository
                .Setup(b => b.GetAsync(It.IsAny<Expression<Func<Procedure, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<Procedure, bool>> filter,
                Func<IQueryable<Procedure>, IOrderedQueryable<Procedure>> ProcedureBy,
                Func<IQueryable<Procedure>, IIncludableQueryable<Procedure, object>> include,
                bool asNoTracking) => Procedures.Where(filter).ToList());

            _procedureRepository.Setup(b => b.DeleteRange(It.IsAny<IEnumerable<Procedure>>()));

            var ProcedureController = new ProcedureController(_procedureService, _mapper, _validator);
            //act
            var result = ProcedureController.DeleteProcedures(ids).Result;

            var badRequest = result as BadRequestObjectResult;
            //assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"{SomeEntitiesInCollectionNotFound} {nameof(Procedure)}s to delete", badRequest.Value);
        }
    }
}
