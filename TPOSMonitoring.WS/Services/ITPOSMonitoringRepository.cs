using TPOSMonitoring.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPOSMonitoring.WS.Services
{
    public interface ITPOSMonitoringRepository
    {
        IEnumerable<RemorteHostStatus> GetRemortHostNames();
    }
}
