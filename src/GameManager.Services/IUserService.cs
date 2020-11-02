using System.Threading.Tasks;
using GameManager.Data.Models;

namespace GameManager.Services
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
        Task<User> Create(User user, string password);
        Task<bool> Delete(int id);
    }
}