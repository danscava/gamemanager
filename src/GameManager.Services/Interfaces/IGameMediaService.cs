using System.Threading.Tasks;
using GameManager.Data.Commands;
using GameManager.Data.Models;

namespace GameManager.Services.Interfaces
{
    public interface IGameMediaService
    {
        Task<GameMedia> CreateAsync(GameMediaCreateRequest createRequest);
        Task<GameMedia> UpdateAsync(GameMediaUpdateRequest updateRequest);
        Task<bool> DeleteAsync(int gameMediaId);

        Task<GameMedia> LendGameAsync(GameMediaLendRequest lendRequest);
        Task<GameMedia> ReturnGameAsync(GameMediaReturnRequest returnRequest);
        
    }
}