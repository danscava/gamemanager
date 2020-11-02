using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GameManager.Data.Commands;
using GameManager.Data.Enums;
using GameManager.Data.Interfaces;
using GameManager.Data.Models;
using GameManager.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace GameManager.Services
{
    public class GameMediaService : IGameMediaService
    {
        private readonly ILogger<GameMediaService> _logger;
        private readonly IAsyncGameMediaRepository _gameMediaRepository;
        private readonly IAsyncFriendRepository _friendRepository;

        public GameMediaService(ILogger<GameMediaService> logger, 
            IAsyncGameMediaRepository gameMediaRepository,
            IAsyncFriendRepository friendRepository)
        {
            _logger = logger;
            _gameMediaRepository = gameMediaRepository;
            _friendRepository = friendRepository;
        }

        public async Task<GameMedia> CreateAsync(GameMediaCreateRequest createRequest)
        {
            if (String.IsNullOrEmpty(createRequest.Title))
            {
                throw new Exception("Game title cannot be empty");
            }

            if (!Enum.IsDefined(typeof(Platform), createRequest.Platform))
            {
                throw new ArgumentException("Platform value is not valid");
            }

            if (!Enum.IsDefined(typeof(MediaType), createRequest.MediaType))
            {
                throw new ArgumentException("Game media type is not valid");
            }

            var newGameMedia = new GameMedia();
            newGameMedia.Active = 1;
            newGameMedia.Title = createRequest.Title;
            newGameMedia.Year = createRequest.Year;
            newGameMedia.Platform = createRequest.Platform;
            newGameMedia.MediaType = createRequest.MediaType;

            var gameMedia = await _gameMediaRepository.AddAsync(newGameMedia);

            return gameMedia;
        }

        public async Task<GameMedia> UpdateAsync(GameMediaUpdateRequest updateRequest)
        {
            var gameMedia = await _gameMediaRepository.GetByIdAsync(updateRequest.Id);

            if (gameMedia == null)
            {
                throw new Exception($"Game with id {updateRequest.Id} not found");
            }

            if (!Enum.IsDefined(typeof(Platform), updateRequest.Platform))
            {
                throw new ArgumentException("Platform value is not valid");
            }

            if (!Enum.IsDefined(typeof(MediaType), updateRequest.MediaType))
            {
                throw new ArgumentException("Game media type is not valid");
            }

            gameMedia.Title = updateRequest.Title;
            gameMedia.Year = updateRequest.Year;
            gameMedia.Platform = updateRequest.Platform;
            gameMedia.MediaType = updateRequest.MediaType;

            gameMedia = await _gameMediaRepository.UpdateAsync(gameMedia);

            return gameMedia;
        }

        public async Task<bool> DeleteAsync(int gameMediaId)
        {
            var gameMedia = await _gameMediaRepository.GetByIdAsync(gameMediaId);

            if (gameMedia != null)
            {
                await _gameMediaRepository.DeleteAsync(gameMedia);
                return true;
            }

            return false;
        }

        public async Task<GameMedia> LendGameAsync(GameMediaLendRequest lendRequest)
        {
            var friend = await _friendRepository.GetByIdAsync(lendRequest.FriendId);
            var gameMedia = await _gameMediaRepository.GetByIdAsync(lendRequest.GameId);

            if (friend == null)
            {
                throw new Exception($"Friend with id {lendRequest.FriendId} not found");
            }

            if (gameMedia == null)
            {
                throw new Exception($"Game with id {lendRequest.GameId} not found");
            }

            if (gameMedia.BorrowerId != null)
            {
                throw new Exception($"Game is already borrowed");
            }

            gameMedia.BorrowerId = friend.Id;
            await _gameMediaRepository.UpdateAsync(gameMedia);

            gameMedia = await _gameMediaRepository.GetByIdAsync(lendRequest.GameId);

            return gameMedia;
        }

        public async Task<GameMedia> ReturnGameAsync(GameMediaReturnRequest returnRequest)
        {
            var gameMedia = await _gameMediaRepository.GetByIdAsync(returnRequest.GameId);

            if (gameMedia == null)
            {
                throw new Exception($"Game with id {returnRequest.GameId} not found");
            }

            if (gameMedia.BorrowerId == null)
            {
                throw new Exception($"Game is not borrowed");
            }

            gameMedia.BorrowerId = null;
            gameMedia = await _gameMediaRepository.UpdateAsync(gameMedia);

            return gameMedia;
        }
    }
}
