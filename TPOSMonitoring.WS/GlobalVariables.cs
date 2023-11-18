using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPOSMonitoring.WS
{
    public class GlobalVariables
    {
        public int Interval { get; set; }
        public string LogFilePath { get; set; }
        public string SkyBuysFilePath { get; set; }
        public string SkyBuysFileName { get; set; }
        public string SkyBuysPromFileName { get; set; }
    }


    public static class GlobalStaticVaiables
    {
        public static string DbConnectionString;
        public static int Interval { get; set; }
        public static string LogFilePath { get; set; }
        public static string SkyBuysFilePath { get; set; }
        public static string SkyBuysFileName { get; set; }
        public static string SkyBuysPromFileName { get; set; }
    }
}

