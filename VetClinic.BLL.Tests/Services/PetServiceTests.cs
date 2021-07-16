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
    public class PetServiceTests
    {
        private readonly PetServise _petServise;
        private readonly Mock<IPetRepository> _petRepository = new Mock<IPetRepository>();

        public PetServiceTests()
        {
            _petServise = new PetServise(_petRepository.Object);
        }

        [Fact]
        public async Task CanReturnAllPets()
        {
            // Arrange
            _petRepository.Setup(x => x.GetAsync(null, null, null, true).Result)
                .Returns(PetFakeData.GetPetFakeData());
            // Act
            IList<Pet> tempPets = await _petServise.GetPetsAsync();

            // Assert
            Assert.NotNull(tempPets);
            Assert.Equal(10,tempPets.Count);
        }

        [Fact]
        public async Task CanReturnPetById()
        {
            // Arrange
            var id = 10;
            var pets = PetFakeData.GetPetFakeData().AsQueryable();
            _petRepository.Setup(x => x.GetFirstOrDefaultAsync(x => x.Id == id, null, false).Result)
                .Returns(pets.FirstOrDefault(x => x.Id == id));
            
            // Act
            var pet = await _petServise.GetByIdAsync(id);

            // Assert
            Assert.Equal("Lord10", pet.Name);
        }

        [Fact]
        public void GetPetById_ShouldReturnExeption()
        {
            // Arrange
            var id = 999;
            var pets = PetFakeData.GetPetFakeData().AsQueryable();

            _petRepository.Setup(x => x.GetFirstOrDefaultAsync(
                x => x.Id == id, null, true).Result)
                .Returns(pets.FirstOrDefault(x => x.Id == id));
            
            // Act,Assert
            Assert.Throws<AggregateException>(() => _petServise.GetByIdAsync(id).Result);

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
            _petRepository.Setup(x => x.InsertAsync(It.IsAny<Pet>()));

            // Act
            await _petServise.InsertAsync(_newPet);

            // Assert
            _petRepository.Verify(x => x.InsertAsync(_newPet));
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

            _petRepository.Setup(x => x.Update(It.IsAny<Pet>()));

            // Act
            _petServise.Update(id, _newPet);

            // Assert
            _petRepository.Verify(x => x.Update(_newPet));

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

            _petRepository.Setup(x => x.Update(It.IsAny<Pet>()));

            // Act, Assert
            Assert.Throws<ArgumentException>(() => _petServise.Update(id, _newPet));

        }

        [Fact]
        public async Task CanDeletPetAsync()
        {
            // Arrange
            var id = 10;
            var pets = PetFakeData.GetPetFakeData().AsQueryable();

            _petRepository.Setup(x => x.GetFirstOrDefaultAsync(x => x.Id == id, null, false).Result)
                .Returns(pets.FirstOrDefault(x => x.Id == id));

            _petRepository.Setup(x => x.Delete(It.IsAny<Pet>())).Verifiable();

            // Act
            await _petServise.DeleteAsync(id);

            // Assert
            _petRepository.Verify(x => x.Delete(It.IsAny<Pet>()));
        }

        [Fact]
        public async Task DeletePet_ThrowExaption()
        {

            // Arrange
            var id = 999;
            var pets = PetFakeData.GetPetFakeData().AsQueryable();

            _petRepository.Setup(x => x.GetFirstOrDefaultAsync(x => x.Id == id, null, false).Result)
                .Returns(pets.FirstOrDefault(x => x.Id == id));

            _petRepository.Setup(x => x.Delete(It.IsAny<Pet>())).Verifiable();

            // Act,Assert
            await Assert.ThrowsAsync<NullReferenceException>( () =>  _petServise.DeleteAsync(id));

        }

        [Fact]
        public void  CanDeleteRangeOfPetsAsync()
        {
            // Arrange
            var listOfIds = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            
            var pets = PetFakeData.GetPetFakeData().AsQueryable();

            _petRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Pet, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<Pet, bool>> filter,
                Func<IQueryable<Pet>, IOrderedQueryable<Employee>> orderBy,
                Func<IQueryable<Pet>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => pets.Where(filter).ToList());

            _petRepository.Setup(x => x.DeleteRange(It.IsAny<IEnumerable<Pet>>()));

            // Act
            _petServise.DeleteRangeAsync(listOfIds).Wait();

            // Assert
            _petRepository.Verify(x => x.DeleteRange(It.IsAny<IEnumerable<Pet>>()));
        }


        [Fact]
        public void DeletePetsRangeAsync_ShouldReturnException_notValidId()
        {
            // Arrange
            var idArr = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 999, 1000 };
            var pets = PetFakeData.GetPetFakeData().AsQueryable();

            // Act
            _petRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Pet, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<Pet, bool>> filter,
                Func<IQueryable<Pet>, IOrderedQueryable<Employee>> orderBy,
                Func<IQueryable<Pet>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => pets.Where(filter).ToList());

            _petRepository.Setup(x => x.DeleteRange(It.IsAny<IEnumerable<Pet>>()));

            // Assert
            Assert.Throws<AggregateException>(() => _petServise.DeleteRangeAsync(idArr).Wait());
        }
    }
}
