using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.DAL.Context;

namespace VetClinic.WebApi.Controllers
{
    [ApiController]
    [Route("api/pets")]
    public class PetsController : ControllerBase
    {
        readonly VetClinicDbContext db;


        public PetsController(VetClinicDbContext context)
        {
            db = context;
        }


        [HttpGet]
        public IEnumerable<Pet> GetPets()
        {
            return db.Pets.ToList();        
        }

        [HttpGet("{id}")]
        public Pet Get(int id)
        {
            Pet pet= db.Pets.FirstOrDefault(x => x.Id == id);
            return pet;
        }

        [HttpPost]
        public IActionResult Post(Pet pet)
        {
            if (ModelState.IsValid)
            {
                db.Pets.Add(pet);
                db.SaveChanges();
                return Ok(pet);
            }
            return BadRequest(ModelState);
        }

        [HttpPut]
        public IActionResult Put(Pet pet)
        {
            if (ModelState.IsValid)
            {
                db.Update(pet);
                db.SaveChanges();
                return Ok(pet);
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Pet pet = db.Pets.FirstOrDefault(x => x.Id == id);
            if (pet != null)
            {
                db.Pets.Remove(pet);
                db.SaveChanges();
            }
            return Ok(pet);
        }

    }
}
