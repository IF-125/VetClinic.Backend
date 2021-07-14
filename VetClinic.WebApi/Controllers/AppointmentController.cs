using AutoMapper;
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
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IMapper _mapper;
        public AppointmentController(IAppointmentService appointmentService, IMapper mapper)
        {
            _appointmentService = appointmentService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAppointmentsAsync()
        {
            var appointments = await _appointmentService.GetAppointmentsAsync(asNoTracking: true);

            var appointmentViewModel = _mapper.Map<IEnumerable<AppointmentViewModel>>(appointments);

            return Ok(appointmentViewModel);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointmentByIdAsync(int id)
        {
            try
            {
                var appointment = await _appointmentService.GetByIdAsync(id);

                var appointmentViewModel = _mapper.Map<AppointmentViewModel>(appointment);

                return Ok(appointmentViewModel);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertAppointmentAsync(AppointmentViewModel model)
        {
            var newAppointment = _mapper.Map<Appointment>(model);

            var validator = new AppointmentValidator();
            var validationResult = validator.Validate(newAppointment);

            if (validationResult.IsValid)
            {
                await _appointmentService.InsertAsync(newAppointment);
                return CreatedAtAction("InsertAppointmentAsync", new { id = newAppointment.Id }, newAppointment);
            }
            return BadRequest(validationResult.Errors);
        }

        [HttpPut]
        public IActionResult Update(int id, AppointmentViewModel model)
        {
            var appointment = _mapper.Map<Appointment>(model);

            var validator = new AppointmentValidator();
            var validationResult = validator.Validate(appointment);

            if (validationResult.IsValid)
            {
                try
                {
                    _appointmentService.Update(id, appointment);
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
        public async Task<IActionResult> DeleteAppointmentAsync(int id)
        {
            try
            {
                await _appointmentService.DeleteAsync(id);
                return Ok($"{nameof(Appointment)} {EntityHasBeenDeleted}");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAppointmentsAsync([FromQuery(Name = "idArr")] int[] idArr)
        {
            try
            {
                await _appointmentService.DeleteRangeAsync(idArr);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
