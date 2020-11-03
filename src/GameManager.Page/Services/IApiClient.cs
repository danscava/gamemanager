using System.Threading.Tasks;

namespace GameManager.Page.Services
{
    public interface IApiClient
    {
        Task<T> Get<T>(string uri);
        Task<T> Post<T>(string uri, object value);
    }
}