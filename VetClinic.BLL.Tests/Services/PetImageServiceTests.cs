using Microsoft.EntityFrameworkCore.Query;
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
using Xunit;

namespace VetClinic.BLL.Tests.Services
{
    public class PetImageServiceTests
    {
        private readonly PetImageService _petImageService;
        private readonly Mock<IPetImageRepository> _mockPetImageRepository = new Mock<IPetImageRepository>();
        private readonly IQueryable<PetImage> _petImages;

        public PetImageServiceTests()
        {
            _petImageService = new PetImageService(_mockPetImageRepository.Object);
            _petImages = PetImageFakeData.GetImageFakeData().AsQueryable();
        }

        [Fact]
        public async Task GetPetImagesAsync_CanReturnAllImages_IfThereIsImagesAsync()
        {
            // Arrange
            var numberOfPetImages = 10;
            _mockPetImageRepository.Setup(x => x.GetAsync(null, null, null, true).Result)
                .Returns(_petImages.ToList);

            // Act
            IList<PetImage> petImages = await _petImageService.GetPetImagesAsync();

            // Assert
            Assert.NotNull(petImages);
            Assert.Equal(numberOfPetImages, petImages.Count);
        }

        [Fact]
        public async Task GetPetImagesByPetId_ReturnsPetImages_IfIdIsCorrect()
        {
            //Arrange
            var petId =1;
            var expectNumberOfPetImages = 2;
            _mockPetImageRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<PetImage, bool>>>(),null,null,true).Result)
                .Returns((Expression < Func<PetImage, bool> > filter,
            Func<IQueryable<PetImage>, IOrderedQueryable<PetImage>> orderBy ,
            Func<IQueryable<PetImage>, IIncludableQueryable<PetImage, object>> include,
            bool asNoTracking)=> _petImages.Where(filter).ToList());

            //Act
            var petImage = await _petImageService.GetPetImagesByPetId(petId);

            //Assert
            Assert.Equal(expectNumberOfPetImages, petImage.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnPetImage_IfIdCorrect()
        {
            //Arrange
            var petImageId = 1;
            _mockPetImageRepository.Setup(x => x.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<PetImage, bool>>>(), null, false).Result)
                .Returns((Expression<Func<PetImage, bool>> filter,
            Func<IQueryable<PetImage>, IIncludableQueryable<PetImage, object>> include,
            bool asNoTracking) => _petImages.FirstOrDefault(filter));

            //Act
            var petImage = _petImageService.GetByIdAsync(petImageId);

            //Assert
            Assert.Equal(petImageId, petImage.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ThrowExeption_IfIdNotExist()
        {
            //Arrange
            var petImageId = 999;
            _mockPetImageRepository.Setup(x => x.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<PetImage, bool>>>(), null, false).Result)
                .Returns((Expression<Func<PetImage, bool>> filter,
            Func<IQueryable<PetImage>, IIncludableQueryable<PetImage, object>> include,
            bool asNoTracking) => _petImages.FirstOrDefault(filter));

            //Act, Assert
            Assert.Throws<AggregateException > (() => _petImageService.GetByIdAsync(petImageId).Result);
        }

        [Fact]
        public async Task InsertAsync_CanInsert()
        {
            //Arrange
            var newPetImage = new PetImage
            {
                PetId = 11
            };

            _mockPetImageRepository.Setup(x => x.InsertAsync(It.IsAny<PetImage>()));

            //Act
            await _petImageService.InsertAsync(newPetImage);

            //Assert
            _mockPetImageRepository.Verify(x => x.InsertAsync(newPetImage));
        }

        [Fact]
        public async Task InsertAsyncWithId_ReturnsPetImageWithPetId()
        {
            //Arrange
            var randomPetImageId = 111;
            var newPetImage = new PetImage
            {
                PetId = 11
            };

            _mockPetImageRepository.Setup(x => x.InsertAsync(It.IsAny<PetImage>())).Callback((PetImage petImage)=> {
                petImage.Id = randomPetImageId; 
            });

            //Act
            await _petImageService.InsertAsyncWithId(newPetImage);

            //Assert
            Assert.Equal(randomPetImageId, newPetImage.Id);
        }

        [Fact]
        public void Update_CanUpdateImage_IfIdCorrect()
        {
            //Arrange
            var updatedPetImage = new PetImage
            {
                Id=1,
                PetId = 999
            };
            var id = 1;

            _mockPetImageRepository.Setup(x => x.Update(It.IsAny<PetImage>()));

            //Act
            _petImageService.Update(id, updatedPetImage);

            //Assert
            _mockPetImageRepository.Verify(x => x.Update(updatedPetImage));
        }

        [Fact]
        public void Update_ThrowExeption_IfIdNotCorrect()
        {
            //Arrange
            var updatedPetImage = new PetImage
            {
                Id = 1,
                PetId = 999
            };
            var id = 2;

            _mockPetImageRepository.Setup(x => x.Update(It.IsAny<PetImage>()));

            //Act,Assert
            Assert.Throws<NotFoundException>(() => _petImageService.Update(id, updatedPetImage));
        }


        [Fact]
        public async Task DeleteAsync_CanDelete_IfIdCorrect()
        {
            //Arrange
            var id = 1;

            _mockPetImageRepository.Setup(x => x.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<PetImage, bool>>>(), null, false).Result)
                .Returns((Expression<Func<PetImage, bool>> filter,
            Func<IQueryable<PetImage>, IIncludableQueryable<PetImage, object>> include,
            bool asNoTracking) => _petImages.FirstOrDefault(filter));

            _mockPetImageRepository.Setup(x => x.Delete(It.IsAny<PetImage>()));

            //Act
            await _petImageService.DeleteAsync(id);

            //Assert
            _mockPetImageRepository.Verify(x => x.Delete(It.IsAny<PetImage>()));
        }

