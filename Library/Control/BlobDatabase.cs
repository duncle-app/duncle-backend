using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Library.Interfaces;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;


namespace Library.Control
{
    public class BlobDatabase: IDatabase
    {

        private const string LIBRARIES = "libraries";

        private CloudBlobContainer container;
        private CloudBlobClient cloudBlobClient;
        
        public BlobDatabase(string storageAccountConnectionString)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageAccountConnectionString);
            this.cloudBlobClient = storageAccount.CreateCloudBlobClient();
            this.container = cloudBlobClient.GetContainerReference(LIBRARIES);
        }
        
        public async Task<List<Model.Library>> getLibraries()
        {
            await this.container.CreateIfNotExistsAsync();
            return await ListBlobsFlatListingAsync(container, 100);
        }
        
        private static async Task<List<Model.Library>> ListBlobsFlatListingAsync(CloudBlobContainer container, int? segmentSize)
        {
            BlobContinuationToken continuationToken = null;

            
            List<Model.Library> libraries = new List<Model.Library>();
            CloudBlob blob;
            // Call the listing operation and enumerate the result segment.
            // When the continuation token is null, the last segment has been returned
            // and execution can exit the loop.
            do
            {
                BlobResultSegment resultSegment = await container.ListBlobsSegmentedAsync(string.Empty,
                    true, BlobListingDetails.Metadata, segmentSize, continuationToken, null, null);

                foreach (var blobItem in resultSegment.Results)
                {
                    // A flat listing operation returns only blobs, not virtual directories.
                    blob = (CloudBlob) blobItem;
                    await using var stream = new MemoryStream();
                    await blob.DownloadToStreamAsync(stream);
                    string text = System.Text.Encoding.UTF8.GetString(stream.ToArray());
                    var library = JsonConvert.DeserializeObject<Model.Library>(text);
                    libraries.Add(library);
                }
                    
                // Get the continuation token and loop until it is null.
                continuationToken = resultSegment.ContinuationToken;

            } while (continuationToken != null);

            return libraries;
        }

        public async Task updateLibrary(Model.Library library)
        {
            await this.container.CreateIfNotExistsAsync();
            CloudBlockBlob blobReference = container.GetBlockBlobReference($"{library.id}.json");
            await blobReference.UploadTextAsync(JsonConvert.SerializeObject(library));
        }

        public async Task<Model.Library> createLibrary(Model.Library library)
        {
            await this.container.CreateIfNotExistsAsync();
            Guid newId = Guid.NewGuid();
            library.id = newId.ToString();
            CloudBlockBlob blobReference = container.GetBlockBlobReference($"{library.id}.json");
            await blobReference.UploadTextAsync(JsonConvert.SerializeObject(library));
            return library;
        }
    }
}