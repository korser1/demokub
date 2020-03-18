using IdentityServerAspNetIdentity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace IdentityServerAspNetIdentity
{
    /// <summary>
    /// Design-time db context generator.
    /// </summary>
    public class AppDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Server=localhost;Database=ApiDbUsers;Integrated Security=SSPI;");
            // optionsBuilder.UseSqlite("Data Source=AspIdUsers.db");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}