using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using Persistence;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace WebApi
{
    /// <summary>
    /// Startup class.
    /// </summary>
    public static class Startup
    {
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="builder"></param>
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

            services.AddDbContext<DemoDbContext>(opt =>
            {
                opt.EnableDetailedErrors().EnableSensitiveDataLogging();
                opt.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddControllers();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.ForwardDefaultSelector = ctx => ctx.Request.Path.StartsWithSegments("/api")
                        ? JwtBearerDefaults.AuthenticationScheme
                        : CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = config.Authority;
                    options.MetadataAddress = config.Authority + config.OpenIdConfigurationEndpoint;
                    options.Audience = config.Audience;
                    options.RequireHttpsMetadata = false;

                    IdentityModelEventSource.ShowPII = true;
                    options.TokenValidationParameters.ValidateIssuer = false;

                    options.BackchannelHttpHandler = new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                    };
                });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Web Api", Version = "v1", Description = "Web Api Swagger"
                });

                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        Description = "OAuth2 Auth code Grant",
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows
                        {
                            AuthorizationCode = new OpenApiOAuthFlow
                            {
                                AuthorizationUrl = new Uri($"{config.IdentityServer}{config.AuthorizationEndpoint}", UriKind.Absolute),
                                TokenUrl = new Uri($"{config.IdentityServer}{config.TokenEndpoint}", UriKind.Absolute),
                                RefreshUrl = new Uri($"{config.IdentityServer}{config.AuthorizationEndpoint}", UriKind.Absolute),
                                Scopes = new Dictionary<string, string> {{config.Scope, "api scope"}},
                            }
                        }
                    }
                );

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = "oauth2",
                                    Type = ReferenceType.SecurityScheme
                                }
                            },
                            new List<string>()
                        }
                    }
                );
                string basePath = AppContext.BaseDirectory;
                string[] xmlFiles = Directory.GetFiles(basePath, "*.xml");
                foreach (var xmlFile in xmlFiles)
                {
                    options.IncludeXmlComments(xmlFile);
                }
            });

            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddAuthorization();
            services.AddHealthChecks().AddCheck("self", () => HealthCheckResult.Healthy());
            services.AddMvc();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        public static void Configure(WebApplication app)
        {
            IOptions<AppConfiguration> appConfigurationOptions = app.Services.GetRequiredService<IOptions<AppConfiguration>>();

            var config = appConfigurationOptions.Value;
            if (app.Environment.IsDevelopment())
            {
                app.UseCors();
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();

                using var scope = app.Services.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<DemoDbContext>();
                dbContext.Database.EnsureCreated();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(c =>
            {
                c.DocExpansion(DocExpansion.None);
                c.SupportedSubmitMethods(SubmitMethod.Get, SubmitMethod.Post, SubmitMethod.Put, SubmitMethod.Delete);
                c.OAuthConfigObject = new OAuthConfigObject
                {
                    ClientId = config.ClientId,
                    ClientSecret = config.ClientSecret,
                    AppName = "Swagger UI",
                    UsePkceWithAuthorizationCodeGrant = true
                };

                c.SwaggerEndpoint($"/swagger/v1/swagger.json", $"Web Api v1");
            });

            app.UseHealthChecks(new PathString("/healthz"), new HealthCheckOptions {AllowCachingResponses = false});
            app.MapControllers();
        }
    }
}
