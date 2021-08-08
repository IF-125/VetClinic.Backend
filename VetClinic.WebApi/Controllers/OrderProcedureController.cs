using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
    public class OrderProcedureController : ControllerBase
    {
        private readonly IOrderProcedureService _orderProcedureService;
        private readonly IMapper _mapper;
        private readonly OrderProcedureValidator _validator;

        public OrderProcedureController(IOrderProcedureService orderProcedureService, IMapper mapper, 
            OrderProcedureValidator validator)
        {
            _orderProcedureService = orderProcedureService;
            _mapper = mapper;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrderProcedures()
        {
            var orderProcedures = await _orderProcedureService.GetOrderProceduresAsync();

            var orderProcedureViewModel = _mapper.Map<IEnumerable<OrderProcedureViewModel>>(orderProcedures);

            return Ok(orderProcedureViewModel);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderProcedure(int id)
        {
            var orderProcedure = await _orderProcedureService.GetByIdAsync(id);

            var orderProcedureViewModel = _mapper.Map<OrderProcedureViewModel>(orderProcedure);

            return Ok(orderProcedureViewModel);
        }

        [HttpGet("GetOrderedProceduresOfDoctor")]
        public async Task<IActionResult> GetOrderedProceduresOfDoctor(string doctorId)
        {
            var orderedProceduresOfDoctor = await _orderProcedureService.GetOrderProceduresOfDoctorAsync(doctorId);

            var orderedProceduresOfDoctorViewModel = _mapper.Map<IEnumerable<OrderProcedureOfDoctorViewModel>>(orderedProceduresOfDoctor);

            return Ok(orderedProceduresOfDoctorViewModel);
        }

        [HttpGet("GetMedicalCard")]
        public async Task<IActionResult> GetMedicalCardOfPetAsync(int petId)
        {
            var medicalCard = await _orderProcedureService.GetMedicalCardOfPetAsync(petId);

            var medicalCardModel = _mapper.Map<IEnumerable<MedicalCardViewModel>>(medicalCard);

            return Ok(medicalCardModel);
        }

        [HttpPost]
        public async Task<IActionResult> InsertOrderProcedure(int petId, int procedureId, OrderToCreateViewModel model)
        {
            var orderProcedure = await _orderProcedureService.GenerateOrderProcedureAsync(petId, procedureId, model.IsPaid);
            await _orderProcedureService.InsertAsync(orderProcedure);
            return Ok();
        }

        [HttpPost("AddAppointmentAndEmployeeToOrderProcedure")]
        public async Task<IActionResult> AddAppointmentAndDoctorToOrderProcedure(int orderProcedureId, string employeeId, AppointmentToCreateViewModel appointmentViewModel)
        {
            var appointment = _mapper.Map<Appointment>(appointmentViewModel);
            await _orderProcedureService.AddAppointmentAndDoctorToOrderProcedureAsync(orderProcedureId, employeeId, appointment);
            return Ok();
        }

        [HttpPut]
        public IActionResult Update(int id, OrderProcedureViewModel model)
        {
            var orderProcedure = _mapper.Map<OrderProcedure>(model);

            var validationResult = _validator.Validate(orderProcedure);

            if (validationResult.IsValid)
            {
                _orderProcedureService.Update(id, orderProcedure);
                return Ok();
            }
            return BadRequest(validationResult.Errors);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderProcedure(int id)
        {
            await _orderProcedureService.DeleteAsync(id);
            return Ok($"{nameof(OrderProcedure)} {EntityHasBeenDeleted}");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteOrderProcedures([FromQuery(Name = "ids")] IList<int> ids)
        {
            await _orderProcedureService.DeleteRangeAsync(ids);
            return Ok();
        }
    }
}
