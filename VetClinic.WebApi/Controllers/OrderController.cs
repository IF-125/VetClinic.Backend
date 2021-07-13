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
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllOrdersAsync()
        {
            var orders = await _orderService.GetOrdersAsync(asNoTracking: true);

            var orderViewModel = _mapper.Map<IEnumerable<OrderViewModel>>(orders);

            return Ok(orderViewModel);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderByIdAsync(int id)
        {
            try
            {
                var order = await _orderService.GetByIdAsync(id);

                var orderViewModel = _mapper.Map<OrderViewModel>(order);

                return Ok(orderViewModel);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertOrderAsync(OrderViewModel model)
        {
            var newOrder = _mapper.Map<Order>(model);

            var validator = new OrderValidator();
            var validationResult = validator.Validate(newOrder);

            if (validationResult.IsValid)
            {
                await _orderService.InsertAsync(newOrder);
                return CreatedAtAction("InsertOrderAsync", new { id = newOrder.Id }, newOrder);
            }
            return BadRequest(validationResult.Errors);
        }

        [HttpPut]
        public IActionResult Update(int id, OrderViewModel model)
        {
            var order = _mapper.Map<Order>(model);

            var validator = new OrderValidator();
            var validationResult = validator.Validate(order);

            if (validationResult.IsValid)
            {
                try
                {
                    _orderService.Update(id, order);
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
        public async Task<IActionResult> DeleteOrderAsync(int id)
        {
            try
            {
                await _orderService.DeleteAsync(id);
                return Ok($"{nameof(Order)} {EntityHasBeenDeleted}");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteOrdersAsync([FromQuery(Name = "idArr")] int[] idArr)
        {
            try
            {
                await _orderService.DeleteRangeAsync(idArr);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
