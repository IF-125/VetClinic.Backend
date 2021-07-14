using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
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
    public class SalariesController : ControllerBase
    {
        private readonly ISalaryService _salaryService;
        private readonly IMapper _mapper;

        public SalariesController(ISalaryService salaryService, IMapper mapper)
        {
            _salaryService = salaryService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetSalariesOfEmployee(int employeePositionId)
        {
            try
            {
                var salaries = await _salaryService.GetSalariesOfEmployee(employeePositionId);
                var model = _mapper.Map<IEnumerable<SalariesController>>(salaries);
                return Ok(model);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSalary(int id)
        {
            try
            {
                var salary = await _salaryService.GetByIdAsync(id);

                var model = _mapper.Map<SalaryViewModel>(salary);

                return Ok(model);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AssignSalaryToEmployee(string employeeId, SalaryViewModel model)
        {
            var salary = _mapper.Map<Salary>(model);

            var validator = new SalaryValidator();
            var validationResult = validator.Validate(salary);

            if (validationResult.IsValid)
            {
                try
                {
                    await _salaryService.AssignSalaryToEmployee(employeeId, salary);
                    return Ok();
                }
                catch (ArgumentException ex)
                {
                    return NotFound(ex.Message);
                }
            }
            else
            {
                return BadRequest(validationResult.Errors);
            }
        }

        [HttpPut]
        public IActionResult UpdateSalary(int id, SalaryViewModel model)
        {
            var salary = _mapper.Map<Salary>(model);

            var validator = new SalaryValidator();
            var validationResult = validator.Validate(salary);

            if (validationResult.IsValid)
            {
                try
                {
                    _salaryService.Update(id, salary);
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
        public async Task<IActionResult> UpdatePatch(int id, [FromBody] JsonPatchDocument<Salary> salaryToUpdate)
        {
            try
            {
                var salary = await _salaryService.GetByIdAsync(id);
                salaryToUpdate.ApplyTo(salary, ModelState);
                return Ok(salary);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalaryAsync(int id)
        {
            try
            {
                await _salaryService.DeleteAsync(id);
                return Ok($"{nameof(Salary)} {EntityHasBeenDeleted}");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteListOfSalariesAsync([FromQuery(Name = "idArr")] int[] idArr)
        {
            try
            {
                await _salaryService.DeleteRangeAsync(idArr);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
