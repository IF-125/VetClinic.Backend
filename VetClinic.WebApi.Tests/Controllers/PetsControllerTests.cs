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
    public class PetsControllerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IPetRepository> _mockPetRepository;
        private readonly IPetService _petService;

        private readonly PetsController _petController;

        public PetsControllerTests()
        {
            _mockPetRepository = new Mock<IPetRepository>();
            _petService = new PetServise(_mockPetRepository.Object);
            

            if (_mapper==null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new PetMapperProfile());
                });
                _mapper = mappingConfig.CreateMapper();
            }

            _petController = new PetsController(_petService,_mapper);
        }

        private List<Pet> GetTestPets()
        {
            return new List<Pet>
            {
                new Pet
                {
                    Id = 1,
                    Name = "Lord1",
                    Information = "Animal from the street1",
                    Breed = "Bebgal",
                    Age = 2
                },
                new Pet
                {
                    Id = 2,
                    Name = "Lord2",
                    Information = "Animal from the street2",
                    Breed = "Persian",
                    Age = 2
                },
                new Pet
                {
                    Id = 3,
                    Name = "Lord3",
                    Information = "Animal from the street3",
                    Breed = "Bebgal",
                    Age = 2
                },
                new Pet
                {
                    Id = 4,
                    Name = "Lord4",
                    Information = "Animal from the street4",
                    Breed = "Bebgal",
                    Age = 2
                },
                new Pet
                {
                    Id = 5,
                    Name = "Lord5",
                    Information = "Animal from the street5",
                    Breed = "Bebgal",
                    Age = 2
                },
                new Pet
                {
                    Id = 6,
                    Name = "Lord6",
                    Information = "Animal from the street6",
                    Breed = "Bebgal",
                    Age = 2
                },
                new Pet
                {
                    Id = 7,
                    Name = "Lord7",
                    Information = "Animal from the street7",
                    Breed = "Bebgal",
                    Age = 2
                },
                new Pet
                {
                    Id = 8,
                    Name = "Lord8",
                    Information = "Animal from the street8",
                    Breed = "Bebgal",
                    Age = 2
                },
                new Pet
                {
                    Id = 9,
                    Name = "Lord9",
                    Information = "Animal from the street9",
                    Breed = "Bebgal",
                    Age = 2
                },
                new Pet
                {
                    Id = 10,
                    Name = "Lord10",
                    Information = "Animal from the street10",
                    Breed = "Bebgal",
                    Age = 2
                }
            };
        }

        [Fact]
        public void CanGetAllPets()
        {
            // Arrange
            var pets = GetTestPets();
            _mockPetRepository.Setup(x => x.GetAsync(null, null, null, true).Result).Returns(() => pets);

            // Act
            var result = _petController.GetAllPets().Result;

            // Assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<PetViewModel>>(viewResult.Value);
            Assert.Equal(pets.Count, model.Count());
        }


        [Fact]
        public void CanGetPetById()
        {
            // Arrange
            var pets = GetTestPets().AsQueryable();
            var id = 10;
            var name = "Lord10";

            _mockPetRepository.Setup(x => x.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Pet, bool>>>(), null, false).Result)
                .Returns((Expression<Func<Pet, bool>> filter,
                Func<IQueryable<Pet>, IIncludableQueryable<Pet, object>> include,
                bool asNoTracking) => pets.FirstOrDefault(filter));

            // Act
            var result = _petController.GetPet(id).Result;

            // Assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<PetViewModel>(viewResult.Value);

            Assert.Equal(id, model.Id);
            Assert.Equal(name, model.Name); 
        }

        [Fact]
        public void GetPetById_ReturnsNotFound()
        {
            // Arrange
            var id = -10;

            _mockPetRepository.Setup(x => x.GetFirstOrDefaultAsync(null,null,false)
                .Result);

            // Act
            var result = _petController.GetPet(id).Result;

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CanInsertPetAsync()
        {
            // Arrange
            var newPetVM = new PetViewModel
            {
                Id = 11,
                Name = "Lord11",
                Information = "Animal from the street11",
                Breed = "Bebgal",
                Age = 2
            };

            _mockPetRepository.Setup(x => x.InsertAsync(It.IsAny<Pet>()));
            
            // Act
            var result = _petController.InsertPet(newPetVM).Result;

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void InsertPetAsync_ShouldReturnValidationError()
        {
            // Arrange
            var newPetVM = new PetViewModel
            {
                Id = 11,
                Name = "Lord11",
                Information = "Animal from the street11",
                Age = 2
            };

            _mockPetRepository.Setup(x => x.InsertAsync(It.IsAny<Pet>()));

            // Act
            var result = _petController.InsertPet(newPetVM).Result;

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CanUpdatePet()
        {
            // Arrange
            var newPetVM = new PetViewModel
            {
                Id = 1,
                Name = "Lord1",
                Information = "New Information",
                Breed = "Bebgal",
                Age = 2
            };

            var id = 1;

            _mockPetRepository.Setup(x => x.Update(It.IsAny<Pet>()));

            // Act
            var result = _petController.UpdatePet(id, newPetVM);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void UpdatePet_ReturnBadRequest_DueToValidationError()
        {
            // Arrange
            var newPetVM = new PetViewModel
            {
                Id = 1,
                Name = "Lord1",
                Information = "New Information",
                Breed = string.Empty,
                Age = 2
            };

            var id = 1;

            _mockPetRepository.Setup(x => x.Update(It.IsAny<Pet>()));

            // Act
            var result = _petController.UpdatePet(id, newPetVM);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void UpdatePet_ReturnBadRequest_DueToIdMismatch()
        {
            // Arrange
            var newPetVM = new PetViewModel
            {
                Id = 1,
                Name = "Lord1",
                Information = "New Information",
                Breed = "Good in here",
                Age = 2
            };

            var id = 99;

            _mockPetRepository.Setup(x => x.Update(It.IsAny<Pet>()));

            // Act
            var result = _petController.UpdatePet(id, newPetVM);


            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CanDeletePet()
        {
            // Arrange
            var id = 1;

            _mockPetRepository.Setup(
                x => x.GetFirstOrDefaultAsync(x => x.Id == id, null, false).Result)
                .Returns(new Pet { Id = id });

            _mockPetRepository.Setup(x => x.Delete(It.IsAny<Pet>()));

            // Act
            var result = _petController.DeletePet(id).Result;

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void DeletePet_WhetPetDoesNotExist()
        {
            // Arrange
            var id = -10;

            _mockPetRepository.Setup(x => x.GetFirstOrDefaultAsync(null, null, false)
               .Result);

            // Act
            var result = _petController.DeletePet(id).Result;

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public  void CanDeleteRange()
        {
            // Arrange
            var pets = GetTestPets().AsQueryable();
            var listOfIds = new List<int> { 1, 2, 3, 4 };

            _mockPetRepository.Setup(x=>x.GetAsync(It.IsAny<Expression<Func<Pet, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<Pet, bool>> filter,
                Func<IQueryable<Pet>, IOrderedQueryable<Employee>> orderBy,
                Func<IQueryable<Pet>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => pets.Where(filter).ToList());

            _mockPetRepository.Setup(x => x.DeleteRange(It.IsAny<IEnumerable<Pet>>()));
            
            // Act
            var result = _petController.DeletePets(listOfIds).Result;

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void DeleteRange_WhenSomePetNotFound()
        {
            // Arrange
            var pets = GetTestPets().AsQueryable();
            var listOfIds = new List<int> { 1, 2, -100, 4 };

            _mockPetRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Pet, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<Pet, bool>> filter,
                Func<IQueryable<Pet>, IOrderedQueryable<Employee>> orderBy,
                Func<IQueryable<Pet>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => pets.Where(filter).ToList());

            _mockPetRepository.Setup(x => x.DeleteRange(It.IsAny<IEnumerable<Pet>>()));

            // Act
            var result = _petController.DeletePets(listOfIds).Result;
            var badRequest = result as BadRequestObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"{SomeEntitiesInCollectionNotFound} {nameof(Pet)}s to delete", badRequest.Value);

        }
    }
}
