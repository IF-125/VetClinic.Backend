using AutoMapper;
using FluentValidation.Results;
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
using VetClinic.Core.Interfaces.Services;
using VetClinic.WebApi.Controllers;
using VetClinic.WebApi.Mappers;
using VetClinic.WebApi.Validators.EntityValidators;
using VetClinic.WebApi.ViewModels;
using Xunit;
using static VetClinic.Core.Resources.TextMessages;

namespace VetClinic.WebApi.Tests.Controllers
{
    public class PositionControllerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IPositionRepository> _mockPositionRepository;
        private readonly IPositionService _positionService;
        private readonly PositionValidator _positionValidator;

        public PositionControllerTests()
        {
            _mockPositionRepository = new Mock<IPositionRepository>();
            _positionService = new PositionService(_mockPositionRepository.Object);
            _positionValidator = new PositionValidator();

            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new PositionMapperProfile());
                });

                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }

        [Fact]
        public void CanGetAllPositions()
        {
            //Arrange
            var positionsController = new PositionsController(
                _positionService,
                _mapper,
                _positionValidator);

            var positions = PositionFakeData.GetPositionFakeData();

            var expectedResult = positions.Count;

            _mockPositionRepository.Setup(x => x
            .GetAsync(null, null, null, true).Result).Returns(() => positions);

            //Act
            var result = positionsController.GetAllPositions().Result;

            //Assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<PositionViewModel>>(viewResult.Value);

            Assert.Equal(expectedResult, model.Count());
        }

        [Fact]
        public void CanGetPositionById()
        {
            //Arrange
            var positionsController = new PositionsController(
                _positionService,
                _mapper,
                _positionValidator);

            var positions = PositionFakeData
                .GetPositionFakeData()
                .AsQueryable();

            var id = 2;

            _mockPositionRepository.Setup(x => x
            .GetFirstOrDefaultAsync(
                x => x.Id == id,
                null,
                false).Result)
                .Returns(
                (Expression<Func<Position, bool>> filter,
                Func<IQueryable<Position>, IIncludableQueryable<Position, object>> include,
                bool asNoTracking) => positions.FirstOrDefault(filter));

            //Act
            var result = positionsController.GetPosition(id).Result;

            //Assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<PositionViewModel>(viewResult.Value);

            Assert.Equal(id, model.Id);
            Assert.Equal("Anesthetist", model.Title);
        }

        [Fact]
        public void GetPositionById_ReturnsNotFound()
        {
            //Arrange
            var positionsController = new PositionsController(
                _positionService,
                _mapper,
                _positionValidator);

            var id = 500;

            //Act
            var result = positionsController.GetPosition(id).Result;

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CanInsertPositionAsync()
        {
            //Arrange
            var positionsController = new PositionsController(
                _positionService,
                _mapper,
                _positionValidator);

            var positionVM = new PositionViewModel
            {
                Id = 12,
                Title = "New Position"
            };

            _mockPositionRepository.Setup(x => x.InsertAsync(It.IsAny<Position>()));

            //Act
            var result = positionsController.InsertPosition(positionVM).Result;

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void InsertPositionAsync_ShouldReturnValidationError()
        {
            //Arrange
            var positionsController = new PositionsController(
                _positionService,
                _mapper,
                _positionValidator);

            var positionVM = new PositionViewModel
            {
                Id = 12,
                Title = string.Empty
            };

            _mockPositionRepository.Setup(x => x.InsertAsync(It.IsAny<Position>()));

            //Act
            var result = positionsController.InsertPosition(positionVM).Result;

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CanUpdatePosition()
        {
            //Arrange
            var positionsController = new PositionsController(
                _positionService,
                _mapper,
                _positionValidator);

            var positionVM = new PositionViewModel
            {
                Id = 2,
                Title = "new title"
            };

            var id = 2;

            _mockPositionRepository.Setup(x => x.Update(It.IsAny<Position>()));

            //Act
            var result = positionsController.UpdatePosition(id, positionVM);

            //Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void UpdatePosition_ReturnsBadRequest_DueToValidationError()
        {
            //Arrange
            var positionsController = new PositionsController(
                _positionService,
                _mapper,
                _positionValidator);

            var positionVM = new PositionViewModel
            {
                Id = 2,
                Title = null
            };

            var id = 2;

            _mockPositionRepository.Setup(x => x.Update(It.IsAny<Position>()));

            //Act
            var result = positionsController.UpdatePosition(id, positionVM);

            //Assert
            var badRequest = result as BadRequestObjectResult;

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(badRequest.Value.GetType() == typeof(List<ValidationFailure>));
        }

        [Fact]
        public void UpdatePosition_ReturnsBadRequest_DueToIdsMismatch()
        {
            //Arrange
            var positionsController = new PositionsController(
                _positionService,
                _mapper,
                _positionValidator);

            var positionVM = new PositionViewModel
            {
                Id = 2,
                Title = "new title"
            };

            var id = 12;

            _mockPositionRepository.Setup(x => x.Update(It.IsAny<Position>()));

            //Act
            var result = positionsController.UpdatePosition(id, positionVM);

            //Assert
            var badRequest = result as BadRequestObjectResult;

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Position id and passed id did not match", badRequest.Value);
        }

        [Fact]
        public void CanDeletePosition()
        {
            //Arrange
            var positionsController = new PositionsController(
                _positionService,
                _mapper,
                _positionValidator);

            var id = 2;

            _mockPositionRepository.Setup(x => x
            .GetFirstOrDefaultAsync(
                x => x.Id == id,
                null,
                false).Result)
                .Returns(new Position() { Id = id });

            _mockPositionRepository.Setup(x => x.Delete(It.IsAny<Position>()));

            //Act
            var result = positionsController.DeletePositionAsync(id).Result;

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void DeletePosition_WhenPositionDoesNotExist()
        {
            //Arrange
            var positionsController = new PositionsController(
                _positionService,
                _mapper,
                _positionValidator);

            var id = 400;

            //Act
            var result = positionsController.DeletePositionAsync(id).Result;

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CanDeleteRange()
        {
            //Arrange
            var positionsController = new PositionsController(
                _positionService,
                _mapper,
                _positionValidator);

            var idArr = new List<int> { 1, 2, 3 };

            var positions = PositionFakeData
                .GetPositionFakeData()
                .AsQueryable();

            _mockPositionRepository
                .Setup(x => x
                .GetAsync(
                    It.IsAny<Expression<Func<Position, bool>>>(),
                    null,
                    null,
                    false).Result)
                .Returns(
                (Expression<Func<Position, bool>> filter,
                Func<IQueryable<Position>, IOrderedQueryable<Position>> orderBy,
                Func<IQueryable<Position>, IIncludableQueryable<Position, object>> include,
                bool asNoTracking) => positions.Where(filter).ToList());

            _mockPositionRepository.Setup(x => x.DeleteRange(It.IsAny<IEnumerable<Position>>()));

            //Act
            var result = positionsController.DeletePositionsAsync(idArr).Result;

            //Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void DeleteRange_WhenSomePositionWasNotFound()
        {
            //Arrange
            var positionsController = new PositionsController(
                _positionService,
                _mapper,
                _positionValidator);

            var idArr = new List<int> { 1, 500, 3 };

            var positions = PositionFakeData
                .GetPositionFakeData()
                .AsQueryable();

            _mockPositionRepository
                .Setup(x => x
                .GetAsync(
                    It.IsAny<Expression<Func<Position, bool>>>(),
                    null,
                    null,
                    false).Result)
                .Returns(
                (Expression<Func<Position, bool>> filter,
                Func<IQueryable<Position>, IOrderedQueryable<Position>> orderBy,
                Func<IQueryable<Position>, IIncludableQueryable<Position, object>> include,
                bool asNoTracking) => positions.Where(filter).ToList());

            _mockPositionRepository.Setup(x => x.DeleteRange(It.IsAny<IEnumerable<Position>>()));

            //Act
            var result = positionsController.DeletePositionsAsync(idArr).Result;

            //Assert
            var badRequest = result as BadRequestObjectResult;

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"{SomeEntitiesInCollectionNotFound} {nameof(Position)}s to delete", badRequest.Value);
        }
    }
}
