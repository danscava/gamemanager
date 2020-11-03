using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GameManager.Data.Dtos;
using Microsoft.AspNetCore.Components;

namespace GameManager.Page.Services
{
    public interface IGameMediaService
    {
        Task<List<GameMediaResponseDto>> GetAllGames();

        Task<GameMediaResponseDto> GetGameById(int id);

        Task<GameMediaResponseDto> LendGame(int gameId, int friendId);

        Task<GameMediaResponseDto> ReturnGame(int gameId);

    }
}
