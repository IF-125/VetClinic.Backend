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
    public class ProcedureController : ControllerBase
    {
        private readonly IProcedureService _procedureService;
        private readonly IMapper _mapper;
        public ProcedureController(IProcedureService procedureService, IMapper mapper)
        {
            _procedureService = procedureService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProceduresAsync()
        {
            var procedures = await _procedureService.GetProceduresAsync(asNoTracking: true);

            var procedureViewModel = _mapper.Map<IEnumerable<ProcedureViewModel>>(procedures);

            return Ok(procedureViewModel);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProcedureByIdAsync(int id)
        {
            try
            {
                var procedure = await _procedureService.GetByIdAsync(id);

                var procedureViewModel = _mapper.Map<ProcedureViewModel>(procedure);

                return Ok(procedureViewModel);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{lowest}/{highest}")]
        public async Task<IActionResult> GetProceduresInPriceRangeAsync(int lowest, int highest)
        {
            var procedures = await _procedureService.GetProceduresAsync(filter:b => b.Price >= lowest && b.Price <= highest);

            var procedureViewModel = _mapper.Map<IEnumerable<ProcedureViewModel>>(procedures);

            return Ok(procedureViewModel);
        }

        [HttpGet("{shortest}/{longest}")]
        public async Task<IActionResult> GetProceduresInDurationRangeAsync(TimeSpan shortest, TimeSpan longest)
        {
            var procedures = await _procedureService.GetProceduresAsync(filter: b => b.Duration >= shortest && b.Duration <= longest);

            var procedureViewModel = _mapper.Map<IEnumerable<ProcedureViewModel>>(procedures);

            return Ok(procedureViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> InsertProcedureAsync(ProcedureViewModel model)
        {
            var newProcedure = _mapper.Map<Procedure>(model);

            var validator = new ProcedureValidator();
            var validationResult = validator.Validate(newProcedure);

            if (validationResult.IsValid)
            {
                await _procedureService.InsertAsync(newProcedure);
                return CreatedAtAction("InsertProcedureAsync", new { id = newProcedure.Id }, newProcedure);
            }
            return BadRequest(validationResult.Errors);
        }

        [HttpPut]
        public IActionResult Update(int id, ProcedureViewModel model)
        {
            var procedure = _mapper.Map<Procedure>(model);

            var validator = new ProcedureValidator();
            var validationResult = validator.Validate(procedure);

            if (validationResult.IsValid)
            {
                try
                {
                    _procedureService.Update(id, procedure);
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
        public async Task<IActionResult> DeleteProcedureAsync(int id)
        {
            try
            {
                await _procedureService.DeleteAsync(id);
                return Ok($"{nameof(Procedure)} {EntityHasBeenDeleted}");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProceduresAsync([FromQuery(Name = "idArr")] int[] idArr)
        {
            try
            {
                await _procedureService.DeleteRangeAsync(idArr);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
