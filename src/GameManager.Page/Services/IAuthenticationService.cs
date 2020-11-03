using System.Threading.Tasks;
using GameManager.Page.Models;

namespace GameManager.Page.Services
{
    public interface IAuthenticationService
    {
        WebUser User { get; }
        Task Login(string username, string password);
        Task Logout();
    }
}