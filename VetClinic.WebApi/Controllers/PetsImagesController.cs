using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using VetClinic.Core.Interfaces.Services;

namespace VetClinic.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetsImagesController : ControllerBase
    {
        private readonly IBlobService _blobService;
        private readonly string _containerName;

        public PetsImagesController(IBlobService blobService)
        {
            _blobService = blobService;
            _containerName = "testcontainer";
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

        //[HttpGet]
        //public IActionResult AddFile()
        //{
        //    return View();
        //}

        [HttpPost]
        public async Task<IActionResult> AddPetImage(IFormFile file)
        {
            if (file == null || file.Length <= 0)
                return BadRequest();
            else
            {
                // fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName.Replace(" ", ""));
                string fileName = "NoName";
                var res = await _blobService.UploadFileBlob(fileName, file, _containerName);

                if (res)
                    return Ok($"Image {file.FileName} was added");
            }
            return BadRequest();
        }


    }
}
