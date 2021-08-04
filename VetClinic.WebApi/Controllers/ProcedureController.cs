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
    public class ProcedureController : ControllerBase
    {
        private readonly IProcedureService _procedureService;
        private readonly IMapper _mapper;
        private readonly ProcedureValidator _validator;
        public ProcedureController(IProcedureService procedureService, IMapper mapper, ProcedureValidator validator)
        {
            _procedureService = procedureService;
            _mapper = mapper;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProcedures()
        {
            var procedures = await _procedureService.GetProceduresAsync();

            var procedureViewModel = _mapper.Map<IEnumerable<ProcedureViewModel>>(procedures);

            return Ok(procedureViewModel);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProcedure(int id)
        {
            var procedure = await _procedureService.GetByIdAsync(id);

            var procedureViewModel = _mapper.Map<ProcedureViewModel>(procedure);

            return Ok(procedureViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> InsertProcedure(ProcedureViewModel model)
        {
            var newProcedure = _mapper.Map<Procedure>(model);

            var validationResult = _validator.Validate(newProcedure);

            if (validationResult.IsValid)
            {
                await _procedureService.InsertAsync(newProcedure);
                return Ok(newProcedure);
            }
            return BadRequest(validationResult.Errors);
        }

        [HttpPut]
        public IActionResult Update(int id, ProcedureViewModel model)
        {
            var procedure = _mapper.Map<Procedure>(model);

            var validationResult = _validator.Validate(procedure);

            if (validationResult.IsValid)
            {
                _procedureService.Update(id, procedure);
                return Ok();
            }
            return BadRequest(validationResult.Errors);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProcedure(int id)
        {
            await _procedureService.DeleteAsync(id);
            return Ok($"{nameof(Procedure)} {EntityHasBeenDeleted}");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProcedures([FromQuery(Name = "ids")] IList<int> ids)
        {
            await _procedureService.DeleteRangeAsync(ids);
            return Ok();
        }
    }
}
