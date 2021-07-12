using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
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
    public class PositionsController : ControllerBase
    {
        private readonly IPositionService _positionService;
        private readonly IMapper _mapper;

        public PositionsController(IPositionService positionService, IMapper mapper)
        {
            _positionService = positionService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPositionsAsync()
        {
            var positions = await _positionService.GetPositionsAsync(asNoTracking: true);

            var positionViewModel = _mapper.Map<IEnumerable<PositionViewModel>>(positions);

            return Ok(positionViewModel);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPositionByIdAsync(int id)
        {
            try
            {
                var position = await _positionService.GetByIdAsync(id);

                var positionViewModel = _mapper.Map<PositionViewModel>(position);

                return Ok(positionViewModel);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertPositionAsync(PositionViewModel model)
        {
            var newPosition = _mapper.Map<Position>(model);

            var validator = new PositionValidator();
            var validationResult = validator.Validate(newPosition);

            if (validationResult.IsValid)
            {
                await _positionService.InsertAsync(newPosition);
                return CreatedAtAction("InsertPositionAsync", new { id = newPosition.Id }, newPosition);
            }
            return BadRequest(validationResult.Errors);
        }

        [HttpPut]
        public IActionResult UpdatePosition(int id, PositionViewModel model)
        {
            var position = _mapper.Map<Position>(model);

            var validator = new PositionValidator();
            var validationResult = validator.Validate(position);

            if (validationResult.IsValid)
            {
                try
                {
                    _positionService.Update(id, position);
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
        public async Task<IActionResult> DeletePositionAsync(int id)
        {
            try
            {
                await _positionService.DeleteAsync(id);
                return Ok($"{nameof(Position)} {EntityHasBeenDeleted}");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePositionsAsync([FromQuery(Name = "idArr")] int[] idArr)
        {
            try
            {
                await _positionService.DeleteRangeAsync(idArr);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
