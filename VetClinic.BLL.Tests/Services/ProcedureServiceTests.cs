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
            _procedureRepository.Setup(b => b.GetAsync(null, null, null, true).Result)
                .Returns(ProcedureFakeData.GetProcedureFakeData());

            IList<Procedure> Procedures = await _procedureService.GetProceduresAsync(null, null, null, asNoTracking: true);

            Assert.NotNull(Procedures);
            Assert.Equal(10, Procedures.Count);
        }

        [Fact]
        public async Task CanReturnProcedureById()
        {
            int id = 4;

            var Procedures = ProcedureFakeData.GetProcedureFakeData().AsQueryable();

            _procedureRepository.Setup(b => b.GetFirstOrDefaultAsync(
                b => b.Id == id, null, true).Result)
                .Returns((Expression<Func<Procedure, bool>> filter,
                Func<IQueryable<Procedure>, IIncludableQueryable<Procedure, object>> include,
                bool asNoTracking) => Procedures.FirstOrDefault(filter));

            var Procedure = await _procedureService.GetByIdAsync(id);

            Assert.Equal("Procedure 4", Procedure.Title);
        }

        [Fact]
        public void GetProcedureByInvalidId()
        {
            int id = 45;

            var Procedures = ProcedureFakeData.GetProcedureFakeData().AsQueryable();

            _procedureRepository.Setup(b => b.GetFirstOrDefaultAsync(
                b => b.Id == id, null, true).Result)
                .Returns((Expression<Func<Procedure, bool>> filter,
                Func<IQueryable<Procedure>, IIncludableQueryable<Procedure, object>> include,
                bool asNoTracking) => Procedures.FirstOrDefault(filter));

            Assert.Throws<AggregateException>(() => _procedureService.GetByIdAsync(id).Result);
        }

        [Fact]
        public async Task CanInsertProcedure()
        {
            Procedure ProcedureToInsert = new Procedure
            {
                Id = 11,
                Title = "Procedure 11",
                Description = "Surgical procedure.",
                Duration = new TimeSpan(hours: 0, minutes: 40, seconds: 0),
                Price = 1550
            };

            _procedureRepository.Setup(b => b.InsertAsync(It.IsAny<Procedure>()));

            await _procedureService.InsertAsync(ProcedureToInsert);

            _procedureRepository.Verify(b => b.InsertAsync(ProcedureToInsert));
        }

        [Fact]
        public void CanUpdateProcedure()
        {
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

            _procedureService.Update(id, ProcedureToUpdate);

            _procedureRepository.Verify(b => b.Update(ProcedureToUpdate));
        }

        [Fact]
        public void UpdateProcedure_InvalidId()
        {
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

            Assert.Throws<ArgumentException>(() => _procedureService.Update(id, ProcedureToUpdate));
        }

        [Fact]
        public async Task CanDeleteProcedure()
        {
            var id = 10;

            _procedureRepository.Setup(b => b.GetFirstOrDefaultAsync(
                b => b.Id == id, null, false).Result)
                .Returns(new Procedure() { Id = id });

            _procedureRepository.Setup(b => b.Delete(It.IsAny<Procedure>())).Verifiable();

            await _procedureService.DeleteAsync(id);

            _procedureRepository.Verify(b => b.Delete(It.IsAny<Procedure>()));
        }

        [Fact]
        public void DeleteProcedureByInvalidId()
        {
            Assert.Throws<AggregateException>(() => _procedureService.DeleteAsync(100).Wait());
        }

        [Fact]
        public void CanDeleteRange()
        {
            int[] ids = new int[] { 8, 9, 10 };

            var Procedures = ProcedureFakeData.GetProcedureFakeData().AsQueryable();

            _procedureRepository.Setup(b => b.GetAsync(It.IsAny<Expression<Func<Procedure, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<Procedure, bool>> filter,
                Func<IQueryable<Procedure>, IOrderedQueryable<Procedure>> ProcedureBy,
                Func<IQueryable<Procedure>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => Procedures.Where(filter).ToList());

            _procedureRepository.Setup(b => b.DeleteRange(It.IsAny<IEnumerable<Procedure>>()));

            _procedureService.DeleteRangeAsync(ids).Wait();

            _procedureRepository.Verify(b => b.DeleteRange(It.IsAny<IEnumerable<Procedure>>()));
        }

        [Fact]
        public void DeleteRangeWithInvalidId()
        {
            int[] ids = new int[] { 8, 9, 100 };

            var Procedures = ProcedureFakeData.GetProcedureFakeData().AsQueryable();

            _procedureRepository.Setup(b => b.GetAsync(It.IsAny<Expression<Func<Procedure, bool>>>(), null, null, false).Result)
                .Returns((Expression<Func<Procedure, bool>> filter,
                Func<IQueryable<Procedure>, IOrderedQueryable<Procedure>> ProcedureBy,
                Func<IQueryable<Procedure>, IIncludableQueryable<Employee, object>> include,
                bool asNoTracking) => Procedures.Where(filter).ToList());

            _procedureRepository.Setup(b => b.DeleteRange(It.IsAny<IEnumerable<Procedure>>()));

            Assert.Throws<AggregateException>(() => _procedureService.DeleteRangeAsync(ids).Wait());
        }
    }
}
