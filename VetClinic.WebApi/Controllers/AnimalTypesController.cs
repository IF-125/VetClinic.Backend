using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Errors.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Services;
using VetClinic.WebApi.Validators.EntityValidators;
using VetClinic.WebApi.ViewModels.AnimalTypesViewModels;
using static VetClinic.Core.Resources.TextMessages;


namespace VetClinic.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalTypesController:ControllerBase
    {

        private readonly IAnimalTypeService _AnimalTypeService;
        private readonly IMapper _mapper;

        public AnimalTypesController(IAnimalTypeService AnimalTypeService, IMapper mapper)
        {
            _AnimalTypeService = AnimalTypeService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAnimalTypes()
        {
            var animalTypes = await _AnimalTypeService.GetAnimalTypesAsync();
            var animalTypeViewModel = _mapper.Map<IEnumerable<AnimalTypeViewModel>>(animalTypes);

            return Ok(animalTypeViewModel);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAnimalType(int id)
        {
            try
            {
                var AnimalType = await _AnimalTypeService.GetByIdAsync(id);
                var AnimalTypeViewModel = _mapper.Map<AnimalTypeViewModel>(AnimalType);

                return Ok(AnimalTypeViewModel);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertAnimalType(AnimalTypeViewModel AnimalTypeViewModel)
        {
            var newAnimalType = _mapper.Map<AnimalType>(AnimalTypeViewModel);

            var validator = new AnimalTypeValidator();
            var validationResult = validator.Validate(newAnimalType);

            if (validationResult.IsValid)
            {
                await _AnimalTypeService.InsertAsync(newAnimalType);
                return Ok(newAnimalType);
            }

            return BadRequest(validationResult.Errors);
        }


        [HttpPut]
        public IActionResult UpdateAnimalType(int id, AnimalTypeViewModel AnimalTypeViewModel)
        {
            var AnimalType = _mapper.Map<AnimalType>(AnimalTypeViewModel);

            var validator = new AnimalTypeValidator();
            var validationResult = validator.Validate(AnimalType);

            if (validationResult.IsValid)
            {
                try
                {
                    _AnimalTypeService.Update(id, AnimalType);
                    return Ok();
                }
                catch (NotFoundException ex)
                {

                    return BadRequest(ex.Message);
                }
            }

            return BadRequest(validationResult.Errors);
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdatePatch(int id, [FromBody] JsonPatchDocument<AnimalType> AnimalTypeToUpdate)
        {
            try
            {
                var AnimalType = await _AnimalTypeService.GetByIdAsync(id);
                AnimalTypeToUpdate.ApplyTo(AnimalType, ModelState);
                return Ok(AnimalType);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteAnimalType(int id)
        {
            try
            {
                await _AnimalTypeService.DeleteAsync(id);
                return Ok($"{nameof(AnimalType)} {EntityHasBeenDeleted}");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAnimalTypes([FromQuery(Name = "listOfIds")] List<int> listOfIds)
        {
            try
            {
                await _AnimalTypeService.DeleteRangeAsync(listOfIds);
                return Ok();
            }
            catch (BadRequestException ex)
            {

                return BadRequest(ex.Message);
            }

        }

    }
}
