using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GameManager.Data.Models;
using GameManager.Data.Repositories;

namespace GameManager.Data.Interfaces
{
    public interface IAsyncUserRepository : IAsyncRepository<User>
    {
        Task<User> GetByUsernameAsync(string username);
    }
}
