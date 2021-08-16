using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Services;
using VetClinic.WebApi.Validators.EntityValidators;
using VetClinic.WebApi.ViewModels;

namespace VetClinic.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetsImagesController : ControllerBase
    {
        private readonly IBlobService _blobService;
        private readonly IPetImageService _petImageService;
        private readonly string _containerName;
        private readonly IMapper _mapper;

        public PetsImagesController(
            IBlobService blobService, 
            IPetImageService petImageService,
            IMapper mapper)
        {
            _blobService = blobService;
            _petImageService = petImageService;
            _containerName = "testcontainer";
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> InsertPetImage(IFormFile file, [FromForm] PetImageViewModel petImageViewModel)
        {
            if (file == null || file.Length <= 0)
                return BadRequest();
            else
            {
                var newPetImage = _mapper.Map<PetImage>(petImageViewModel);

                var validator = new PetImageValidator();
                var validationResult = validator.Validate(newPetImage);

                if (validationResult.IsValid)
                {
                    newPetImage = await _petImageService.InsertAsyncWithId(newPetImage);
                }

               

                string fileName = newPetImage.PetId.ToString()+ newPetImage.Id.ToString();
                var res = await _blobService.UploadFileBlob(fileName, file, _containerName);

                if (res)
                    return Ok($"Image {file.FileName} was added");
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPetImages()
        {
            var files = await _blobService.AllBlobs(_containerName);
            return Ok(files);
        }

        [HttpGet("GetPetImageByName/{name}")]
        public async Task<IActionResult> GetPetImageByName(string name)
        {
            var res = await _blobService.GetBlob(name, _containerName);

            return Ok(res);
        }

        [HttpDelete("DeletePetImageByName/{name}")]
        public async Task<IActionResult> DeletePetImageByName(string name)
        {
            await _blobService.DeleteBlob(name, _containerName);

            return Ok($"Image \"{name}\" has been deleted");
        }

        
      


    }
}
