using System.Text;
using System.Text.Json;

using Microsoft.Azure.Functions.Worker.Http;

namespace Testing
{
    public static class Processing
    {
        public class ClientPrincipal
        {
            public IEnumerable<ClientPrincipalClaim>? Claims { get; set; }
        }

        public class ClientPrincipalClaim
        {
            public string Typ { get; set; } = null!;
            public string Val { get; set; } = null!;
        }

        /// <summary>
        /// Code below originally from Microsoft Docs - https://docs.microsoft.com/en-gb/azure/static-web-apps/user-information?tabs=csharp#api-functions
        /// </summary>
        /// <param name="req">The HttpRequestData header.</param>
        /// <returns>Parsed ClaimsPrincipal from 'x-ms-client-principal' header.</returns>
        public static IEnumerable<string> ParsePrincipal(this HttpRequestData req)
        {
            var principal = new ClientPrincipal();

            if (req.Headers.TryGetValues("x-ms-client-principal", out var header))
            {
                var data = header.First();
                var decoded = Convert.FromBase64String(data);
                var json = Encoding.UTF8.GetString(decoded);
                principal = JsonSerializer.Deserialize<ClientPrincipal>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
            }
            
            try
            {
                var roles = principal.Claims.Where(item => item.Typ == "roles").First();
                List<string> roleValues = new List<string>(roles.Val.Split(','));

                return roleValues;       
            } catch (Exception ex)
            {
                return new List<string>();
            }
        }
    }
}