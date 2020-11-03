using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using GameManager.Data.Dtos;
using GameManager.Page.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace GameManager.Page.Services
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly NavigationManager _navigationManager;
        private readonly ILocalStorageService _localStorageService;
        private readonly IHttpClientFactory _httpClientFactory;

        public ApiClient(
            NavigationManager navigationManager,
            ILocalStorageService localStorageService,
            IHttpClientFactory clientFactory
        )
        {
            _httpClientFactory = clientFactory;
            _httpClient = _httpClientFactory.CreateClient("ServerAPI");
            _navigationManager = navigationManager;
            _localStorageService = localStorageService;
        }

        public async Task<T> Get<T>(string uri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            return await Request<T>(request);
        }

        public async Task<T> Post<T>(string uri, object value)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
            return await Request<T>(request);
        }

        public async Task<T> Put<T>(string uri, object value)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, uri);
            request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
            return await Request<T>(request);
        }

        public async Task<T> Delete<T>(string uri)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, uri);
            return await Request<T>(request);
        }

        private async Task<T> Request<T>(HttpRequestMessage request)
        {
            var user = await _localStorageService.GetItemAsync<WebUser>("user");
            var isApiUrl = !request.RequestUri.IsAbsoluteUri;

            if (user != null && isApiUrl)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
            }

            using var response = await _httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _navigationManager.NavigateTo("login");
                return default;
            }

            var stringResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var json = JsonConvert.DeserializeObject<Dictionary<String, String>>(stringResponse);
                throw new Exception(json["message"]);
            }

            var model = JsonConvert.DeserializeObject<T>(stringResponse);
            return model;
        }
    }
}