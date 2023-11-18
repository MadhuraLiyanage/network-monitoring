using Microsoft.EntityFrameworkCore;
using TPOSMonitoring.Models;

namespace TPOSMonitoring.ServerApi.Services
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<RemorteHostName> RemortHostNames { get; set; }

    }
}
