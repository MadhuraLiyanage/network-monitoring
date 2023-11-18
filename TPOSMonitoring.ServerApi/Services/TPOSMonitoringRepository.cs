using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using TPOSMonitoring.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPOSMonitoring.ServerApi;

namespace TPOSMonitoring.ServerApi.Services
{
    public class TPOSMonitoringRepository : ITPOSMonitoringRepository
    {
        private AppDbContext _appDbContext;

        private DbContextOptions<AppDbContext> GetAllOptions()
        {
            DbContextOptionsBuilder<AppDbContext> optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            optionsBuilder.UseSqlServer(GlobalStaticVariables.DbConnectionString);

            return optionsBuilder.Options;
        }

        public IEnumerable<RemorteHostName> GetRemortHostNames()
        {
            List<RemorteHostName> remortHostNames = null;
            try
            {

                using (_appDbContext = new AppDbContext(GetAllOptions()))
                {
                    remortHostNames = _appDbContext.RemortHostNames.FromSqlRaw("EXEC sp_GetActiveRemortTOPSHosts").ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return remortHostNames;
        }
    }
}
