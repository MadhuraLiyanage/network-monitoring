namespace TPOSMonotoring.Web.Data
{
    public class GlobalSettings
    {
        public int Interval { get; set; }
        public string LogFilePath { get; set; }
        public string Token { get; set; }
        public string MonitoringURI { get; set; }
        public string MonitoringEndpoint { get; set; }
    }

    public static class GlobalStaticSettings
    {
        public static int Interval { get; set; }
        public static string LogFilePath { get; set; }
        public static string Token { get; set; }
        public static string MonitoringURI { get; set; }
        public static string MonitoringEndpoint { get; set; }
    }
}
