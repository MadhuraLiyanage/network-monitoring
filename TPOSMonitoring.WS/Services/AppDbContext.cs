using Microsoft.EntityFrameworkCore;
using TPOSMonitoring.Models;

namespace TPOSMonitoring.WS.Services
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<RemorteHostStatus> RemortHostNames { get; set; }

    }
}
