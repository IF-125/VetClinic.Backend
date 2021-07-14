using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Services.Base;

namespace VetClinic.Core.Interfaces.Services
{
    public interface IAppointmentService : IBaseService<Appointment, int>
    {
        public Task<IList<Appointment>> GetAppointmentsAsync(
            Expression<Func<Appointment, bool>> filter = null,
            Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>> orderBy = null,
            Func<IQueryable<Appointment>, IIncludableQueryable<Appointment, object>> include = null,
            bool asNoTracking = false);
    }
}
