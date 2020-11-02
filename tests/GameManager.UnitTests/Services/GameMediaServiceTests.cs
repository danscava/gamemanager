using System;
using GameManager.Data.Commands;
using GameManager.Data.Enums;
using GameManager.Data.Interfaces;
using GameManager.Data.Models;
using GameManager.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GameManager.UnitTests.Services
{
    public class GameMediaServiceTests
    {
        private readonly GameMedia _testGame;

        private readonly Friend _testFriend;


        public GameMediaServiceTests()
        {
            _testGame = new GameMedia()
            {
                Id = 15,
                Platform = Platform.PC,
                Title = "Testing game",
                Year = 1998,
                MediaType = MediaType.CD
            };

            _testFriend = new Friend()
            {
                Id = 20,
                Name = "Ice King",
                Email = "iceking@ooo.com",
                Telephone = "99918827"
            };
        }

        [Fact]
        public async void CanCreateGameMedia()
        {
            var mockLogger = new Mock<ILogger<GameMediaService>>();
            var mockGameRepo = new Mock<IAsyncGameMediaRepository>();
            var mockFriendRepo = new Mock<IAsyncFriendRepository>();
            mockGameRepo.Setup(m => m.AddAsync(It.IsAny<GameMedia>())).ReturnsAsync(_testGame);

            var gameMediaService = new GameMediaService(mockLogger.Object, mockGameRepo.Object, mockFriendRepo.Object);

            var request = new GameMediaCreateRequest()
            {
                MediaType = _testGame.MediaType,
                Year = _testGame.Year,
                Platform = _testGame.Platform,
                Title = _testGame.Title
            };

            var createdGame = await gameMediaService.CreateAsync(request);

            Assert.NotNull(createdGame);
            Assert.Equal(_testGame.Title, createdGame.Title);
            Assert.Equal(_testGame.Year, createdGame.Year);
            Assert.Equal(_testGame.Platform, createdGame.Platform);
            Assert.Equal(_testGame.MediaType, createdGame.MediaType);
        }

        [Fact]
        public async void CanUpdateGameMedia()
        {
            var mockLogger = new Mock<ILogger<GameMediaService>>();
            var mockFriendRepo = new Mock<IAsyncFriendRepository>();

            var mockGameRepo = new Mock<IAsyncGameMediaRepository>();
            
            mockGameRepo.Setup(m => m.GetByIdAsync(_testGame.Id))
                                                .ReturnsAsync(_testGame);
            mockGameRepo.Setup(m => m.UpdateAsync(It.IsAny<GameMedia>()))
                                                .ReturnsAsync((GameMedia g) => g);

            var gameMediaService = new GameMediaService(mockLogger.Object, mockGameRepo.Object, mockFriendRepo.Object);

            var request = new GameMediaUpdateRequest()
            {
                Id = _testGame.Id,
                MediaType = MediaType.Cartridge,
                Year = 2000,
                Platform = Platform.SNES,
                Title = _testGame.Title
            };

            var updatedGame = await gameMediaService.UpdateAsync(request);

            Assert.NotNull(updatedGame);
            Assert.Equal(request.Id, updatedGame.Id);
            Assert.Equal(request.Title, updatedGame.Title);
            Assert.Equal(request.Year, updatedGame.Year);
            Assert.Equal(request.Platform, updatedGame.Platform);
            Assert.Equal(request.MediaType, updatedGame.MediaType);
        }

        [Fact]
        public async void CanDeleteGameMedia()
        {
            var mockLogger = new Mock<ILogger<GameMediaService>>();
            var mockFriendRepo = new Mock<IAsyncFriendRepository>();
            var mockGameRepo = new Mock<IAsyncGameMediaRepository>();

            mockGameRepo.Setup(m => m.GetByIdAsync(_testGame.Id))
                .ReturnsAsync(_testGame);

            var gameMediaService = new GameMediaService(mockLogger.Object, mockGameRepo.Object, mockFriendRepo.Object);

            var deleted = await gameMediaService.DeleteAsync(_testGame.Id);

            Assert.True(deleted);
        }

        [Fact]
        public async void CanLendGameMedia()
        {
            var mockLogger = new Mock<ILogger<GameMediaService>>();
            var mockFriendRepo = new Mock<IAsyncFriendRepository>();
            mockFriendRepo.Setup(m => m.GetByIdAsync(_testFriend.Id))
                .ReturnsAsync(_testFriend);

            var mockGameRepo = new Mock<IAsyncGameMediaRepository>();

            mockGameRepo.Setup(m => m.GetByIdAsync(_testGame.Id))
                .ReturnsAsync(_testGame);
            mockGameRepo.Setup(m => m.UpdateAsync(It.IsAny<GameMedia>()))
                .ReturnsAsync((GameMedia g) => g);

            var gameMediaService = new GameMediaService(mockLogger.Object, mockGameRepo.Object, mockFriendRepo.Object);

            var request = new GameMediaLendRequest()
            {
                FriendId = _testFriend.Id,
                GameId = _testGame.Id
            };

            var lentGame = await gameMediaService.LendGameAsync(request);

            Assert.NotNull(lentGame);
            Assert.NotNull(lentGame.BorrowerId);
            Assert.Equal(_testGame.Id, lentGame.Id);
            Assert.Equal(_testGame.Title, lentGame.Title);
            Assert.Equal(_testGame.Year, lentGame.Year);
            Assert.Equal(_testGame.Platform, lentGame.Platform);
            Assert.Equal(_testGame.MediaType, lentGame.MediaType);
            Assert.Equal(_testFriend.Id, lentGame.BorrowerId );
        }


        [Fact]
        public async System.Threading.Tasks.Task CannotLendBorrowedGameMedia()
        {
            var mockLogger = new Mock<ILogger<GameMediaService>>();
            var mockFriendRepo = new Mock<IAsyncFriendRepository>();
            var mockGameRepo = new Mock<IAsyncGameMediaRepository>();

            var borrowedGame = new GameMedia()
            {
                BorrowerId = _testFriend.Id,
                Id = _testGame.Id,
                Title = _testGame.Title,
                Year = _testGame.Year,
                Platform = _testGame.Platform,
                MediaType = _testGame.MediaType
            };

            mockFriendRepo.Setup(m => m.GetByIdAsync(_testFriend.Id))
                .ReturnsAsync(_testFriend);

            mockGameRepo.Setup(m => m.GetByIdAsync(_testGame.Id))
                .ReturnsAsync(borrowedGame);
            mockGameRepo.Setup(m => m.UpdateAsync(It.IsAny<GameMedia>()))
                .ReturnsAsync((GameMedia g) => g);

            var gameMediaService = new GameMediaService(mockLogger.Object, mockGameRepo.Object, mockFriendRepo.Object);

            var request = new GameMediaLendRequest()
            {
                FriendId = _testFriend.Id,
                GameId = _testGame.Id
            };

            await Assert.ThrowsAsync<Exception>(() => gameMediaService.LendGameAsync(request));
        }

        [Fact]
        public async void CanReturnGameMedia()
        {
            var mockLogger = new Mock<ILogger<GameMediaService>>();
            var mockFriendRepo = new Mock<IAsyncFriendRepository>();
            var mockGameRepo = new Mock<IAsyncGameMediaRepository>();

            var borrowedGame = new GameMedia()
            {
                BorrowerId = _testFriend.Id,
                Id = _testGame.Id,
                Title = _testGame.Title,
                Year = _testGame.Year,
                Platform = _testGame.Platform,
                MediaType = _testGame.MediaType
            };

            mockGameRepo.Setup(m => m.GetByIdAsync(_testGame.Id))
                .ReturnsAsync(borrowedGame);
            mockGameRepo.Setup(m => m.UpdateAsync(It.IsAny<GameMedia>()))
                .ReturnsAsync((GameMedia g) => g);

            var gameMediaService = new GameMediaService(mockLogger.Object, mockGameRepo.Object, mockFriendRepo.Object);

            var request = new GameMediaReturnRequest()
            {
                GameId = _testGame.Id
            };

            var returnedGame = await gameMediaService.ReturnGameAsync(request);

            Assert.NotNull(returnedGame);
            Assert.Null(returnedGame.BorrowerId);
            Assert.Equal(_testGame.Id, returnedGame.Id);
            Assert.Equal(_testGame.Title, returnedGame.Title);
            Assert.Equal(_testGame.Year, returnedGame.Year);
            Assert.Equal(_testGame.Platform, returnedGame.Platform);
            Assert.Equal(_testGame.MediaType, returnedGame.MediaType);
        }

        [Fact]
        public async void CannotReturnNotBorrowedGameMedia()
        {
            var mockLogger = new Mock<ILogger<GameMediaService>>();
            var mockFriendRepo = new Mock<IAsyncFriendRepository>();
            var mockGameRepo = new Mock<IAsyncGameMediaRepository>();

            mockGameRepo.Setup(m => m.GetByIdAsync(_testGame.Id))
                .ReturnsAsync(_testGame);
            mockGameRepo.Setup(m => m.UpdateAsync(It.IsAny<GameMedia>()))
                .ReturnsAsync((GameMedia g) => g);

            var gameMediaService = new GameMediaService(mockLogger.Object, mockGameRepo.Object, mockFriendRepo.Object);

            var request = new GameMediaReturnRequest()
            {
                GameId = _testGame.Id
            };

            await Assert.ThrowsAsync<Exception>(() => gameMediaService.ReturnGameAsync(request));
        }
    }
}
