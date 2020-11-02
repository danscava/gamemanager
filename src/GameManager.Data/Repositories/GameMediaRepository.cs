using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameManager.Data.Interfaces;
using GameManager.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GameManager.Data.Repositories
{
    public class GameMediaRepository : GenericAsyncRepository<GameMedia>, IAsyncGameMediaRepository
    {
        public GameMediaRepository(GameManagerContext context) : base(context)
        {
        }

        /// <summary>
        /// Returns a game media by id
        /// </summary>
        /// <remarks>Only returns active game media</remarks>
        /// <param name="id">Game media Id</param>
        /// <returns></returns>
        public new Task<GameMedia> GetByIdAsync(int id)
        {
            return _dbContext.GameMediaSet.Include(g => g.Borrower)
                .FirstOrDefaultAsync(gm => gm.Id == id && gm.Active == 1);
        }

        /// <summary>
        /// Returns all game medias
        /// </summary>
        /// <remarks>Only returns active game media</remarks>
        /// <returns></returns>
        public Task<List<GameMedia>> GetAllAsync()
        {
            return _dbContext.GameMediaSet.Include(g => g.Borrower)
                .Where(gm => gm.Active == 1).ToListAsync();
        }

        /// <summary>
        /// Returns not borrowed games
        /// </summary>
        /// <remarks>Only returns active game media</remarks>
        /// <returns></returns>
        public Task<List<GameMedia>> GetAvailableGamesAsync()
        {
            return _dbContext.GameMediaSet.Include(g => g.Borrower)
                .Where(gm => gm.BorrowerId == null && gm.Active == 1).ToListAsync();
        }

        /// <summary>
        /// Returns borrowed games
        /// </summary>
        /// <remarks>Only returns active game media</remarks>
        /// <returns></returns>
        public Task<List<GameMedia>> GetLentGamesAsync()
        {
            return _dbContext.GameMediaSet.Include(g => g.Borrower)
                .Where(gm => gm.BorrowerId != null && gm.Active == 1).ToListAsync();
        }

        /// <summary>
        /// Returns a list of games borrowed to a friend
        /// </summary>
        /// <remarks>Only returns active game media</remarks>
        /// <param name="friendId">Friend Id</param>
        /// <returns></returns>
        public Task<List<GameMedia>> GetGamesWithFriendAsync(int friendId)
        {
            return _dbContext.GameMediaSet.Include(g => g.Borrower)
                .Where(gm => gm.BorrowerId == friendId && gm.Active == 1).ToListAsync();

        }
    }
}