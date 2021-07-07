using VetClinic.BLL.Services.Base;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Repositories.Base;
using VetClinic.Core.Interfaces.Services;

namespace VetClinic.BLL.Services
{
    public class EmployeeService : BaseService<Employee>, IEmployeeService
    {
        public EmployeeService(IRepository<Employee> repository) : base(repository)
        {

        }
    }
}
