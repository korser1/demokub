namespace Mobile
{
    /// <summary>
    /// Application configuration class.
    /// </summary>
    public class AppConfiguration
    {
        /// <summary>
        /// Internal path to identity.
        /// </summary>
        public string Authority { get; set; }

        /// <summary>
        /// Public path to identity.
        /// </summary>
        public string IdentityServer { get; set; }

        /// <summary>
        /// OpenId configuration endpoint.
        /// </summary>
        public string OpenIdConfigurationEndpoint { get; set; }

        /// <summary>
        /// Authorization endpoint.
        /// </summary>
        public string AuthorizationEndpoint { get; set; }

        /// <summary>
        /// Client Id.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Client Secret, if needed.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Web API Scope.
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// API Endpoint.
        /// </summary>
        public string ApiWeatherForecastsUrl { get; set; }
    }
}