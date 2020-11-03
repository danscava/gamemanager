using System.Collections.Generic;
using System.Threading.Tasks;
using GameManager.Data.Dtos;

namespace GameManager.Page.Services
{
    public class FriendService : IFriendService
    {
        private readonly IApiClient _httpService;

        public FriendService(IApiClient httpService)
        {
            _httpService = httpService;
        }

        public async Task<List<FriendResponseDto>> GetAllFriends()
        {
            return await _httpService.Get<List<FriendResponseDto>>("api/friend");
        }
    }
}