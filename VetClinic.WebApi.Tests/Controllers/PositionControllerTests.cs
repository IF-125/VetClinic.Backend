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

        private List<Position> GetTestPositions()
        {
            return new List<Position>
            {
                new Position
                {
                    Id = 1,
                    Title = "Doctor",
                },
                new Position
                {
                    Id = 2,
                    Title = "Anesthetist",
                },
                new Position
                {
                    Id = 3,
                    Title = "Accountant",
                },
                new Position
                {
                    Id = 4,
                    Title = "Admin",
                }
            };
        }

        [Fact]
        public void CanGetAllPositions()
        {
            var positionsController = new PositionsController(_positionService, _mapper, _positionValidator);

            var positions = GetTestPositions();

            _mockPositionRepository.Setup(x => x.GetAsync(null, null, null, true).Result).Returns(() => positions);

            var result = positionsController.GetAllPositions().Result;

            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<PositionViewModel>>(viewResult.Value);
            Assert.Equal(GetTestPositions().Count, model.Count());
        }

        [Fact]
        public void CanGetPositionById()
        {
            var positionsController = new PositionsController(_positionService, _mapper, _positionValidator);

            var positions = GetTestPositions().AsQueryable();

            var id = 2;

            _mockPositionRepository.Setup(x => x.GetFirstOrDefaultAsync(x => x.Id == id, null, false).Result)
                .Returns((Expression<Func<Position, bool>> filter,
                Func<IQueryable<Position>, IIncludableQueryable<Position, object>> include,
                bool asNoTracking) => positions.FirstOrDefault(filter));

            var result = positionsController.GetPosition(id).Result;

            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<PositionViewModel>(viewResult.Value);

            Assert.Equal(id, model.Id);
            Assert.Equal("Anesthetist", model.Title);
        }

        [Fact]
        public void GetPositionById_ReturnsNotFound()
        {
            var positionsController = new PositionsController(_positionService, _mapper, _positionValidator);

            var id = 500;

            var result = positionsController.GetPosition(id).Result;

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CanInsertPositionAsync()
        {
            var positionsController = new PositionsController(_positionService, _mapper, _positionValidator);

            var positionVM = new PositionViewModel
            {
                Id = 12,
                Title = "New Position"
            };

            _mockPositionRepository.Setup(x => x.InsertAsync(It.IsAny<Position>()));

            var result = positionsController.InsertPosition(positionVM).Result;

            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public void InsertPositionAsync_ShouldReturnValidationError()
        {
            var positionsController = new PositionsController(_positionService, _mapper, _positionValidator);

            var positionVM = new PositionViewModel
            {
                Id = 12,
                Title = string.Empty
            };

            _mockPositionRepository.Setup(x => x.InsertAsync(It.IsAny<Position>()));

            var result = positionsController.InsertPosition(positionVM).Result;

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CanUpdatePosition()
        {
            var positionsController = new PositionsController(_positionService, _mapper, _positionValidator);

            var positionVM = new PositionViewModel
            {
                Id = 2,
                Title = "new title"
            };

            var id = 2;

            _mockPositionRepository.Setup(x => x.Update(It.IsAny<Position>()));

            var result = positionsController.UpdatePosition(id, positionVM);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void UpdatePosition_ReturnsBadRequest_DueToValidationError()
        {
            var positionsController = new PositionsController(_positionService, _mapper, _positionValidator);

            var positionVM = new PositionViewModel
            {
                Id = 2,
                Title = null
            };

            var id = 2;

            _mockPositionRepository.Setup(x => x.Update(It.IsAny<Position>()));

            var result = positionsController.UpdatePosition(id, positionVM);

            var badRequest = result as BadRequestObjectResult;

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(badRequest.Value.GetType() == typeof(List<ValidationFailure>));
        }

        [Fact]
        public void UpdatePosition_ReturnsBadRequest_DueToIdsMismatch()
        {
            var positionsController = new PositionsController(_positionService, _mapper, _positionValidator);

            var positionVM = new PositionViewModel
            {
                Id = 2,
                Title = "new title"
            };

            var id = 12;

            _mockPositionRepository.Setup(x => x.Update(It.IsAny<Position>()));

            var result = positionsController.UpdatePosition(id, positionVM);

            var badRequest = result as BadRequestObjectResult;

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Position id and passed id did not match", badRequest.Value);
        }

        [Fact]
        public void CanDeletePosition()
        {
            var id = 2;

            _mockPositionRepository.Setup(x => x.GetFirstOrDefaultAsync(
                x => x.Id == id, null, false).Result)
                .Returns(new Position() { Id = id });

            _mockPositionRepository.Setup(x => x.Delete(It.IsAny<Position>()));

            var positionsController = new PositionsController(_positionService, _mapper, _positionValidator);

            var result = positionsController.DeletePositionAsync(id).Result;

            Assert.IsType<OkObjectResult>(result);

        }

        [Fact]
        public void DeletePosition_WhenPositionDoesNotExist()
        {
            var id = 400;

            var positionsController = new PositionsController(_positionService, _mapper, _positionValidator);

            var result = positionsController.DeletePositionAsync(id).Result;

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CanDeleteRange()
        {
            var idArr = new List<int> { 1, 2, 3 };

            var positions = GetTestPositions().AsQueryable();

            _mockPositionRepository
                .Setup(x => x.GetAsync(It.IsAny<Expression<Func<Position, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<Position, bool>> filter,
                Func<IQueryable<Position>, IOrderedQueryable<Position>> orderBy,
                Func<IQueryable<Position>, IIncludableQueryable<Position, object>> include,
                bool asNoTracking) => positions.Where(filter).ToList());

            _mockPositionRepository.Setup(x => x.DeleteRange(It.IsAny<IEnumerable<Position>>()));

            var positionsController = new PositionsController(_positionService, _mapper, _positionValidator);

            var result = positionsController.DeletePositionsAsync(idArr).Result;

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void DeleteRange_WhenSomePositionWasNotFound()
        {
            var idArr = new List<int> { 1, 500, 3 };

            var positions = GetTestPositions().AsQueryable();

            _mockPositionRepository
                .Setup(x => x.GetAsync(It.IsAny<Expression<Func<Position, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<Position, bool>> filter,
                Func<IQueryable<Position>, IOrderedQueryable<Position>> orderBy,
                Func<IQueryable<Position>, IIncludableQueryable<Position, object>> include,
                bool asNoTracking) => positions.Where(filter).ToList());

            _mockPositionRepository.Setup(x => x.DeleteRange(It.IsAny<IEnumerable<Position>>()));

            var positionsController = new PositionsController(_positionService, _mapper, _positionValidator);

            var result = positionsController.DeletePositionsAsync(idArr).Result;

            var badRequest = result as BadRequestObjectResult;

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"{SomeEntitiesInCollectionNotFound} {nameof(Position)}s to delete", badRequest.Value);
        }
    }
}
