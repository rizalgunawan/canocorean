using System;
using System.Reflection;
using Canocorean.Infrastructure.EntityFramework;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Canocorean.Frontend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            var logger = host.Services.GetRequiredService<ILogger<Program>>();

            try
            {
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    using (var context = services.GetRequiredService<CanocoreanDbContext>())
                    {
                        logger.LogInformation("Database creation started");
                        context.Database.EnsureCreated();
                        logger.LogInformation("Database creation completed");
                    }
                }

                logger.LogInformation("The {@serviceName} service was started", Assembly.GetExecutingAssembly().GetName().Name);
                host.Run();
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "The {@serviceName} service startup failed", Assembly.GetExecutingAssembly().GetName().Name);
            }
            finally
            {
                logger.LogInformation("The {@serviceName} service was stopped", Assembly.GetExecutingAssembly().GetName().Name);
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) => WebHost.CreateDefaultBuilder(args).UseStartup<Startup>()
            .UseSerilog((hostingContext, loggerConfiguration) =>
            {
                loggerConfiguration
                    .ReadFrom.Configuration(hostingContext.Configuration)
                    .Destructure.AsScalar<byte[]>()
                    // Serialized EntityEntry can be more than 15 MB since it contains DbContext object. It's bad idea to log such objects
                    .Destructure.AsScalar<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry>()
                    .Enrich.WithProperty("Application", Assembly.GetExecutingAssembly().GetName().Name)
                    .Enrich.WithProperty("Version", Assembly.GetExecutingAssembly().GetName().Version)
                    .Enrich.WithProperty("Environment", hostingContext.HostingEnvironment.EnvironmentName);
            });
    }
}
