using System.Collections.Generic;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Services.Base;

namespace VetClinic.Core.Interfaces.Services
{
    public interface IScheduleService : IBaseService<Schedule, int>
    {
        public Task InsertRangeAsync(IEnumerable<Schedule> schedule);

        public Task<IEnumerable<Schedule>> GetScheduleOfEmployee(string emoloyeeId);
    }
}
