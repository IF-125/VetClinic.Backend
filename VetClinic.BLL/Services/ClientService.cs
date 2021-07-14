using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Repositories;
using VetClinic.Core.Interfaces.Services;
using VetClinic.WebApi.ViewModels;

namespace VetClinic.BLL.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

       
        public async Task<Client> AddAsync(Client client)
        {
            await _clientRepository.InsertAsync(client);

            return client;
        }

        public async Task<Client> GetByIdAsync(string id)
        {
            return await _clientRepository.GetFirstOrDefaultAsync(c => c.Id == id, 
                                                                  c=> c.Include(c=> c.PhoneNumbers),
                                                                  true);
        }

        public async Task InsertAsync(Client entity)
        {
            await _clientRepository.InsertAsync(entity);
        }

        public void Update(string id,  Client clientToUpdate)
        {
            if (id != clientToUpdate.Id)
            {
                throw new ArgumentException();
            }
            _clientRepository.Update(clientToUpdate);

        }

        public async Task DeleteAsync(string id)
        {
            var clientToDelete = await _clientRepository.GetFirstOrDefaultAsync(x => x.Id == id);

            if (clientToDelete == null)
            {
                throw new ArgumentException();
            }
            _clientRepository.Delete(clientToDelete);
        }
    }
}
