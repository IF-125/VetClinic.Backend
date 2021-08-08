using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Repositories;
using VetClinic.Core.Interfaces.Services;
using static VetClinic.Core.Resources.TextMessages;

namespace VetClinic.BLL.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<Client> GetByIdAsync(string id)
        {
            return await _clientRepository
                .GetFirstOrDefaultAsync(
                filter: c => c.Id == id,
                include: c => c.Include(c => c.PhoneNumbers),
                asNoTracking: true) ??
                    throw new NotFoundException($"{nameof(Client)} {EntityWasNotFound}");
        }

        public async Task InsertAsync(Client entity)
        {
            await _clientRepository.InsertAsync(entity);
            await _clientRepository.SaveChangesAsync(); 
        }

        public void Update(string id,  Client clientToUpdate)
        {
            if (id != clientToUpdate.Id)
            {
                throw new ArgumentException("id and passed id did not match");
            }
            _clientRepository.Update(clientToUpdate);
            _clientRepository.SaveChanges();

        }

        public async Task DeleteAsync(string id)
        {
            var clientToDelete = await _clientRepository.GetFirstOrDefaultAsync(x => x.Id == id) ??
                throw new NotFoundException($"{nameof(Client)} {EntityWasNotFound}");
            
            _clientRepository.Delete(clientToDelete);
            await _clientRepository.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IList<string> listOfIds)
        {
            var clientsToDelete = await _clientRepository.GetAsync(x => listOfIds.Contains(x.Id));

            if (clientsToDelete.Count() != listOfIds.Count)
            {
                throw new BadRequestException($"{SomeEntitiesInCollectionNotFound} {nameof(Client)}s to delete");
            }
            _clientRepository.DeleteRange(clientsToDelete);
            await _clientRepository.SaveChangesAsync();
        }
    }
}
