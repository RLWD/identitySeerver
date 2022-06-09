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

        public BlobClient  GetBlobClient (string connectionString, string containerName, string blobName)
        {
            if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(containerName))
                throw new ArgumentException("invalid argument, check your settings");
            try
            {
                var serviceClient = new BlobServiceClient(connectionString);
                if (!serviceClient.GetBlobContainerClient(containerName).Exists())
                    throw new ArgumentException("container name does not exist");
                var container = serviceClient.GetBlobContainerClient(containerName);
                if(!container.GetBlobClient(blobName).Exists())
                    throw new ArgumentException("unable to find the selected blob");
                return container.GetBlobClient(blobName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetBlobClientContent(string connectionString, string containerName, string blobName)
        {
            if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(containerName))
                throw new ArgumentException("invalid argument, check your settings");
            try
            {
                var serviceClient = new BlobServiceClient(connectionString);
                if (!serviceClient.GetBlobContainerClient(containerName).Exists())
                    throw new ArgumentException("container name does not exist");
                var container = serviceClient.GetBlobContainerClient(containerName);
                if (!container.GetBlobClient(blobName).Exists())
                    throw new ArgumentException("unable to find the specified blobItem");
                var blobClient= container.GetBlobClient(blobName);
                return blobClient.DownloadContent().Value.Content.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
