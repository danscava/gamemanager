using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using GameManager.Data;
using GameManager.Data.Dtos;
using GameManager.Data.Enums;
using GameManager.Data.Models;
using GameManager.IntegrationTests.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Xunit;

namespace GameManager.IntegrationTests
{
    [Collection("TestServerCollection")]
    public class GameMediaControllerTests
    {
        private readonly IntegrationTestServerProvider _provider;

        private readonly GameManagerContext _context;

        private readonly GameMedia _validTestGame;

        private readonly Friend _validTestFriend;

        public GameMediaControllerTests(IntegrationTestServerProvider provider)
        {
            _provider = provider;
            _context = _provider.GetService<GameManagerContext>();

            _validTestGame = new GameMedia()
            {
                Platform = Platform.PC,
                Title = "Total Annihilation",
                Year = 1997,
                MediaType = MediaType.CD,
                Active = 1,
            };

            _validTestFriend = new Friend()
            {
                Name = "Ice King",
                Email = "iceking@ooo.com",
                Telephone = "99918827",
                Active = 1,
            };

            Setup();
        }

        private async void Setup()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            _context.GetDependencies().StateManager.ResetState();

            await IntegrationTestDatabaseSeed.SeedAsync(_context);
        }

        [Fact]
        public async void CanGetListGames()
        {
            var adminToken = TokenHelper.GetAdminToken();
            _provider.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

            var request = new
            {
                Url = $"/api/gamemedia/"
            };

            var response = await _provider.Client.GetAsync(request.Url);

            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<List<GameMediaResponseDto>>(stringResponse);

            Assert.Equal(5, model.Count);
        }

        [Fact]
        public async void CanGetOneGame()
        {
            var adminToken = TokenHelper.GetAdminToken();
            _provider.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
            var expectedGame = IntegrationTestDatabaseSeed.GetGameMedias().First();

            var request = new
            {
                Url = $"/api/gamemedia/{expectedGame.Id}"
            };

            var response = await _provider.Client.GetAsync(request.Url);

            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<GameMediaResponseDto>(stringResponse);

            Assert.NotNull(model);
            Assert.True(model.id > 0);
            Assert.Equal(expectedGame.Title, model.title);
            Assert.Equal(expectedGame.Year, model.year);
            Assert.Equal((int)expectedGame.MediaType, model.media);
            Assert.Equal((int)expectedGame.Platform, model.platform);
        }

        [Fact]
        public async void CanGetOneGameBorrowed()
        {
            var adminToken = TokenHelper.GetAdminToken();
            _provider.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
            var expectedGame = IntegrationTestDatabaseSeed.GetGameMedias().First(g => g.BorrowerId != null);

            var request = new
            {
                Url = $"/api/gamemedia/{expectedGame.Id}"
            };

            var response = await _provider.Client.GetAsync(request.Url);

            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<GameMediaResponseDto>(stringResponse);

            Assert.NotNull(model);
            Assert.True(model.id > 0);
            Assert.Equal(expectedGame.Title, model.title);
            Assert.Equal(expectedGame.Year, model.year);
            Assert.Equal((int)expectedGame.MediaType, model.media);
            Assert.Equal((int)expectedGame.Platform, model.platform);
            Assert.NotNull(model.borrower);
        }

        [Fact]
        public async void CanCreateNewGame()
        {
            var adminToken = TokenHelper.GetAdminToken();
            _provider.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

            var request = new
            {
                Url = $"/api/gamemedia/",
                Body = GetValidGameMediaContent()
            };

            var response = await _provider.Client.PostAsync(request.Url, request.Body);

            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<GameMediaResponseDto>(stringResponse);

            Assert.NotNull(model);
            Assert.True(model.id > 0);
            Assert.Equal(_validTestGame.Title, model.title);
            Assert.Equal(_validTestGame.Year, model.year);
            Assert.Equal((int) _validTestGame.MediaType, model.media);
            Assert.Equal((int) _validTestGame.Platform, model.platform);
        }

        [Fact]
        public async void CanUpdateGame()
        {
            var adminToken = TokenHelper.GetAdminToken();
            _provider.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
            var game = IntegrationTestDatabaseSeed.GetGameMedias().First();

            var request = new
            {
                Url = $"/api/gamemedia/{game.Id}",
                Body = new StringContent(JsonConvert.SerializeObject(
                        new GameMediaCreateDto()
                        {
                            title = game.Title,
                            year = 2001,
                            media_type = 1,
                            platform = 1,
                        })
                    , Encoding.UTF8, "application/json")
            };

            var response = await _provider.Client.PutAsync(request.Url, request.Body);

            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<GameMediaResponseDto>(stringResponse);

            Assert.NotNull(model);
            Assert.Equal(game.Id, model.id);
            Assert.Equal(game.Title, model.title);
            Assert.Equal(2001, model.year);
            Assert.Equal(1, model.media);
            Assert.Equal(1, model.platform);
        }

        [Fact]
        public async void CanLendAGame()
        {
            var adminToken = TokenHelper.GetAdminToken();
            _provider.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
            var game = IntegrationTestDatabaseSeed.GetGameMedias().First();
            var friend = IntegrationTestDatabaseSeed.GetFriends().First();

            var request = new
            {
                Url = $"/api/gamemedia/{game.Id}/lend",
                Body = new StringContent(JsonConvert.SerializeObject(
                    new GameMediaLendDto()
                    {
                        friend_id = friend.Id
                    })
                    , Encoding.UTF8, "application/json")
            };

            var response = await _provider.Client.PostAsync(request.Url, request.Body);

            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<GameMediaResponseDto>(stringResponse);

            Assert.NotNull(model);
            Assert.True(model.id > 0);
            Assert.Equal(game.Title, model.title);
            Assert.Equal(game.Year, model.year);
            Assert.Equal((int)game.MediaType, model.media);
            Assert.Equal((int)game.Platform, model.platform);
            Assert.NotNull(model.borrower);
            Assert.Equal(friend.Email, model.borrower.email);
            Assert.Equal(friend.Name, model.borrower.name);
            Assert.Equal(friend.Telephone, model.borrower.telephone);
        }

        [Fact]
        public async void CanReturnAGame()
        {
            var adminToken = TokenHelper.GetAdminToken();
            _provider.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
            var game = IntegrationTestDatabaseSeed.GetGameMedias().First(g => g.BorrowerId != null);
            var friend = IntegrationTestDatabaseSeed.GetFriends().First();

            var request = new
            {
                Url = $"/api/gamemedia/{game.Id}/return",
                Body = new StringContent("", Encoding.UTF8, "application/json")
            };

            var response = await _provider.Client.PostAsync(request.Url, request.Body);

            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<GameMediaResponseDto>(stringResponse);

            Assert.NotNull(model);
            Assert.Null(model.borrower);
            Assert.True(model.id > 0);
            Assert.Equal(game.Title, model.title);
            Assert.Equal(game.Year, model.year);
            Assert.Equal((int)game.MediaType, model.media);
            Assert.Equal((int)game.Platform, model.platform);
        }

        private StringContent GetValidGameMediaContent()
        {
            var request = new GameMediaCreateDto()
            {
                platform = (int) _validTestGame.Platform,
                media_type = (int) _validTestGame.MediaType,
                year = _validTestGame.Year,
                title = _validTestGame.Title
            };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            return jsonContent;
        }
    }
}
