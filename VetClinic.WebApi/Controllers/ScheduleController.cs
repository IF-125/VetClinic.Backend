using AutoMapper;
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
    //[Authorize(Policy = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;
        private readonly IMapper _mapper;
        private readonly ScheduleValidator _scheduleValidator;
        private readonly ScheduleCollectionValidator _scheduleCollectionValidator;

        public ScheduleController(IScheduleService scheduleService,
            IMapper mapper,
            ScheduleValidator scheduleValidator,
            ScheduleCollectionValidator scheduleCollectionValidator)
        {
            _scheduleService = scheduleService;
            _mapper = mapper;
            _scheduleValidator = scheduleValidator;
            _scheduleCollectionValidator = scheduleCollectionValidator;
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
            catch (NotFoundException ex)
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
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("InsertSchedule")]
        public async Task<IActionResult> InsertSchedule(ScheduleViewModel model)
        {
            var newSchedule = _mapper.Map<Schedule>(model);

            var validationResult = _scheduleValidator.Validate(newSchedule);

            if (validationResult.IsValid)
            {
                await _scheduleService.InsertAsync(newSchedule);
                return Ok(newSchedule);
            }
            return BadRequest(validationResult.Errors);
        }

        [HttpPost("InsertSchedules")]
        public async Task<IActionResult> InsertSchedules(IEnumerable<ScheduleViewModel> schedules)
        {
            var schedulesToInsert = _mapper.Map<IEnumerable<Schedule>>(schedules);

            var validationResult = _scheduleCollectionValidator.Validate(schedulesToInsert);

            if(validationResult.IsValid)
            {
                await _scheduleService.InsertRangeAsync(schedulesToInsert);
                return Ok();
            }

            return BadRequest(validationResult.Errors);
        }

        [HttpPut]
        public IActionResult UpdateSchedule(int id, ScheduleViewModel model)
        {
            var schedule = _mapper.Map<Schedule>(model);

            var validationResult = _scheduleValidator.Validate(schedule);

            if (validationResult.IsValid)
            {
                try
                {
                    _scheduleService.Update(id, schedule);
                    return Ok();
                }
                catch (BadRequestException ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest(validationResult.Errors);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            try
            {
                await _scheduleService.DeleteAsync(id);
                return Ok($"{nameof(Schedule)} {EntityHasBeenDeleted}");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteListOfSchedule([FromQuery(Name = "listOfIds")] IList<int> listOfIds)
        {
            try
            {
                await _scheduleService.DeleteRangeAsync(listOfIds);
                return Ok();
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
