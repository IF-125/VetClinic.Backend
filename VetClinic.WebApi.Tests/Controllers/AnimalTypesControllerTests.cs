using AutoMapper;
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
using VetClinic.WebApi.ViewModels;
using Xunit;
using static VetClinic.Core.Resources.TextMessages;

namespace VetClinic.WebApi.Tests.Controllers
{
    public class AnimalTypesControllerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IAnimalTypeRepository> _mockAnimalTypeRepository;
        private readonly IAnimalTypeService _animalTypeService;

        private readonly AnimalTypesController _AnimalTypeController;

        public AnimalTypesControllerTests()
        {
            _mockAnimalTypeRepository = new Mock<IAnimalTypeRepository>();
            _animalTypeService = new AnimalTypeService(_mockAnimalTypeRepository.Object);


            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new AnimalTypeMapperProfile());
                });
                _mapper = mappingConfig.CreateMapper();
            }

            _AnimalTypeController = new AnimalTypesController(_animalTypeService, _mapper);
        }

        private List<AnimalType> GetTestAnimalTypes()
        {
            return new List<AnimalType>
            {
                new AnimalType
                {
                    Id=1,
                    Type="Dog1"
                },
                new AnimalType
                {
                    Id=2,
                    Type="Dog2"
                },
                 new AnimalType
                {
                    Id=3,
                    Type="Dog3"
                },
                new AnimalType
                {
                    Id=4,
                    Type="Dog4"
                }, new AnimalType
                {
                    Id=5,
                    Type="Dog5"
                },
                new AnimalType
                {
                    Id=6,
                    Type="Dog6"
                }, new AnimalType
                {
                    Id=7,
                    Type="Dog7"
                },
                new AnimalType
                {
                    Id=8,
                    Type="Dog8"
                },
                 new AnimalType
                {
                    Id=9,
                    Type="Dog9"
                },
                new AnimalType
                {
                    Id=10,
                    Type="Dog10"
                }
            };
        }

        [Fact]
        public void CanGetAllAnimalTypes()
        {
            // Arrange
            var AnimalTypes = GetTestAnimalTypes();
            _mockAnimalTypeRepository.Setup(x => x.GetAsync(null, null, null, true).Result).Returns(() => AnimalTypes);

            // Act
            var result = _AnimalTypeController.GetAllAnimalTypes().Result;

            // Assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<AnimalTypeViewModel>>(viewResult.Value);
            Assert.Equal(AnimalTypes.Count, model.Count());
        }


        [Fact]
        public void CanGetAnimalTypeById()
        {
            // Arrange
            var AnimalTypes = GetTestAnimalTypes().AsQueryable();
            var id = 10;
            var name = "Dog10";

            _mockAnimalTypeRepository.Setup(x => x.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<AnimalType, bool>>>(), null, false).Result)
                .Returns((Expression<Func<AnimalType, bool>> filter,
                Func<IQueryable<AnimalType>, IIncludableQueryable<AnimalType, object>> include,
                bool asNoTracking) => AnimalTypes.FirstOrDefault(filter));

            // Act
            var result = _AnimalTypeController.GetAnimalType(id).Result;

            // Assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<AnimalTypeViewModel>(viewResult.Value);

            Assert.Equal(id, model.Id);
            Assert.Equal(name, model.Type);
        }

        [Fact]
        public void GetAnimalTypeById_ReturnsNotFound()
        {
            // Arrange
            var id = -10;

            _mockAnimalTypeRepository.Setup(x => x.GetFirstOrDefaultAsync(null, null, false)
                .Result);

            // Act
            var result = _AnimalTypeController.GetAnimalType(id).Result;

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CanInsertAnimalTypeAsync()
        {
            // Arrange
            var newAnimalTypeVM = new AnimalTypeViewModel
            {
                Id=11,
                Type="Dog11"
            };

            _mockAnimalTypeRepository.Setup(x => x.InsertAsync(It.IsAny<AnimalType>()));

            // Act
            var result = _AnimalTypeController.InsertAnimalType(newAnimalTypeVM).Result;

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void InsertAnimalTypeAsync_ShouldReturnValidationError()
        {
            // Arrange
            var newAnimalTypeVM = new AnimalTypeViewModel
            {
                Id = 11
            };

            _mockAnimalTypeRepository.Setup(x => x.InsertAsync(It.IsAny<AnimalType>()));

            // Act
            var result = _AnimalTypeController.InsertAnimalType(newAnimalTypeVM).Result;

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CanUpdateAnimalType()
        {
            // Arrange
            var newAnimalTypeVM = new AnimalTypeViewModel
            {
                Id = 1,
                Type = "NewDog"
            };

            var id = 1;

            _mockAnimalTypeRepository.Setup(x => x.Update(It.IsAny<AnimalType>()));

            // Act
            var result = _AnimalTypeController.UpdateAnimalType(id, newAnimalTypeVM);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void UpdateAnimalType_ReturnBadRequest_DueToValidationError()
        {
            // Arrange
            var newAnimalTypeVM = new AnimalTypeViewModel
            {
                Id = 1
            };

            var id = 1;

            _mockAnimalTypeRepository.Setup(x => x.Update(It.IsAny<AnimalType>()));

            // Act
            var result = _AnimalTypeController.UpdateAnimalType(id, newAnimalTypeVM);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void UpdateAnimalType_ReturnBadRequest_DueToIdMismatch()
        {
            // Arrange
            var newAnimalTypeVM = new AnimalTypeViewModel
            {
                Id = 1,
                Type = "Dog1"
            };

            var id = 99;

            _mockAnimalTypeRepository.Setup(x => x.Update(It.IsAny<AnimalType>()));

            // Act
            var result = _AnimalTypeController.UpdateAnimalType(id, newAnimalTypeVM);


            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CanDeleteAnimalType()
        {
            // Arrange
            var id = 1;

            _mockAnimalTypeRepository.Setup(
                x => x.GetFirstOrDefaultAsync(x => x.Id == id, null, false).Result)
                .Returns(new AnimalType { Id = id });

            _mockAnimalTypeRepository.Setup(x => x.Delete(It.IsAny<AnimalType>()));

            // Act
            var result = _AnimalTypeController.DeleteAnimalType(id).Result;

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void DeleteAnimalType_WhetAnimalTypeDoesNotExist()
        {
            // Arrange
            var id = -10;

            _mockAnimalTypeRepository.Setup(x => x.GetFirstOrDefaultAsync(null, null, false)
               .Result);

            // Act
            var result = _AnimalTypeController.DeleteAnimalType(id).Result;

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CanDeleteRange()
        {
            // Arrange
            var AnimalTypes = GetTestAnimalTypes().AsQueryable();
            var listOfIds = new List<int> { 1, 2, 3, 4 };

            _mockAnimalTypeRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<AnimalType, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<AnimalType, bool>> filter,
                Func<IQueryable<AnimalType>, IOrderedQueryable<Employee>> orderBy,
                Func<IQueryable<AnimalType>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => AnimalTypes.Where(filter).ToList());

            _mockAnimalTypeRepository.Setup(x => x.DeleteRange(It.IsAny<IEnumerable<AnimalType>>()));

            // Act
            var result = _AnimalTypeController.DeleteAnimalTypes(listOfIds).Result;

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void DeleteRange_WhenSomeAnimalTypeNotFound()
        {
            // Arrange
            var AnimalTypes = GetTestAnimalTypes().AsQueryable();
            var listOfIds = new List<int> { 1, 2, -100, 4 };

            _mockAnimalTypeRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<AnimalType, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<AnimalType, bool>> filter,
                Func<IQueryable<AnimalType>, IOrderedQueryable<Employee>> orderBy,
                Func<IQueryable<AnimalType>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => AnimalTypes.Where(filter).ToList());

            _mockAnimalTypeRepository.Setup(x => x.DeleteRange(It.IsAny<IEnumerable<AnimalType>>()));

            // Act
            var result = _AnimalTypeController.DeleteAnimalTypes(listOfIds).Result;
            var badRequest = result as BadRequestObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"{SomeEntitiesInCollectionNotFound} {nameof(AnimalType)}s to delete", badRequest.Value);

        }
    }
}
