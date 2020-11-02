using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameManager.Data.Interfaces;
using GameManager.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GameManager.Data.Repositories
{
    public class FriendRepository : GenericAsyncRepository<Friend>, IAsyncFriendRepository
    {
        public FriendRepository(GameManagerContext context) : base(context)
        {
        }

        /// <summary>
        /// Returns all friends
        /// </summary>
        /// <remarks>Only returns active friends</remarks>
        /// <returns></returns>
        public Task<List<Friend>> GetAllAsync()
        {
            return _dbContext.FriendSet.Include(f =>f.BorrowedGames)
                .Where(gm => gm.Active == 1).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Returns a friend by id
        /// </summary>
        /// <remarks>Only returns active friends</remarks>
        /// <param name="friendId">Friend Id</param>
        /// <returns></returns>
        public new Task<Friend> GetByIdAsync(int friendId)
        {
            return _dbContext.FriendSet.Include(f => f.BorrowedGames)
                .Where(gm => gm.Id == friendId && gm.Active == 1).AsNoTracking().FirstOrDefaultAsync();
        }
    }
}