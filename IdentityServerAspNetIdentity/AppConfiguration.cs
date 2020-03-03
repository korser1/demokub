namespace IdentityServerAspNetIdentity
{
    public class AppConfiguration
    {
        public string[] Redirects { get; set; }
        public string[] Callbacks { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }
    }
}