using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Mobile
{
    /// <summary>
    /// Configuration retriever to route requests inside cluster.
    /// </summary>
    public class InternalOpenIdConnectConfigurationRetriever : IConfigurationRetriever<OpenIdConnectConfiguration>
    {
        private readonly AppConfiguration _config;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="config"></param>
        public InternalOpenIdConnectConfigurationRetriever(AppConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Retrieves configuration.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="retriever"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        async Task<OpenIdConnectConfiguration> IConfigurationRetriever<OpenIdConnectConfiguration>.GetConfigurationAsync(string address, IDocumentRetriever retriever, CancellationToken cancel)
        {
            ((HttpDocumentRetriever) retriever).RequireHttps = false;
            OpenIdConnectConfiguration result = await OpenIdConnectConfigurationRetriever.GetAsync(address, retriever, cancel);

            result.AuthorizationEndpoint = _config.IdentityServer + _config.AuthorizationEndpoint;

            return result;
        }
    }
}