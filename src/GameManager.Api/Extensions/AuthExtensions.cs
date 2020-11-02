using System.Threading.Tasks;
using GameManager.Data.Interfaces;
using GameManager.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace GameManager.Api.Extensions
{
    public static class AuthExtensions
    {
        /// <summary>
        /// Adds the authentication method for this application (JWT)
        /// </summary>
        /// <param name="services"></param>
        /// <param name="key"></param>
        public static void AddGameManagerAuthentication(this IServiceCollection services, byte[] key)
        {
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            var userRepository = context.HttpContext.RequestServices.GetRequiredService<IAsyncUserRepository>();
                            var userId = context.Principal.Identity.Name;
                            var user = userRepository.GetByUsernameAsync(userId);

                            if (user == null)
                            {
                                context.Fail("Unauthorized");
                            }
                            return Task.CompletedTask;
                        },
                    };
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }
    }
}
