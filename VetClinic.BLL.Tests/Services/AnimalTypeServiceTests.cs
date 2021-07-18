using Microsoft.EntityFrameworkCore.Query;
using Moq;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VetClinic.BLL.Services;
using VetClinic.BLL.Tests.FakeData;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Repositories;
using Xunit;

namespace VetClinic.BLL.Tests.Services
{
    public class AnimalTypeServiceTests
    {
        private readonly AnimalTypeService _animalTypeServise;
        private readonly Mock<IAnimalTypeRepository> _animalTypeRepository = new Mock<IAnimalTypeRepository>();

        public AnimalTypeServiceTests()
        {
            _animalTypeServise = new AnimalTypeService(_animalTypeRepository.Object);
        }

        [Fact]
        public async Task CanReturnAllAnimalTypes()
        {
            // Arrange
            _animalTypeRepository.Setup(x => x.GetAsync(null, null, null, true).Result)
                .Returns(AnimalTypeFakeData.GetAnimalTypeFakeData());
            // Act
            IList<AnimalType> tempAnimalTypes = await _animalTypeServise.GetAnimalTypesAsync();

            // Assert
            Assert.NotNull(tempAnimalTypes);
            Assert.Equal(10, tempAnimalTypes.Count);
        }

        [Fact]
        public async Task CanReturnAnimalTypeById()
        {
            // Arrange
            var id = 10;
            var animalTypes = AnimalTypeFakeData.GetAnimalTypeFakeData().AsQueryable();
            _animalTypeRepository.Setup(x => x.GetFirstOrDefaultAsync(x => x.Id == id, null, false).Result)
                .Returns(animalTypes.FirstOrDefault(x => x.Id == id));

            // Act
            var animalType = await _animalTypeServise.GetByIdAsync(id);

            // Assert
            Assert.Equal("Dog10", animalType.Type);
        }

        [Fact]
        public void GetAnimalTypeById_ShouldReturnExeption()
        {
            // Arrange
            var id = 999;
            var AnimalTypes = AnimalTypeFakeData.GetAnimalTypeFakeData().AsQueryable();

            _animalTypeRepository.Setup(x => x.GetFirstOrDefaultAsync(
                x => x.Id == id, null, true).Result)
                .Returns(AnimalTypes.FirstOrDefault(x => x.Id == id));

            // Act,Assert
            Assert.Throws<AggregateException>(() => _animalTypeServise.GetByIdAsync(id).Result);

        }

        [Fact]
        public async Task CanInsertAnimalTypeAsync()
        {
            // Arrange
            var _newAnimalType = new AnimalType
            {
                Id = 1,
                Type = "Dog1"
            };

            _animalTypeRepository.Setup(x => x.InsertAsync(It.IsAny<AnimalType>()));

            // Act
            await _animalTypeServise.InsertAsync(_newAnimalType);

            // Assert
            _animalTypeRepository.Verify(x => x.InsertAsync(_newAnimalType));
        }

        [Fact]
        public void CanUpdateAnimalType()
        {
            // Arrange
            var _newAnimalType = new AnimalType
            {
                Id = 1,
                Type = "Dog1"
            };

            var id = 1;

            _animalTypeRepository.Setup(x => x.Update(It.IsAny<AnimalType>()));

            // Act
            _animalTypeServise.Update(id, _newAnimalType);

            // Assert
            _animalTypeRepository.Verify(x => x.Update(_newAnimalType));

        }

        [Fact]
        public void UpdateAnimalType_ThrowsExeption()
        {
            // Arrange
            var _newAnimalType = new AnimalType
            {
                Id = 1,
                Type = "Dog1"
            };

            var id = 999;

            _animalTypeRepository.Setup(x => x.Update(It.IsAny<AnimalType>()));

            // Act, Assert
            Assert.Throws<NotFoundException>(() => _animalTypeServise.Update(id, _newAnimalType));

        }

        [Fact]
        public async Task CanDeletAnimalTypeAsync()
        {
            // Arrange
            var id = 10;
            var AnimalTypes = AnimalTypeFakeData.GetAnimalTypeFakeData().AsQueryable();

            _animalTypeRepository.Setup(x => x.GetFirstOrDefaultAsync(x => x.Id == id, null, false).Result)
                .Returns(AnimalTypes.FirstOrDefault(x => x.Id == id));

            _animalTypeRepository.Setup(x => x.Delete(It.IsAny<AnimalType>())).Verifiable();

            // Act
            await _animalTypeServise.DeleteAsync(id);

            // Assert
            _animalTypeRepository.Verify(x => x.Delete(It.IsAny<AnimalType>()));
        }

        [Fact]
        public async Task DeleteAnimalType_ThrowExaption()
        {

            // Arrange
            var id = 999;
            var AnimalTypes = AnimalTypeFakeData.GetAnimalTypeFakeData().AsQueryable();

            _animalTypeRepository.Setup(x => x.GetFirstOrDefaultAsync(x => x.Id == id, null, false).Result)
                .Returns(AnimalTypes.FirstOrDefault(x => x.Id == id));

            _animalTypeRepository.Setup(x => x.Delete(It.IsAny<AnimalType>())).Verifiable();

            // Act,Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _animalTypeServise.DeleteAsync(id));

        }

        [Fact]
        public void CanDeleteRangeOfAnimalTypesAsync()
        {
            // Arrange
            var listOfIds = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var AnimalTypes = AnimalTypeFakeData.GetAnimalTypeFakeData().AsQueryable();

            _animalTypeRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<AnimalType, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<AnimalType, bool>> filter,
                Func<IQueryable<AnimalType>, IOrderedQueryable<Employee>> orderBy,
                Func<IQueryable<AnimalType>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => AnimalTypes.Where(filter).ToList());

            _animalTypeRepository.Setup(x => x.DeleteRange(It.IsAny<IEnumerable<AnimalType>>()));

            // Act
            _animalTypeServise.DeleteRangeAsync(listOfIds).Wait();

            // Assert
            _animalTypeRepository.Verify(x => x.DeleteRange(It.IsAny<IEnumerable<AnimalType>>()));
        }


        [Fact]
        public void DeleteAnimalTypesRangeAsync_ShouldReturnException_notValidId()
        {
            // Arrange
            var idArr = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 999, 1000 };
            var AnimalTypes = AnimalTypeFakeData.GetAnimalTypeFakeData().AsQueryable();

            // Act
            _animalTypeRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<AnimalType, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<AnimalType, bool>> filter,
                Func<IQueryable<AnimalType>, IOrderedQueryable<Employee>> orderBy,
                Func<IQueryable<AnimalType>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => AnimalTypes.Where(filter).ToList());

            _animalTypeRepository.Setup(x => x.DeleteRange(It.IsAny<IEnumerable<AnimalType>>()));

            // Assert
            Assert.Throws<AggregateException>(() => _animalTypeServise.DeleteRangeAsync(idArr).Wait());
        }
    }
}
