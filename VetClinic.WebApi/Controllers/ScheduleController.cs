using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Services;
using VetClinic.WebApi.Validators.EntityValidators;
using VetClinic.WebApi.ViewModels.ScheduleViewModels;
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
            var schedule = await _scheduleService.GetScheduleOfEmployee(employeeId);
            var model = _mapper.Map<IEnumerable<ScheduleViewModel>>(schedule);
            return Ok(model);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSchedule(int id)
        {
            var schedule = await _scheduleService.GetByIdAsync(id);

            var scheduleViewModel = _mapper.Map<ScheduleViewModel>(schedule);

            return Ok(scheduleViewModel);
        }

        [HttpPost("AssignScheduleToEmployee")]
        public async Task<IActionResult> AssignScheduleToEmployee(ScheduleViewModel model, string employeeId)
        {
            var newSchedule = _mapper.Map<Schedule>(model);

            var validationResult = _scheduleValidator.Validate(newSchedule);

            if (validationResult.IsValid)
            {
                await _scheduleService.AssignScheduleToEmployeeAsync(newSchedule, employeeId);
                return Ok();
            }
            return BadRequest(validationResult.Errors);
        }

        [HttpPost("AssignSchedulesToEmployee")]
        public async Task<IActionResult> AssignSchedulesToEmployee(IEnumerable<ScheduleViewModel> schedules, string employeeId)
        {
            var schedulesToInsert = _mapper.Map<IEnumerable<Schedule>>(schedules);

            var validationResult = _scheduleCollectionValidator.Validate(schedulesToInsert);

            if(validationResult.IsValid)
            {
                await _scheduleService.AssignMultipleSchedulesToEmployeeAsync(schedulesToInsert, employeeId);
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
                _scheduleService.Update(id, schedule);
                return Ok();
            }
            return BadRequest(validationResult.Errors);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            await _scheduleService.DeleteAsync(id);
            return Ok($"{nameof(Schedule)} {EntityHasBeenDeleted}");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteListOfSchedule([FromQuery(Name = "listOfIds")] IList<int> listOfIds)
        {
            await _scheduleService.DeleteRangeAsync(listOfIds);
            return Ok();
        }
    }
}
