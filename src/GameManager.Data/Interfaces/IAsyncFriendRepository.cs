using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GameManager.Data.Models;
using GameManager.Data.Repositories;

namespace GameManager.Data.Interfaces
{
    public interface IAsyncFriendRepository : IAsyncRepository<Friend>
    {
        Task<List<Friend>> GetAllAsync();
        new Task<Friend> GetByIdAsync(int friendId);
    }
}
