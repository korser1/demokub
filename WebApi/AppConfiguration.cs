namespace WebApi
{
    /// <summary>
    /// Describes application configuration.
    /// </summary>
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

        /// <summary>
        /// Audience.
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Client id.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Client Secret, if needed.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Scope.
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// OpenId configuration endpoint.
        /// </summary>
        public string OpenIdConfigurationEndpoint { get; set; }

        /// <summary>
        /// Authorization endpoint.
        /// </summary>
        public string AuthorizationEndpoint { get; set; }

        /// <summary>
        /// Token endpoint.
        /// </summary>
        public string TokenEndpoint { get; set; }
    }
}