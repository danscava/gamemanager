using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using GameManager.Data.Models;
using GameManager.Page.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GameManager.Page.Services;

namespace GameManager.Page
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IGameMediaService, GameMediaService>();
            services.AddScoped<IFriendService, FriendService>();
            services.AddBlazoredLocalStorage();

            var apiSettings = Configuration.GetSection("ApiAddress");

            services.AddHttpClient("ServerAPI", client =>
            {
                client.BaseAddress = new Uri(apiSettings.Value);
                
            }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                // Warning: This bypass certificate errors
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) => true
            });

            services.AddScoped<IApiClient, ApiClient>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
