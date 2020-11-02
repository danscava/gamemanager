using System.Threading.Tasks;
using GameManager.Data.Commands;
using GameManager.Data.Models;

namespace GameManager.Services.Interfaces
{
    public interface IFriendService
    {
        Task<Friend> CreateAsync(FriendCreateRequest createRequest);
        Task<Friend> UpdateAsync(FriendUpdateRequest updateRequest);
        Task<bool> DeleteAsync(int friendId);
    }
}