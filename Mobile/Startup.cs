using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Mobile.Data;

namespace Mobile
{
    public static class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            var services = builder.Services;
            var configuration = builder.Configuration;
            services.Configure<AppConfiguration>(configuration);
            AppConfiguration config = configuration.Get<AppConfiguration>();

            services.AddCors(o => o
                .AddDefaultPolicy(b => b
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()));

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.Lax;
            });

            // Add authentication services
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie()
                .AddOpenIdConnect(Constants.OpenIdScheme, options =>
                {
                    options.ConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                        config.Authority + config.OpenIdConfigurationEndpoint,
                        new InternalOpenIdConnectConfigurationRetriever(config));
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                    options.Authority = config.IdentityServer;
                    options.MetadataAddress = config.IdentityServer + config.OpenIdConfigurationEndpoint;
                    options.UseTokenLifetime = true;

                    options.ClientId = config.ClientId;
                    options.ClientSecret = config.ClientSecret;
                    options.RequireHttpsMetadata = false;

                    options.UsePkce = true;
                    // Set response type to code
                    options.ResponseType = OpenIdConnectResponseType.Code;

                    // Configure the scope
                    options.Scope.Clear();
                    options.Scope.Add("openid");
                    options.Scope.Add(config.Scope);

                    options.CallbackPath = new PathString("/signin-" + Constants.OpenIdScheme);

                    IdentityModelEventSource.ShowPII = true;
                    options.TokenValidationParameters.ValidateIssuer = false;
                    options.SaveTokens = true;

                    options.Events = new OpenIdConnectEvents
                    {
                        // handle the logout redirection
                        OnRedirectToIdentityProviderForSignOut = context =>
                        {
                            context.Response.Redirect(context.Properties.RedirectUri);
                            context.HandleResponse();

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddHttpContextAccessor();
            services.AddHttpClient();
            services.AddAuthorization();

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();
            services.AddHealthChecks().AddCheck("self", () => HealthCheckResult.Healthy());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseCors();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHealthChecks(new PathString("/healthz"), new HealthCheckOptions {AllowCachingResponses = false});
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
