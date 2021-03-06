using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Configuration;
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
using VetClinic.Core.Interfaces.Services;
using Xunit;

namespace VetClinic.BLL.Tests.Services
{
    public class PetServiceTests
    {
        private readonly PetServise _petService;
        private readonly Mock<IPetRepository> _mockPetRepository = new Mock<IPetRepository>();
        private readonly Mock<IOrderProcedureRepository> _mockOrderProcedureRepository = new Mock<IOrderProcedureRepository>();
        private readonly Mock<IBlobService> _mockIblobService = new Mock<IBlobService>();
        private readonly Mock<IConfiguration> _mockConfiguration = new Mock<IConfiguration>();


        public PetServiceTests()
        {
            _petService = new PetServise(_mockPetRepository.Object, _mockIblobService.Object, _mockOrderProcedureRepository.Object, _mockConfiguration.Object);
        }

        [Fact]
        public async Task CanReturnAllPets()
        {
            // Arrange
            var numberOfPets = 10;
            _mockPetRepository.Setup(x => x.GetAsync(null, null,
               It.IsAny<Func<IQueryable<Pet>, IIncludableQueryable<Pet, object>>>(), true).Result)
                .Returns(PetFakeData.GetPetFakeData());
            // Act
            IList<Pet> tempPets = await _petService.GetPetsAsync();

            // Assert
            Assert.NotNull(tempPets);
            Assert.Equal(numberOfPets, tempPets.Count);
        }

        [Fact]
        public async Task CanReturnPetById()
        {
            // Arrange
            var id = 10;
            var name = "Lord10";
            var pets = PetFakeData.GetPetFakeData().AsQueryable();
            _mockPetRepository.Setup(x => x.GetFirstOrDefaultAsync(x => x.Id == id, It.IsAny<Func<IQueryable<Pet>, IIncludableQueryable<Pet, object>>>(), false).Result)
                .Returns(pets.FirstOrDefault(x => x.Id == id));
            
            // Act
            var pet = await _petService.GetByIdAsync(id);

            // Assert
            Assert.Equal(name, pet.Name);
        }

        [Fact]
        public void GetPetById_ShouldReturnExeption()
        {
            // Arrange
            var id = 999;
            var pets = PetFakeData.GetPetFakeData().AsQueryable();

            _mockPetRepository.Setup(x => x.GetFirstOrDefaultAsync(
                x => x.Id == id, null, true).Result)
                .Returns(pets.FirstOrDefault(x => x.Id == id));
            
            // Act,Assert
            Assert.Throws<AggregateException>(() => _petService.GetByIdAsync(id).Result);

        }

        [Fact] 
        public async Task   CanInsertPetAsync()
        {
            // Arrange
            var _newPet = new Pet
            {
                Id = 11,
                Name = "Lord11",
                Information = "Animal from the street11",
                Breed = "Foo",
                Age = 4
            };
            _mockPetRepository.Setup(x => x.InsertAsync(It.IsAny<Pet>()));

            // Act
            await _petService.InsertAsync(_newPet);

            // Assert
            _mockPetRepository.Verify(x => x.InsertAsync(_newPet));
        }

        [Fact]
        public void CanUpdatePet()
        {
            // Arrange
            var _newPet = new Pet
            {
                Id = 10,
                Name = "Lord10",
                Information = "Animal from the street10",
                Breed = "Persian",
                Age = 2
            };
            
            var id = 10;

            _mockPetRepository.Setup(x => x.Update(It.IsAny<Pet>()));

            // Act
            _petService.Update(id, _newPet);

            // Assert
            _mockPetRepository.Verify(x => x.Update(_newPet));

        }

        [Fact]
        public void UpdatePet_ThrowsExeption()
        {
            // Arrange
            var _newPet = new Pet
            {
                Id = 10,
                Name = "Lord10",
                Information = "Animal from the street10",
                Breed = "Persian",
                Age = 2
            };
            
            var id = 999;

            _mockPetRepository.Setup(x => x.Update(It.IsAny<Pet>()));

            // Act, Assert
            Assert.Throws<NotFoundException>(() => _petService.Update(id, _newPet));

        }

        [Fact]
        public async Task CanDeletPetAsync()
        {
            // Arrange
            var id = 10;
            var pets = PetFakeData.GetPetFakeData().AsQueryable();

            _mockPetRepository.Setup(x => x.GetFirstOrDefaultAsync(x => x.Id == id, It.IsAny<Func<IQueryable<Pet>, IIncludableQueryable<Pet, object>>>(), false).Result)
                .Returns(pets.FirstOrDefault(x => x.Id == id));

            _mockPetRepository.Setup(x => x.Delete(It.IsAny<Pet>())).Verifiable();

            // Act
            await _petService.DeleteAsync(id);

            // Assert
            _mockPetRepository.Verify(x => x.Delete(It.IsAny<Pet>()));
        }

