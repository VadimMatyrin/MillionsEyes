using System.Threading.Tasks;

namespace MillionsEyesWebApi.Interfaces
{
    public interface IHttpClientHelper
    {
        Task<string> GetMethodAsync(string url);

        Task<string> GetAuthorizationHeader(string tenantId, string userId, string secret);
    }
}