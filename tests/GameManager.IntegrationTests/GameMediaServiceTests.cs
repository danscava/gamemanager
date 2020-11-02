using System;
using GameManager.Data;
using GameManager.Data.Commands;
using GameManager.Data.Enums;
using GameManager.Data.Interfaces;
using GameManager.Data.Models;
using GameManager.Data.Repositories;
using GameManager.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GameManager.IntegrationTests
{
    public class GameMediaServiceTests
    {
        private readonly GameManagerContext _context;
        private readonly IAsyncGameMediaRepository _gameMediaRepository;
        private readonly GameMedia _testGame;
        private readonly Friend _testFriend;

        public GameMediaServiceTests()
        {
            var dbOptions = new DbContextOptionsBuilder<GameManagerContext>()
                .UseInMemoryDatabase("gamemanager_test")
                .Options;
            _context = new GameManagerContext(dbOptions);
            _gameMediaRepository = new GameMediaRepository(_context);

            _testGame = new GameMedia()
            {
                Platform = Platform.PC,
                Title = "Testing game",
                Year = 1998,
                MediaType = MediaType.CD
            };

            _testFriend = new Friend()
            {
                Name = "Ice King",
                Email = "iceking@ooo.com",
                Telephone = "99918827"
            };
        }

        [Fact]
        public async void CanAddGameMedia()
        {
            var mockLogger = new Mock<ILogger<GameMediaService>>(); 
            var mockFriendRepo = new Mock<IAsyncFriendRepository>();
            var gameMediaService = new GameMediaService(mockLogger.Object, _gameMediaRepository, mockFriendRepo.Object);

            var request = new GameMediaCreateRequest()
            {
                MediaType = _testGame.MediaType,
                Year = _testGame.Year,
                Platform = _testGame.Platform,
                Title = _testGame.Title
            };

            var createdGame = await gameMediaService.CreateAsync(request);
            var dbGame = await _gameMediaRepository.GetByIdAsync(createdGame.Id);

            Assert.NotNull(dbGame);
            Assert.NotNull(createdGame);
            Assert.Equal(_testGame.Title, createdGame.Title);
            Assert.Equal(_testGame.Year, createdGame.Year);
            Assert.Equal(_testGame.Platform, createdGame.Platform);
            Assert.Equal(_testGame.MediaType, createdGame.MediaType);
        }
    }
}
