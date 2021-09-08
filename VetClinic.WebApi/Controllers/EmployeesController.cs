using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Errors.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Services;
using VetClinic.WebApi.Validators.EntityValidators;
using VetClinic.WebApi.ViewModels.EmployeeViewModels;
using static VetClinic.Core.Resources.TextMessages;

namespace VetClinic.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        private readonly EmployeeValidator _employeeValidator;
        public EmployeesController(IEmployeeService employeeService,
            IMapper mapper,
            EmployeeValidator employeeValidator)
        {
            _employeeService = employeeService;
            _mapper = mapper;
            _employeeValidator = employeeValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployeesAsync()
        {
            var employees = await _employeeService.GetEmployeesAsync();

            var employeeViewModel = _mapper.Map<IEnumerable<EmployeeViewModel>>(employees);

            return Ok(employeeViewModel);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeByIdAsync(string id)
        {
            try
            {
                var employee = await _employeeService.GetByIdAsync(id);

                var employeeViewModel = _mapper.Map<EmployeeViewModel>(employee);

                return Ok(employeeViewModel);
            }
            catch(NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertEmployeeAsync(EmployeeToAddViewModel model)
        {
            var newEmployee = _mapper.Map<Employee>(model);

            var validationResult = _employeeValidator.Validate(newEmployee);

            if (validationResult.IsValid)
            {
                await _employeeService.InsertAsync(newEmployee);
                return Ok(_mapper.Map<EmployeeViewModel>(newEmployee));
            }
            return BadRequest(validationResult.Errors);
        }

        [HttpPut]
        public IActionResult UpdateEmployee(string id, EmployeeViewModel model)
        {
            var employee = _mapper.Map<Employee>(model);

            var validationResult = _employeeValidator.Validate(employee);

            if (validationResult.IsValid)
            {
                try
                {
                    _employeeService.Update(id, employee);
                    return Ok();
                }
                catch(BadRequestException ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest(validationResult.Errors);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdatePatch(string id, [FromBody] JsonPatchDocument<Employee> employeeToUpdate)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            employeeToUpdate.ApplyTo(employee, ModelState);
            return Ok(_mapper.Map<EmployeeViewModel>(employee));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeAsync(string id)
        {
            try
            {
                await _employeeService.DeleteAsync(id);
                return Ok($"{nameof(Employee)} {EntityHasBeenDeleted}");
            }
            catch(NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEmployeesAsync([FromQuery(Name = "listOfIds")] IList<string> listOfIds)
        {
            await _employeeService.DeleteRangeAsync(listOfIds);
            return Ok();
        }
    }
}
