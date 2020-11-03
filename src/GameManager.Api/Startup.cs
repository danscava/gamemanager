using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GameManager.Api.Extensions;
using GameManager.Data;
using GameManager.Data.Interfaces;
using GameManager.Data.Repositories;
using GameManager.Services;
using GameManager.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace GameManager.Api
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
            services.AddControllers();

            // Load api settings
            var apiSettingsConfig = Configuration.GetSection("ApiSettings");
            services.Configure<ApiSettings>(apiSettingsConfig);

            // Configure Authentication
            var apiSettings = apiSettingsConfig.Get<ApiSettings>();
            var key = Encoding.ASCII.GetBytes(apiSettings.Secret);

            services.AddGameManagerAuthentication(key);

            // Add database context
            services.AddDbContext<GameManagerContext>(options =>
            {
                var connectionString = Configuration.GetConnectionString("Default");
                options.UseMySQL(connectionString);
            });

            // Inject data related classes
            services.AddScoped(typeof(IAsyncRepository<>), typeof(GenericAsyncRepository<>));
            services.AddScoped<IAsyncUserRepository, UserRepository>();
            services.AddScoped<IAsyncGameMediaRepository, GameMediaRepository>();
            services.AddScoped<IAsyncFriendRepository, FriendRepository>();

            // Inject services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGameMediaService, GameMediaService>();
            services.AddScoped<IFriendService, FriendService>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "Game Manager API", 
                    Version = "v1",
                    Description = "Web API for managing your borrowed games",
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                s.IncludeXmlComments(xmlPath);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger(o =>
            {
                o.RouteTemplate = "swagger/{documentName}/swagger.json";
            });
            
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "Game Manager API v1");
            });

            app.UseHttpsRedirection();

            app.UseExceptionHandler("/error");

            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
