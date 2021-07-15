using System.Collections.Generic;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Services.Base;

namespace VetClinic.Core.Interfaces.Services
{
    public interface IEmployeePositionService : IBaseService<EmployeePosition, int>
    {
        public Task AssignPositionToEmployeeAsync(EmployeePosition employeePosition);

        public Task<IList<EmployeePosition>> GetEmployeePositionsAsync();
    }
}
