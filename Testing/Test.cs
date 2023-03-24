using System.Net;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Testing
{
    public class Test
    {
        private readonly ILogger _logger;

        public Test(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Test>();
        }

        [Function("Test")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");


            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(req.Headers);
            return response;
        }
    }
}