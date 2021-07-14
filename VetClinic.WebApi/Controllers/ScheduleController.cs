using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Policy = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;
        private readonly IMapper _mapper;

        public ScheduleController(IScheduleService scheduleService, IMapper mapper)
        {
            _scheduleService = scheduleService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetScheduleOfEmployee(string employeeId)
        {
            try
            {
                var schedule = await _scheduleService.GetScheduleOfEmployee(employeeId);
                var model = _mapper.Map<IEnumerable<ScheduleViewModel>>(schedule);
                return Ok(model);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message); 
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSchedule(int id)
        {
            try
            {
                var schedule = await _scheduleService.GetByIdAsync(id);

                var scheduleViewModel = _mapper.Map<ScheduleViewModel>(schedule);

                return Ok(scheduleViewModel);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertScheduleAsync(ScheduleViewModel model)
        {
            var newSchedule = _mapper.Map<Schedule>(model);

            var validator = new ScheduleValidator();
            var validationResult = validator.Validate(newSchedule);

            if (validationResult.IsValid)
            {
                await _scheduleService.InsertAsync(newSchedule);
                return CreatedAtAction("InsertScheduleAsync", new { id = newSchedule.Id }, newSchedule);
            }
            return BadRequest(validationResult.Errors);
        }

        [HttpPut]
        public IActionResult UpdateSchedule(int id, ScheduleViewModel model)
        {
            var schedule = _mapper.Map<Schedule>(model);

            var validator = new ScheduleValidator();
            var validationResult = validator.Validate(schedule);

            if (validationResult.IsValid)
            {
                try
                {
                    _scheduleService.Update(id, schedule);
                    return Ok();
                }
                catch (ArgumentException ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest(validationResult.Errors);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScheduleAsync(int id)
        {
            try
            {
                await _scheduleService.DeleteAsync(id);
                return Ok($"{nameof(Schedule)} {EntityHasBeenDeleted}");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteListOfScheduleAsync([FromQuery(Name = "idArr")] int[] idArr)
        {
            try
            {
                await _scheduleService.DeleteRangeAsync(idArr);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
