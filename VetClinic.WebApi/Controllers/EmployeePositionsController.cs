using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Errors.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Services;
using VetClinic.WebApi.Validators.EntityValidators;
using VetClinic.WebApi.ViewModels;
using static VetClinic.Core.Resources.TextMessages;

namespace VetClinic.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeePositionsController : ControllerBase
    {
        private readonly IEmployeePositionService _employeePositionService;
        private readonly IMapper _mapper;
        private readonly EmployeePositionValidator _employeePositionValidator;
        public EmployeePositionsController(IEmployeePositionService employeePositionService,
            IMapper mapper,
            EmployeePositionValidator employeePositionValidator)
        {
            _employeePositionService = employeePositionService;
            _mapper = mapper;
            _employeePositionValidator = employeePositionValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployeePositions()
        {
            var employeePositions = await _employeePositionService.GetEmployeePositionsAsync();

            var employeeViewModel = _mapper.Map<IEnumerable<EmployeePositionViewModel>>(employeePositions);

            return Ok(employeeViewModel);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeePosition(int id)
        {
            try
            {
                var employeePosition = await _employeePositionService.GetByIdAsync(id);

                var model = _mapper.Map<EmployeePositionViewModel>(employeePosition);

                return Ok(model);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AssignPositionToEmployee(EmployeePositionViewModel model)
        {
            var employeePosition = _mapper.Map<EmployeePosition>(model);

            var validationResult = _employeePositionValidator.Validate(employeePosition);

            if (validationResult.IsValid)
            {
                try
                {
                    await _employeePositionService.AssignPositionToEmployeeAsync(employeePosition);
                    return Ok();
                }
                catch (NotFoundException ex)
                {
                    return NotFound(ex.Message);
                }
            }
            return BadRequest(validationResult.Errors);
        }

        [HttpPut]
        public IActionResult UpdateEmployeePosition(int id, EmployeePositionViewModel model)
        {
            var employeePosition = _mapper.Map<EmployeePosition>(model);

            var validationResult = _employeePositionValidator.Validate(employeePosition);

            if (validationResult.IsValid)
            {
                try
                {
                    _employeePositionService.Update(id, employeePosition);
                    return Ok();
                }
                catch (BadRequestException ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest(validationResult.Errors);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdatePatch(int id, [FromBody] JsonPatchDocument<EmployeePosition> employeeToUpdate)
        {
            try
            {
                var employeePosition = await _employeePositionService.GetByIdAsync(id);
                employeeToUpdate.ApplyTo(employeePosition, ModelState);
                return Ok(employeePosition);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeePosition(int id)
        {
            try
            {
                await _employeePositionService.DeleteAsync(id);
                return Ok($"{nameof(EmployeePosition)} {EntityHasBeenDeleted}");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEmployeePositions([FromQuery(Name = "listOfIds")] IList<int> listOfIds)
        {
            try
            {
                await _employeePositionService.DeleteRangeAsync(listOfIds);
                return Ok();
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
