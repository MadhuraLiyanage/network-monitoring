using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using TPOSMonitoring.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPOSMonitoring.WS.Services
{
    public class TPOSMonitoringRepository : ITPOSMonitoringRepository
    {
        private AppDbContext _appDbContext;

        private DbContextOptions<AppDbContext> GetAllOptions()
        {
            DbContextOptionsBuilder<AppDbContext> optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            optionsBuilder.UseSqlServer(GlobalStaticVaiables.DbConnectionString);

            return optionsBuilder.Options;
        }

        public IEnumerable<RemorteHostStatus> GetRemortHostNames()
        {
            List<RemorteHostStatus> remortHostNames;
            using (_appDbContext = new AppDbContext(GetAllOptions()))
            {
                remortHostNames = _appDbContext.RemortHostNames.FromSqlRaw("EXEC sp_GetActiveRemortTOPSHosts").ToList();
            }
            return remortHostNames;
        }
    }
}
