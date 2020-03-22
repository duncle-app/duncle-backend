using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Library;
using Library.Control;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Functions
{
    public static class LibraryEndpoints
    {
        [FunctionName("Libraries")]
        public static async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function,"post", "patch", "get", Route = null)]
            HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            log.LogInformation(req.Method);
            var lib = new DuncleLibrary(
                new BlobDatabase(
                    Environment.GetEnvironmentVariable("StorageAccountConnectionString")));

            if (req.Method == "GET")
            {
                try
                {
                    List<Library.Model.Library> libraries = await lib.getLibraries();
                    return new OkObjectResult(libraries);
                }
                catch (Exception e)
                {
                    var errorString = $"Failed to get the libraries with error {e}";
                    log.LogError(errorString);
                
                    return new BadRequestObjectResult(errorString);
                }
            }
            
            
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Library.Model.Library inputLibrary;
            try
            {
                inputLibrary = JsonConvert.DeserializeObject<Library.Model.Library>(requestBody);
            }
            catch (Exception)
            {
                string errorMessage = $"failed to parse input body to library";
                log.LogError(errorMessage);
                return new BadRequestObjectResult(errorMessage);
            }
            
            
            try
            {
                
                switch (req.Method)
                {
                    case "POST":
                    {
                        Library.Model.Library createdLibrary = await lib.createLibrary(inputLibrary);
                        return new OkObjectResult(createdLibrary);
                    }
                    case "PATCH)":
                        await lib.updateLibrary(inputLibrary);
                        return new OkResult();
                    default:
                        return 
                            new BadRequestObjectResult("Invalid method type. Accepted Methods are post and patch");
                }
            }
            catch (Exception e)
            {
                string errorMessage = $"Failed to create library with error ${e}";
                return new BadRequestObjectResult(errorMessage);
            }
        }
    }
}