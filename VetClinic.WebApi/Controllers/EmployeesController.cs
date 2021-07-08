using static VetClinic.Core.Resources.TextMessages;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Services;
using VetClinic.WebApi.Validators.EntityValidators;
using VetClinic.WebApi.ViewModels;
using Microsoft.AspNetCore.JsonPatch;

namespace VetClinic.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        public EmployeesController(IEmployeeService employeeService, IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployeesAsync()
        {
            var employees = await _employeeService.GetEmployees(asNoTracking: true);
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeByIdAsync(string id)
        {
            try
            {
                var employee = await _employeeService.GetByIdAsync(id);
                return Ok(employee);
            }
            catch(ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertEmployeeAsync(EmployeeViewModel model)
        {
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

        [HttpPut]
        public IActionResult Update(string id, EmployeeViewModel model)
        {
            var employee = _mapper.Map<Employee>(model);

            var validator = new EmployeeValidator();
            var validationResult = validator.Validate(employee);

            if (validationResult.IsValid)
            {
                try
                {
                    _employeeService.Update(id, employee);
                    return Ok();
                }
                catch(ArgumentException ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest(validationResult.Errors);
        }

        [HttpPatch("{id:string}")]
        public async Task<IActionResult> UpdatePatch(string id, [FromBody] JsonPatchDocument<Employee> employeeToUpdate)
        {
            try
            {
                var employee = await _employeeService.GetByIdAsync(id);
                employeeToUpdate.ApplyTo(employee, ModelState);
                return Ok(employee);
            }
            catch(ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            try
            {
                await _employeeService.DeleteAsync(id);
                return Ok($"{nameof(Employee)} {EntityHasBeenDeleted}");
            }
            catch(ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEmployees([FromQuery(Name = "idArr")] string[] idArr)
        {
            try
            {
                await _employeeService.DeleteRangeAsync(idArr);
                return Ok();
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
