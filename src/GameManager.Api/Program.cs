using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace GameManager.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day,
                    restrictedToMinimumLevel: LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information("Starting up");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(serverOptions =>
                    {
                        serverOptions.Listen(IPAddress.Loopback, 5000);
                        serverOptions.Listen(IPAddress.Loopback, 5001,
                            listenOptions =>
                            {
                                listenOptions.UseHttps("https.pfx", "4\\<JhH@[C>@K_T4");
                                listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1;
                            });

                    }).UseStartup<Startup>();
                });
        }
    }
}
