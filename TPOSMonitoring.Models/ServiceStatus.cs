using System.Diagnostics.CodeAnalysis;

namespace TPOSMonitoring.Models
{
    public class ServiceStatus
    {
        private string sqlServerStatus;
        private string sqlServerAgnetStatus;

        public string RemortHostName{ get; set; }
        public int IsRunning { get; set; } = 1;

        public string SqlServerStatus
        {
            get
            {
                return sqlServerStatus;
            }
            set
            {
                sqlServerStatus = value;
                if (sqlServerStatus != "Running")
                {
                    IsRunning = 0;
                }
            }
        }

        public string SqlServerStartupType { get; set; } = null;
       
        public string SqlServerAgnetStatus 
        {
            get
            {
                return sqlServerAgnetStatus;
            }
            set
            {
                sqlServerAgnetStatus = value;
                if (sqlServerAgnetStatus != "Running")
                {
                    IsRunning = 0;
                }
            }
        }
        public string SqlServerAgnetStartupType { get; set; } = null;
        public string RemortHostType { get; set; }
        public string RemortHostDescription { get; set; }
        public string Parent { get; set; }

    }
}