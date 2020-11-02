using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GameManager.Data;
using GameManager.Data.Dtos;
using GameManager.Data.Models;
using GameManager.IntegrationTests.Common;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using Xunit;

namespace GameManager.IntegrationTests
{
    [Collection("TestServerCollection")]
    public class AuthControllerTests
    {
        private readonly IntegrationTestServerProvider _provider;

        private readonly GameManagerContext _context;

        public AuthControllerTests(IntegrationTestServerProvider provider)
        {
            _provider = provider;
            _context = _provider.GetService<GameManagerContext>();


            Setup();
        }

        private async void Setup()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            _context.GetDependencies().StateManager.ResetState();

            await IntegrationTestDatabaseSeed.SeedAsync(_context);
        }

        [Theory]
        [InlineData("admin", IntegrationTestDatabaseSeed.AdminPassword, HttpStatusCode.OK)]
        [InlineData("admin", "badpass", HttpStatusCode.BadRequest)]
        public async Task ExpectedResultsPerCredentials(string login, string password, HttpStatusCode expected)
        {
            var request = new
            {
                Url = $"/api/user/authenticate",
                Body = new StringContent(JsonConvert.SerializeObject(
                        new AuthenticateRequestDto()
                        {
                            username = login,
                            password = password
                        })
                    , Encoding.UTF8, "application/json")
            };

            var response = await _provider.Client.PostAsync(request.Url, request.Body);
  
            Assert.Equal(response.StatusCode, expected);
        }
    }
}
