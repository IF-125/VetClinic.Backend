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
    public class OrderProcedureController : ControllerBase
    {
        private readonly IOrderProcedureService _orderProcedureService;
        private readonly IMapper _mapper;
        public OrderProcedureController(IOrderProcedureService orderProcedureService, IMapper mapper)
        {
            _orderProcedureService = orderProcedureService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrderProceduresAsync()
        {
            var orderProcedures = await _orderProcedureService.GetOrderProceduresAsync(asNoTracking: true);

            var orderProcedureViewModel = _mapper.Map<IEnumerable<OrderProcedureViewModel>>(orderProcedures);

            return Ok(orderProcedureViewModel);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderProcedureByIdAsync(int id)
        {
            try
            {
                var orderProcedure = await _orderProcedureService.GetByIdAsync(id);

                var orderProcedureViewModel = _mapper.Map<OrderProcedureViewModel>(orderProcedure);

                return Ok(orderProcedureViewModel);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{petId}")]
        public async Task<IActionResult> GetOrderProcedureByPetIdAsync(int petId)
        {
            try
            {
                var orderProcedure = await _orderProcedureService.GetOrderProceduresAsync(b => b.PetId == petId);

                var orderProcedureViewModel = _mapper.Map<OrderProcedureViewModel>(orderProcedure);

                return Ok(orderProcedureViewModel);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetOrderProcedureByEmployeeIdAsync(int employeeId)
        {
            try
            {
                var orderProcedure = await _orderProcedureService.GetOrderProceduresAsync(b => b.EmployeeId == employeeId);

                var orderProcedureViewModel = _mapper.Map<OrderProcedureViewModel>(orderProcedure);

                return Ok(orderProcedureViewModel);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertOrderProcedureAsync(OrderProcedureViewModel model)
        {
            var newOrderProcedure = _mapper.Map<OrderProcedure>(model);

            var validator = new OrderProcedureValidator();
            var validationResult = validator.Validate(newOrderProcedure);

            if (validationResult.IsValid)
            {
                await _orderProcedureService.InsertAsync(newOrderProcedure);
                return CreatedAtAction("InsertOrderProcedureAsync", new { id = newOrderProcedure.Id }, newOrderProcedure);
            }
            return BadRequest(validationResult.Errors);
        }

        [HttpPut]
        public IActionResult Update(int id, OrderProcedureViewModel model)
        {
            var orderProcedure = _mapper.Map<OrderProcedure>(model);

            var validator = new OrderProcedureValidator();
            var validationResult = validator.Validate(orderProcedure);

            if (validationResult.IsValid)
            {
                try
                {
                    _orderProcedureService.Update(id, orderProcedure);
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
        public async Task<IActionResult> DeleteOrderProcedureAsync(int id)
        {
            try
            {
                await _orderProcedureService.DeleteAsync(id);
                return Ok($"{nameof(OrderProcedure)} {EntityHasBeenDeleted}");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteOrderProceduresAsync([FromQuery(Name = "idArr")] int[] idArr)
        {
            try
            {
                await _orderProcedureService.DeleteRangeAsync(idArr);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
