using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Services;
using VetClinic.WebApi.Validators.EntityValidators;
using VetClinic.WebApi.ViewModels;

namespace VetClinic.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        public EmployeeController(IEmployeeService employeeService, IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
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
            if (employee != null)
            {
                return Ok(employee);
            }
            return NotFound();
        }

        //TODO: Add update method

        [HttpPost]
        public async Task<IActionResult> InsertEmployeeAsync(EmployeeViewModel model)
        {
            //TODO: Consider doing mapping in services, not in controllers
            var newEmployee = _mapper.Map<Employee>(model);

            var validator = new EmployeeValidator();
            var validationResult = validator.Validate(newEmployee);

            if (validationResult.IsValid)
            {
                await _employeeService.InsertAsync(newEmployee);
                return CreatedAtAction("InsertEmployeeAsync", new { id = newEmployee.Id }, newEmployee);
            }
            return BadRequest(validationResult.Errors);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            var employeeToDelete = await _employeeService.GetFirstOrDefaultAsync(x => x.Id == id);
            //throw exceptions in service when user was not found while delete
            if (employeeToDelete != null)
            {
                //TODO: Consider using class with constants to pass text for responses 
                _employeeService.Delete(employeeToDelete);
                return Ok("The employee has been deleted");
            }
            return NotFound("Employee was not found");
        }
    }
}
