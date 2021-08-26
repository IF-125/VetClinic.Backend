using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Services;
using VetClinic.WebApi.Validators.EntityValidators;
using VetClinic.WebApi.ViewModels.AppointmentViewModels;
using static VetClinic.Core.Resources.TextMessages;

namespace VetClinic.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IMapper _mapper;
        private readonly AppointmentValidator _validator;
        public AppointmentController(IAppointmentService appointmentService, IMapper mapper, AppointmentValidator validator)
        {
            _appointmentService = appointmentService;
            _mapper = mapper;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAppointments()
        {
            var appointments = await _appointmentService.GetAppointmentsAsync();

            var appointmentViewModel = _mapper.Map<IEnumerable<AppointmentViewModel>>(appointments);

            return Ok(appointmentViewModel);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointment(int id)
        {
            var appointment = await _appointmentService.GetByIdAsync(id);

            var appointmentViewModel = _mapper.Map<AppointmentViewModel>(appointment);

            return Ok(appointmentViewModel);
        }

        [HttpGet("GetAppointmentsOfDoctor/{doctorId}")]
        public async Task<IActionResult> GetAppointmentsOfDoctor(string doctorId)
        {
            var appointments = await _appointmentService.GetAppointmentsOfDoctorAsync(doctorId);

            var appointmentsViewModel = _mapper.Map<IEnumerable<AppointmentViewModel>>(appointments);

            return Ok(appointmentsViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> InsertAppointment(AppointmentToCreateViewModel model)
        {
            var newAppointment = _mapper.Map<Appointment>(model);

            var validationResult = _validator.Validate(newAppointment);

            if (validationResult.IsValid)
            {
                await _appointmentService.InsertAsync(newAppointment);
                return Ok(newAppointment);
            }
            return BadRequest(validationResult.Errors);
        }

        [HttpPut]
        public IActionResult Update(int id, AppointmentViewModel model)
        {
            var appointment = _mapper.Map<Appointment>(model);

            var validationResult = _validator.Validate(appointment);

            if (validationResult.IsValid)
            {
                _appointmentService.Update(id, appointment);
                return Ok();
            }
            return BadRequest(validationResult.Errors);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            await _appointmentService.DeleteAsync(id);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAppointments([FromQuery(Name = "ids")] IList<int> ids)
        {
            await _appointmentService.DeleteRangeAsync(ids);
            return Ok();
        }
    }
}
