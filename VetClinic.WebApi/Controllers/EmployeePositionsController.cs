using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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
        public EmployeePositionsController(IEmployeePositionService employeePositionService, IMapper mapper)
        {
            _employeePositionService = employeePositionService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployeePositionsAsync()
        {
            var employeePositions = await _employeePositionService.GetEmployeePositionsAsync(
                include: x => x
                    .Include(e => e.Employee)
                    .Include(p => p.Position),
                asNoTracking: true);

            var employeeViewModel = _mapper.Map<IEnumerable<EmployeePositionViewModel>>(employeePositions);

            return Ok(employeeViewModel);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeePositionByIdAsync(int id)
        {
            try
            {
                var employeePosition = await _employeePositionService.GetByIdAsync(id);

                var model = _mapper.Map<EmployeePositionViewModel>(employeePosition);

                return Ok(model);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AssignPositionToEmployeeAsync(EmployeePositionViewModel model)
        {
            var employeePosition = _mapper.Map<EmployeePosition>(model);

            var validator = new EmployeePositionValidator();
            var validationResult = validator.Validate(employeePosition);

            if (validationResult.IsValid)
            {
                try
                {
                    await _employeePositionService.AssignPositionToEmployeeAsync(employeePosition);
                }
                catch (ArgumentException ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest(validationResult.Errors);
        }

        [HttpPut]
        public IActionResult UpdateEmployeePosition(int id, EmployeePositionViewModel model)
        {
            var employeePosition = _mapper.Map<EmployeePosition>(model);

            var validator = new EmployeePositionValidator();
            var validationResult = validator.Validate(employeePosition);

            if (validationResult.IsValid)
            {
                try
                {
                    _employeePositionService.Update(id, employeePosition);
                    return Ok();
                }
                catch (ArgumentException ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest(validationResult.Errors);
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> UpdatePatch(int id, [FromBody] JsonPatchDocument<EmployeePosition> employeeToUpdate)
        {
            try
            {
                var employeePosition = await _employeePositionService.GetByIdAsync(id);
                employeeToUpdate.ApplyTo(employeePosition, ModelState);
                return Ok(employeePosition);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeePositionAsync(int id)
        {
            try
            {
                await _employeePositionService.DeleteAsync(id);
                return Ok($"{nameof(EmployeePosition)} {EntityHasBeenDeleted}");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEmployeePositionsAsync([FromQuery(Name = "idArr")] int[] idArr)
        {
            try
            {
                await _employeePositionService.DeleteRangeAsync(idArr);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
