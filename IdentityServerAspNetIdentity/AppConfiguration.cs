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
        /// Path to ssl certificate.
        /// </summary>
        public string CertificatePath { get; set; }

        /// <summary>
        /// Certificate password.
        /// </summary>
        public string Certificate_Password { get; set; }

        /// <summary>
        /// Data protection keys.
        /// </summary>
        public string DataProtectionKeys { get; set; }

        /// <summary>
        /// Audience.
        /// </summary>
        public string Audience { get; set; }
    }
}