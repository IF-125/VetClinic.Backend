using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Services;
using VetClinic.WebApi.Validators.EntityValidators;
using VetClinic.WebApi.ViewModels.PetViewModels;

namespace VetClinic.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetsImagesController : ControllerBase
    {
        private readonly IBlobService _blobService;
        private readonly IPetImageService _petImageService;
        private readonly string _containerName;
        private readonly string _containerPath;
        private readonly IMapper _mapper;

        public PetsImagesController(
            IBlobService blobService, 
            IPetImageService petImageService,
            IMapper mapper)
        {
            _blobService = blobService;
            _petImageService = petImageService;
            _containerName = "testcontainer";
            _containerPath = "https://blobuploadsample21.blob.core.windows.net/testcontainer/";
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> InsertPetImage(IFormFile file, [FromForm] PetImageViewModel petImageViewModel)
        {
           if (petImageViewModel.PetId == 0)
            {
                petImageViewModel= JsonConvert.DeserializeObject<PetImageViewModel>(Request.Form["data"]);
            }

            if (file == null || file.Length <= 0)
                return BadRequest();
            else
            {
                var newPetImage = _mapper.Map<PetImage>(petImageViewModel);

                var validator = new PetImageValidator();
                var validationResult = validator.Validate(newPetImage);

                if (validationResult.IsValid)
                {
                    string fileName = Guid.NewGuid().ToString();
                    newPetImage.Path = _containerPath + fileName;

                    await _petImageService.InsertAsync(newPetImage);

                    var res = await _blobService.UploadFileBlob(fileName, file, _containerName);

                    if (res)
                        return Ok(newPetImage);
                }

            }
            return BadRequest();
        }

       

        [HttpGet]
        public async Task<IActionResult> GetAllPetImages(int petId)
        {
            IList<PetImage> petImageList = await _petImageService.GetPetImagesByPetId(petId);

            return Ok(petImageList);
        }

       

        [HttpDelete("DeletePetImage/{petImageId}")]
        public async Task<IActionResult> DeletePetImageByName(int petImageId)
        {
            var petImage = await _petImageService.GetByIdAsync(petImageId);

            var petImageBlobName = petImage.Path.Replace(_containerPath, default);

            await _blobService.DeleteBlob(petImageBlobName, _containerName);

            return Ok($"Image \"{petImageBlobName}\" has been deleted");
        }





        
      


    }
}
