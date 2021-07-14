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
    public class PetsController : ControllerBase
    {
        private readonly IPetService _petService;
        private readonly IMapper _mapper;

        public PetsController(IPetService petService, IMapper mapper)
        {
            _petService = petService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPetsAsync()
        {
            var pets = await _petService.GetPetsAsync();
            var petViewModel = _mapper.Map<IEnumerable<PetViewModel>>(pets);

            return Ok(petViewModel);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPetByIdAsync(int id)
        {
            try
            {
                var pet = await _petService.GetByIdAsync(id);
                var petViewModel = _mapper.Map<PetViewModel>(pet);
               
                return Ok(petViewModel);
            }
            catch (ArgumentException  ex)
            {
                return NotFound(ex.Message);
            }                     
        }
        
        [HttpPost]
        public async Task<IActionResult> InsertPetAsync(PetViewModel petViewModel)
        {
            var newPet = _mapper.Map<Pet>(petViewModel);

            var validator = new PetValidator();
            var validationResult = validator.Validate(newPet);

            if (validationResult.IsValid)
            {
                await _petService.InsertAsync(newPet);
                return CreatedAtAction("InsertPetAsync", new { id = newPet.Id }, newPet);
            }

            return BadRequest(validationResult.Errors);
        }


        [HttpPut]
        public  IActionResult Update (int id, PetViewModel petViewModel)
        {
            var pet = _mapper.Map<Pet>(petViewModel);

            var validator = new PetValidator();
            var validationResult = validator.Validate(pet);

            if (validationResult.IsValid)
            {
                try
                {
                    _petService.Update(id, pet);
                    return Ok();
                }
                catch (ArgumentException ex)
                {

                    return BadRequest(ex.Message);
                }
            }

            return BadRequest(validationResult.Errors);
        }


        ///HttpPatch
        ///

        [HttpDelete("id")]
        public async Task<IActionResult> DeletePetAsync (int id)
        {
            try
            {
                await _petService.DeleteAsync(id);
                return Ok($"{nameof(Pet)} {EntityHasBeenDeleted}");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePetsAsync([FromQuery(Name = "idArr")] int[] idArr)
        {
            try
            {
                await _petService.DeleteRangeAsync(idArr);
                return Ok();
            }
            catch (ArgumentException ex)
            {

                return BadRequest(ex.Message); 
            }

        }
    }
}
