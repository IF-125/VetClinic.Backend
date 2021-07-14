using System.Collections.Generic;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Services.Base;

namespace VetClinic.Core.Interfaces.Services
{
    public interface ISalaryService : IBaseService<Salary, int>
    {
        public Task<IEnumerable<Salary>> GetSalariesOfEmployee(int employeePositionId);
        public Task AssignSalaryToEmployee(string employeeId, Salary salary);
    }
}
