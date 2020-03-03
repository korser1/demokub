namespace WebApi
{
    public class AppConfiguration
    {
        /// <summary>
        /// internal path to identity
        /// </summary>
        public string Authority { get; set; }
        
        /// <summary>
        /// public path to identity
        /// </summary>
        public string IdentityServer { get; set; }
        public string Audience { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }
    }
}