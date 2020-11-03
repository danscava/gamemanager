using System.Threading.Tasks;
using Blazored.LocalStorage;
using GameManager.Page.Models;
using Microsoft.AspNetCore.Components;

namespace GameManager.Page.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IApiClient _httpService;
        private readonly ILocalStorageService _localStorageService;

        public WebUser User { get; private set; }

        public AuthenticationService(
            IApiClient httpService,
            ILocalStorageService localStorageService
        )
        {
            _httpService = httpService;
            _localStorageService = localStorageService;
        }

        public async Task Login(string username, string password)
        {
            User = await _httpService.Post<WebUser>("api/user/authenticate", new { username, password });
            await _localStorageService.SetItemAsync("user", User);
        }

        public async Task Logout()
        {
            User = null;
            await _localStorageService.RemoveItemAsync("user");
        }
    }
}