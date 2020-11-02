using System;
using GameManager.Data.Interfaces;
using GameManager.Data.Models;
using GameManager.Services;
using GameManager.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GameManager.UnitTests.Services
{
    public class UserServiceTests
    {
        [Fact]
        public async void CanCreateUser()
        {
            var mockLogger = new Mock<ILogger<UserService>>();
            var mockUserRepo = new Mock<IAsyncUserRepository>();

            var userService = new UserService(mockLogger.Object, mockUserRepo.Object);

            User createdUser = await userService.Create(new User() {Login = "tester"}, "greatpassword");

            Assert.NotNull(createdUser);
            Assert.Equal("tester", createdUser.Login);
            Assert.NotNull(createdUser.Password);
            Assert.NotNull(createdUser.Salt);
        }

        [Fact]
        public async void CanDeleteUser()
        {
            var mockLogger = new Mock<ILogger<UserService>>();
            var mockUserRepo = new Mock<IAsyncUserRepository>();
            mockUserRepo.Setup(m => m.GetByIdAsync(1)).ReturnsAsync(new User());

            var userService = new UserService(mockLogger.Object, mockUserRepo.Object);

            var result = await userService.Delete(1);

            Assert.True(result);
        }

        [Fact]
        public async void CanAuthenticateUser()
        {
            var testUser = "tester";
            var testPassword = "greatpassword";

            var hmac = new System.Security.Cryptography.HMACSHA512();
            var passwordSalt = Convert.ToBase64String(hmac.Key);
            var passwordHash = Convert.ToBase64String(hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(testPassword)));

            var mockLogger = new Mock<ILogger<UserService>>();
            var mockUserRepo = new Mock<IAsyncUserRepository>();

            mockUserRepo.Setup(m => m.GetByUsernameAsync(testUser))
                .ReturnsAsync(new User() {Login=testUser, Password = passwordHash, Salt = passwordSalt});

            var userService = new UserService(mockLogger.Object, mockUserRepo.Object);

            var result = await userService.Authenticate(testUser, testPassword);

            Assert.NotNull(result);
            Assert.Equal("tester", result.Login);
            Assert.NotNull(result.Password);
            Assert.NotNull(result.Salt);
        }
    }
}
