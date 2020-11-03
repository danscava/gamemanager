using System.Collections.Generic;
using System.Threading.Tasks;
using GameManager.Data.Dtos;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace GameManager.Page.Services
{
    public class GameMediaService : IGameMediaService
    {
        private readonly IApiClient _httpService;

        public GameMediaService(IApiClient httpService)
        {
            _httpService = httpService;
        }

        public async Task<GameMediaResponseDto> CreateGame(GameMediaCreateDto createDto)
        {
            return await _httpService.Post<GameMediaResponseDto>("api/gamemedia", createDto);
        }

        public async Task<GameMediaResponseDto> UpdateGame(int id, GameMediaCreateDto createDto)
        {
            return await _httpService.Put<GameMediaResponseDto>($"api/gamemedia/{id}", createDto);
        }

        public async Task<GameMediaResponseDto> DeleteGame(int id)
        {
            return await _httpService.Delete<GameMediaResponseDto>($"api/gamemedia/{id}");
        }

        public async Task<List<GameMediaResponseDto>> GetAllGames()
        {
            return await _httpService.Get<List<GameMediaResponseDto>>("api/gamemedia");
        }

        public async Task<GameMediaResponseDto> GetGameById(int id)
        {
            return await _httpService.Get<GameMediaResponseDto>($"api/gamemedia/{id}");
        }

        public async Task<GameMediaResponseDto> LendGame(int gameId, int friendId)
        {
            var request = new GameMediaLendDto()
            {
                friend_id = friendId
            };

            return await _httpService.Post<GameMediaResponseDto>($"api/gamemedia/{gameId}/lend", request);
        }

        public async Task<GameMediaResponseDto> ReturnGame(int gameId)
        {
            return await _httpService.Post<GameMediaResponseDto>($"api/gamemedia/{gameId}/return", "");
        }
    }
}