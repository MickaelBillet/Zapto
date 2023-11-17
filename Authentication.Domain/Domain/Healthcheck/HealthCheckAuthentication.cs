namespace Authentication.Model.Healthcheck
{
    public class HealthCheckAuthentication
    {
        public Entries? Entries { get; set; }
        public int Status { get; set; }
        public string? TotalDuration { get; set; }
    }
}
