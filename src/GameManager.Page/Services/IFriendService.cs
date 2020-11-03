using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameManager.Data.Dtos;

namespace GameManager.Page.Services
{
    public interface IFriendService
    {
        Task<List<FriendResponseDto>> GetAllFriends();
    }
}