        [Fact]
        public async Task DeletePet_ThrowExaption()
        {

            // Arrange
            var id = 999;
            var pets = PetFakeData.GetPetFakeData().AsQueryable();

            _mockPetRepository.Setup(x => x.GetFirstOrDefaultAsync(x => x.Id == id, It.IsAny<Func<IQueryable<Pet>, IIncludableQueryable<Pet, object>>>(), false).Result)
                .Returns(pets.FirstOrDefault(x => x.Id == id));

            _mockPetRepository.Setup(x => x.Delete(It.IsAny<Pet>())).Verifiable();

            // Act,Assert
            await Assert.ThrowsAsync<NullReferenceException>( () =>  _petService.DeleteAsync(id));

        }

        [Fact]
        public void  CanDeleteRangeOfPetsAsync()
        {
            // Arrange
            var listOfIds = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            
            var pets = PetFakeData.GetPetFakeData().AsQueryable();

            _mockPetRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Pet, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<Pet, bool>> filter,
                Func<IQueryable<Pet>, IOrderedQueryable<Employee>> orderBy,
                Func<IQueryable<Pet>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => pets.Where(filter).ToList());

            _mockPetRepository.Setup(x => x.DeleteRange(It.IsAny<IEnumerable<Pet>>()));

            // Act
            _petService.DeleteRangeAsync(listOfIds).Wait();

            // Assert
            _mockPetRepository.Verify(x => x.DeleteRange(It.IsAny<IEnumerable<Pet>>()));
        }


        [Fact]
        public void DeletePetsRangeAsync_ShouldReturnException_notValidId()
        {
            // Arrange
            var idArr = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 999, 1000 };
            var pets = PetFakeData.GetPetFakeData().AsQueryable();

            
            _mockPetRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Pet, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<Pet, bool>> filter,
                Func<IQueryable<Pet>, IOrderedQueryable<Employee>> orderBy,
                Func<IQueryable<Pet>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => pets.Where(filter).ToList());

            _mockPetRepository.Setup(x => x.DeleteRange(It.IsAny<IEnumerable<Pet>>()));

            // Act, Assert
            Assert.Throws<AggregateException>(() => _petService.DeleteRangeAsync(idArr).Wait());
        }

        [Fact]
        public void GetMedicalCardOfPet_ShouldReturnPetWithAssignOrderProcedure()
        {
            // Arrange
            var petId = 1;
            var pets = PetFakeData.GetPetFakeData().AsQueryable();

            _mockPetRepository.Setup(x => x.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Pet, bool>>>(), It.IsAny<Func<IQueryable<Pet>, IIncludableQueryable<Pet, object>>>(), true).Result)
                .Returns((Expression<Func<Pet, bool>> filter,
                Func<IQueryable<Pet>, IIncludableQueryable<Pet, object>> include,
                bool asNoTracking) => pets.Where(filter).FirstOrDefault());

            // Act
            var pet =  _petService.GetMedicalCardOfPetAsync(petId).Result;

            // Assert
            Assert.Equal(1, pet.OrderProcedures.Count);
        }

        [Fact]
        public void GetPetsToTreat_ShouldReturnPetToTreat()
        {
            // Arrange
            var orderProcedures = OrderProcedureFakeData.GetOrderProcedureFakeData().AsQueryable();
            var doctorId = "f1a05cca-b479-4f72-bbda-96b8979f4afe";
            var numberAssignedPetInFakeData = 2;

            _mockOrderProcedureRepository.Setup(x => x.GetAsync(
                It.IsAny<Expression<Func<OrderProcedure, bool>>>(), null,
                It.IsAny<Func<IQueryable<OrderProcedure>, IIncludableQueryable<OrderProcedure, object>>>(),
                false).Result)
                .Returns((Expression<Func<OrderProcedure, bool>> filter,
                        Func<IQueryable<OrderProcedure>, IOrderedQueryable<OrderProcedure>> orderBy,
                        Func<IQueryable<OrderProcedure>, IIncludableQueryable<OrderProcedure, object>> include,
                        bool asNoTracking) =>
                {
                    var orders = orderProcedures.Where(filter);
                    var inc= include(orders).ToList();
                    return inc;
                });

            // Act
            var pets = _petService.GetPetsToTreat(doctorId).Result;

            // Assert
            Assert.Equal(numberAssignedPetInFakeData, pets.Count());

        }



    }
}
