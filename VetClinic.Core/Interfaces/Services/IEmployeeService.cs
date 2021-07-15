using System.Collections.Generic;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Services.Base;

namespace VetClinic.Core.Interfaces.Services
{
    public interface IEmployeeService : IBaseService<Employee, string>
    {
        public Task<IList<Employee>> GetEmployeesAsync();
    }
}
