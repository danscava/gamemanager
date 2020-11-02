using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using GameManager.Api;
using GameManager.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace GameManager.IntegrationTests.Common
{
    public class IntegrationTestServerProvider : IDisposable
    {
        public HttpClient Client { get; set; }

        public TestServer TestServer { get; set; }

        public IServiceScope ServiceScope { get; set; }

        public IntegrationTestServerProvider()
        {
            TestServer = new TestServer(
                new WebHostBuilder()
                    .ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        config.AddJsonFile("appsettings.Development.json", true, true);
                    })
                    .ConfigureServices(services =>
                    {
                        var dbDescriptor = services.SingleOrDefault(d =>
                            d.ServiceType == typeof(DbContextOptions<GameManagerContext>));

                        services.Remove(dbDescriptor);

                        services.AddDbContext<GameManagerContext>(options =>
                        {
                            options.UseInMemoryDatabase("InMemoryDbForTesting");
                        });



                    }).UseStartup<Startup>());


            ServiceScope = TestServer.Host.Services.CreateScope();

            //TestServer.BaseAddress = new Uri("http://localhost/");
            Client = TestServer.CreateClient();
        }

        public T GetService<T>()
        {
            return TestServer.Host.Services.CreateScope().ServiceProvider.GetRequiredService<T>();
        }

        public void Dispose()
        {
            TestServer?.Dispose();
            Client?.Dispose();
        }

        public Task<HttpContext> PostAsync(String path, String body)
        {
            return PostAsync(path, body, null);
        }

        public async Task<HttpContext> PostAsync(String path, String body, List<KeyValuePair<String, StringValues>> headers)
        {
            return await TestServer.SendAsync(context =>
            {
                foreach (var header in headers)
                {
                    context.Request.Headers.Add(header.Key, header.Value);
                }

                context.Request.Path = path;
                context.Request.Body = new MemoryStream(Encoding.ASCII.GetBytes(body));
                context.Request.Method = "POST";
            });
        }

        public async Task<HttpContext> GetAsync(String path, List<KeyValuePair<String, StringValues>> headers = null)
        {
            return await TestServer.SendAsync(context =>
            {
                foreach (var header in headers)
                {
                    context.Request.Headers.Add(header.Key, header.Value);
                }

                context.Request.Path = path;
                context.Request.Method = "GET";
            });
        }

        public async Task<String> ReadBodyAsync(HttpContext context)
        {
            using (var reader = new StreamReader(context.Response.Body, encoding: Encoding.UTF8))
            {
                var bodyString = await reader.ReadToEndAsync();

                return bodyString;
            }
        }
    }
}
