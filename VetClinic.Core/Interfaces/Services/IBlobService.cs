using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VetClinic.Core.Interfaces.Services
{
    public interface IBlobService
    {
        Task<string> GetBlob(string name, string containerName);
        Task<IEnumerable<string>> AllBlobs(string containerName);
        Task<bool> UploadFileBlob(string name, IFormFile file, string containerName);
        Task<bool> DeleteBlob(string name, string containerName);
    }
}
