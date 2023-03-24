using System.Net;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Testing
{
    public class Health
    {
        private readonly IClaimsPrincipalAccessor _claimsPrincipalAccessor;
        private readonly ILogger _logger;
        private readonly string[] _validRoles = { "Read.Health" };

        public Health(IClaimsPrincipalAccessor claimsPrincipalAccessor, ILoggerFactory loggerFactory)
        {
            _claimsPrincipalAccessor = claimsPrincipalAccessor;
            _logger = loggerFactory.CreateLogger<Health>();
        }

        [Function("Health")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var isMember = _claimsPrincipalAccessor.Roles.Intersect(_validRoles).Count() > 0;

            var message = "No Access!";

            if (isMember)
            {
                string roles = string.Join(" ", _claimsPrincipalAccessor.Roles);
                message = $"Has Access! Roles: {roles}";
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync(message);
            return response;
        }
    }
}