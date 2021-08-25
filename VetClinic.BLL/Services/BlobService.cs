using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VetClinic.Core.Interfaces.Services;

namespace VetClinic.BLL.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobClient;

        public BlobService(BlobServiceClient blobClient)
        {
            _blobClient = blobClient;
        }

        public async Task<string> GetBlob(string name, string containerName)
        {
            var containerClient = _blobClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(name);

            var str = blobClient.Uri.AbsoluteUri;

            return str;
        }

        public async Task<IEnumerable<string>> AllBlobs(string containerName)
        {
            var containerClient = _blobClient.GetBlobContainerClient(containerName);
            var files = new List<string>();

            var blobs = containerClient.GetBlobsAsync();

            await foreach (var item in blobs)
            {
                files.Add(item.Name);
            }

            return files;
        }
        public async Task<bool> UploadFileBlob(string name, IFormFile file, string containerName)
        {
            try
            {
                var containerClient = _blobClient.GetBlobContainerClient(containerName);

                var blobClient = containerClient.GetBlobClient(name);

                var httpHeaders = new BlobHttpHeaders()
                {
                    ContentType = file.ContentType
                };

                var blobInfo = await blobClient.UploadAsync(file.OpenReadStream(), httpHeaders);

                if (blobInfo != null)
                    return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        public async Task<bool> DeleteBlob(string name, string containerName)
        {
            var containerClient = _blobClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(name);

            return await blobClient.DeleteIfExistsAsync();
        }

       
    }
}
