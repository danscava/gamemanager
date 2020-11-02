using GameManager.Data.Commands;
using GameManager.Data.Interfaces;
using GameManager.Data.Models;
using GameManager.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GameManager.UnitTests.Services
{
    public class FriendServiceTests
    {
        private readonly Friend _testItem;

        public FriendServiceTests()
        {
            _testItem = new Friend()
            {
                Id = 1,
                Email = "iceking@ooo.com",
                Telephone = "992881",
                Name = "Ice King"
            };
        }

        [Fact]
        public async void CanCreateFriend()
        {
            var mockLogger = new Mock<ILogger<FriendService>>();
            var mockFriendRepo = new Mock<IAsyncFriendRepository>();
            mockFriendRepo.Setup(m => m.AddAsync(It.IsAny<Friend>())).ReturnsAsync(_testItem);

            var friendService = new FriendService(mockLogger.Object, mockFriendRepo.Object);

            var request = new FriendCreateRequest()
            {
                Name = _testItem.Name,
                Email = _testItem.Email,
                Telephone = _testItem.Telephone,
            };

            var createdFriend = await friendService.CreateAsync(request);

            Assert.NotNull(createdFriend);
            Assert.Equal(_testItem.Name, createdFriend.Name);
            Assert.Equal(_testItem.Email, createdFriend.Email);
            Assert.Equal(_testItem.Telephone, createdFriend.Telephone);
        }

        [Fact]
        public async void CanUpdateFriend()
        {
            var mockLogger = new Mock<ILogger<FriendService>>();
            var mockFriendRepo = new Mock<IAsyncFriendRepository>();

            mockFriendRepo.Setup(m => m.GetByIdAsync(_testItem.Id))
                                                .ReturnsAsync(_testItem);
            mockFriendRepo.Setup(m => m.UpdateAsync(It.IsAny<Friend>()))
                                                .ReturnsAsync((Friend f) => f);

            var gameMediaService = new FriendService(mockLogger.Object, mockFriendRepo.Object);

            var request = new FriendUpdateRequest()
            {
                Id = _testItem.Id,
                Name = "New name",
                Email = "test@ooo.com",
                Telephone = _testItem.Telephone
            };

            var updatedFriend = await gameMediaService.UpdateAsync(request);

            Assert.NotNull(updatedFriend);
            Assert.Equal(request.Id, updatedFriend.Id);
            Assert.Equal(request.Name, updatedFriend.Name);
            Assert.Equal(request.Email, updatedFriend.Email);
            Assert.Equal(request.Telephone, updatedFriend.Telephone);
        }

        [Fact]
        public async void CanDeleteFriend()
        {
            var mockLogger = new Mock<ILogger<FriendService>>();
            var mockFriendRepo = new Mock<IAsyncFriendRepository>();
            mockFriendRepo.Setup(m => m.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_testItem);

            var friendService = new FriendService(mockLogger.Object, mockFriendRepo.Object);

            var deleted = await friendService.DeleteAsync(_testItem.Id);

            Assert.True(deleted);
        }
    }
}
