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
    public class FriendControllerTests
    {
        private readonly IntegrationTestServerProvider _provider;

        private readonly GameManagerContext _context;

        private readonly Friend _validTestFriend;

        public FriendControllerTests(IntegrationTestServerProvider provider)
        {
            _provider = provider;
            _context = _provider.GetService<GameManagerContext>();

            _validTestFriend = new Friend()
            {
                Name = "Marceline",
                Email = "marceline@ooo.com",
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
        public async void CanGetListOfFriends()
        {
            var adminToken = TokenHelper.GetAdminToken();
            _provider.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

            var request = new
            {
                Url = $"/api/friend/"
            };

            var response = await _provider.Client.GetAsync(request.Url);

            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<List<FriendResponseDto>>(stringResponse);

            Assert.Equal(4, model.Count);
        }

        [Fact]
        public async void CanGetOneFriend()
        {
            var adminToken = TokenHelper.GetAdminToken();
            _provider.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
            var expected = IntegrationTestDatabaseSeed.GetFriends().First();

            var request = new
            {
                Url = $"/api/friend/{expected.Id}"
            };

            var response = await _provider.Client.GetAsync(request.Url);

            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<FriendResponseDto>(stringResponse);

            Assert.NotNull(model);
            Assert.True(model.id > 0);
            Assert.Equal(expected.Name, model.name);
            Assert.Equal(expected.Email, model.email);
            Assert.Equal(expected.Telephone, model.telephone);
        }
        
        [Fact]
        public async void CanCreateNewFriend()
        {
            var adminToken = TokenHelper.GetAdminToken();
            _provider.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

            var request = new
            {
                Url = $"/api/friend/",
                Body = new StringContent(JsonConvert.SerializeObject(
                        new FriendCreateDto()
                        {
                            email = _validTestFriend.Email,
                            name = _validTestFriend.Name,
                            telephone = _validTestFriend.Telephone
                        })
                    , Encoding.UTF8, "application/json")
            };

            var response = await _provider.Client.PostAsync(request.Url, request.Body);

            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<FriendResponseDto>(stringResponse);

            Assert.NotNull(model);
            Assert.True(model.id > 0);
            Assert.Equal(_validTestFriend.Name, model.name);
            Assert.Equal(_validTestFriend.Email, model.email);
            Assert.Equal(_validTestFriend.Telephone, model.telephone);
        }

        [Fact]
        public async void CanUpdateFriend()
        {
            var adminToken = TokenHelper.GetAdminToken();
            _provider.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
            var friend = IntegrationTestDatabaseSeed.GetFriends().First();

            var request = new
            {
                Url = $"/api/friend/{friend.Id}",
                Body = new StringContent(JsonConvert.SerializeObject(
                        new FriendCreateDto()
                        {
                            name = friend.Name,
                            email = "newemail@ooo.com",
                            telephone = "9982776"
                        })
                    , Encoding.UTF8, "application/json")
            };

            var response = await _provider.Client.PutAsync(request.Url, request.Body);

            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<FriendResponseDto>(stringResponse);

            Assert.NotNull(model);
            Assert.Equal(friend.Id, model.id);
            Assert.Equal(friend.Name, model.name);
            Assert.Equal("newemail@ooo.com", model.email);
            Assert.Equal("9982776", model.telephone);
        }

    }
}
