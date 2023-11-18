using System.Diagnostics.CodeAnalysis;

namespace TPOSMonitoring.Models
{
    public class PosServiceStatus
    {
        public string SqlServerStatus { get; set; }
        public string SqlServerStartupType { get; set; } = null;
        public string SqlServerAgnetStatus { get; set; }
        public string SqlServerAgnetStartupType { get; set; } = null;

    }
}