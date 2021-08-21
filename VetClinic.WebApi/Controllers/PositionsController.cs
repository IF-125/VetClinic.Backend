using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Errors.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Services;
using VetClinic.WebApi.Validators.EntityValidators;
using VetClinic.WebApi.ViewModels.PositionViewModels;
using static VetClinic.Core.Resources.TextMessages;

namespace VetClinic.WebApi.Controllers
{
    //[Authorize(Policy = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        private readonly IPositionService _positionService;
        private readonly IMapper _mapper;
        private readonly PositionValidator _positionValidator;

        public PositionsController(IPositionService positionService, 
            IMapper mapper,
            PositionValidator positionValidator)
        {
            _positionService = positionService;
            _mapper = mapper;
            _positionValidator = positionValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPositions()
        {
            var positions = await _positionService.GetPositionsAsync();

            var positionViewModel = _mapper.Map<IEnumerable<PositionViewModel>>(positions);

            return Ok(positionViewModel);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPosition(int id)
        {
            try
            {
                var position = await _positionService.GetByIdAsync(id);

                var positionViewModel = _mapper.Map<PositionViewModel>(position);

                return Ok(positionViewModel);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertPosition(PositionViewModel model)
        {
            var newPosition = _mapper.Map<Position>(model);

            var validationResult = _positionValidator.Validate(newPosition);

            if (validationResult.IsValid)
            {
                await _positionService.InsertAsync(newPosition);
                return Ok(newPosition);
            }
            return BadRequest(validationResult.Errors);
        }

        [HttpPut]
        public IActionResult UpdatePosition(int id, PositionViewModel model)
        {
            var position = _mapper.Map<Position>(model);

            var validationResult = _positionValidator.Validate(position);

            if (validationResult.IsValid)
            {
                try
                {
                    _positionService.Update(id, position);
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
        public async Task<IActionResult> DeletePositionAsync(int id)
        {
            try
            {
                await _positionService.DeleteAsync(id);
                return Ok($"{nameof(Position)} {EntityHasBeenDeleted}");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePositionsAsync([FromQuery(Name = "listOfIds")] IList<int> listOfIds)
        {
            try
            {
                await _positionService.DeleteRangeAsync(listOfIds);
                return Ok();
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
