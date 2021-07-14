using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Repositories;
using VetClinic.DAL.Context;
using VetClinic.DAL.Repositories.Base;

namespace VetClinic.DAL.Repositories
{
    public class ScheduleRepository : Repository<Schedule>, IScheduleRepository
    {
        public ScheduleRepository(VetClinicDbContext context) : base(context)
        {
        }
    }
}
