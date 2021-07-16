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
        private readonly OrderValidator _validator;
        public OrderController(IOrderService orderService, IMapper mapper, OrderValidator validator)
        {
            _orderService = orderService;
            _mapper = mapper;
            _validator = validator;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetOrdersAsync(asNoTracking: true);

            var orderViewModel = _mapper.Map<IEnumerable<OrderViewModel>>(orders);

            return Ok(orderViewModel);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
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
        public async Task<IActionResult> InsertOrder(OrderViewModel model)
        {
            var newOrder = _mapper.Map<Order>(model);

            var validationResult = _validator.Validate(newOrder);

            if (validationResult.IsValid)
            {
                await _orderService.InsertAsync(newOrder);
                return Ok(newOrder);
            }
            return BadRequest(validationResult.Errors);
        }

        [HttpPut]
        public IActionResult Update(int id, OrderViewModel model)
        {
            var order = _mapper.Map<Order>(model);

            var validationResult = _validator.Validate(order);

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
        public async Task<IActionResult> DeleteOrder(int id)
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
        public async Task<IActionResult> DeleteOrders([FromQuery(Name = "ids")] IList<int> ids)
        {
            try
            {
                await _orderService.DeleteRangeAsync(ids);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
