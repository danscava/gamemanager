using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GameManager.Data.Models;
using GameManager.Data.Repositories;

namespace GameManager.Data.Interfaces
{
    public interface IAsyncGameMediaRepository : IAsyncRepository<GameMedia>
    {
        Task<List<GameMedia>> GetAllAsync();

        Task<List<GameMedia>> GetAvailableGamesAsync();

        Task<List<GameMedia>> GetLentGamesAsync();

        Task<List<GameMedia>> GetGamesWithFriendAsync(int friendId);
    }
}
