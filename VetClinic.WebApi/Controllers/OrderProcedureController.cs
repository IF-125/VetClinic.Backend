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
            var orderProcedures = await _orderProcedureService.GetOrderProceduresAsync(asNoTracking: true);

            var orderProcedureViewModel = _mapper.Map<IEnumerable<OrderProcedureViewModel>>(orderProcedures);

            return Ok(orderProcedureViewModel);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderProcedure(int id)
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

        [HttpPost]
        public async Task<IActionResult> InsertOrderProcedure(OrderProcedureViewModel model)
        {
            var newOrderProcedure = _mapper.Map<OrderProcedure>(model);

            var validationResult = _validator.Validate(newOrderProcedure);

            if (validationResult.IsValid)
            {
                await _orderProcedureService.InsertAsync(newOrderProcedure);
                return Ok();
            }
            return BadRequest(validationResult.Errors);
        }

        [HttpPut]
        public IActionResult Update(int id, OrderProcedureViewModel model)
        {
            var orderProcedure = _mapper.Map<OrderProcedure>(model);

            var validationResult = _validator.Validate(orderProcedure);

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
        public async Task<IActionResult> DeleteOrderProcedure(int id)
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
        public async Task<IActionResult> DeleteOrderProcedures([FromQuery(Name = "ids")] IList<int> ids)
        {
            try
            {
                await _orderProcedureService.DeleteRangeAsync(ids);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
