using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace PapagoFunction
{
    public static class PapagoFunctions
    {
        [FunctionName("Translate")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ExecutionContext context,
            ILogger log)
        {

            log.LogInformation("C# HTTP trigger function processed a request.");

            string trans = req.Query["trans"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            trans = trans ?? data?.trans;

            Papago papago = new Papago();

            
            var myConfig = new ConfigurationBuilder()
                                .SetBasePath(context.FunctionAppDirectory)
                                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                                .AddEnvironmentVariables()
                                .Build();

            string papagoID = myConfig["X-Naver-Client-Id"];
            string papagoPS = myConfig["X-Naver-Client-Secret"];
                        
            string responseMessage = string.IsNullOrEmpty(trans)
                ? papago.GetTransResult("Papago ?????? API",papagoID,papagoPS)
                : papago.GetTransResult(trans, papagoID, papagoPS, papago.GetWhatIsLang(trans,papagoID,papagoPS));

            return new OkObjectResult(responseMessage);
                            

        }

        //[FunctionName("Translate")]
        //public static async Task<IActionResult> Run(
        //    [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
        //    [Queue("outqueue"), StorageAccount("AzureWebJobsStorage")] ICollector<string> msg,
        //    ILogger log)
        //{

        //    log.LogInformation("C# HTTP trigger function processed a request.");

        //    string trans = req.Query["trans"];

        //    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //    dynamic data = JsonConvert.DeserializeObject(requestBody);
        //    trans = trans ?? data?.trans;

        //    if (!string.IsNullOrEmpty(trans))
        //    {
        //        // Add a message to the output collection.
        //        msg.Add(string.Format("Translate passed to the function: {0}", trans));
        //    }

        //    Papago papago = new Papago();

        //    string responseMessage = string.IsNullOrEmpty(trans)
        //        ? papago.GetTransResult("Papago ?????? API")
        //        : papago.GetTransResult(trans);

        //    return new OkObjectResult(responseMessage);


        //}
    }
}

