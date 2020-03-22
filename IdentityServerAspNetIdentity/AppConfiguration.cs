namespace IdentityServerAspNetIdentity
{
    /// <summary>
    /// Describes application configuration.
    /// </summary>
    public class AppConfiguration
    {
        /// <summary>
        /// Allowed redirects.
        /// </summary>
        public string[] Redirects { get; set; }

        /// <summary>
        /// Callbacks.
        /// </summary>
        public string[] Callbacks { get; set; }

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
        /// Audience.
        /// </summary>
        public string Audience { get; set; }
    }
}