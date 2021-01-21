using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Serilog;

namespace WebApi
{
    /// <summary>
    /// Implements data seeding.
    /// </summary>
    public static class SeedData
    {
        /// <summary>
        /// Seeds random data.
        /// </summary>
        /// <param name="connectionString"></param>
        public static void EnsureSeedData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<DemoDbContext>(options => options.UseSqlite(connectionString));

            using var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var context = scope.ServiceProvider.GetService<DemoDbContext>();
            context.Database.EnsureCreated();
            context.Database.Migrate();

            if (!context.WeatherForecasts.Any())
            {
                var rnd = new Random();
                var startDate = DateTime.Now.Date;
                var forecasts = Enumerable.Range(1, 5).Select(i => new WeatherForecast
                {
                    Date = startDate.AddDays(i),
                    TemperatureC = rnd.Next(-20, 55),
                    Summary = Summaries[rnd.Next(Summaries.Length)]
                });

                context.WeatherForecasts.AddRange(forecasts);
                context.SaveChanges();

                Log.Debug("created records: {Count}", context.WeatherForecasts.Count());
            }
            else
            {
                Log.Debug("data already exists!");
            }
        }

        private static readonly string[] Summaries =
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
    }
}
