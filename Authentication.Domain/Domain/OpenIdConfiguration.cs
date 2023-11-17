namespace Authentication.Model
{
    public class OpenIdConfiguration
    {
        public string? AuthorizationEndpoint { get; set; }
        public string? TokenEndpoint { get; set; }
        public string? UserInfoEndpoint { get; set; }
    }
}
