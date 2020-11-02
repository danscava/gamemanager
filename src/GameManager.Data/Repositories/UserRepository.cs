using System.Threading.Tasks;
using GameManager.Data.Interfaces;
using GameManager.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GameManager.Data.Repositories
{
    public class UserRepository : GenericAsyncRepository<User>, IAsyncUserRepository
    {
        public UserRepository(GameManagerContext context) : base(context)
        {
        }

        public Task<User> GetByUsernameAsync(string username)
        {
            return _dbContext.UserSet.Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Login.Equals(username));
        }
    }
}