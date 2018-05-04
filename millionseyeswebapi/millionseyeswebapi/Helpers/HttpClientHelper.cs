using Microsoft.IdentityModel.Clients.ActiveDirectory;
using MillionsEyesWebApi.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

namespace MillionsEyesWebApi.Helpers
{
    public class HttpClientHelper : IHttpClientHelper
    {
        private string _header;


        public HttpClientHelper(string tenantId, string clientId, string secret)
        {
            _header = GetAuthorizationHeader(tenantId, clientId, secret).Result;
        }

        public async Task<string> GetAuthorizationHeader(string tenantId, string userId, string secret)
        {
            string authUrl = $"https://login.microsoftonline.com/{tenantId}";

            AuthenticationContext context = new AuthenticationContext(authUrl);

            ClientCredential cred = new ClientCredential(userId, secret);

            AuthenticationResult result = await context.AcquireTokenAsync("https://management.core.windows.net/", cred);
            return result.CreateAuthorizationHeader();
        }

        public async Task<string> GetMethodAsync(string url)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", _header);
            var result = httpClient.GetAsync(url);
            string res = await result.Result.Content.ReadAsStringAsync();
            return res;
        }
    }
}