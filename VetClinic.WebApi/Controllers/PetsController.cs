using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Services;
using VetClinic.WebApi.Validators.EntityValidators;
using VetClinic.WebApi.ViewModels.PetViewModels;
using static VetClinic.Core.Resources.TextMessages;

namespace VetClinic.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetsController : ControllerBase
    {
        private readonly IPetService _petService;
        private readonly IMapper _mapper;

        //TODO: provide validator via DI

        public PetsController(IPetService petService, IMapper mapper)
        {
            _petService = petService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPets()
        {
            var pets = await _petService.GetPetsAsync();
            var petViewModel = _mapper.Map<IEnumerable<PetResponseViewModel>>(pets);

            return Ok(petViewModel);
        }

        [HttpGet("GetByClientId/{clientId}")]
        public async Task<IActionResult> GetPetsByClientId(string clientId)
        {
            var pets = await _petService.GetPetsByClientId(clientId);
            var petViewModel = _mapper.Map<IEnumerable<PetResponseViewModel>>(pets);

            return Ok(petViewModel);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPet(int id)
        {
            var pet = await _petService.GetByIdAsync(id);
            var petViewModel = _mapper.Map<PetResponseViewModel>(pet);
               
            return Ok(petViewModel);                   
        }

        [HttpGet("GetPetsToTreat/{doctorId}")]
        public async Task<IActionResult> GetPetsToTreat(string doctorId)
        {
            var pets = await _petService.GetPetsToTreat(doctorId);

            var petsViewModel = _mapper.Map<IEnumerable<PetViewModelOrigin>>(pets);

            return Ok(petsViewModel);
        }

        [HttpGet("GetMedicalCard/{petId}")]
        public async Task<IActionResult> GetMedicalCardOfPetAsync(int petId)
        {
            var pet = await _petService.GetMedicalCardOfPetAsync(petId);

            var medicalCardModel = _mapper.Map<PetResponseViewModel>(pet);

            return Ok(medicalCardModel);
        }

        [HttpPost]
        public async Task<IActionResult> InsertPet(PetViewModelOrigin petViewModel)
        {
            var newPet = _mapper.Map<Pet>(petViewModel);

            var validator = new PetValidator();
            var validationResult = validator.Validate(newPet);

            if (validationResult.IsValid)
            {
                await _petService.InsertAsync(newPet);
                return Ok(newPet);
            }

            return BadRequest(validationResult.Errors);
        }

        [HttpPut]
        public IActionResult UpdatePet(int id, PetViewModelOrigin petViewModel)
        {
            var pet = _mapper.Map<Pet>(petViewModel);

            var validator = new PetValidator();
            var validationResult = validator.Validate(pet);

            if (validationResult.IsValid)
            {
                _petService.Update(id, pet);
                return Ok();
            }

            return BadRequest(validationResult.Errors);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdatePatch(int id, [FromBody] JsonPatchDocument<Pet> petToUpdate)
        {
            var pet = await _petService.GetByIdAsync(id);
            petToUpdate.ApplyTo(pet, ModelState);
            _petService.Update(id, pet);

            var petViewModel = _mapper.Map<PetViewModelOrigin>(pet);
            return Ok(petViewModel);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePet (int id)
        {
            await _petService.DeleteAsync(id);
            return Ok($"{nameof(Pet)} {EntityHasBeenDeleted}");
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePets([FromQuery(Name = "listOfIds")] List<int> listOfIds)
        {
            await _petService.DeleteRangeAsync(listOfIds);
            return Ok();
        }
    }
}