        [Fact]
        public async Task DeleteAsync_ThrowExaption_IfIdNotCorrect()
        {
            //Arrange
            var notCorretId = 999;

            _mockPetImageRepository.Setup(x => x.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<PetImage, bool>>>(), null, false).Result)
                .Returns((Expression<Func<PetImage, bool>> filter,
            Func<IQueryable<PetImage>, IIncludableQueryable<PetImage, object>> include,
            bool asNoTracking) => _petImages.FirstOrDefault(filter));

            _mockPetImageRepository.Setup(x => x.Delete(It.IsAny<PetImage>()));

            //Act, Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _petImageService.DeleteAsync(notCorretId));
        }

        [Fact]
        public async Task DeleteRangeAsync_DeleteRange_IfIdListIsCorrect()
        {
            //Arrange
            var IdArr = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            _mockPetImageRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<PetImage, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<PetImage, bool>> filter,
            Func<IQueryable<PetImage>, IOrderedQueryable<PetImage>> orderBy,
            Func<IQueryable<PetImage>, IIncludableQueryable<PetImage, object>> include,
            bool asNoTracking) => _petImages.Where(filter).ToList());

            _mockPetImageRepository.Setup(x => x.DeleteRange(It.IsAny<IList<PetImage>>()));

            //Act
            _petImageService.DeleteRangeAsync(IdArr);

            //Assert
            _mockPetImageRepository.Verify(x => x.DeleteRange(It.IsAny<IList<PetImage>>()));
        }

        [Fact]
        public async Task DeleteRangeAsync_ThrowExeption_IfIdListIsNotCorrect()
        {
            //Arrange
            var IdArr = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 999 };
            _mockPetImageRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<PetImage, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<PetImage, bool>> filter,
            Func<IQueryable<PetImage>, IOrderedQueryable<PetImage>> orderBy,
            Func<IQueryable<PetImage>, IIncludableQueryable<PetImage, object>> include,
            bool asNoTracking) => _petImages.Where(filter).ToList());

            _mockPetImageRepository.Setup(x => x.DeleteRange(It.IsAny<IList<PetImage>>()));

            //Act, Assert
            Assert.Throws<AggregateException>(() => _petImageService.DeleteRangeAsync(IdArr).Wait());
        }

    }
}
