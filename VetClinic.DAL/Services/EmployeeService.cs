using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Repositories.Base;
using VetClinic.Core.Interfaces.Services;
using VetClinic.DAL.Services.Base;

namespace VetClinic.DAL.Services
{
    public class EmployeeService : BaseService<Employee>, IEmployeeService
    {
        public EmployeeService(IRepository<Employee> repository) : base(repository)
        {
         
        }
    }
}
