using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Functions
{
    public static class GetLibraries
    {
        [FunctionName("GetLibraries")]
        public static async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]
            HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var library = new DuncleLibrary(null);

            List<Library.Model.Library> libraries;
            try
            {
                libraries = await library.getLibraries();
            }
            catch (Exception e)
            {
                var errorString = $"Failed to get the libraries with error {e}";
                log.LogError(errorString);
                
                return new BadRequestObjectResult(errorString);
            }
            
            return new OkObjectResult(libraries);

        }
    }
}