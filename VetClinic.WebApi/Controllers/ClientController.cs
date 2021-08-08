using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Services;
using VetClinic.WebApi.ViewModels;

namespace VetClinic.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly IMapper _mapper;

        public ClientController(IClientService clientService, IMapper mapper)
        {
            _clientService = clientService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            var client = await _clientService.GetByIdAsync(id);

            var clientViewModel = _mapper.Map<ClientViewModel>(client);

            return Ok(clientViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(ClientViewModel model)
        {
            var client = _mapper.Map<Client>(model);

            await _clientService.InsertAsync(client);

            return Ok(client);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] JsonPatchDocument<Client> clientToUpdate)
        {
            var client = await _clientService.GetByIdAsync(id);

            clientToUpdate.ApplyTo(client, ModelState);
            _clientService.Update(id, client);

            var clientViewModel = _mapper.Map<ClientViewModel>(client);
            return Ok(clientViewModel);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(string id)
        {
            await _clientService.DeleteAsync(id);
            return Ok($"{nameof(Client)} has been deleted");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteClients(IList<string> listOfIds)
        {
            await _clientService.DeleteRangeAsync(listOfIds);
            return Ok();
        }
    }
}
