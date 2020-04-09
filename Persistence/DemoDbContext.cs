using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    /// <summary>
    /// Implements data context.
    /// </summary>
    public class DemoDbContext : DbContext
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options"></param>
        public DemoDbContext(DbContextOptions<DemoDbContext> options)
            : base(options)
        {
        }

        public DbSet<WeatherForecast> WeatherForecasts { get; set; }
    }
}