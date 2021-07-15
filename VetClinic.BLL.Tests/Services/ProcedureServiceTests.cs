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
    public class ProcedureServiceTests
    {
        private readonly ProcedureService _procedureService;
        private readonly Mock<IProcedureRepository> _procedureRepository = new Mock<IProcedureRepository>();
        public ProcedureServiceTests()
        {
            _procedureService = new ProcedureService(
                _procedureRepository.Object);
        }

        [Fact]
        public async Task CanReturnAllProcedures()
        {
            //arrange
            _procedureRepository.Setup(b => b.GetAsync(null, null, null, true).Result)
                .Returns(ProcedureFakeData.GetProcedureFakeData());
            //act
            IList<Procedure> Procedures = await _procedureService.GetProceduresAsync(null, null, null, asNoTracking: true);
            //assert
            Assert.NotNull(Procedures);
            Assert.Equal(10, Procedures.Count);
        }

        [Fact]
        public async Task CanReturnProcedureById()
        {
            //arrange
            int id = 4;

            var Procedures = ProcedureFakeData.GetProcedureFakeData().AsQueryable();

            _procedureRepository.Setup(b => b.GetFirstOrDefaultAsync(
                b => b.Id == id, null, true).Result)
                .Returns((Expression<Func<Procedure, bool>> filter,
                Func<IQueryable<Procedure>, IIncludableQueryable<Procedure, object>> include,
                bool asNoTracking) => Procedures.FirstOrDefault(filter));
            //act
            var Procedure = await _procedureService.GetByIdAsync(id);
            //assert
            Assert.Equal("Procedure 4", Procedure.Title);
        }

        [Fact]
        public void GetProcedureByInvalidId()
        {
            //arrange
            int id = 45;

            var Procedures = ProcedureFakeData.GetProcedureFakeData().AsQueryable();

            _procedureRepository.Setup(b => b.GetFirstOrDefaultAsync(
                b => b.Id == id, null, true).Result)
                .Returns((Expression<Func<Procedure, bool>> filter,
                Func<IQueryable<Procedure>, IIncludableQueryable<Procedure, object>> include,
                bool asNoTracking) => Procedures.FirstOrDefault(filter));
            //assert
            Assert.Throws<AggregateException>(() => _procedureService.GetByIdAsync(id).Result);
        }

        [Fact]
        public async Task CanInsertProcedure()
        {
            //arrange
            Procedure ProcedureToInsert = new Procedure
            {
                Id = 11,
                Title = "Procedure 11",
                Description = "Surgical procedure.",
                Duration = new TimeSpan(hours: 0, minutes: 40, seconds: 0),
                Price = 1550
            };

            _procedureRepository.Setup(b => b.InsertAsync(It.IsAny<Procedure>()));
            //act
            await _procedureService.InsertAsync(ProcedureToInsert);
            //assert
            _procedureRepository.Verify(b => b.InsertAsync(ProcedureToInsert));
        }

        [Fact]
        public void CanUpdateProcedure()
        {
            //arrange
            Procedure ProcedureToUpdate = new Procedure
            {
                Id = 11,
                Title = "Procedure 11",
                Description = "Surgical procedure.",
                Duration = new TimeSpan(hours: 0, minutes: 40, seconds: 0),
                Price = 1550
            };

            int id = 11;

            _procedureRepository.Setup(b => b.Update(It.IsAny<Procedure>()));
            //act
            _procedureService.Update(id, ProcedureToUpdate);
            //assert
            _procedureRepository.Verify(b => b.Update(ProcedureToUpdate));
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
                Duration = new TimeSpan(hours: 0, minutes: 40, seconds: 0),
                Price = 1550
            };

            int id = 10;

            _procedureRepository.Setup(b => b.Update(It.IsAny<Procedure>()));
            //assert
            Assert.Throws<ArgumentException>(() => _procedureService.Update(id, ProcedureToUpdate));
        }

        [Fact]
        public async Task CanDeleteProcedure()
        {
            //arrange
            var id = 10;

            _procedureRepository.Setup(b => b.GetFirstOrDefaultAsync(
                b => b.Id == id, null, false).Result)
                .Returns(new Procedure() { Id = id });

            _procedureRepository.Setup(b => b.Delete(It.IsAny<Procedure>())).Verifiable();
            //act
            await _procedureService.DeleteAsync(id);
            //assert
            _procedureRepository.Verify(b => b.Delete(It.IsAny<Procedure>()));
        }

        [Fact]
        public void DeleteProcedureByInvalidId()
        {
            //assert
            Assert.Throws<AggregateException>(() => _procedureService.DeleteAsync(100).Wait());
        }

        [Fact]
        public void CanDeleteRange()
        {
            //arrange
            List<int> ids = new List<int>() { 8, 9, 10 };

            var Procedures = ProcedureFakeData.GetProcedureFakeData().AsQueryable();

            _procedureRepository.Setup(b => b.GetAsync(It.IsAny<Expression<Func<Procedure, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<Procedure, bool>> filter,
                Func<IQueryable<Procedure>, IOrderedQueryable<Procedure>> ProcedureBy,
                Func<IQueryable<Procedure>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => Procedures.Where(filter).ToList());

            _procedureRepository.Setup(b => b.DeleteRange(It.IsAny<IEnumerable<Procedure>>()));
            //act
            _procedureService.DeleteRangeAsync(ids).Wait();
            //assert
            _procedureRepository.Verify(b => b.DeleteRange(It.IsAny<IEnumerable<Procedure>>()));
        }

        [Fact]
        public void DeleteRangeWithInvalidId()
        {
            //arrange
            List<int> ids = new List<int>() { 8, 9, 100 };

            var Procedures = ProcedureFakeData.GetProcedureFakeData().AsQueryable();

            _procedureRepository.Setup(b => b.GetAsync(It.IsAny<Expression<Func<Procedure, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<Procedure, bool>> filter,
                Func<IQueryable<Procedure>, IOrderedQueryable<Procedure>> ProcedureBy,
                Func<IQueryable<Procedure>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => Procedures.Where(filter).ToList());

            _procedureRepository.Setup(b => b.DeleteRange(It.IsAny<IEnumerable<Procedure>>()));
            //assert
            Assert.Throws<AggregateException>(() => _procedureService.DeleteRangeAsync(ids).Wait());
        }
    }
}
