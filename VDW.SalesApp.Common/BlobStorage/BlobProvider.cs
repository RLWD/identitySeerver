using Azure.Storage.Blobs;
using System;

namespace VDW.SalesApp.Common.BlobStorage
{
    public class BlobProvider
    {
        public BlobProvider() { }
        public BlobContainerClient GetBlobContainerClient(string connectionString, string containerName)
        {
            if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(containerName))
                throw new ArgumentException("invalid argument, check your settings");
            try
            {
                var serviceClient = new BlobServiceClient(connectionString);
                if (!serviceClient.GetBlobContainerClient(containerName).Exists())
                    throw new ArgumentException("container name does not exist");
                return serviceClient.GetBlobContainerClient(containerName);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
