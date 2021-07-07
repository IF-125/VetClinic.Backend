using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VetClinic.Core.Interfaces.Services;

namespace VetClinic.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployeesAsync()
        {
            var employees = await _employeeService.GetAsync(asNoTracking: true);
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeByIdAsync(string id)
        {
            var employee = await _employeeService.GetFirstOrDefaultAsync(x => x.Id == id);
            if(employee != null)
            {
                return Ok(employee);
            }
            return NotFound();
        }

        //TODO: finish implementing base employee controller's methods
    }
}
