using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPOSMonitoring.ServerApi
{
    public static class GlobalStaticVariables
    {
        public static string DbConnectionString;
        public static int Interval { get; set; }
        public static string LogFilePath { get; set; }
        public static string Token { get; set; }
        public static string RemorteHostStatusUri { get; set; }
        public static string RemortHostStatusEndpint { get; set; }
        public static string APIHealthCheckEndpoint { get; set; }
        public static int APIHealthStatusCheckEndpointTimeout { get; set; }
        public static int APIHealthCheckEndpointTimeout { get; set; }
    }
}

