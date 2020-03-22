using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Interfaces;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;


namespace Library.Control
{
    public class BlobDatabase: IDatabase
    {

        private const string LIBRARIES = "libraries";

        private CloudBlobContainer container;
        
        public BlobDatabase(string storageAccountConnectionString)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageAccountConnectionString);
            CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
            this.container = cloudBlobClient.GetContainerReference(LIBRARIES);
        }
        
        public async Task<List<Model.Library>> getLibraries()
        {
            await this.container.CreateIfNotExistsAsync();
            throw new NotImplementedException();
        }

        public async Task updateLibrary(Model.Library library)
        {
            await this.container.CreateIfNotExistsAsync();
            throw new System.NotImplementedException();
        }

        public async Task createLibrary(Model.Library library)
        {
            await this.container.CreateIfNotExistsAsync();
            throw new System.NotImplementedException();
        }
    }
}