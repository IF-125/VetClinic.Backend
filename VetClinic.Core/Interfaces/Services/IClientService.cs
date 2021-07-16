using System.Threading.Tasks;
using VetClinic.Core.Entities;

namespace VetClinic.Core.Interfaces.Services
{
    public interface IClientService
    {
        public Task<Client> AddAsync(Client client);

        public Task<Client> GetByIdAsync(string id);
        Task DeleteAsync(string id);
        void Update(string id, Client client);
    }
}
